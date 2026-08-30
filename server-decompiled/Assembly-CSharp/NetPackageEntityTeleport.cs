using System;
using UnityEngine.Scripting;

// Token: 0x020007EE RID: 2030
[Preserve]
public class NetPackageEntityTeleport : NetPackageEntityPosAndRot
{
	// Token: 0x06003BF9 RID: 15353 RVA: 0x00180A11 File Offset: 0x0017EC11
	public new NetPackageEntityTeleport Setup(Entity _entity)
	{
		base.Setup(_entity);
		return this;
	}

	// Token: 0x06003BFA RID: 15354 RVA: 0x00180A1C File Offset: 0x0017EC1C
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

	// Token: 0x06003BFB RID: 15355 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}
}
