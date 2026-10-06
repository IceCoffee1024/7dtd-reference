using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007EB RID: 2027
[Preserve]
public class NetPackageEntitySpawn : NetPackageEntityTargeted
{
	// Token: 0x1700065C RID: 1628
	// (get) Token: 0x06003BDE RID: 15326 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003BDF RID: 15327 RVA: 0x001808B1 File Offset: 0x0017EAB1
	public NetPackageEntitySpawn Setup(EntityCreationData _es)
	{
		base.Setup(_es.id);
		this.es = _es;
		return this;
	}

	// Token: 0x06003BE0 RID: 15328 RVA: 0x001808C7 File Offset: 0x0017EAC7
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.es = new EntityCreationData();
		this.es.read(_reader, StreamModeRead.FromServer);
	}

	// Token: 0x06003BE1 RID: 15329 RVA: 0x001808E8 File Offset: 0x0017EAE8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.es.write(_writer, StreamModeWrite.ToClient);
	}

	// Token: 0x06003BE2 RID: 15330 RVA: 0x00180900 File Offset: 0x0017EB00
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

	// Token: 0x040030AD RID: 12461
	public EntityCreationData es;
}
