using System;
using UnityEngine.Scripting;

// Token: 0x02000833 RID: 2099
[Preserve]
public class NetPackagePOIMetadataRequest : NetPackage
{
	// Token: 0x1700066F RID: 1647
	// (get) Token: 0x06003DA2 RID: 15778 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003DA3 RID: 15779 RVA: 0x0012A165 File Offset: 0x00128365
	public NetPackagePOIMetadataRequest Setup()
	{
		return this;
	}

	// Token: 0x06003DA4 RID: 15780 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _br)
	{
	}

	// Token: 0x06003DA5 RID: 15781 RVA: 0x0017BC33 File Offset: 0x00179E33
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
	}

	// Token: 0x06003DA6 RID: 15782 RVA: 0x00186B53 File Offset: 0x00184D53
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.GetDynamicPrefabDecorator().SendPOIMetadataToClient(base.Sender);
	}

	// Token: 0x06003DA7 RID: 15783 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}
}
