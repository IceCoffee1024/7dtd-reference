using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Platform
{
	// Token: 0x02001B69 RID: 7017
	public class BlockedPlayerList : IRemotePlayerStorageObject
	{
		// Token: 0x17001A47 RID: 6727
		// (get) Token: 0x0600D25F RID: 53855 RVA: 0x004C8958 File Offset: 0x004C6B58
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

		// Token: 0x0600D260 RID: 53856 RVA: 0x004C89A8 File Offset: 0x004C6BA8
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

		// Token: 0x0600D261 RID: 53857 RVA: 0x004C8A68 File Offset: 0x004C6C68
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

		// Token: 0x0600D262 RID: 53858 RVA: 0x004C8AFC File Offset: 0x004C6CFC
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

		// Token: 0x0600D263 RID: 53859 RVA: 0x004C8C38 File Offset: 0x004C6E38
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

		// Token: 0x0600D264 RID: 53860 RVA: 0x004C8C70 File Offset: 0x004C6E70
		public int EntryCount(bool _blocked, bool _resolveRequired)
		{
			return this.playerStates.list.Count<BlockedPlayerList.ListEntry>((BlockedPlayerList.ListEntry entry) => entry.Blocked == _blocked && (!_resolveRequired || entry.ResolvedOnce));
		}

		// Token: 0x0600D265 RID: 53861 RVA: 0x004C8CAD File Offset: 0x004C6EAD
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

		// Token: 0x0600D266 RID: 53862 RVA: 0x004C8CCC File Offset: 0x004C6ECC
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

		// Token: 0x0600D267 RID: 53863 RVA: 0x004C8D24 File Offset: 0x004C6F24
		[PublicizedFrom(EAccessModifier.Private)]
		public void SortPlayerStates()
		{
			this.playerStates.list.Sort((BlockedPlayerList.ListEntry p1, BlockedPlayerList.ListEntry p2) => p2.LastSeen.CompareTo(p1.LastSeen));
		}

		// Token: 0x0600D268 RID: 53864 RVA: 0x004C8D55 File Offset: 0x004C6F55
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

		// Token: 0x0600D269 RID: 53865 RVA: 0x004C8D64 File Offset: 0x004C6F64
		public void ReadInto(PooledBinaryReader _reader)
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

		// Token: 0x0600D26A RID: 53866 RVA: 0x004C8DF0 File Offset: 0x004C6FF0
		public void MarkForWrite()
		{
			if (this.writeRequestTime == null)
			{
				this.writeRequestTime = new DateTime?(DateTime.UtcNow);
			}
		}

		// Token: 0x0600D26B RID: 53867 RVA: 0x004C8E10 File Offset: 0x004C7010
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

		// Token: 0x0600D26C RID: 53868 RVA: 0x004C8EB8 File Offset: 0x004C70B8
		[PublicizedFrom(EAccessModifier.Private)]
		public void WriteRPFSCallback(IRemotePlayerFileStorage.CallbackResult _result)
		{
			this.writeToStorageState = ERoutineState.NotStarted;
			if (_result != IRemotePlayerFileStorage.CallbackResult.Success)
			{
				Log.Warning("[BlockedPlayerList] Recent Player List failed to write to remote storage.");
			}
		}

		// Token: 0x0600D26D RID: 53869 RVA: 0x004C8ED0 File Offset: 0x004C70D0
		public void WriteFrom(PooledBinaryWriter _writer)
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

		// Token: 0x0600D26E RID: 53870 RVA: 0x004C8F64 File Offset: 0x004C7164
		public bool PendingResolve()
		{
			return this.readStorageState != ERoutineState.Succeeded || this.resolveState == ERoutineState.Running;
		}

		// Token: 0x0600D26F RID: 53871 RVA: 0x004C8F7A File Offset: 0x004C717A
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

		// Token: 0x0400A0B4 RID: 41140
		[PublicizedFrom(EAccessModifier.Private)]
		public static BlockedPlayerList instance;

		// Token: 0x0400A0B5 RID: 41141
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan WriteThreshold = TimeSpan.FromMinutes(10.0);

		// Token: 0x0400A0B6 RID: 41142
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan WriteRequestDelay = TimeSpan.FromSeconds(5.0);

		// Token: 0x0400A0B7 RID: 41143
		public const int MaxBlockedPlayerEntries = 500;

		// Token: 0x0400A0B8 RID: 41144
		public const int MaxRecentPlayerEntries = 100;

		// Token: 0x0400A0B9 RID: 41145
		[PublicizedFrom(EAccessModifier.Private)]
		public const int Version = 1;

		// Token: 0x0400A0BA RID: 41146
		[PublicizedFrom(EAccessModifier.Private)]
		public const string FilePath = "BlockedPlayerList";

		// Token: 0x0400A0BB RID: 41147
		[PublicizedFrom(EAccessModifier.Private)]
		public const int TimeoutHours = 168;

		// Token: 0x0400A0BC RID: 41148
		[PublicizedFrom(EAccessModifier.Private)]
		public object bplLock = new object();

		// Token: 0x0400A0BD RID: 41149
		[PublicizedFrom(EAccessModifier.Private)]
		public DictionaryList<PlatformUserIdentifierAbs, BlockedPlayerList.ListEntry> playerStates = new DictionaryList<PlatformUserIdentifierAbs, BlockedPlayerList.ListEntry>();

		// Token: 0x0400A0BE RID: 41150
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastWriteTime = DateTime.UtcNow;

		// Token: 0x0400A0BF RID: 41151
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime? writeRequestTime;

		// Token: 0x0400A0C0 RID: 41152
		[PublicizedFrom(EAccessModifier.Private)]
		public ERoutineState readStorageState;

		// Token: 0x0400A0C1 RID: 41153
		[PublicizedFrom(EAccessModifier.Private)]
		public IRemotePlayerFileStorage.CallbackResult readStorageResult = IRemotePlayerFileStorage.CallbackResult.Other;

		// Token: 0x0400A0C2 RID: 41154
		[PublicizedFrom(EAccessModifier.Private)]
		public ERoutineState writeToStorageState;

		// Token: 0x0400A0C3 RID: 41155
		[PublicizedFrom(EAccessModifier.Private)]
		public ERoutineState resolveState;

		// Token: 0x02001B6A RID: 7018
		public class ListEntry
		{
			// Token: 0x17001A48 RID: 6728
			// (get) Token: 0x0600D272 RID: 53874 RVA: 0x004C8FE1 File Offset: 0x004C71E1
			// (set) Token: 0x0600D273 RID: 53875 RVA: 0x004C8FE9 File Offset: 0x004C71E9
			public bool ResolvedOnce
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x17001A49 RID: 6729
			// (get) Token: 0x0600D274 RID: 53876 RVA: 0x004C8FF2 File Offset: 0x004C71F2
			// (set) Token: 0x0600D275 RID: 53877 RVA: 0x004C8FFA File Offset: 0x004C71FA
			public bool Blocked
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x0600D276 RID: 53878 RVA: 0x004C9003 File Offset: 0x004C7203
			public ListEntry(PlayerData _playerData, DateTime _lastSeen, bool _blockState)
			{
				this.PlayerData = _playerData;
				this.LastSeen = _lastSeen;
				this.Blocked = _blockState;
			}

			// Token: 0x0600D277 RID: 53879 RVA: 0x004C9020 File Offset: 0x004C7220
			public static BlockedPlayerList.ListEntry Read(PooledBinaryReader _reader)
			{
				PlayerData playerData = PlayerData.Read(_reader);
				DateTime utcDateTime = DateTimeOffset.FromUnixTimeSeconds(_reader.ReadInt64()).UtcDateTime;
				bool flag = _reader.ReadBoolean();
				return new BlockedPlayerList.ListEntry(playerData, utcDateTime, flag);
			}

			// Token: 0x0600D278 RID: 53880 RVA: 0x004C9058 File Offset: 0x004C7258
			public void Write(PooledBinaryWriter _writer)
			{
				this.PlayerData.Write(_writer);
				long num = new DateTimeOffset(this.LastSeen).ToUnixTimeSeconds();
				_writer.Write(num);
				_writer.Write(this.Blocked);
			}

			// Token: 0x0600D279 RID: 53881 RVA: 0x004C9098 File Offset: 0x004C7298
			public void SetResolvedOnce()
			{
				this.ResolvedOnce = true;
			}

			// Token: 0x0600D27A RID: 53882 RVA: 0x004C90A4 File Offset: 0x004C72A4
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

			// Token: 0x0400A0C4 RID: 41156
			public readonly PlayerData PlayerData;

			// Token: 0x0400A0C5 RID: 41157
			public readonly DateTime LastSeen;
		}
	}
}
