using System;
using UnityEngine.Scripting;

// Token: 0x020007C9 RID: 1993
[Preserve]
public class NetPackageEntityAddExpServer : NetPackageEntityAddExpClient
{
	// Token: 0x1700061F RID: 1567
	// (get) Token: 0x06003AF9 RID: 15097 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003AFA RID: 15098 RVA: 0x0017C724 File Offset: 0x0017A924
	public NetPackageEntityAddExpServer Setup(int _entityId, int _experience)
	{
		base.Setup(_entityId, _experience, Progression.XPTypes.Other, null);
		return this;
	}

	// Token: 0x06003AFB RID: 15099 RVA: 0x0017C734 File Offset: 0x0017A934
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityPlayer entityPlayer = (EntityPlayer)_world.GetEntity(this.entityId);
		if (entityPlayer == null)
		{
			return;
		}
		if (entityPlayer.isEntityRemote)
		{
			entityPlayer.Progression.AddLevelExp(this.xp, "_xpOther", Progression.XPTypes.Other, true, true, -1, this.usedItem);
		}
	}
}
