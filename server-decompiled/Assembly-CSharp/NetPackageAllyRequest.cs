using System;
using UnityEngine.Scripting;

// Token: 0x020008FB RID: 2299
[Preserve]
public class NetPackageAllyRequest : NetPackage
{
	// Token: 0x17000714 RID: 1812
	// (get) Token: 0x060041F3 RID: 16883 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x060041F4 RID: 16884 RVA: 0x0019EA3B File Offset: 0x0019CC3B
	public NetPackageAllyRequest Setup(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target, bool _addAlly)
	{
		this.source = _source;
		this.target = _target;
		this.addAlly = _addAlly;
		return this;
	}

	// Token: 0x060041F5 RID: 16885 RVA: 0x0019EA53 File Offset: 0x0019CC53
	public override void read(PooledBinaryReader _br)
	{
		this.source = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.target = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.addAlly = _br.ReadBoolean();
	}

	// Token: 0x060041F6 RID: 16886 RVA: 0x0019EA7D File Offset: 0x0019CC7D
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.source.ToStream(_bw, false);
		this.target.ToStream(_bw, false);
		_bw.Write(this.addAlly);
	}

	// Token: 0x060041F7 RID: 16887 RVA: 0x0019EAAC File Offset: 0x0019CCAC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.persistentPlayers.Allies.ProcessAllyRequest(this.source, this.target, this.addAlly);
	}

	// Token: 0x040035C1 RID: 13761
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs source;

	// Token: 0x040035C2 RID: 13762
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs target;

	// Token: 0x040035C3 RID: 13763
	[PublicizedFrom(EAccessModifier.Private)]
	public bool addAlly;
}
