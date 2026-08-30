using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Platform
{
	// Token: 0x02001B37 RID: 6967
	public class BlockedPlayerList : IRemotePlayerStorageObject
	{
		// Token: 0x170019A1 RID: 6561
		// (get) Token: 0x0600D0E8 RID: 53480 RVA: 0x004C3E70 File Offset: 0x004C2070
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

		// Token: 0x0600D0E9 RID: 53481 RVA: 0x004C3EC0 File Offset: 0x004C20C0
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

		// Token: 0x0600D0EA RID: 53482 RVA: 0x004C3F80 File Offset: 0x004C2180
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

		// Token: 0x0600D0EB RID: 53483 RVA: 0x004C4014 File Offset: 0x004C2214
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

		// Token: 0x0600D0EC RID: 53484 RVA: 0x004C4150 File Offset: 0x004C2350
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

		// Token: 0x0600D0ED RID: 53485 RVA: 0x004C4188 File Offset: 0x004C2388
		public int EntryCount(bool _blocked, bool _resolveRequired)
		{
			return this.playerStates.list.Count<BlockedPlayerList.ListEntry>((BlockedPlayerList.ListEntry entry) => entry.Blocked == _blocked && (!_resolveRequired || entry.ResolvedOnce));
		}

		// Token: 0x0600D0EE RID: 53486 RVA: 0x004C41C5 File Offset: 0x004C23C5
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

		// Token: 0x0600D0EF RID: 53487 RVA: 0x004C41E4 File Offset: 0x004C23E4
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

		// Token: 0x0600D0F0 RID: 53488 RVA: 0x004C423C File Offset: 0x004C243C
		[PublicizedFrom(EAccessModifier.Private)]
		public void SortPlayerStates()
		{
			this.playerStates.list.Sort((BlockedPlayerList.ListEntry p1, BlockedPlayerList.ListEntry p2) => p2.LastSeen.CompareTo(p1.LastSeen));
		}

		// Token: 0x0600D0F1 RID: 53489 RVA: 0x004C426D File Offset: 0x004C246D
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

		// Token: 0x0600D0F2 RID: 53490 RVA: 0x004C427C File Offset: 0x004C247C
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

		// Token: 0x0600D0F3 RID: 53491 RVA: 0x004C4308 File Offset: 0x004C2508
		public void MarkForWrite()
		{
			if (this.writeRequestTime == null)
			{
				this.writeRequestTime = new DateTime?(DateTime.UtcNow);
			}
		}

		// Token: 0x0600D0F4 RID: 53492 RVA: 0x004C4328 File Offset: 0x004C2528
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

		// Token: 0x0600D0F5 RID: 53493 RVA: 0x004C43D0 File Offset: 0x004C25D0
		[PublicizedFrom(EAccessModifier.Private)]
		public void WriteRPFSCallback(IRemotePlayerFileStorage.CallbackResult _result)
		{
			this.writeToStorageState = ERoutineState.NotStarted;
			if (_result != IRemotePlayerFileStorage.CallbackResult.Success)
			{
				Log.Warning("[BlockedPlayerList] Recent Player List failed to write to remote storage.");
			}
		}

		// Token: 0x0600D0F6 RID: 53494 RVA: 0x004C43E8 File Offset: 0x004C25E8
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

		// Token: 0x0600D0F7 RID: 53495 RVA: 0x004C447C File Offset: 0x004C267C
		public bool PendingResolve()
		{
			return this.readStorageState != ERoutineState.Succeeded || this.resolveState == ERoutineState.Running;
		}

		// Token: 0x0600D0F8 RID: 53496 RVA: 0x004C4492 File Offset: 0x004C2692
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

		// Token: 0x04009FDF RID: 40927
		[PublicizedFrom(EAccessModifier.Private)]
		public static BlockedPlayerList instance;

		// Token: 0x04009FE0 RID: 40928
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan WriteThreshold = TimeSpan.FromMinutes(10.0);

		// Token: 0x04009FE1 RID: 40929
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan WriteRequestDelay = TimeSpan.FromSeconds(5.0);

		// Token: 0x04009FE2 RID: 40930
		public const int MaxBlockedPlayerEntries = 500;

		// Token: 0x04009FE3 RID: 40931
		public const int MaxRecentPlayerEntries = 100;

		// Token: 0x04009FE4 RID: 40932
		[PublicizedFrom(EAccessModifier.Private)]
		public const int Version = 1;

		// Token: 0x04009FE5 RID: 40933
		[PublicizedFrom(EAccessModifier.Private)]
		public const string FilePath = "BlockedPlayerList";

		// Token: 0x04009FE6 RID: 40934
		[PublicizedFrom(EAccessModifier.Private)]
		public const int TimeoutHours = 168;

		// Token: 0x04009FE7 RID: 40935
		[PublicizedFrom(EAccessModifier.Private)]
		public object bplLock = new object();

		// Token: 0x04009FE8 RID: 40936
		[PublicizedFrom(EAccessModifier.Private)]
		public DictionaryList<PlatformUserIdentifierAbs, BlockedPlayerList.ListEntry> playerStates = new DictionaryList<PlatformUserIdentifierAbs, BlockedPlayerList.ListEntry>();

		// Token: 0x04009FE9 RID: 40937
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastWriteTime = DateTime.UtcNow;

		// Token: 0x04009FEA RID: 40938
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime? writeRequestTime;

		// Token: 0x04009FEB RID: 40939
		[PublicizedFrom(EAccessModifier.Private)]
		public ERoutineState readStorageState;

		// Token: 0x04009FEC RID: 40940
		[PublicizedFrom(EAccessModifier.Private)]
		public IRemotePlayerFileStorage.CallbackResult readStorageResult = IRemotePlayerFileStorage.CallbackResult.Other;

		// Token: 0x04009FED RID: 40941
		[PublicizedFrom(EAccessModifier.Private)]
		public ERoutineState writeToStorageState;

		// Token: 0x04009FEE RID: 40942
		[PublicizedFrom(EAccessModifier.Private)]
		public ERoutineState resolveState;

		// Token: 0x02001B38 RID: 6968
		public class ListEntry
		{
			// Token: 0x170019A2 RID: 6562
			// (get) Token: 0x0600D0FB RID: 53499 RVA: 0x004C44F9 File Offset: 0x004C26F9
			// (set) Token: 0x0600D0FC RID: 53500 RVA: 0x004C4501 File Offset: 0x004C2701
			public bool ResolvedOnce
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x170019A3 RID: 6563
			// (get) Token: 0x0600D0FD RID: 53501 RVA: 0x004C450A File Offset: 0x004C270A
			// (set) Token: 0x0600D0FE RID: 53502 RVA: 0x004C4512 File Offset: 0x004C2712
			public bool Blocked
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x0600D0FF RID: 53503 RVA: 0x004C451B File Offset: 0x004C271B
			public ListEntry(PlayerData _playerData, DateTime _lastSeen, bool _blockState)
			{
				this.PlayerData = _playerData;
				this.LastSeen = _lastSeen;
				this.Blocked = _blockState;
			}

			// Token: 0x0600D100 RID: 53504 RVA: 0x004C4538 File Offset: 0x004C2738
			public static BlockedPlayerList.ListEntry Read(BinaryReader _reader)
			{
				PlayerData playerData = PlayerData.Read(_reader);
				DateTime utcDateTime = DateTimeOffset.FromUnixTimeSeconds(_reader.ReadInt64()).UtcDateTime;
				bool flag = _reader.ReadBoolean();
				return new BlockedPlayerList.ListEntry(playerData, utcDateTime, flag);
			}

			// Token: 0x0600D101 RID: 53505 RVA: 0x004C4570 File Offset: 0x004C2770
			public void Write(BinaryWriter _writer)
			{
				this.PlayerData.Write(_writer);
				long num = new DateTimeOffset(this.LastSeen).ToUnixTimeSeconds();
				_writer.Write(num);
				_writer.Write(this.Blocked);
			}

			// Token: 0x0600D102 RID: 53506 RVA: 0x004C45B0 File Offset: 0x004C27B0
			public void SetResolvedOnce()
			{
				this.ResolvedOnce = true;
			}

			// Token: 0x0600D103 RID: 53507 RVA: 0x004C45BC File Offset: 0x004C27BC
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

			// Token: 0x04009FEF RID: 40943
			public readonly PlayerData PlayerData;

			// Token: 0x04009FF0 RID: 40944
			public readonly DateTime LastSeen;
		}
	}
}
