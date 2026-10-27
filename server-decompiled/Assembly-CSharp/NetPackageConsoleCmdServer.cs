using System;
using UnityEngine.Scripting;

// Token: 0x020007CC RID: 1996
[Preserve]
public class NetPackageConsoleCmdServer : NetPackage
{
	// Token: 0x06003B2E RID: 15150 RVA: 0x0017E6BF File Offset: 0x0017C8BF
	public NetPackageConsoleCmdServer Setup(string _cmd)
	{
		this.cmd = _cmd;
		return this;
	}

	// Token: 0x06003B2F RID: 15151 RVA: 0x0017E6C9 File Offset: 0x0017C8C9
	public override void read(PooledBinaryReader _br)
	{
		this.cmd = _br.ReadString();
	}

	// Token: 0x06003B30 RID: 15152 RVA: 0x0017E6D7 File Offset: 0x0017C8D7
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.cmd);
	}

	// Token: 0x06003B31 RID: 15153 RVA: 0x0017E6EC File Offset: 0x0017C8EC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.ServerConsoleCommand(base.Sender, this.cmd);
	}

	// Token: 0x17000647 RID: 1607
	// (get) Token: 0x06003B32 RID: 15154 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x04003024 RID: 12324
	[PublicizedFrom(EAccessModifier.Private)]
	public string cmd;
}
