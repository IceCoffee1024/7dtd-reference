using System;
using UnityEngine.Scripting;

// Token: 0x020007FB RID: 2043
[Preserve]
public class NetPackageEntityTeleport : NetPackageEntityPosAndRot
{
	// Token: 0x06003C22 RID: 15394 RVA: 0x0018204D File Offset: 0x0018024D
	public new NetPackageEntityTeleport Setup(Entity _entity)
	{
		base.Setup(_entity);
		return this;
	}

	// Token: 0x06003C23 RID: 15395 RVA: 0x00182058 File Offset: 0x00180258
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
