using System;
using UnityEngine.Scripting;

// Token: 0x020007C4 RID: 1988
[Preserve]
public class NetPackageConsoleCmdServer : NetPackage
{
	// Token: 0x06003B17 RID: 15127 RVA: 0x0017D727 File Offset: 0x0017B927
	public NetPackageConsoleCmdServer Setup(string _cmd)
	{
		this.cmd = _cmd;
		return this;
	}

	// Token: 0x06003B18 RID: 15128 RVA: 0x0017D731 File Offset: 0x0017B931
	public override void read(PooledBinaryReader _br)
	{
		this.cmd = _br.ReadString();
	}

	// Token: 0x06003B19 RID: 15129 RVA: 0x0017D73F File Offset: 0x0017B93F
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.cmd);
	}

	// Token: 0x06003B1A RID: 15130 RVA: 0x0017D754 File Offset: 0x0017B954
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.ServerConsoleCommand(base.Sender, this.cmd);
	}

	// Token: 0x17000643 RID: 1603
	// (get) Token: 0x06003B1B RID: 15131 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x04003008 RID: 12296
	[PublicizedFrom(EAccessModifier.Private)]
	public string cmd;
}
