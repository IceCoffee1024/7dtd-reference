using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x020007B6 RID: 1974
[Preserve]
public class NetPackageAudioPlayInHead : NetPackage
{
	// Token: 0x17000630 RID: 1584
	// (get) Token: 0x06003AB2 RID: 15026 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003AB3 RID: 15027 RVA: 0x0017D1FD File Offset: 0x0017B3FD
	public NetPackageAudioPlayInHead Setup(string _soundName, bool _isUnique)
	{
		this.soundName = _soundName;
		this.isUnique = _isUnique;
		return this;
	}

	// Token: 0x06003AB4 RID: 15028 RVA: 0x0017D20E File Offset: 0x0017B40E
	public override void read(PooledBinaryReader _br)
	{
		this.soundName = _br.ReadString();
		this.isUnique = _br.ReadBoolean();
	}

	// Token: 0x06003AB5 RID: 15029 RVA: 0x0017D228 File Offset: 0x0017B428
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.soundName);
		_bw.Write(this.isUnique);
	}

	// Token: 0x06003AB6 RID: 15030 RVA: 0x0017D249 File Offset: 0x0017B449
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		Manager.PlayInsidePlayerHead(this.soundName, -1, 0f, false, this.isUnique);
	}

	// Token: 0x04002FE5 RID: 12261
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundName;

	// Token: 0x04002FE6 RID: 12262
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isUnique;
}
