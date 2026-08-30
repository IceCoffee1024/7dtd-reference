using System;
using UnityEngine.Scripting;

// Token: 0x020007B6 RID: 1974
[Preserve]
public class NetPackageConsoleCmdServer : NetPackage
{
	// Token: 0x06003A8F RID: 14991 RVA: 0x0017ADD7 File Offset: 0x00178FD7
	public NetPackageConsoleCmdServer Setup(string _cmd)
	{
		this.cmd = _cmd;
		return this;
	}

	// Token: 0x06003A90 RID: 14992 RVA: 0x0017ADE1 File Offset: 0x00178FE1
	public override void read(PooledBinaryReader _br)
	{
		this.cmd = _br.ReadString();
	}

	// Token: 0x06003A91 RID: 14993 RVA: 0x0017ADEF File Offset: 0x00178FEF
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.cmd);
	}

	// Token: 0x06003A92 RID: 14994 RVA: 0x0017AE04 File Offset: 0x00179004
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.ServerConsoleCommand(base.Sender, this.cmd);
	}

	// Token: 0x17000613 RID: 1555
	// (get) Token: 0x06003A93 RID: 14995 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003A94 RID: 14996 RVA: 0x00104CB5 File Offset: 0x00102EB5
	public override int GetLength()
	{
		return 30;
	}

	// Token: 0x04002F93 RID: 12179
	[PublicizedFrom(EAccessModifier.Private)]
	public string cmd;
}
