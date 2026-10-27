using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007F5 RID: 2037
[Preserve]
public class NetPackageEntitySpawn : NetPackageEntityTargeted
{
	// Token: 0x17000662 RID: 1634
	// (get) Token: 0x06003C01 RID: 15361 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C02 RID: 15362 RVA: 0x001819B5 File Offset: 0x0017FBB5
	public NetPackageEntitySpawn Setup(EntityCreationData _es)
	{
		base.Setup(_es.id);
		this.es = _es;
		return this;
	}

	// Token: 0x06003C03 RID: 15363 RVA: 0x001819CB File Offset: 0x0017FBCB
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.es = new EntityCreationData();
		this.es.read(_reader, StreamModeRead.FromServer);
	}

	// Token: 0x06003C04 RID: 15364 RVA: 0x001819EC File Offset: 0x0017FBEC
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.es.write(_writer, StreamModeWrite.ToClient);
	}

	// Token: 0x06003C05 RID: 15365 RVA: 0x00181A04 File Offset: 0x0017FC04
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

	// Token: 0x040030CD RID: 12493
	public EntityCreationData es;
}
