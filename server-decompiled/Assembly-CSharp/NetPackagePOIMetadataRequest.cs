using System;
using UnityEngine.Scripting;

// Token: 0x02000834 RID: 2100
[Preserve]
public class NetPackagePOIMetadataRequest : NetPackage
{
	// Token: 0x1700068E RID: 1678
	// (get) Token: 0x06003D5B RID: 15707 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D5C RID: 15708 RVA: 0x0012AC85 File Offset: 0x00128E85
	public NetPackagePOIMetadataRequest Setup()
	{
		return this;
	}

	// Token: 0x06003D5D RID: 15709 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _br)
	{
	}

	// Token: 0x06003D5E RID: 15710 RVA: 0x0017C2CF File Offset: 0x0017A4CF
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
	}

	// Token: 0x06003D5F RID: 15711 RVA: 0x0018668B File Offset: 0x0018488B
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.GetDynamicPrefabDecorator().SendPOIMetadataToClient(base.Sender);
	}
}
