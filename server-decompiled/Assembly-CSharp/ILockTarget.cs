using System;

// Token: 0x02000580 RID: 1408
public interface ILockTarget
{
	// Token: 0x170004EB RID: 1259
	// (get) Token: 0x06002E10 RID: 11792
	LockTargetType LockTargetType { get; }

	// Token: 0x06002E11 RID: 11793
	bool IsSharedLock(ushort _channel);

	// Token: 0x06002E12 RID: 11794
	bool CanLockOnServer(int _lockingPlayerID, ILockContext _context, ushort _channel);

	// Token: 0x06002E13 RID: 11795
	bool CanLockLocally(ILockContext _context, ushort _channel);

	// Token: 0x06002E14 RID: 11796
	void OnLockedServer(bool _success, int _lockingPlayerID, ILockContext _context, ushort _channel);

	// Token: 0x06002E15 RID: 11797
	void OnLockedLocal(bool _success, ILockContext _context, ushort _channel);

	// Token: 0x06002E16 RID: 11798
	void OnUnlockedServer(int _unlockingPlayerId, ushort _channel);

	// Token: 0x06002E17 RID: 11799 RVA: 0x00124904 File Offset: 0x00122B04
	public static void WriteIdentifyingInfo(ILockTarget _target, PooledBinaryWriter _bw)
	{
		if (_target == null)
		{
			_bw.Write(0);
			return;
		}
		_bw.Write(1);
		LockTargetType lockTargetType = _target.LockTargetType;
		_bw.Write((byte)lockTargetType);
		switch (lockTargetType)
		{
		case LockTargetType.TileEntity:
		{
			TileEntity tileEntity = _target as TileEntity;
			if (tileEntity == null)
			{
				throw new InvalidOperationException(string.Format("[{0}] Expected {1} for {2}.", "ILockTarget", "TileEntity", lockTargetType));
			}
			StreamUtils.Write(_bw, tileEntity.ToWorldPos());
			return;
		}
		case LockTargetType.TEFeature:
		{
			TEFeatureAbs tefeatureAbs = _target as TEFeatureAbs;
			if (tefeatureAbs == null)
			{
				throw new InvalidOperationException(string.Format("[{0}] Expected {1} for {2}.", "ILockTarget", "TEFeatureAbs", lockTargetType));
			}
			StreamUtils.Write(_bw, tefeatureAbs.ToWorldPos());
			TileEntityFeatureData featureData = tefeatureAbs.FeatureData;
			_bw.Write(((featureData != null) ? featureData.Name : null) ?? string.Empty);
			return;
		}
		case LockTargetType.Entity:
		{
			Entity entity = _target as Entity;
			if (entity == null)
			{
				throw new InvalidOperationException(string.Format("[{0}] Expected {1} for {2}.", "ILockTarget", "Entity", lockTargetType));
			}
			_bw.Write(entity.entityId);
			return;
		}
		case LockTargetType.TransactionalInventory:
		{
			TransactionalInventory transactionalInventory = _target as TransactionalInventory;
			if (transactionalInventory == null)
			{
				throw new InvalidOperationException(string.Format("[{0}] Expected {1} for {2}.", "ILockTarget", "TransactionalInventory", lockTargetType));
			}
			StreamUtils.Write(_bw, transactionalInventory.Key);
			return;
		}
		default:
			throw new NotSupportedException(string.Format("[{0}] Unsupported type {1}.", "ILockTarget", lockTargetType));
		}
	}

	// Token: 0x06002E18 RID: 11800 RVA: 0x00124A68 File Offset: 0x00122C68
	public static ILockTarget ReadIdentifyingInfo(PooledBinaryReader _br)
	{
		if (_br.ReadByte() == 0)
		{
			return null;
		}
		LockTargetType lockTargetType = (LockTargetType)_br.ReadByte();
		switch (lockTargetType)
		{
		case LockTargetType.TileEntity:
		{
			Vector3i vector3i = StreamUtils.ReadVector3i(_br);
			TileEntity tileEntity = GameManager.Instance.World.GetTileEntity(vector3i);
			if (tileEntity == null)
			{
				Log.Warning(string.Format("[{0}] {1} not found (pos={2}).", "ILockTarget", "TileEntity", vector3i));
			}
			return tileEntity;
		}
		case LockTargetType.TEFeature:
		{
			Vector3i vector3i2 = StreamUtils.ReadVector3i(_br);
			string text = _br.ReadString();
			TileEntityComposite tileEntityComposite = GameManager.Instance.World.GetTileEntity(vector3i2) as TileEntityComposite;
			if (tileEntityComposite == null)
			{
				Log.Warning(string.Format("[{0}] {1} parent not found (pos={2}, name='{3}').", new object[] { "ILockTarget", "TEFeatureAbs", vector3i2, text }));
				return null;
			}
			TEFeatureAbs tefeatureAbs = tileEntityComposite.GetFeature(text.AsMemory()) as TEFeatureAbs;
			if (tefeatureAbs == null)
			{
				Log.Warning("[ILockTarget] TEFeatureAbs '" + text + "' not found on parent.");
			}
			return tefeatureAbs;
		}
		case LockTargetType.Entity:
		{
			int num = _br.ReadInt32();
			ILockTarget lockTarget = null;
			if (num != -1)
			{
				lockTarget = GameManager.Instance.World.GetEntity(num);
			}
			if (lockTarget == null)
			{
				Log.Warning(string.Format("[{0}] {1} not found (id={2}).", "ILockTarget", "Entity", num));
			}
			return lockTarget;
		}
		case LockTargetType.TransactionalInventory:
		{
			Guid guid = StreamUtils.ReadGuid(_br);
			TransactionalInventory transactionalInventory;
			if (!InventoryManager.Instance.TryGetTransactionalInventory(guid, out transactionalInventory))
			{
				Log.Warning(string.Format("[{0}] {1} not found (key={2}).", "ILockTarget", "TransactionalInventory", guid));
				return null;
			}
			return transactionalInventory;
		}
		default:
			Log.Warning(string.Format("[{0}] Unsupported type during read: {1}.", "ILockTarget", lockTargetType));
			return null;
		}
	}
}
