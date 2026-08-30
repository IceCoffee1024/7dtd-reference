using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007E8 RID: 2024
[Preserve]
public class NetPackageEntitySpawn : NetPackageEntityTargeted
{
	// Token: 0x17000637 RID: 1591
	// (get) Token: 0x06003BD4 RID: 15316 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003BD5 RID: 15317 RVA: 0x00180379 File Offset: 0x0017E579
	public NetPackageEntitySpawn Setup(EntityCreationData _es)
	{
		base.Setup(_es.id);
		this.es = _es;
		return this;
	}

	// Token: 0x06003BD6 RID: 15318 RVA: 0x0018038F File Offset: 0x0017E58F
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.es = new EntityCreationData();
		this.es.read(_reader, true);
	}

	// Token: 0x06003BD7 RID: 15319 RVA: 0x001803B0 File Offset: 0x0017E5B0
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.es.write(_writer, true);
	}

	// Token: 0x06003BD8 RID: 15320 RVA: 0x001803C8 File Offset: 0x0017E5C8
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.es.clientEntityId != 0)
		{
			List<EntityPlayerLocal> localPlayers = _world.GetLocalPlayers();
			for (int i = 0; i < localPlayers.Count; i++)
			{
				if (localPlayers[i].entityId == this.es.belongsPlayerId)
				{
					_world.ChangeClientEntityIdToServer(this.es.clientEntityId, this.es.id);
					return;
				}
			}
		}
		_world.entityAsyncManager.StartCreateEntity(this.es, delegate(EntityAsyncManager.EntityCreateHandle op)
		{
			_world.SpawnEntityInWorld(op.Entity);
		});
	}

	// Token: 0x06003BD9 RID: 15321 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x040030B4 RID: 12468
	public EntityCreationData es;
}
