using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x020007A1 RID: 1953
[Preserve]
public class NetPackageAudioPlayInHead : NetPackage
{
	// Token: 0x170005FD RID: 1533
	// (get) Token: 0x06003A07 RID: 14855 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003A08 RID: 14856 RVA: 0x00179949 File Offset: 0x00177B49
	public NetPackageAudioPlayInHead Setup(string _soundName, bool _isUnique)
	{
		this.soundName = _soundName;
		this.isUnique = _isUnique;
		return this;
	}

	// Token: 0x06003A09 RID: 14857 RVA: 0x0017995A File Offset: 0x00177B5A
	public override void read(PooledBinaryReader _br)
	{
		this.soundName = _br.ReadString();
		this.isUnique = _br.ReadBoolean();
	}

	// Token: 0x06003A0A RID: 14858 RVA: 0x00179974 File Offset: 0x00177B74
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.soundName);
		_bw.Write(this.isUnique);
	}

	// Token: 0x06003A0B RID: 14859 RVA: 0x00179995 File Offset: 0x00177B95
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		Manager.PlayInsidePlayerHead(this.soundName, -1, 0f, false, this.isUnique);
	}

	// Token: 0x06003A0C RID: 14860 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04002F56 RID: 12118
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundName;

	// Token: 0x04002F57 RID: 12119
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isUnique;
}
