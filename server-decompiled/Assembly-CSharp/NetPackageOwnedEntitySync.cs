using System;
using UnityEngine.Scripting;

// Token: 0x02000818 RID: 2072
[Preserve]
public class NetPackageOwnedEntitySync : NetPackage
{
	// Token: 0x06003CFD RID: 15613 RVA: 0x00184692 File Offset: 0x00182892
	public NetPackageOwnedEntitySync Setup(int _ownerId, int _entityId, int _entityClassId, NetPackageOwnedEntitySync.SyncType _syncType)
	{
		this.ownerId = _ownerId;
		this.entityId = _entityId;
		this.entityClassId = _entityClassId;
		this.syncType = _syncType;
		return this;
	}

	// Token: 0x06003CFE RID: 15614 RVA: 0x001846B2 File Offset: 0x001828B2
	public override void read(PooledBinaryReader _reader)
	{
		this.ownerId = _reader.ReadInt32();
		this.entityId = _reader.ReadInt32();
		this.entityClassId = _reader.ReadInt32();
		this.syncType = (NetPackageOwnedEntitySync.SyncType)_reader.ReadByte();
	}

	// Token: 0x06003CFF RID: 15615 RVA: 0x001846E4 File Offset: 0x001828E4
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.ownerId);
		_writer.Write(this.entityId);
		_writer.Write(this.entityClassId);
		_writer.Write((byte)this.syncType);
	}

	// Token: 0x06003D00 RID: 15616 RVA: 0x00184720 File Offset: 0x00182920
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

	// Token: 0x06003D01 RID: 15617 RVA: 0x000F9E96 File Offset: 0x000F8096
	public override int GetLength()
	{
		return 13;
	}

	// Token: 0x1700065B RID: 1627
	// (get) Token: 0x06003D02 RID: 15618 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x0400317F RID: 12671
	public int ownerId;

	// Token: 0x04003180 RID: 12672
	public int entityId;

	// Token: 0x04003181 RID: 12673
	public int entityClassId;

	// Token: 0x04003182 RID: 12674
	public NetPackageOwnedEntitySync.SyncType syncType;

	// Token: 0x02000819 RID: 2073
	public enum SyncType : byte
	{
		// Token: 0x04003184 RID: 12676
		Remove,
		// Token: 0x04003185 RID: 12677
		Add
	}
}
