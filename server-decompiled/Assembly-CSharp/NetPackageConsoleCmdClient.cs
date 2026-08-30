using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007B5 RID: 1973
[Preserve]
public class NetPackageConsoleCmdClient : NetPackage
{
	// Token: 0x06003A87 RID: 14983 RVA: 0x0017ACCD File Offset: 0x00178ECD
	public NetPackageConsoleCmdClient Setup(List<string> _lines, bool _bExecute)
	{
		this.lines = _lines;
		this.bExecute = _bExecute;
		return this;
	}

	// Token: 0x06003A88 RID: 14984 RVA: 0x0017ACDE File Offset: 0x00178EDE
	public NetPackageConsoleCmdClient Setup(string _line, bool _bExecute)
	{
		this.lines = new List<string> { _line };
		this.bExecute = _bExecute;
		return this;
	}

	// Token: 0x06003A89 RID: 14985 RVA: 0x0017ACFC File Offset: 0x00178EFC
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

	// Token: 0x06003A8A RID: 14986 RVA: 0x0017AD48 File Offset: 0x00178F48
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

	// Token: 0x06003A8B RID: 14987 RVA: 0x0017ADA1 File Offset: 0x00178FA1
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

	// Token: 0x17000612 RID: 1554
	// (get) Token: 0x06003A8C RID: 14988 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003A8D RID: 14989 RVA: 0x0017A89E File Offset: 0x00178A9E
	public override int GetLength()
	{
		return 40;
	}

	// Token: 0x04002F91 RID: 12177
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> lines;

	// Token: 0x04002F92 RID: 12178
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bExecute;
}
