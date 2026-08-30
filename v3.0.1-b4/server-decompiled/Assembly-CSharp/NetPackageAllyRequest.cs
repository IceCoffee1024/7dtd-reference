using System;
using UnityEngine.Scripting;

// Token: 0x020008E0 RID: 2272
[Preserve]
public class NetPackageAllyRequest : NetPackage
{
	// Token: 0x170006E3 RID: 1763
	// (get) Token: 0x060041D5 RID: 16853 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x060041D6 RID: 16854 RVA: 0x0019B7A7 File Offset: 0x001999A7
	public NetPackageAllyRequest Setup(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target, bool _addAlly)
	{
		this.source = _source;
		this.target = _target;
		this.addAlly = _addAlly;
		return this;
	}

	// Token: 0x060041D7 RID: 16855 RVA: 0x0019B7BF File Offset: 0x001999BF
	public override void read(PooledBinaryReader _br)
	{
		this.source = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.target = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.addAlly = _br.ReadBoolean();
	}

	// Token: 0x060041D8 RID: 16856 RVA: 0x0019B7E9 File Offset: 0x001999E9
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.source.ToStream(_bw, false);
		this.target.ToStream(_bw, false);
		_bw.Write(this.addAlly);
	}

	// Token: 0x060041D9 RID: 16857 RVA: 0x0019B818 File Offset: 0x00199A18
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.persistentPlayers.Allies.ProcessAllyRequest(this.source, this.target, this.addAlly);
	}

	// Token: 0x060041DA RID: 16858 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x0400352C RID: 13612
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs source;

	// Token: 0x0400352D RID: 13613
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs target;

	// Token: 0x0400352E RID: 13614
	[PublicizedFrom(EAccessModifier.Private)]
	public bool addAlly;
}
