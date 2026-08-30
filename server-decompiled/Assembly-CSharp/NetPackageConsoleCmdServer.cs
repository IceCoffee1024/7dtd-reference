using System;
using UnityEngine.Scripting;

// Token: 0x020007C1 RID: 1985
[Preserve]
public class NetPackageConsoleCmdServer : NetPackage
{
	// Token: 0x06003AEE RID: 15086 RVA: 0x0017D10B File Offset: 0x0017B30B
	public NetPackageConsoleCmdServer Setup(string _cmd)
	{
		this.cmd = _cmd;
		return this;
	}

	// Token: 0x06003AEF RID: 15087 RVA: 0x0017D115 File Offset: 0x0017B315
	public override void read(PooledBinaryReader _br)
	{
		this.cmd = _br.ReadString();
	}

	// Token: 0x06003AF0 RID: 15088 RVA: 0x0017D123 File Offset: 0x0017B323
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.cmd);
	}

	// Token: 0x06003AF1 RID: 15089 RVA: 0x0017D138 File Offset: 0x0017B338
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.ServerConsoleCommand(base.Sender, this.cmd);
	}

	// Token: 0x1700061E RID: 1566
	// (get) Token: 0x06003AF2 RID: 15090 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003AF3 RID: 15091 RVA: 0x00105CC1 File Offset: 0x00103EC1
	public override int GetLength()
	{
		return 30;
	}

	// Token: 0x0400300E RID: 12302
	[PublicizedFrom(EAccessModifier.Private)]
	public string cmd;
}
