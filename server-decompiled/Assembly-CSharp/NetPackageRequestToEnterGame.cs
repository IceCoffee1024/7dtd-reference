using System;
using UnityEngine.Scripting;

// Token: 0x0200084C RID: 2124
[Preserve]
public class NetPackageRequestToEnterGame : NetPackage
{
	// Token: 0x1700069A RID: 1690
	// (get) Token: 0x06003DC4 RID: 15812 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003DC5 RID: 15813 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003DC6 RID: 15814 RVA: 0x0017D267 File Offset: 0x0017B467
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
	}

	// Token: 0x06003DC7 RID: 15815 RVA: 0x00189B91 File Offset: 0x00187D91
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		ThreadManager.StartCoroutine(_callbacks.RequestToEnterGame(base.Sender));
	}
}
