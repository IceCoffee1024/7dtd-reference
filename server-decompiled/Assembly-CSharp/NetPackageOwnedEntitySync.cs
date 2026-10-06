using System;
using UnityEngine.Scripting;

// Token: 0x02000816 RID: 2070
[Preserve]
public class NetPackageOwnedEntitySync : NetPackage
{
	// Token: 0x06003CC4 RID: 15556 RVA: 0x0018426E File Offset: 0x0018246E
	public NetPackageOwnedEntitySync Setup(int _ownerId, int _entityId, int _entityClassId, NetPackageOwnedEntitySync.SyncType _syncType)
	{
		this.ownerId = _ownerId;
		this.entityId = _entityId;
		this.entityClassId = _entityClassId;
		this.syncType = _syncType;
		return this;
	}

	// Token: 0x06003CC5 RID: 15557 RVA: 0x0018428E File Offset: 0x0018248E
	public override void read(PooledBinaryReader _reader)
	{
		this.ownerId = _reader.ReadInt32();
		this.entityId = _reader.ReadInt32();
		this.entityClassId = _reader.ReadInt32();
		this.syncType = (NetPackageOwnedEntitySync.SyncType)_reader.ReadByte();
	}

	// Token: 0x06003CC6 RID: 15558 RVA: 0x001842C0 File Offset: 0x001824C0
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.ownerId);
		_writer.Write(this.entityId);
		_writer.Write(this.entityClassId);
		_writer.Write((byte)this.syncType);
	}

	// Token: 0x06003CC7 RID: 15559 RVA: 0x001842FC File Offset: 0x001824FC
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

	// Token: 0x1700067A RID: 1658
	// (get) Token: 0x06003CC8 RID: 15560 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003163 RID: 12643
	public int ownerId;

	// Token: 0x04003164 RID: 12644
	public int entityId;

	// Token: 0x04003165 RID: 12645
	public int entityClassId;

	// Token: 0x04003166 RID: 12646
	public NetPackageOwnedEntitySync.SyncType syncType;

	// Token: 0x02000817 RID: 2071
	public enum SyncType : byte
	{
		// Token: 0x04003168 RID: 12648
		Remove,
		// Token: 0x04003169 RID: 12649
		Add
	}
}
