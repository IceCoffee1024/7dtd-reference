using System;
using UnityEngine.Scripting;

// Token: 0x020007F1 RID: 2033
[Preserve]
public class NetPackageEntityTeleport : NetPackageEntityPosAndRot
{
	// Token: 0x06003BFF RID: 15359 RVA: 0x00180F49 File Offset: 0x0017F149
	public new NetPackageEntityTeleport Setup(Entity _entity)
	{
		base.Setup(_entity);
		return this;
	}

	// Token: 0x06003C00 RID: 15360 RVA: 0x00180F54 File Offset: 0x0017F154
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
}
