using System;
using UnityEngine.Scripting;

// Token: 0x02000835 RID: 2101
[Preserve]
public class NetPackageRequestToEnterGame : NetPackage
{
	// Token: 0x17000669 RID: 1641
	// (get) Token: 0x06003D8C RID: 15756 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D8D RID: 15757 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003D8E RID: 15758 RVA: 0x001799B3 File Offset: 0x00177BB3
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
	}

	// Token: 0x06003D8F RID: 15759 RVA: 0x00186F35 File Offset: 0x00185135
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		ThreadManager.StartCoroutine(_callbacks.RequestToEnterGame(base.Sender));
	}

	// Token: 0x06003D90 RID: 15760 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}
}
