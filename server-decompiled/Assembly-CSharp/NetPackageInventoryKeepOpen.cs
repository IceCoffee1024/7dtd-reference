using System;
using UnityEngine.Scripting;

// Token: 0x0200080C RID: 2060
[Preserve]
public class NetPackageInventoryKeepOpen : NetPackage
{
	// Token: 0x17000670 RID: 1648
	// (get) Token: 0x06003C75 RID: 15477 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C76 RID: 15478 RVA: 0x0012B879 File Offset: 0x00129A79
	public NetPackageInventoryKeepOpen Setup()
	{
		return this;
	}

	// Token: 0x06003C77 RID: 15479 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _br)
	{
	}

	// Token: 0x06003C78 RID: 15480 RVA: 0x0018384C File Offset: 0x00181A4C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		LockManager.Instance.ProcessKeepOpen(base.Sender.entityId);
	}
}
