using System;
using UnityEngine.Scripting;

// Token: 0x020007D7 RID: 2007
[Preserve]
public class NetPackageEntityAddExpServer : NetPackageEntityAddExpClient
{
	// Token: 0x1700064F RID: 1615
	// (get) Token: 0x06003B72 RID: 15218 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003B73 RID: 15219 RVA: 0x0017F07C File Offset: 0x0017D27C
	public NetPackageEntityAddExpServer Setup(int _entityId, int _experience)
	{
		base.Setup(_entityId, _experience, Progression.XPTypes.Other, null);
		return this;
	}

	// Token: 0x06003B74 RID: 15220 RVA: 0x0017F08C File Offset: 0x0017D28C
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
