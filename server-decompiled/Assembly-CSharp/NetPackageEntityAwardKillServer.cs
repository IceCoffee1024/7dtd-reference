using System;
using UnityEngine.Scripting;

// Token: 0x020007E8 RID: 2024
[Preserve]
public class NetPackageEntityAwardKillServer : NetPackage
{
	// Token: 0x1700065A RID: 1626
	// (get) Token: 0x06003BBA RID: 15290 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003BBB RID: 15291 RVA: 0x001807C4 File Offset: 0x0017E9C4
	public NetPackageEntityAwardKillServer Setup(int _killerEntityId, int _killedEntityId)
	{
		this.EntityId = _killerEntityId;
		this.KilledEntityId = _killedEntityId;
		return this;
	}

	// Token: 0x06003BBC RID: 15292 RVA: 0x001807D5 File Offset: 0x0017E9D5
	public override void read(PooledBinaryReader _reader)
	{
		this.EntityId = _reader.ReadInt32();
		this.KilledEntityId = _reader.ReadInt32();
	}

	// Token: 0x06003BBD RID: 15293 RVA: 0x001807EF File Offset: 0x0017E9EF
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.EntityId);
		_writer.Write(this.KilledEntityId);
	}

	// Token: 0x06003BBE RID: 15294 RVA: 0x00180810 File Offset: 0x0017EA10
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityPlayerLocal entityPlayerLocal = _world.GetEntity(this.EntityId) as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			EntityAlive entityAlive = _world.GetEntity(this.KilledEntityId) as EntityAlive;
			if (entityAlive != null)
			{
				QuestEventManager.Current.EntityKilled(entityPlayerLocal, entityAlive);
			}
		}
	}

	// Token: 0x0400309C RID: 12444
	public int EntityId;

	// Token: 0x0400309D RID: 12445
	public int KilledEntityId;
}
