using System;
using UnityEngine.Scripting;

// Token: 0x020008F1 RID: 2289
[Preserve]
public class NetPackageAllyRequest : NetPackage
{
	// Token: 0x1700070E RID: 1806
	// (get) Token: 0x060041D0 RID: 16848 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x060041D1 RID: 16849 RVA: 0x0019D993 File Offset: 0x0019BB93
	public NetPackageAllyRequest Setup(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target, bool _addAlly)
	{
		this.source = _source;
		this.target = _target;
		this.addAlly = _addAlly;
		return this;
	}

	// Token: 0x060041D2 RID: 16850 RVA: 0x0019D9AB File Offset: 0x0019BBAB
	public override void read(PooledBinaryReader _br)
	{
		this.source = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.target = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.addAlly = _br.ReadBoolean();
	}

	// Token: 0x060041D3 RID: 16851 RVA: 0x0019D9D5 File Offset: 0x0019BBD5
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.source.ToStream(_bw, false);
		this.target.ToStream(_bw, false);
		_bw.Write(this.addAlly);
	}

	// Token: 0x060041D4 RID: 16852 RVA: 0x0019DA04 File Offset: 0x0019BC04
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.persistentPlayers.Allies.ProcessAllyRequest(this.source, this.target, this.addAlly);
	}

	// Token: 0x040035A1 RID: 13729
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs source;

	// Token: 0x040035A2 RID: 13730
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs target;

	// Token: 0x040035A3 RID: 13731
	[PublicizedFrom(EAccessModifier.Private)]
	public bool addAlly;
}
