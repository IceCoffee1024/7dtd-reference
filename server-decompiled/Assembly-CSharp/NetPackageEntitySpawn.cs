using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007DD RID: 2013
[Preserve]
public class NetPackageEntitySpawn : NetPackageEntityTargeted
{
	// Token: 0x1700062C RID: 1580
	// (get) Token: 0x06003B75 RID: 15221 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B76 RID: 15222 RVA: 0x0017DFA1 File Offset: 0x0017C1A1
	public NetPackageEntitySpawn Setup(EntityCreationData _es)
	{
		base.Setup(_es.id);
		this.es = _es;
		return this;
	}

	// Token: 0x06003B77 RID: 15223 RVA: 0x0017DFB7 File Offset: 0x0017C1B7
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.es = new EntityCreationData();
		this.es.read(_reader, true);
	}

	// Token: 0x06003B78 RID: 15224 RVA: 0x0017DFD8 File Offset: 0x0017C1D8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.es.write(_writer, true);
	}

	// Token: 0x06003B79 RID: 15225 RVA: 0x0017DFF0 File Offset: 0x0017C1F0
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

	// Token: 0x06003B7A RID: 15226 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x04003035 RID: 12341
	public EntityCreationData es;
}
