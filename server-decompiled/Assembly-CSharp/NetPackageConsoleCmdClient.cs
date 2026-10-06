using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007C3 RID: 1987
[Preserve]
public class NetPackageConsoleCmdClient : NetPackage
{
	// Token: 0x06003B10 RID: 15120 RVA: 0x0017D61F File Offset: 0x0017B81F
	public NetPackageConsoleCmdClient Setup(List<string> _lines, bool _bExecute)
	{
		this.lines = _lines;
		this.bExecute = _bExecute;
		return this;
	}

	// Token: 0x06003B11 RID: 15121 RVA: 0x0017D630 File Offset: 0x0017B830
	public NetPackageConsoleCmdClient Setup(string _line, bool _bExecute)
	{
		this.lines = new List<string> { _line };
		this.bExecute = _bExecute;
		return this;
	}

	// Token: 0x06003B12 RID: 15122 RVA: 0x0017D64C File Offset: 0x0017B84C
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

	// Token: 0x06003B13 RID: 15123 RVA: 0x0017D698 File Offset: 0x0017B898
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

	// Token: 0x06003B14 RID: 15124 RVA: 0x0017D6F1 File Offset: 0x0017B8F1
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

	// Token: 0x17000642 RID: 1602
	// (get) Token: 0x06003B15 RID: 15125 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003006 RID: 12294
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> lines;

	// Token: 0x04003007 RID: 12295
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bExecute;
}
