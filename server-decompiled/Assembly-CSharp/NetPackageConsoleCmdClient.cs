using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007C0 RID: 1984
[Preserve]
public class NetPackageConsoleCmdClient : NetPackage
{
	// Token: 0x06003AE6 RID: 15078 RVA: 0x0017D003 File Offset: 0x0017B203
	public NetPackageConsoleCmdClient Setup(List<string> _lines, bool _bExecute)
	{
		this.lines = _lines;
		this.bExecute = _bExecute;
		return this;
	}

	// Token: 0x06003AE7 RID: 15079 RVA: 0x0017D014 File Offset: 0x0017B214
	public NetPackageConsoleCmdClient Setup(string _line, bool _bExecute)
	{
		this.lines = new List<string> { _line };
		this.bExecute = _bExecute;
		return this;
	}

	// Token: 0x06003AE8 RID: 15080 RVA: 0x0017D030 File Offset: 0x0017B230
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

	// Token: 0x06003AE9 RID: 15081 RVA: 0x0017D07C File Offset: 0x0017B27C
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

	// Token: 0x06003AEA RID: 15082 RVA: 0x0017D0D5 File Offset: 0x0017B2D5
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

	// Token: 0x1700061D RID: 1565
	// (get) Token: 0x06003AEB RID: 15083 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003AEC RID: 15084 RVA: 0x0017CB1E File Offset: 0x0017AD1E
	public override int GetLength()
	{
		return 40;
	}

	// Token: 0x0400300C RID: 12300
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> lines;

	// Token: 0x0400300D RID: 12301
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bExecute;
}
