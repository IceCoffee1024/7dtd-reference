using System;
using UnityEngine.Scripting;

// Token: 0x020007D4 RID: 2004
[Preserve]
public class NetPackageEntityAddExpServer : NetPackageEntityAddExpClient
{
	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x06003B58 RID: 15192 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003B59 RID: 15193 RVA: 0x0017EAFC File Offset: 0x0017CCFC
	public NetPackageEntityAddExpServer Setup(int _entityId, int _experience)
	{
		base.Setup(_entityId, _experience, Progression.XPTypes.Other, null);
		return this;
	}

	// Token: 0x06003B5A RID: 15194 RVA: 0x0017EB0C File Offset: 0x0017CD0C
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
