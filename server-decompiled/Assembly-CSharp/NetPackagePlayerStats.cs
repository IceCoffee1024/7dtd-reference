using System;
using UnityEngine.Scripting;

// Token: 0x02000831 RID: 2097
[Preserve]
public class NetPackagePlayerStats : NetPackageEntityTargeted
{
	// Token: 0x06003D4B RID: 15691 RVA: 0x00186258 File Offset: 0x00184458
	public NetPackagePlayerStats Setup(EntityAlive _entity)
	{
		base.Setup(_entity.entityId);
		this.entityNetworkStats.FillFromEntity(_entity);
		return this;
	}

	// Token: 0x06003D4C RID: 15692 RVA: 0x00186274 File Offset: 0x00184474
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackagePlayerStats()
	{
	}

	// Token: 0x06003D4D RID: 15693 RVA: 0x0018629C File Offset: 0x0018449C
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.entityNetworkStats.read(_reader);
	}

	// Token: 0x06003D4E RID: 15694 RVA: 0x001862B1 File Offset: 0x001844B1
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.entityNetworkStats.write(_writer);
	}

	// Token: 0x06003D4F RID: 15695 RVA: 0x001862C8 File Offset: 0x001844C8
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

	// Token: 0x040031C4 RID: 12740
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive.EntityNetworkStats entityNetworkStats = new EntityAlive.EntityNetworkStats();
}
