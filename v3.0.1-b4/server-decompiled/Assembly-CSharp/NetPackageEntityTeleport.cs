using System;
using UnityEngine.Scripting;

// Token: 0x020007E3 RID: 2019
[Preserve]
public class NetPackageEntityTeleport : NetPackageEntityPosAndRot
{
	// Token: 0x06003B9A RID: 15258 RVA: 0x0017E639 File Offset: 0x0017C839
	public new NetPackageEntityTeleport Setup(Entity _entity)
	{
		base.Setup(_entity);
		return this;
	}

	// Token: 0x06003B9B RID: 15259 RVA: 0x0017E644 File Offset: 0x0017C844
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!base.ValidEntityIdForSender(this.entityId, true))
		{
			return;
		}
		Entity entity = _world.GetEntity(this.entityId);
		if (entity != null)
		{
			entity.serverPos = NetEntityDistributionEntry.EncodePos(this.pos);
			entity.SetPosAndRotFromNetwork(this.pos, this.rot, 0);
			entity.SetPosition(this.pos, true);
			entity.SetRotation(this.rot);
			entity.SetLastTickPos(this.pos);
			entity.onGround = this.onGround;
			return;
		}
		Log.Out("Discarding " + base.GetType().Name + " for entity Id=" + this.entityId.ToString());
	}

	// Token: 0x06003B9C RID: 15260 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}
}
