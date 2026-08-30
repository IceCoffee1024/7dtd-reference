using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x020007AB RID: 1963
[Preserve]
public class NetPackageAudioPlayInHead : NetPackage
{
	// Token: 0x17000607 RID: 1543
	// (get) Token: 0x06003A5F RID: 14943 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003A60 RID: 14944 RVA: 0x0017BBC9 File Offset: 0x00179DC9
	public NetPackageAudioPlayInHead Setup(string _soundName, bool _isUnique)
	{
		this.soundName = _soundName;
		this.isUnique = _isUnique;
		return this;
	}

	// Token: 0x06003A61 RID: 14945 RVA: 0x0017BBDA File Offset: 0x00179DDA
	public override void read(PooledBinaryReader _br)
	{
		this.soundName = _br.ReadString();
		this.isUnique = _br.ReadBoolean();
	}

	// Token: 0x06003A62 RID: 14946 RVA: 0x0017BBF4 File Offset: 0x00179DF4
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.soundName);
		_bw.Write(this.isUnique);
	}

	// Token: 0x06003A63 RID: 14947 RVA: 0x0017BC15 File Offset: 0x00179E15
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		Manager.PlayInsidePlayerHead(this.soundName, -1, 0f, false, this.isUnique);
	}

	// Token: 0x06003A64 RID: 14948 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04002FCF RID: 12239
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundName;

	// Token: 0x04002FD0 RID: 12240
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isUnique;
}
