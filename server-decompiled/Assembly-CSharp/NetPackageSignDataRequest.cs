using System;
using UnityEngine.Scripting;

// Token: 0x0200084F RID: 2127
[Preserve]
public class NetPackageSignDataRequest : NetPackage
{
	// Token: 0x17000679 RID: 1657
	// (get) Token: 0x06003E41 RID: 15937 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003E42 RID: 15938 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003E43 RID: 15939 RVA: 0x0017BC33 File Offset: 0x00179E33
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
	}

	// Token: 0x06003E44 RID: 15940 RVA: 0x00189FB4 File Offset: 0x001881B4
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SignDataManager.Instance.SendSignDataToClient(base.Sender);
	}

	// Token: 0x06003E45 RID: 15941 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}
}
