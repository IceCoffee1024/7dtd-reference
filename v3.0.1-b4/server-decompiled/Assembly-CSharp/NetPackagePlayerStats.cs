using System;
using UnityEngine.Scripting;

// Token: 0x02000825 RID: 2085
[Preserve]
public class NetPackagePlayerStats : NetPackageEntityTargeted
{
	// Token: 0x06003D30 RID: 15664 RVA: 0x00184348 File Offset: 0x00182548
	public NetPackagePlayerStats Setup(EntityAlive _entity)
	{
		base.Setup(_entity.entityId);
		this.entityNetworkStats.FillFromEntity(_entity);
		return this;
	}

	// Token: 0x06003D31 RID: 15665 RVA: 0x00184364 File Offset: 0x00182564
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackagePlayerStats()
	{
	}

	// Token: 0x06003D32 RID: 15666 RVA: 0x0018438C File Offset: 0x0018258C
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.entityNetworkStats.read(_reader);
	}

	// Token: 0x06003D33 RID: 15667 RVA: 0x001843A1 File Offset: 0x001825A1
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.entityNetworkStats.write(_writer);
	}

	// Token: 0x06003D34 RID: 15668 RVA: 0x001843B8 File Offset: 0x001825B8
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

	// Token: 0x06003D35 RID: 15669 RVA: 0x00175819 File Offset: 0x00173A19
	public override int GetLength()
	{
		return 60;
	}

	// Token: 0x0400315E RID: 12638
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive.EntityNetworkStats entityNetworkStats = new EntityAlive.EntityNetworkStats();
}
