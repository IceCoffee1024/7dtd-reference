using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Platform
{
	// Token: 0x02001B9A RID: 7066
	public class BlockedPlayerList : IRemotePlayerStorageObject
	{
		// Token: 0x17001A8F RID: 6799
		// (get) Token: 0x0600D373 RID: 54131 RVA: 0x004CC480 File Offset: 0x004CA680
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

		// Token: 0x0600D374 RID: 54132 RVA: 0x004CC4D0 File Offset: 0x004CA6D0
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

		// Token: 0x0600D375 RID: 54133 RVA: 0x004CC590 File Offset: 0x004CA790
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

		// Token: 0x0600D376 RID: 54134 RVA: 0x004CC624 File Offset: 0x004CA824
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

		// Token: 0x0600D377 RID: 54135 RVA: 0x004CC760 File Offset: 0x004CA960
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

		// Token: 0x0600D378 RID: 54136 RVA: 0x004CC798 File Offset: 0x004CA998
		public int EntryCount(bool _blocked, bool _resolveRequired)
		{
			return this.playerStates.list.Count<BlockedPlayerList.ListEntry>((BlockedPlayerList.ListEntry entry) => entry.Blocked == _blocked && (!_resolveRequired || entry.ResolvedOnce));
		}

		// Token: 0x0600D379 RID: 54137 RVA: 0x004CC7D5 File Offset: 0x004CA9D5
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

		// Token: 0x0600D37A RID: 54138 RVA: 0x004CC7F4 File Offset: 0x004CA9F4
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

		// Token: 0x0600D37B RID: 54139 RVA: 0x004CC84C File Offset: 0x004CAA4C
		[PublicizedFrom(EAccessModifier.Private)]
		public void SortPlayerStates()
		{
			this.playerStates.list.Sort((BlockedPlayerList.ListEntry p1, BlockedPlayerList.ListEntry p2) => p2.LastSeen.CompareTo(p1.LastSeen));
		}

		// Token: 0x0600D37C RID: 54140 RVA: 0x004CC87D File Offset: 0x004CAA7D
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

		// Token: 0x0600D37D RID: 54141 RVA: 0x004CC88C File Offset: 0x004CAA8C
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

		// Token: 0x0600D37E RID: 54142 RVA: 0x004CC918 File Offset: 0x004CAB18
		public void MarkForWrite()
		{
			if (this.writeRequestTime == null)
			{
				this.writeRequestTime = new DateTime?(DateTime.UtcNow);
			}
		}

		// Token: 0x0600D37F RID: 54143 RVA: 0x004CC938 File Offset: 0x004CAB38
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

		// Token: 0x0600D380 RID: 54144 RVA: 0x004CC9E0 File Offset: 0x004CABE0
		[PublicizedFrom(EAccessModifier.Private)]
		public void WriteRPFSCallback(IRemotePlayerFileStorage.CallbackResult _result)
		{
			this.writeToStorageState = ERoutineState.NotStarted;
			if (_result != IRemotePlayerFileStorage.CallbackResult.Success)
			{
				Log.Warning("[BlockedPlayerList] Recent Player List failed to write to remote storage.");
			}
		}

		// Token: 0x0600D381 RID: 54145 RVA: 0x004CC9F8 File Offset: 0x004CABF8
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

		// Token: 0x0600D382 RID: 54146 RVA: 0x004CCA8C File Offset: 0x004CAC8C
		public bool PendingResolve()
		{
			return this.readStorageState != ERoutineState.Succeeded || this.resolveState == ERoutineState.Running;
		}

		// Token: 0x0600D383 RID: 54147 RVA: 0x004CCAA2 File Offset: 0x004CACA2
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

		// Token: 0x0400A19D RID: 41373
		[PublicizedFrom(EAccessModifier.Private)]
		public static BlockedPlayerList instance;

		// Token: 0x0400A19E RID: 41374
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan WriteThreshold = TimeSpan.FromMinutes(10.0);

		// Token: 0x0400A19F RID: 41375
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan WriteRequestDelay = TimeSpan.FromSeconds(5.0);

		// Token: 0x0400A1A0 RID: 41376
		public const int MaxBlockedPlayerEntries = 500;

		// Token: 0x0400A1A1 RID: 41377
		public const int MaxRecentPlayerEntries = 100;

		// Token: 0x0400A1A2 RID: 41378
		[PublicizedFrom(EAccessModifier.Private)]
		public const int Version = 1;

		// Token: 0x0400A1A3 RID: 41379
		[PublicizedFrom(EAccessModifier.Private)]
		public const string FilePath = "BlockedPlayerList";

		// Token: 0x0400A1A4 RID: 41380
		[PublicizedFrom(EAccessModifier.Private)]
		public const int TimeoutHours = 168;

		// Token: 0x0400A1A5 RID: 41381
		[PublicizedFrom(EAccessModifier.Private)]
		public object bplLock = new object();

		// Token: 0x0400A1A6 RID: 41382
		[PublicizedFrom(EAccessModifier.Private)]
		public DictionaryList<PlatformUserIdentifierAbs, BlockedPlayerList.ListEntry> playerStates = new DictionaryList<PlatformUserIdentifierAbs, BlockedPlayerList.ListEntry>();

		// Token: 0x0400A1A7 RID: 41383
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastWriteTime = DateTime.UtcNow;

		// Token: 0x0400A1A8 RID: 41384
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime? writeRequestTime;

		// Token: 0x0400A1A9 RID: 41385
		[PublicizedFrom(EAccessModifier.Private)]
		public ERoutineState readStorageState;

		// Token: 0x0400A1AA RID: 41386
		[PublicizedFrom(EAccessModifier.Private)]
		public IRemotePlayerFileStorage.CallbackResult readStorageResult = IRemotePlayerFileStorage.CallbackResult.Other;

		// Token: 0x0400A1AB RID: 41387
		[PublicizedFrom(EAccessModifier.Private)]
		public ERoutineState writeToStorageState;

		// Token: 0x0400A1AC RID: 41388
		[PublicizedFrom(EAccessModifier.Private)]
		public ERoutineState resolveState;

		// Token: 0x02001B9B RID: 7067
		public class ListEntry
		{
			// Token: 0x17001A90 RID: 6800
			// (get) Token: 0x0600D386 RID: 54150 RVA: 0x004CCB09 File Offset: 0x004CAD09
			// (set) Token: 0x0600D387 RID: 54151 RVA: 0x004CCB11 File Offset: 0x004CAD11
			public bool ResolvedOnce
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x17001A91 RID: 6801
			// (get) Token: 0x0600D388 RID: 54152 RVA: 0x004CCB1A File Offset: 0x004CAD1A
			// (set) Token: 0x0600D389 RID: 54153 RVA: 0x004CCB22 File Offset: 0x004CAD22
			public bool Blocked
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x0600D38A RID: 54154 RVA: 0x004CCB2B File Offset: 0x004CAD2B
			public ListEntry(PlayerData _playerData, DateTime _lastSeen, bool _blockState)
			{
				this.PlayerData = _playerData;
				this.LastSeen = _lastSeen;
				this.Blocked = _blockState;
			}

			// Token: 0x0600D38B RID: 54155 RVA: 0x004CCB48 File Offset: 0x004CAD48
			public static BlockedPlayerList.ListEntry Read(PooledBinaryReader _reader)
			{
				PlayerData playerData = PlayerData.Read(_reader);
				DateTime utcDateTime = DateTimeOffset.FromUnixTimeSeconds(_reader.ReadInt64()).UtcDateTime;
				bool flag = _reader.ReadBoolean();
				return new BlockedPlayerList.ListEntry(playerData, utcDateTime, flag);
			}

			// Token: 0x0600D38C RID: 54156 RVA: 0x004CCB80 File Offset: 0x004CAD80
			public void Write(PooledBinaryWriter _writer)
			{
				this.PlayerData.Write(_writer);
				long num = new DateTimeOffset(this.LastSeen).ToUnixTimeSeconds();
				_writer.Write(num);
				_writer.Write(this.Blocked);
			}

			// Token: 0x0600D38D RID: 54157 RVA: 0x004CCBC0 File Offset: 0x004CADC0
			public void SetResolvedOnce()
			{
				this.ResolvedOnce = true;
			}

			// Token: 0x0600D38E RID: 54158 RVA: 0x004CCBCC File Offset: 0x004CADCC
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

			// Token: 0x0400A1AD RID: 41389
			public readonly PlayerData PlayerData;

			// Token: 0x0400A1AE RID: 41390
			public readonly DateTime LastSeen;
		}
	}
}
