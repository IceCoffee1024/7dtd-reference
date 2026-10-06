using System;
using System.Collections.Generic;
using UnityEngine.Pool;

// Token: 0x02000588 RID: 1416
public class LockManager
{
	// Token: 0x17000518 RID: 1304
	// (get) Token: 0x06002E99 RID: 11929 RVA: 0x00126AF4 File Offset: 0x00124CF4
	public static LockManager Instance
	{
		get
		{
			if (LockManager._instance == null)
			{
				LockManager._instance = new LockManager();
			}
			return LockManager._instance;
		}
	}

	// Token: 0x06002E9A RID: 11930 RVA: 0x00126B0C File Offset: 0x00124D0C
	public void Init()
	{
		if (this.pendingClientLocks.Count > 0 || this.singleLocks.CountKeys > 0 || this.sharedLocks.CountValues > 0)
		{
			Log.Warning("[LockManager] had locked entities from a previous session!");
			this.pendingClientLocks.Clear();
			this.singleLocks.Clear();
			this.sharedLocks.Clear();
		}
		this.playersWithLocks.Clear();
		this.keepOpenTimes.Clear();
		this.lastUpdate = DateTime.UtcNow;
	}

	// Token: 0x06002E9B RID: 11931 RVA: 0x00126B90 File Offset: 0x00124D90
	public void Update()
	{
		if (GameManager.Instance.World == null)
		{
			return;
		}
		if ((DateTime.UtcNow - this.lastUpdate).TotalSeconds <= 2.5)
		{
			return;
		}
		this.lastUpdate = DateTime.UtcNow;
		int primaryPlayerId = GameManager.Instance.World.GetPrimaryPlayerId();
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			List<int> list = null;
			foreach (KeyValuePair<int, DateTime> keyValuePair in this.keepOpenTimes)
			{
				if (keyValuePair.Key != primaryPlayerId && (DateTime.UtcNow - keyValuePair.Value).TotalSeconds > 10.0)
				{
					if (list == null)
					{
						list = new List<int>();
					}
					list.Add(keyValuePair.Key);
				}
			}
			if (list == null)
			{
				return;
			}
			using (List<int>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					int num = enumerator2.Current;
					Log.Warning(string.Format("[{0}] No keep-open packet received from player {1}.", "LockManager", num));
					this.ForceUnlockByPlayer(num);
				}
				return;
			}
		}
		if (this.playersWithLocks.Contains(primaryPlayerId))
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageInventoryKeepOpen>().Setup(), false);
		}
	}

	// Token: 0x06002E9C RID: 11932 RVA: 0x00126D00 File Offset: 0x00124F00
	public bool IsLockedByLocalPlayer(ILockTarget _target, ushort _channel = 0)
	{
		if (_target == null)
		{
			return false;
		}
		LockManager.LockEntry lockEntry = new LockManager.LockEntry(_target, _channel);
		if (this.pendingClientLocks.Contains(lockEntry))
		{
			return true;
		}
		int primaryPlayerId = GameManager.Instance.World.GetPrimaryPlayerId();
		if (primaryPlayerId == -1)
		{
			return false;
		}
		IReadOnlyCollection<int> readOnlyCollection;
		if (_target.IsSharedLock(_channel) && this.sharedLocks.TryGetByValue(lockEntry, out readOnlyCollection))
		{
			using (IEnumerator<int> enumerator = readOnlyCollection.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == primaryPlayerId)
					{
						return true;
					}
				}
			}
			return false;
		}
		int num;
		return !_target.IsSharedLock(_channel) && this.singleLocks.TryGetByValue(lockEntry, out num) && num == primaryPlayerId;
	}

	// Token: 0x06002E9D RID: 11933 RVA: 0x00126DC0 File Offset: 0x00124FC0
	public bool IsLockedServer(ILockTarget _target, ushort _channel = 0)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Log.Warning("[LockManager] Clients may only check local lock states by using IsLockedByLocalPlayer.");
			return false;
		}
		LockManager.LockEntry lockEntry = new LockManager.LockEntry(_target, _channel);
		return (_target.IsSharedLock(_channel) && this.sharedLocks.ContainsValue(lockEntry)) || (!_target.IsSharedLock(_channel) && this.singleLocks.ContainsValue(lockEntry));
	}

	// Token: 0x06002E9E RID: 11934 RVA: 0x00126E21 File Offset: 0x00125021
	public void LockRequestLocal(ILockTarget _target, ushort _channel = 0)
	{
		if (_target == null)
		{
			Log.Error("[LockManager] target supplied for locking was null.");
			return;
		}
		this.requestTargets[0] = _target;
		this.LockRequestLocal(1, _channel);
	}

	// Token: 0x06002E9F RID: 11935 RVA: 0x00126E42 File Offset: 0x00125042
	public void LockRequestLocal(ILockTarget _targetA, ILockTarget _targetB, ushort _channel = 0)
	{
		if (_targetA == null || _targetB == null)
		{
			Log.Error("[LockManager] target supplied for locking was null.");
			return;
		}
		this.requestTargets[0] = _targetA;
		this.requestTargets[1] = _targetB;
		this.LockRequestLocal(2, _channel);
	}

	// Token: 0x06002EA0 RID: 11936 RVA: 0x00126E6F File Offset: 0x0012506F
	[PublicizedFrom(EAccessModifier.Private)]
	public void LockRequestLocal(int _count, ushort _channel)
	{
		this.LockRequestLocal(new ReadOnlySpan<ILockTarget>(this.requestTargets, 0, _count), _channel);
		Array.Clear(this.requestTargets, 0, _count);
	}

	// Token: 0x06002EA1 RID: 11937 RVA: 0x00126E94 File Offset: 0x00125094
	public unsafe void LockRequestLocal(ReadOnlySpan<ILockTarget> _targets, ushort _channel = 0)
	{
		if (_targets == null || _targets.Length == 0)
		{
			Log.Error("[LockManager] no targets supplied for locking.");
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			if (this.pendingClientLocks.Count > 0)
			{
				Log.Warning("[LockManager] lock request already in progress.");
				return;
			}
			for (int i = 0; i < _targets.Length; i++)
			{
				this.pendingClientLocks.Add(new LockManager.LockEntry(*_targets[i], _channel));
			}
		}
		PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true);
		try
		{
			using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
			{
				pooledBinaryWriter.SetBaseStream(pooledExpandableMemoryStream);
				pooledBinaryWriter.Write(true);
				pooledBinaryWriter.Write(_channel);
				pooledBinaryWriter.Write(_targets.Length);
				for (int j = 0; j < _targets.Length; j++)
				{
					WorldAddress.Create(*_targets[j]).Write(pooledBinaryWriter);
				}
				for (int k = 0; k < _targets.Length; k++)
				{
					_targets[k]->OnLockRequestLocal(pooledBinaryWriter, _channel);
				}
			}
			this.DispatchRequest(pooledExpandableMemoryStream);
		}
		finally
		{
			MemoryPools.poolMemoryStream.FreeSync(pooledExpandableMemoryStream);
		}
	}

	// Token: 0x06002EA2 RID: 11938 RVA: 0x00126FE0 File Offset: 0x001251E0
	public void ProcessRequest(PooledBinaryReader _br, int _playerId)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Log.Warning("[LockManager] Clients must only request a lock by using LockRequestLocal.");
			return;
		}
		if (_br.ReadBoolean())
		{
			this.LockRequestServer(_playerId, _br);
			return;
		}
		this.UnlockRequestServer(_playerId, false);
	}

	// Token: 0x06002EA3 RID: 11939 RVA: 0x00127014 File Offset: 0x00125214
	[PublicizedFrom(EAccessModifier.Private)]
	public void LockRequestServer(int _playerId, PooledBinaryReader _br)
	{
		ushort num = _br.ReadUInt16();
		int num2 = _br.ReadInt32();
		if (num2 < 1 || num2 > 5)
		{
			Log.Warning(string.Format("[{0}] Player {1} requested {2} lock targets, limit is {3}.", new object[] { "LockManager", _playerId, num2, 5 }));
			return;
		}
		ILockTarget[] array = new ILockTarget[num2];
		for (int i = 0; i < num2; i++)
		{
			array[i] = WorldAddress.Read(_br).Resolve<ILockTarget>();
		}
		if (this.playersWithLocks.Contains(_playerId))
		{
			Log.Warning(string.Format("[{0}] Invalid lock state for player {1} when calling {2}. The player tried requesting a lock despite having an existing lock state.", "LockManager", _playerId, "LockRequestServer"));
			this.ForceUnlockByPlayer(_playerId);
			return;
		}
		bool flag = false;
		string text = null;
		try
		{
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] == null)
				{
					text = "[LockManager] Failed to lock targets. A requested target was null.";
					return;
				}
			}
			foreach (ILockTarget lockTarget in array)
			{
				LockManager.LockEntry lockEntry = new LockManager.LockEntry(lockTarget, num);
				if (!lockTarget.IsSharedLock(num) && this.singleLocks.ContainsValue(lockEntry))
				{
					text = string.Format("[{0}] Failed to lock {1} on channel {2}. It is already locked by another player.", "LockManager", lockTarget.GetType(), num);
					return;
				}
			}
			foreach (ILockTarget lockTarget2 in array)
			{
				if (!lockTarget2.OnLockRequestServer(_playerId, _br, num))
				{
					text = string.Format("[{0}] Failed to lock {1} on channel {2}. Target refused to be locked on server.", "LockManager", lockTarget2.GetType(), num);
					return;
				}
			}
			flag = true;
		}
		finally
		{
			PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true);
			try
			{
				using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
				{
					pooledBinaryWriter.SetBaseStream(pooledExpandableMemoryStream);
					pooledBinaryWriter.Write(true);
					pooledBinaryWriter.Write(flag);
					pooledBinaryWriter.Write(text ?? string.Empty);
					pooledBinaryWriter.Write(num);
					pooledBinaryWriter.Write(array.Length);
					for (int m = 0; m < array.Length; m++)
					{
						WorldAddress.Create(array[m]).Write(pooledBinaryWriter);
					}
					if (flag)
					{
						foreach (ILockTarget lockTarget3 in array)
						{
							LockManager.LockEntry lockEntry2 = new LockManager.LockEntry(lockTarget3, num);
							if (lockTarget3.IsSharedLock(num))
							{
								this.sharedLocks.Add(_playerId, lockEntry2);
							}
							else
							{
								this.singleLocks.Add(_playerId, lockEntry2);
							}
						}
						this.RefreshPlayerActive(_playerId);
						for (int num3 = 0; num3 < array.Length; num3++)
						{
							array[num3].OnLockResponseServer(_playerId, pooledBinaryWriter, num);
						}
					}
				}
				this.DispatchResponse(pooledExpandableMemoryStream, _playerId);
			}
			finally
			{
				MemoryPools.poolMemoryStream.FreeSync(pooledExpandableMemoryStream);
			}
		}
	}

	// Token: 0x06002EA4 RID: 11940 RVA: 0x00127308 File Offset: 0x00125508
	public void ProcessResponse(PooledBinaryReader _br)
	{
		if (_br.ReadBoolean())
		{
			this.LockResponseLocal(_br);
			return;
		}
		this.UnlockResponseLocal(_br);
	}

	// Token: 0x06002EA5 RID: 11941 RVA: 0x00127324 File Offset: 0x00125524
	[PublicizedFrom(EAccessModifier.Private)]
	public void LockResponseLocal(PooledBinaryReader _br)
	{
		bool flag = _br.ReadBoolean();
		string text = _br.ReadString();
		ushort num = _br.ReadUInt16();
		int num2 = _br.ReadInt32();
		ILockTarget[] array = new ILockTarget[num2];
		for (int i = 0; i < num2; i++)
		{
			array[i] = WorldAddress.Read(_br).Resolve<ILockTarget>();
		}
		this.pendingClientLocks.Clear();
		if (!string.IsNullOrEmpty(text))
		{
			Log.Warning(text);
		}
		if (GameManager.Instance.World == null)
		{
			Log.Warning("[LockManager] World not fully initialized.");
			return;
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j] == null)
			{
				Log.Warning("[LockManager] target supplied for locking was null.");
				if (flag)
				{
					this.UnlockRequestLocal();
				}
				return;
			}
		}
		int primaryPlayerId = GameManager.Instance.World.GetPrimaryPlayerId();
		if (flag && SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			foreach (ILockTarget lockTarget in array)
			{
				LockManager.LockEntry lockEntry = new LockManager.LockEntry(lockTarget, num);
				if (lockTarget.IsSharedLock(num))
				{
					this.sharedLocks.Add(primaryPlayerId, lockEntry);
				}
				else
				{
					this.singleLocks.Add(primaryPlayerId, lockEntry);
				}
			}
			this.RefreshPlayerActive(primaryPlayerId);
		}
		foreach (ILockTarget lockTarget2 in array)
		{
			if (!lockTarget2.OnLockResponseLocal(flag, _br, num) && flag)
			{
				Log.Warning(string.Format("[{0}] {1} on channel {2} refused the lock locally, unlocking.", "LockManager", lockTarget2.GetType(), num));
				this.UnlockRequestLocal();
				return;
			}
		}
	}

	// Token: 0x06002EA6 RID: 11942 RVA: 0x001274A0 File Offset: 0x001256A0
	public void UnlockRequestLocal()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			LocalPlayerUI.GetUIForPrimaryPlayer().windowManager.CloseAllOpenModalWindows(null, false);
		}
		PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true);
		try
		{
			using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
			{
				pooledBinaryWriter.SetBaseStream(pooledExpandableMemoryStream);
				pooledBinaryWriter.Write(false);
			}
			this.DispatchRequest(pooledExpandableMemoryStream);
		}
		finally
		{
			MemoryPools.poolMemoryStream.FreeSync(pooledExpandableMemoryStream);
		}
	}

	// Token: 0x06002EA7 RID: 11943 RVA: 0x00127530 File Offset: 0x00125730
	public void UnlockRequestServer(int _playerId, bool _isForceUnlocked)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Log.Warning("[LockManager] Clients must unlock by using UnlockRequestLocal.");
			return;
		}
		bool flag = false;
		string text = null;
		try
		{
			if (!this.playersWithLocks.Contains(_playerId))
			{
				text = "[LockManager] Player entity has nothing to unlock.";
			}
			else
			{
				List<LockManager.LockEntry> list;
				using (CollectionPool<List<LockManager.LockEntry>, LockManager.LockEntry>.Get(out list))
				{
					this.singleLocks.TryGetByKey(_playerId, list);
					this.sharedLocks.TryGetByKey(_playerId, list);
					this.singleLocks.RemoveByKey(_playerId);
					this.sharedLocks.RemoveByKey(_playerId);
					this.RefreshPlayerActive(_playerId);
					if (list.Count == 0)
					{
						text = string.Format("[{0}] Found no targets while trying to unlock for player {1}.", "LockManager", _playerId);
						return;
					}
					for (int i = 0; i < list.Count; i++)
					{
						list[i].Target.OnUnlockedServer(_playerId, list[i].Channel);
					}
				}
				flag = true;
			}
		}
		finally
		{
			if (GameManager.Instance.World.GetPrimaryPlayerId() == _playerId)
			{
				LocalPlayerUI.GetUIForPrimaryPlayer().windowManager.CloseAllOpenModalWindows(null, false);
			}
			PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true);
			try
			{
				using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
				{
					pooledBinaryWriter.SetBaseStream(pooledExpandableMemoryStream);
					pooledBinaryWriter.Write(false);
					pooledBinaryWriter.Write(flag);
					pooledBinaryWriter.Write(text ?? string.Empty);
					pooledBinaryWriter.Write(_isForceUnlocked);
				}
				this.DispatchResponse(pooledExpandableMemoryStream, _playerId);
			}
			finally
			{
				MemoryPools.poolMemoryStream.FreeSync(pooledExpandableMemoryStream);
			}
		}
	}

	// Token: 0x06002EA8 RID: 11944 RVA: 0x001276F0 File Offset: 0x001258F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void DispatchRequest(PooledExpandableMemoryStream _request)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageLockTarget>().Setup(_request), false);
			return;
		}
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
		{
			pooledBinaryReader.SetBaseStream(_request);
			_request.Position = 0L;
			this.ProcessRequest(pooledBinaryReader, GameManager.Instance.World.GetPrimaryPlayerId());
		}
	}

	// Token: 0x06002EA9 RID: 11945 RVA: 0x00127770 File Offset: 0x00125970
	[PublicizedFrom(EAccessModifier.Private)]
	public void DispatchResponse(PooledExpandableMemoryStream _response, int _playerId)
	{
		if (GameManager.Instance.World.GetPrimaryPlayerId() != _playerId)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageLockTarget>().Setup(_response), false, _playerId, -1, -1, null, 192, false);
			return;
		}
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
		{
			pooledBinaryReader.SetBaseStream(_response);
			_response.Position = 0L;
			this.ProcessResponse(pooledBinaryReader);
		}
	}

	// Token: 0x06002EAA RID: 11946 RVA: 0x001277F8 File Offset: 0x001259F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void UnlockResponseLocal(PooledBinaryReader _br)
	{
		bool flag = _br.ReadBoolean();
		string text = _br.ReadString();
		bool flag2 = _br.ReadBoolean();
		int primaryPlayerId = GameManager.Instance.World.GetPrimaryPlayerId();
		this.singleLocks.RemoveByKey(primaryPlayerId);
		this.sharedLocks.RemoveByKey(primaryPlayerId);
		this.RefreshPlayerActive(primaryPlayerId);
		if (!flag && !string.IsNullOrEmpty(text))
		{
			Log.Warning(text);
		}
		if (flag2)
		{
			LocalPlayerUI.GetUIForPrimaryPlayer().windowManager.CloseAllOpenModalWindows(null, false);
			this.pendingClientLocks.Clear();
		}
	}

	// Token: 0x06002EAB RID: 11947 RVA: 0x0012787C File Offset: 0x00125A7C
	public void ForceUnlockAll()
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Log.Warning("[LockManager] Only servers can use ForceUnlockAll");
			return;
		}
		List<int> list;
		using (CollectionPool<List<int>, int>.Get(out list))
		{
			list.AddRange(this.playersWithLocks);
			for (int i = 0; i < list.Count; i++)
			{
				this.ForceUnlockByPlayer(list[i]);
			}
		}
	}

	// Token: 0x06002EAC RID: 11948 RVA: 0x001278F4 File Offset: 0x00125AF4
	public void ForceUnlockByPlayer(int _playerId)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Log.Warning("[LockManager] Only servers can use ForceUnlockByPlayer");
			return;
		}
		this.UnlockRequestServer(_playerId, true);
	}

	// Token: 0x06002EAD RID: 11949 RVA: 0x00127918 File Offset: 0x00125B18
	public void ForceUnlockLockTarget(ILockTarget _target)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Log.Warning("[LockManager] Only servers can use ForceUnlockLockTarget");
			return;
		}
		if (_target == null)
		{
			return;
		}
		List<int> list = new List<int>();
		foreach (LockManager.LockEntry lockEntry in this.singleLocks.Values)
		{
			int num;
			if (lockEntry.Target == _target && this.singleLocks.TryGetByValue(lockEntry, out num) && !list.Contains(num))
			{
				list.Add(num);
			}
		}
		foreach (LockManager.LockEntry lockEntry2 in this.sharedLocks.Values)
		{
			IReadOnlyCollection<int> readOnlyCollection;
			if (lockEntry2.Target == _target && this.sharedLocks.TryGetByValue(lockEntry2, out readOnlyCollection))
			{
				foreach (int num2 in readOnlyCollection)
				{
					if (!list.Contains(num2))
					{
						list.Add(num2);
					}
				}
			}
		}
		if (list.Count > 0)
		{
			foreach (int num3 in list)
			{
				this.UnlockRequestServer(num3, true);
			}
			return;
		}
		Log.Warning("[LockManager] Target is not locked.");
	}

	// Token: 0x06002EAE RID: 11950 RVA: 0x00127AA4 File Offset: 0x00125CA4
	public void ForceUnlockByChunk(HashSetLong chunks)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Log.Warning("[LockManager] Only servers can use ForceUnlockByChunk");
			return;
		}
		List<ILockTarget> list = new List<ILockTarget>();
		foreach (long num in chunks)
		{
			foreach (LockManager.LockEntry lockEntry in this.singleLocks.Values)
			{
				TileEntity tileEntity = lockEntry.Target as TileEntity;
				if (tileEntity != null)
				{
					Chunk chunk = tileEntity.GetChunk();
					if ((chunk == null || chunk.Key == num) && !list.Contains(lockEntry.Target))
					{
						list.Add(lockEntry.Target);
					}
				}
				else
				{
					TEFeatureAbs tefeatureAbs = lockEntry.Target as TEFeatureAbs;
					if (tefeatureAbs != null)
					{
						TileEntityComposite parent = tefeatureAbs.Parent;
						Chunk chunk2 = ((parent != null) ? parent.GetChunk() : null);
						if ((chunk2 == null || chunk2.Key == num) && !list.Contains(lockEntry.Target))
						{
							list.Add(lockEntry.Target);
						}
					}
				}
			}
			foreach (LockManager.LockEntry lockEntry2 in this.sharedLocks.Values)
			{
				TileEntity tileEntity2 = lockEntry2.Target as TileEntity;
				if (tileEntity2 != null)
				{
					Chunk chunk3 = tileEntity2.GetChunk();
					if ((chunk3 == null || chunk3.Key == num) && !list.Contains(lockEntry2.Target))
					{
						list.Add(lockEntry2.Target);
					}
				}
				else
				{
					TEFeatureAbs tefeatureAbs2 = lockEntry2.Target as TEFeatureAbs;
					if (tefeatureAbs2 != null)
					{
						TileEntityComposite parent2 = tefeatureAbs2.Parent;
						Chunk chunk4 = ((parent2 != null) ? parent2.GetChunk() : null);
						if ((chunk4 == null || chunk4.Key == num) && !list.Contains(lockEntry2.Target))
						{
							list.Add(lockEntry2.Target);
						}
					}
				}
			}
		}
		foreach (ILockTarget lockTarget in list)
		{
			this.ForceUnlockLockTarget(lockTarget);
		}
	}

	// Token: 0x06002EAF RID: 11951 RVA: 0x00127D30 File Offset: 0x00125F30
	public void ProcessKeepOpen(int _playerId)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		this.RefreshPlayerActive(_playerId);
		if (!this.playersWithLocks.Contains(_playerId))
		{
			Log.Warning("[LockManager] Keep Open request received but player owns no locks.");
			this.ForceUnlockByPlayer(_playerId);
		}
	}

	// Token: 0x06002EB0 RID: 11952 RVA: 0x00127D68 File Offset: 0x00125F68
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshPlayerActive(int playerId)
	{
		if (this.singleLocks.ContainsKey(playerId) || this.sharedLocks.ContainsKey(playerId))
		{
			this.playersWithLocks.Add(playerId);
			this.keepOpenTimes[playerId] = DateTime.UtcNow;
			return;
		}
		this.playersWithLocks.Remove(playerId);
		this.keepOpenTimes.Remove(playerId);
	}

	// Token: 0x040023EB RID: 9195
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MaxLocksPerPlayer = 5;

	// Token: 0x040023EC RID: 9196
	[PublicizedFrom(EAccessModifier.Private)]
	public const float KeepOpenTimeoutSeconds = 10f;

	// Token: 0x040023ED RID: 9197
	[PublicizedFrom(EAccessModifier.Private)]
	public const float KeepOpenPeriodSeconds = 2.5f;

	// Token: 0x040023EE RID: 9198
	[PublicizedFrom(EAccessModifier.Private)]
	public static LockManager _instance;

	// Token: 0x040023EF RID: 9199
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> playersWithLocks = new HashSet<int>();

	// Token: 0x040023F0 RID: 9200
	[PublicizedFrom(EAccessModifier.Private)]
	public OneToManyDictionary<int, LockManager.LockEntry> singleLocks = new OneToManyDictionary<int, LockManager.LockEntry>();

	// Token: 0x040023F1 RID: 9201
	[PublicizedFrom(EAccessModifier.Private)]
	public ManyToManyDictionary<int, LockManager.LockEntry> sharedLocks = new ManyToManyDictionary<int, LockManager.LockEntry>();

	// Token: 0x040023F2 RID: 9202
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<LockManager.LockEntry> pendingClientLocks = new HashSet<LockManager.LockEntry>();

	// Token: 0x040023F3 RID: 9203
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ILockTarget[] requestTargets = new ILockTarget[5];

	// Token: 0x040023F4 RID: 9204
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, DateTime> keepOpenTimes = new Dictionary<int, DateTime>();

	// Token: 0x040023F5 RID: 9205
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime lastUpdate;

	// Token: 0x02000589 RID: 1417
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly struct LockEntry : IEquatable<LockManager.LockEntry>
	{
		// Token: 0x06002EB2 RID: 11954 RVA: 0x00127E26 File Offset: 0x00126026
		public LockEntry(ILockTarget target, ushort channel = 0)
		{
			this.Target = target;
			this.Channel = channel;
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x00127E36 File Offset: 0x00126036
		public bool Equals(LockManager.LockEntry other)
		{
			return this.Target == other.Target && this.Channel == other.Channel;
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x00127E56 File Offset: 0x00126056
		public override int GetHashCode()
		{
			ILockTarget target = this.Target;
			return HashCode.Combine<int, ushort>((target != null) ? target.GetHashCode() : 0, this.Channel);
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x00127E78 File Offset: 0x00126078
		public override bool Equals(object obj)
		{
			if (obj is LockManager.LockEntry)
			{
				LockManager.LockEntry lockEntry = (LockManager.LockEntry)obj;
				return this.Equals(lockEntry);
			}
			return false;
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x00127E9D File Offset: 0x0012609D
		public static bool operator ==(LockManager.LockEntry left, LockManager.LockEntry right)
		{
			return left.Equals(right);
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x00127EA7 File Offset: 0x001260A7
		public static bool operator !=(LockManager.LockEntry left, LockManager.LockEntry right)
		{
			return !left.Equals(right);
		}

		// Token: 0x040023F6 RID: 9206
		public readonly ILockTarget Target;

		// Token: 0x040023F7 RID: 9207
		public readonly ushort Channel;
	}
}
