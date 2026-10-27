using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007CB RID: 1995
[Preserve]
public class NetPackageConsoleCmdClient : NetPackage
{
	// Token: 0x06003B27 RID: 15143 RVA: 0x0017E5B7 File Offset: 0x0017C7B7
	public NetPackageConsoleCmdClient Setup(List<string> _lines, bool _bExecute)
	{
		this.lines = _lines;
		this.bExecute = _bExecute;
		return this;
	}

	// Token: 0x06003B28 RID: 15144 RVA: 0x0017E5C8 File Offset: 0x0017C7C8
	public NetPackageConsoleCmdClient Setup(string _line, bool _bExecute)
	{
		this.lines = new List<string> { _line };
		this.bExecute = _bExecute;
		return this;
	}

	// Token: 0x06003B29 RID: 15145 RVA: 0x0017E5E4 File Offset: 0x0017C7E4
	public override void read(PooledBinaryReader _br)
	{
		int num = _br.ReadInt32();
		this.lines = new List<string>(num);
		for (int i = 0; i < num; i++)
		{
			this.lines.Add(_br.ReadString());
		}
		this.bExecute = _br.ReadBoolean();
	}

	// Token: 0x06003B2A RID: 15146 RVA: 0x0017E630 File Offset: 0x0017C830
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.lines.Count);
		for (int i = 0; i < this.lines.Count; i++)
		{
			_bw.Write(this.lines[i]);
		}
		_bw.Write(this.bExecute);
	}

	// Token: 0x06003B2B RID: 15147 RVA: 0x0017E689 File Offset: 0x0017C889
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (this.bExecute)
		{
			GUIWindowConsole.AddLines(SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteSync(this.lines[0], null));
			return;
		}
		GUIWindowConsole.AddLines(this.lines);
	}

	// Token: 0x17000646 RID: 1606
	// (get) Token: 0x06003B2C RID: 15148 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003022 RID: 12322
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> lines;

	// Token: 0x04003023 RID: 12323
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bExecute;
}
