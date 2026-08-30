using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Platform
{
	// Token: 0x02001B10 RID: 6928
	public class BlockedPlayerList : IRemotePlayerStorageObject
	{
		// Token: 0x1700192F RID: 6447
		// (get) Token: 0x0600CF5F RID: 53087 RVA: 0x004BDA7C File Offset: 0x004BBC7C
		public static BlockedPlayerList Instance
		{
			get
			{
				if (BlockedPlayerList.instance == null)
				{
					IPlatform multiPlatform = PlatformManager.MultiPlatform;
					if (((multiPlatform != null) ? multiPlatform.RemotePlayerFileStorage : null) != null)
					{
						BlockedPlayerList.instance = new BlockedPlayerList();
						PlayerInteractions.Instance.OnNewPlayerInteraction += BlockedPlayerList.instance.OnPlayerInteraction;
					}
				}
				return BlockedPlayerList.instance;
			}
		}

		// Token: 0x0600CF60 RID: 53088 RVA: 0x004BDACC File Offset: 0x004BBCCC
		public void Update()
		{
			IPlatform multiPlatform = PlatformManager.MultiPlatform;
			if (((multiPlatform != null) ? multiPlatform.RemotePlayerFileStorage : null) == null)
			{
				return;
			}
			if ((this.writeRequestTime != null && DateTime.UtcNow - this.writeRequestTime >= BlockedPlayerList.WriteRequestDelay) || DateTime.UtcNow - this.lastWriteTime >= BlockedPlayerList.WriteThreshold)
			{
				this.WriteToStorage();
				this.lastWriteTime = DateTime.UtcNow;
				this.writeRequestTime = null;
			}
		}

		// Token: 0x0600CF61 RID: 53089 RVA: 0x004BDB8C File Offset: 0x004BBD8C
		public void UpdatePlayersSeenInWorld(World _world)
		{
			if (((_world != null) ? _world.Players : null) == null)
			{
				return;
			}
			foreach (EntityPlayer entityPlayer in _world.Players.list)
			{
				PersistentPlayerData playerDataFromEntityID = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(entityPlayer.entityId);
				this.AddOrUpdatePlayer(playerDataFromEntityID.PlayerData, DateTime.UtcNow, null, false);
			}
		}

		// Token: 0x0600CF62 RID: 53090 RVA: 0x004BDC20 File Offset: 0x004BBE20
		[PublicizedFrom(EAccessModifier.Private)]
		public BlockedPlayerList.ListEntry AddOrUpdatePlayer(PlayerData _playerData, DateTime _timeStamp, bool? _blocked = null, bool _ignoreLimit = false)
		{
			if (_playerData == null || _playerData.PrimaryId.Equals(PlatformManager.MultiPlatform.User.PlatformUserId))
			{
				return null;
			}
			DateTime dateTime = DateTime.UtcNow.AddHours(-168.0);
			bool? flag = _blocked;
			bool flag2 = false;
			if (((flag.GetValueOrDefault() == flag2) & (flag != null)) && dateTime >= _timeStamp)
			{
				return null;
			}
			object obj = this.bplLock;
			BlockedPlayerList.ListEntry listEntry2;
			lock (obj)
			{
				BlockedPlayerList.ListEntry valueOrDefault = this.playerStates.dict.GetValueOrDefault(_playerData.PrimaryId);
				if (!_ignoreLimit)
				{
					flag = _blocked;
					bool flag3 = true;
					if (((flag.GetValueOrDefault() == flag3) & (flag != null)) && (valueOrDefault == null || !valueOrDefault.Blocked) && this.EntryCount(true, false) >= 500)
					{
						return null;
					}
				}
				BlockedPlayerList.ListEntry listEntry;
				if (_blocked == null && valueOrDefault != null)
				{
					listEntry = new BlockedPlayerList.ListEntry(_playerData, _timeStamp, valueOrDefault.Blocked);
				}
				else
				{
					listEntry = new BlockedPlayerList.ListEntry(_playerData, _timeStamp, _blocked.GetValueOrDefault());
				}
				this.playerStates.Set(_playerData.PrimaryId, listEntry);
				listEntry2 = listEntry;
			}
			return listEntry2;
		}

		// Token: 0x0600CF63 RID: 53091 RVA: 0x004BDD5C File Offset: 0x004BBF5C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPlayerInteraction(PlayerInteraction _interaction)
		{
			BlockedPlayerList.ListEntry listEntry = this.AddOrUpdatePlayer(_interaction.PlayerData, DateTime.UtcNow, null, false);
			if (listEntry != null)
			{
				this.MarkForWrite();
				listEntry.SetResolvedOnce();
			}
		}

		// Token: 0x0600CF64 RID: 53092 RVA: 0x004BDD94 File Offset: 0x004BBF94
		public int EntryCount(bool _blocked, bool _resolveRequired)
		{
			return this.playerStates.list.Count<BlockedPlayerList.ListEntry>((BlockedPlayerList.ListEntry entry) => entry.Blocked == _blocked && (!_resolveRequired || entry.ResolvedOnce));
		}

		// Token: 0x0600CF65 RID: 53093 RVA: 0x004BDDD1 File Offset: 0x004BBFD1
		public IEnumerable<BlockedPlayerList.ListEntry> GetEntriesOrdered(bool _blocked, bool _resolveRequired)
		{
			object obj = this.bplLock;
			lock (obj)
			{
				this.SortPlayerStates();
				int num;
				for (int i = 0; i < this.playerStates.list.Count; i = num + 1)
				{
					BlockedPlayerList.ListEntry listEntry = this.playerStates.list[i];
					if (listEntry.Blocked == _blocked && (!_resolveRequired || listEntry.ResolvedOnce))
					{
						yield return listEntry;
					}
					num = i;
				}
			}
			obj = null;
			yield break;
			yield break;
		}

		// Token: 0x0600CF66 RID: 53094 RVA: 0x004BDDF0 File Offset: 0x004BBFF0
		public BlockedPlayerList.ListEntry GetPlayerStateInfo(PlatformUserIdentifierAbs _primaryId)
		{
			object obj = this.bplLock;
			BlockedPlayerList.ListEntry listEntry2;
			lock (obj)
			{
				BlockedPlayerList.ListEntry listEntry;
				if (this.playerStates.dict.TryGetValue(_primaryId, out listEntry))
				{
					listEntry2 = listEntry;
				}
				else
				{
					listEntry2 = null;
				}
			}
			return listEntry2;
		}

		// Token: 0x0600CF67 RID: 53095 RVA: 0x004BDE48 File Offset: 0x004BC048
		[PublicizedFrom(EAccessModifier.Private)]
		public void SortPlayerStates()
		{
			this.playerStates.list.Sort((BlockedPlayerList.ListEntry p1, BlockedPlayerList.ListEntry p2) => p2.LastSeen.CompareTo(p1.LastSeen));
		}

		// Token: 0x0600CF68 RID: 53096 RVA: 0x004BDE79 File Offset: 0x004BC079
		public IEnumerator ReadStorageAndResolve()
		{
			BlockedPlayerList.<>c__DisplayClass25_0 CS$<>8__locals1 = new BlockedPlayerList.<>c__DisplayClass25_0();
			CS$<>8__locals1.<>4__this = this;
			this.readStorageState = ERoutineState.Running;
			object obj = this.bplLock;
			lock (obj)
			{
				BlockedPlayerList blockedPlayerList = IRemotePlayerFileStorage.ReadCachedObject<BlockedPlayerList>(PlatformManager.MultiPlatform.User, "BlockedPlayerList");
				if (blockedPlayerList != null)
				{
					this.playerStates = blockedPlayerList.playerStates;
				}
			}
			CS$<>8__locals1.callbackComplete = false;
			IRemotePlayerFileStorage remotePlayerFileStorage = PlatformManager.MultiPlatform.RemotePlayerFileStorage;
			if (remotePlayerFileStorage != null)
			{
				remotePlayerFileStorage.ReadRemoteObject<BlockedPlayerList>("BlockedPlayerList", true, new IRemotePlayerFileStorage.FileReadObjectCompleteCallback<BlockedPlayerList>(CS$<>8__locals1.<ReadStorageAndResolve>g__ReadRPFSCallback|0));
				while (!CS$<>8__locals1.callbackComplete)
				{
					yield return null;
				}
			}
			if (this.playerStates.Count > 0)
			{
				yield return this.ResolveUserDetails();
			}
			this.readStorageState = ERoutineState.Succeeded;
			yield break;
		}

		// Token: 0x0600CF69 RID: 53097 RVA: 0x004BDE88 File Offset: 0x004BC088
		public void ReadInto(BinaryReader _reader)
		{
			object obj = this.bplLock;
			lock (obj)
			{
				this.playerStates.Clear();
				_reader.ReadInt32();
				int num = _reader.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					BlockedPlayerList.ListEntry listEntry = BlockedPlayerList.ListEntry.Read(_reader);
					this.AddOrUpdatePlayer(listEntry.PlayerData, listEntry.LastSeen, new bool?(listEntry.Blocked), false);
				}
			}
		}

		// Token: 0x0600CF6A RID: 53098 RVA: 0x004BDF14 File Offset: 0x004BC114
		public void MarkForWrite()
		{
			if (this.writeRequestTime == null)
			{
				this.writeRequestTime = new DateTime?(DateTime.UtcNow);
			}
		}

		// Token: 0x0600CF6B RID: 53099 RVA: 0x004BDF34 File Offset: 0x004BC134
		[PublicizedFrom(EAccessModifier.Private)]
		public void WriteToStorage()
		{
			if (this.writeToStorageState == ERoutineState.Running)
			{
				Log.Warning("[BlockedPlayerList] Tried to write to storage while another write is already in progress.");
				return;
			}
			if (this.readStorageResult != IRemotePlayerFileStorage.CallbackResult.Success && this.readStorageResult != IRemotePlayerFileStorage.CallbackResult.MalformedData && this.readStorageResult != IRemotePlayerFileStorage.CallbackResult.FileNotFound)
			{
				Log.Out("[BlockedPlayerList] Error when processing remote list. Saving to local cache only.");
				if (!IRemotePlayerFileStorage.WriteCachedObject(PlatformManager.MultiPlatform.User, "BlockedPlayerList", this))
				{
					Log.Warning("[BlockedPlayerList] Failed to write to local cache.");
				}
				return;
			}
			if (this.readStorageResult == IRemotePlayerFileStorage.CallbackResult.MalformedData)
			{
				Log.Out("[BlockedPlayerList] Previous remote list was malformed so it will be overwritten.");
			}
			this.writeToStorageState = ERoutineState.Running;
			PlatformManager.MultiPlatform.RemotePlayerFileStorage.WriteRemoteObject("BlockedPlayerList", this, true, new IRemotePlayerFileStorage.FileWriteCompleteCallback(this.WriteRPFSCallback));
		}

		// Token: 0x0600CF6C RID: 53100 RVA: 0x004BDFDC File Offset: 0x004BC1DC
		[PublicizedFrom(EAccessModifier.Private)]
		public void WriteRPFSCallback(IRemotePlayerFileStorage.CallbackResult _result)
		{
			this.writeToStorageState = ERoutineState.NotStarted;
			if (_result != IRemotePlayerFileStorage.CallbackResult.Success)
			{
				Log.Warning("[BlockedPlayerList] Recent Player List failed to write to remote storage.");
			}
		}

		// Token: 0x0600CF6D RID: 53101 RVA: 0x004BDFF4 File Offset: 0x004BC1F4
		public void WriteFrom(BinaryWriter _writer)
		{
			object obj = this.bplLock;
			lock (obj)
			{
				_writer.Write(1);
				this.SortPlayerStates();
				int num = this.EntryCount(true, false);
				int num2 = Math.Min(this.EntryCount(false, false), 100);
				_writer.Write(num + num2);
				for (int i = 0; i < num + num2; i++)
				{
					this.playerStates.list[i].Write(_writer);
				}
			}
		}

		// Token: 0x0600CF6E RID: 53102 RVA: 0x004BE088 File Offset: 0x004BC288
		public bool PendingResolve()
		{
			return this.readStorageState != ERoutineState.Succeeded || this.resolveState == ERoutineState.Running;
		}

		// Token: 0x0600CF6F RID: 53103 RVA: 0x004BE09E File Offset: 0x004BC29E
		public IEnumerator ResolveUserDetails()
		{
			while (this.resolveState == ERoutineState.Running)
			{
				yield return null;
			}
			try
			{
				this.resolveState = ERoutineState.Running;
				List<IPlatformUserData> dataList = new List<IPlatformUserData>();
				object obj = this.bplLock;
				lock (obj)
				{
					foreach (BlockedPlayerList.ListEntry listEntry in this.playerStates.list)
					{
						listEntry.PlayerData.PlatformData.RequestUserDetailsUpdate();
						dataList.Add(listEntry.PlayerData.PlatformData);
					}
				}
				yield return PlatformUserManager.ResolveUsersDetailsCoroutine(dataList);
				foreach (IPlatformUserData platformUserData in dataList)
				{
					AuthoredText playerName = this.playerStates.dict[platformUserData.PrimaryId].PlayerData.PlayerName;
					if (platformUserData.Name != null && platformUserData.Name != playerName.Text)
					{
						playerName.Update(platformUserData.Name, playerName.Author);
						GeneratedTextManager.PrefilterText(playerName, GeneratedTextManager.TextFilteringMode.Filter);
					}
					this.playerStates.dict[platformUserData.PrimaryId].SetResolvedOnce();
				}
				this.resolveState = ERoutineState.Succeeded;
				dataList = null;
			}
			finally
			{
				if (this.resolveState != ERoutineState.Succeeded)
				{
					this.resolveState = ERoutineState.Failed;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x04009EAE RID: 40622
		[PublicizedFrom(EAccessModifier.Private)]
		public static BlockedPlayerList instance;

		// Token: 0x04009EAF RID: 40623
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan WriteThreshold = TimeSpan.FromMinutes(10.0);

		// Token: 0x04009EB0 RID: 40624
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan WriteRequestDelay = TimeSpan.FromSeconds(5.0);

		// Token: 0x04009EB1 RID: 40625
		public const int MaxBlockedPlayerEntries = 500;

		// Token: 0x04009EB2 RID: 40626
		public const int MaxRecentPlayerEntries = 100;

		// Token: 0x04009EB3 RID: 40627
		[PublicizedFrom(EAccessModifier.Private)]
		public const int Version = 1;

		// Token: 0x04009EB4 RID: 40628
		[PublicizedFrom(EAccessModifier.Private)]
		public const string FilePath = "BlockedPlayerList";

		// Token: 0x04009EB5 RID: 40629
		[PublicizedFrom(EAccessModifier.Private)]
		public const int TimeoutHours = 168;

		// Token: 0x04009EB6 RID: 40630
		[PublicizedFrom(EAccessModifier.Private)]
		public object bplLock = new object();

		// Token: 0x04009EB7 RID: 40631
		[PublicizedFrom(EAccessModifier.Private)]
		public DictionaryList<PlatformUserIdentifierAbs, BlockedPlayerList.ListEntry> playerStates = new DictionaryList<PlatformUserIdentifierAbs, BlockedPlayerList.ListEntry>();

		// Token: 0x04009EB8 RID: 40632
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastWriteTime = DateTime.UtcNow;

		// Token: 0x04009EB9 RID: 40633
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime? writeRequestTime;

		// Token: 0x04009EBA RID: 40634
		[PublicizedFrom(EAccessModifier.Private)]
		public ERoutineState readStorageState;

		// Token: 0x04009EBB RID: 40635
		[PublicizedFrom(EAccessModifier.Private)]
		public IRemotePlayerFileStorage.CallbackResult readStorageResult = IRemotePlayerFileStorage.CallbackResult.Other;

		// Token: 0x04009EBC RID: 40636
		[PublicizedFrom(EAccessModifier.Private)]
		public ERoutineState writeToStorageState;

		// Token: 0x04009EBD RID: 40637
		[PublicizedFrom(EAccessModifier.Private)]
		public ERoutineState resolveState;

		// Token: 0x02001B11 RID: 6929
		public class ListEntry
		{
			// Token: 0x17001930 RID: 6448
			// (get) Token: 0x0600CF72 RID: 53106 RVA: 0x004BE105 File Offset: 0x004BC305
			// (set) Token: 0x0600CF73 RID: 53107 RVA: 0x004BE10D File Offset: 0x004BC30D
			public bool ResolvedOnce
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x17001931 RID: 6449
			// (get) Token: 0x0600CF74 RID: 53108 RVA: 0x004BE116 File Offset: 0x004BC316
			// (set) Token: 0x0600CF75 RID: 53109 RVA: 0x004BE11E File Offset: 0x004BC31E
			public bool Blocked
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x0600CF76 RID: 53110 RVA: 0x004BE127 File Offset: 0x004BC327
			public ListEntry(PlayerData _playerData, DateTime _lastSeen, bool _blockState)
			{
				this.PlayerData = _playerData;
				this.LastSeen = _lastSeen;
				this.Blocked = _blockState;
			}

			// Token: 0x0600CF77 RID: 53111 RVA: 0x004BE144 File Offset: 0x004BC344
			public static BlockedPlayerList.ListEntry Read(BinaryReader _reader)
			{
				PlayerData playerData = PlayerData.Read(_reader);
				DateTime utcDateTime = DateTimeOffset.FromUnixTimeSeconds(_reader.ReadInt64()).UtcDateTime;
				bool flag = _reader.ReadBoolean();
				return new BlockedPlayerList.ListEntry(playerData, utcDateTime, flag);
			}

			// Token: 0x0600CF78 RID: 53112 RVA: 0x004BE17C File Offset: 0x004BC37C
			public void Write(BinaryWriter _writer)
			{
				this.PlayerData.Write(_writer);
				long num = new DateTimeOffset(this.LastSeen).ToUnixTimeSeconds();
				_writer.Write(num);
				_writer.Write(this.Blocked);
			}

			// Token: 0x0600CF79 RID: 53113 RVA: 0x004BE1BC File Offset: 0x004BC3BC
			public void SetResolvedOnce()
			{
				this.ResolvedOnce = true;
			}

			// Token: 0x0600CF7A RID: 53114 RVA: 0x004BE1C8 File Offset: 0x004BC3C8
			public ValueTuple<bool, string> SetBlockState(bool _blockState)
			{
				if (this.Blocked == _blockState)
				{
					return new ValueTuple<bool, string>(false, null);
				}
				if (PlatformManager.NativePlatform.User.CanShowProfile(this.PlayerData.NativeId))
				{
					this.Blocked = false;
					Log.Warning(string.Format("[BlockedPlayerList] Cannot change block state of native user {0} through the block list", this.PlayerData.NativeId));
					return new ValueTuple<bool, string>(false, null);
				}
				if (_blockState && BlockedPlayerList.Instance.EntryCount(true, false) >= 500)
				{
					return new ValueTuple<bool, string>(false, Localization.Get("xuiBlockedPlayersCantAddMessage", false, null));
				}
				this.PlayerData.PlatformData.MarkBlockedStateChanged();
				BlockedPlayerList.Instance.MarkForWrite();
				this.Blocked = _blockState;
				return new ValueTuple<bool, string>(true, null);
			}

			// Token: 0x04009EBE RID: 40638
			public readonly PlayerData PlayerData;

			// Token: 0x04009EBF RID: 40639
			public readonly DateTime LastSeen;
		}
	}
}
