using System;
using UnityEngine.Scripting;

// Token: 0x0200085A RID: 2138
[Preserve]
public class NetPackageSignDataRequest : NetPackage
{
	// Token: 0x1700069E RID: 1694
	// (get) Token: 0x06003E09 RID: 15881 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003E0A RID: 15882 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003E0B RID: 15883 RVA: 0x0017D267 File Offset: 0x0017B467
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
	}

	// Token: 0x06003E0C RID: 15884 RVA: 0x0018ABFC File Offset: 0x00188DFC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SignDataManager.Instance.SendSignDataToClient(base.Sender);
	}
}
