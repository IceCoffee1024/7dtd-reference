using System;
using UnityEngine.Scripting;

// Token: 0x02000830 RID: 2096
[Preserve]
public class NetPackagePlayerStats : NetPackageEntityTargeted
{
	// Token: 0x06003D8F RID: 15759 RVA: 0x00186720 File Offset: 0x00184920
	public NetPackagePlayerStats Setup(EntityAlive _entity)
	{
		base.Setup(_entity.entityId);
		this.entityNetworkStats.FillFromEntity(_entity);
		return this;
	}

	// Token: 0x06003D90 RID: 15760 RVA: 0x0018673C File Offset: 0x0018493C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackagePlayerStats()
	{
	}

	// Token: 0x06003D91 RID: 15761 RVA: 0x00186764 File Offset: 0x00184964
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.entityNetworkStats.read(_reader);
	}

	// Token: 0x06003D92 RID: 15762 RVA: 0x00186779 File Offset: 0x00184979
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.entityNetworkStats.write(_writer);
	}

	// Token: 0x06003D93 RID: 15763 RVA: 0x00186790 File Offset: 0x00184990
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

	// Token: 0x06003D94 RID: 15764 RVA: 0x00177A99 File Offset: 0x00175C99
	public override int GetLength()
	{
		return 60;
	}

	// Token: 0x040031DD RID: 12765
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive.EntityNetworkStats entityNetworkStats = new EntityAlive.EntityNetworkStats();
}
