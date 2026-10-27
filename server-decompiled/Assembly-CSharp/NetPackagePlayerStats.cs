using System;
using UnityEngine.Scripting;

// Token: 0x0200083B RID: 2107
[Preserve]
public class NetPackagePlayerStats : NetPackageEntityTargeted
{
	// Token: 0x06003D6E RID: 15726 RVA: 0x0018735C File Offset: 0x0018555C
	public NetPackagePlayerStats Setup(EntityAlive _entity)
	{
		base.Setup(_entity.entityId);
		this.entityNetworkStats.FillFromEntity(_entity);
		return this;
	}

	// Token: 0x06003D6F RID: 15727 RVA: 0x00187378 File Offset: 0x00185578
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackagePlayerStats()
	{
	}

	// Token: 0x06003D70 RID: 15728 RVA: 0x001873A0 File Offset: 0x001855A0
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.entityNetworkStats.read(_reader);
	}

	// Token: 0x06003D71 RID: 15729 RVA: 0x001873B5 File Offset: 0x001855B5
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.entityNetworkStats.write(_writer);
	}

	// Token: 0x06003D72 RID: 15730 RVA: 0x001873CC File Offset: 0x001855CC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityAlive entityAlive = _world.GetEntity(this.entityId) as EntityAlive;
		if (!entityAlive)
		{
			Log.Out("Discarding " + base.GetType().Name + " for entity Id=" + this.entityId.ToString());
			return;
		}
		if (!base.ValidEntityIdForSender(this.entityId, true))
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && entityAlive is EntityPlayer)
		{
			this.entityNetworkStats.SetName(base.Sender.playerName);
		}
		this.entityNetworkStats.ToEntity(entityAlive);
		entityAlive.EnqueueNetworkStats(this.entityNetworkStats);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePlayerStats>().Setup(entityAlive), false, -1, base.Sender.entityId, -1, null, 192, false);
		}
	}

	// Token: 0x040031E4 RID: 12772
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive.EntityNetworkStats entityNetworkStats = new EntityAlive.EntityNetworkStats();
}
