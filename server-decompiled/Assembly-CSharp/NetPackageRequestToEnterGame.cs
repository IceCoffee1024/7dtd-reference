using System;
using UnityEngine.Scripting;

// Token: 0x02000841 RID: 2113
[Preserve]
public class NetPackageRequestToEnterGame : NetPackage
{
	// Token: 0x17000675 RID: 1653
	// (get) Token: 0x06003DF1 RID: 15857 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003DF2 RID: 15858 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003DF3 RID: 15859 RVA: 0x0017BC33 File Offset: 0x00179E33
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
	}

	// Token: 0x06003DF4 RID: 15860 RVA: 0x00188F55 File Offset: 0x00187155
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		ThreadManager.StartCoroutine(_callbacks.RequestToEnterGame(base.Sender));
	}

	// Token: 0x06003DF5 RID: 15861 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}
}
