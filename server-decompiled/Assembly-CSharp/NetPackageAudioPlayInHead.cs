using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x020007AE RID: 1966
[Preserve]
public class NetPackageAudioPlayInHead : NetPackage
{
	// Token: 0x1700062C RID: 1580
	// (get) Token: 0x06003A9B RID: 15003 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003A9C RID: 15004 RVA: 0x0017C265 File Offset: 0x0017A465
	public NetPackageAudioPlayInHead Setup(string _soundName, bool _isUnique)
	{
		this.soundName = _soundName;
		this.isUnique = _isUnique;
		return this;
	}

	// Token: 0x06003A9D RID: 15005 RVA: 0x0017C276 File Offset: 0x0017A476
	public override void read(PooledBinaryReader _br)
	{
		this.soundName = _br.ReadString();
		this.isUnique = _br.ReadBoolean();
	}

	// Token: 0x06003A9E RID: 15006 RVA: 0x0017C290 File Offset: 0x0017A490
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.soundName);
		_bw.Write(this.isUnique);
	}

	// Token: 0x06003A9F RID: 15007 RVA: 0x0017C2B1 File Offset: 0x0017A4B1
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		Manager.PlayInsidePlayerHead(this.soundName, -1, 0f, false, this.isUnique);
	}

	// Token: 0x04002FC9 RID: 12233
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundName;

	// Token: 0x04002FCA RID: 12234
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isUnique;
}
