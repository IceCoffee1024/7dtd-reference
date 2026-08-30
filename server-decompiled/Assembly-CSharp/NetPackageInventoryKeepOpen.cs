using System;
using UnityEngine.Scripting;

// Token: 0x020007F6 RID: 2038
[Preserve]
public class NetPackageInventoryKeepOpen : NetPackage
{
	// Token: 0x1700063C RID: 1596
	// (get) Token: 0x06003C08 RID: 15368 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C09 RID: 15369 RVA: 0x001290F5 File Offset: 0x001272F5
	public NetPackageInventoryKeepOpen Setup()
	{
		return this;
	}

	// Token: 0x06003C0A RID: 15370 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _br)
	{
	}

	// Token: 0x06003C0B RID: 15371 RVA: 0x00180140 File Offset: 0x0017E340
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		LockManager.Instance.ProcessKeepOpen(base.Sender.entityId);
	}

	// Token: 0x06003C0C RID: 15372 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}
}
