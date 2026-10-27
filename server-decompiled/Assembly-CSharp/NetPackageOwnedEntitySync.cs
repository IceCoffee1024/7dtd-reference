using System;
using UnityEngine.Scripting;

// Token: 0x02000820 RID: 2080
[Preserve]
public class NetPackageOwnedEntitySync : NetPackage
{
	// Token: 0x06003CE7 RID: 15591 RVA: 0x00185372 File Offset: 0x00183572
	public NetPackageOwnedEntitySync Setup(int _ownerId, int _entityId, int _entityClassId, NetPackageOwnedEntitySync.SyncType _syncType)
	{
		this.ownerId = _ownerId;
		this.entityId = _entityId;
		this.entityClassId = _entityClassId;
		this.syncType = _syncType;
		return this;
	}

	// Token: 0x06003CE8 RID: 15592 RVA: 0x00185392 File Offset: 0x00183592
	public override void read(PooledBinaryReader _reader)
	{
		this.ownerId = _reader.ReadInt32();
		this.entityId = _reader.ReadInt32();
		this.entityClassId = _reader.ReadInt32();
		this.syncType = (NetPackageOwnedEntitySync.SyncType)_reader.ReadByte();
	}

	// Token: 0x06003CE9 RID: 15593 RVA: 0x001853C4 File Offset: 0x001835C4
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.ownerId);
		_writer.Write(this.entityId);
		_writer.Write(this.entityClassId);
		_writer.Write((byte)this.syncType);
	}

	// Token: 0x06003CEA RID: 15594 RVA: 0x00185400 File Offset: 0x00183600
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

	// Token: 0x17000680 RID: 1664
	// (get) Token: 0x06003CEB RID: 15595 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003183 RID: 12675
	public int ownerId;

	// Token: 0x04003184 RID: 12676
	public int entityId;

	// Token: 0x04003185 RID: 12677
	public int entityClassId;

	// Token: 0x04003186 RID: 12678
	public NetPackageOwnedEntitySync.SyncType syncType;

	// Token: 0x02000821 RID: 2081
	public enum SyncType : byte
	{
		// Token: 0x04003188 RID: 12680
		Remove,
		// Token: 0x04003189 RID: 12681
		Add
	}
}
