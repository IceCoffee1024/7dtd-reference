using System;
using UnityEngine.Scripting;

// Token: 0x02000850 RID: 2128
[Preserve]
public class NetPackageSignDataRequest : NetPackage
{
	// Token: 0x17000698 RID: 1688
	// (get) Token: 0x06003DE6 RID: 15846 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003DE7 RID: 15847 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003DE8 RID: 15848 RVA: 0x0017C2CF File Offset: 0x0017A4CF
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
	}

	// Token: 0x06003DE9 RID: 15849 RVA: 0x00189B54 File Offset: 0x00187D54
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SignDataManager.Instance.SendSignDataToClient(base.Sender);
	}
}
