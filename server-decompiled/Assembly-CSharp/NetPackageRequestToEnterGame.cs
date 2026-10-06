using System;
using UnityEngine.Scripting;

// Token: 0x02000842 RID: 2114
[Preserve]
public class NetPackageRequestToEnterGame : NetPackage
{
	// Token: 0x17000694 RID: 1684
	// (get) Token: 0x06003DA1 RID: 15777 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003DA2 RID: 15778 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003DA3 RID: 15779 RVA: 0x0017C2CF File Offset: 0x0017A4CF
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
	}

	// Token: 0x06003DA4 RID: 15780 RVA: 0x00188A8D File Offset: 0x00186C8D
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		ThreadManager.StartCoroutine(_callbacks.RequestToEnterGame(base.Sender));
	}
}
