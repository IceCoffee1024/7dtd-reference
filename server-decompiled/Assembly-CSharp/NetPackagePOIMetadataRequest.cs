using System;
using UnityEngine.Scripting;

// Token: 0x0200083E RID: 2110
[Preserve]
public class NetPackagePOIMetadataRequest : NetPackage
{
	// Token: 0x17000694 RID: 1684
	// (get) Token: 0x06003D7E RID: 15742 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D7F RID: 15743 RVA: 0x0012B879 File Offset: 0x00129A79
	public NetPackagePOIMetadataRequest Setup()
	{
		return this;
	}

	// Token: 0x06003D80 RID: 15744 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _br)
	{
	}

	// Token: 0x06003D81 RID: 15745 RVA: 0x0017D267 File Offset: 0x0017B467
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
	}

	// Token: 0x06003D82 RID: 15746 RVA: 0x0018778F File Offset: 0x0018598F
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.GetDynamicPrefabDecorator().SendPOIMetadataToClient(base.Sender);
	}
}
