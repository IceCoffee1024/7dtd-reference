using System;
using UnityEngine.Scripting;

// Token: 0x02000801 RID: 2049
[Preserve]
public class NetPackageInventoryKeepOpen : NetPackage
{
	// Token: 0x17000647 RID: 1607
	// (get) Token: 0x06003C67 RID: 15463 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C68 RID: 15464 RVA: 0x0012A165 File Offset: 0x00128365
	public NetPackageInventoryKeepOpen Setup()
	{
		return this;
	}

	// Token: 0x06003C69 RID: 15465 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _br)
	{
	}

	// Token: 0x06003C6A RID: 15466 RVA: 0x00182518 File Offset: 0x00180718
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		LockManager.Instance.ProcessKeepOpen(base.Sender.entityId);
	}

	// Token: 0x06003C6B RID: 15467 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}
}
