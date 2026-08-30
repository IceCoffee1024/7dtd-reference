using System;
using UnityEngine.Scripting;

// Token: 0x0200080D RID: 2061
[Preserve]
public class NetPackageOwnedEntitySync : NetPackage
{
	// Token: 0x06003C9E RID: 15518 RVA: 0x001822BA File Offset: 0x001804BA
	public NetPackageOwnedEntitySync Setup(int _ownerId, int _entityId, int _entityClassId, NetPackageOwnedEntitySync.SyncType _syncType)
	{
		this.ownerId = _ownerId;
		this.entityId = _entityId;
		this.entityClassId = _entityClassId;
		this.syncType = _syncType;
		return this;
	}

	// Token: 0x06003C9F RID: 15519 RVA: 0x001822DA File Offset: 0x001804DA
	public override void read(PooledBinaryReader _reader)
	{
		this.ownerId = _reader.ReadInt32();
		this.entityId = _reader.ReadInt32();
		this.entityClassId = _reader.ReadInt32();
		this.syncType = (NetPackageOwnedEntitySync.SyncType)_reader.ReadByte();
	}

	// Token: 0x06003CA0 RID: 15520 RVA: 0x0018230C File Offset: 0x0018050C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.ownerId);
		_writer.Write(this.entityId);
		_writer.Write(this.entityClassId);
		_writer.Write((byte)this.syncType);
	}

	// Token: 0x06003CA1 RID: 15521 RVA: 0x00182348 File Offset: 0x00180548
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityAlive entityAlive = GameManager.Instance.World.GetEntity(this.ownerId) as EntityAlive;
		if (entityAlive)
		{
			NetPackageOwnedEntitySync.SyncType syncType = this.syncType;
			if (syncType != NetPackageOwnedEntitySync.SyncType.Remove)
			{
				if (syncType == NetPackageOwnedEntitySync.SyncType.Add)
				{
					entityAlive.AddOwnedEntity(new OwnedEntityData(this.entityId, this.entityClassId));
					return;
				}
			}
			else
			{
				entityAlive.RemoveOwnedEntity(this.entityId);
			}
		}
	}

	// Token: 0x06003CA2 RID: 15522 RVA: 0x000F8E8A File Offset: 0x000F708A
	public override int GetLength()
	{
		return 13;
	}

	// Token: 0x17000650 RID: 1616
	// (get) Token: 0x06003CA3 RID: 15523 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003100 RID: 12544
	public int ownerId;

	// Token: 0x04003101 RID: 12545
	public int entityId;

	// Token: 0x04003102 RID: 12546
	public int entityClassId;

	// Token: 0x04003103 RID: 12547
	public NetPackageOwnedEntitySync.SyncType syncType;

	// Token: 0x0200080E RID: 2062
	public enum SyncType : byte
	{
		// Token: 0x04003105 RID: 12549
		Remove,
		// Token: 0x04003106 RID: 12550
		Add
	}
}
