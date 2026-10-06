using System;
using UnityEngine.Scripting;

// Token: 0x02000802 RID: 2050
[Preserve]
public class NetPackageInventoryKeepOpen : NetPackage
{
	// Token: 0x1700066A RID: 1642
	// (get) Token: 0x06003C52 RID: 15442 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C53 RID: 15443 RVA: 0x0012AC85 File Offset: 0x00128E85
	public NetPackageInventoryKeepOpen Setup()
	{
		return this;
	}

	// Token: 0x06003C54 RID: 15444 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _br)
	{
	}

	// Token: 0x06003C55 RID: 15445 RVA: 0x00182748 File Offset: 0x00180948
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		LockManager.Instance.ProcessKeepOpen(base.Sender.entityId);
	}
}
