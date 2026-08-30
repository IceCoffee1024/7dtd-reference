using System;
using UnityEngine.Scripting;

// Token: 0x020008EC RID: 2284
[Preserve]
public class NetPackageAllyRequest : NetPackage
{
	// Token: 0x170006EF RID: 1775
	// (get) Token: 0x0600423A RID: 16954 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x0600423B RID: 16955 RVA: 0x0019D87F File Offset: 0x0019BA7F
	public NetPackageAllyRequest Setup(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target, bool _addAlly)
	{
		this.source = _source;
		this.target = _target;
		this.addAlly = _addAlly;
		return this;
	}

	// Token: 0x0600423C RID: 16956 RVA: 0x0019D897 File Offset: 0x0019BA97
	public override void read(PooledBinaryReader _br)
	{
		this.source = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.target = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.addAlly = _br.ReadBoolean();
	}

	// Token: 0x0600423D RID: 16957 RVA: 0x0019D8C1 File Offset: 0x0019BAC1
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.source.ToStream(_bw, false);
		this.target.ToStream(_bw, false);
		_bw.Write(this.addAlly);
	}

	// Token: 0x0600423E RID: 16958 RVA: 0x0019D8F0 File Offset: 0x0019BAF0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.persistentPlayers.Allies.ProcessAllyRequest(this.source, this.target, this.addAlly);
	}

	// Token: 0x0600423F RID: 16959 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x040035AD RID: 13741
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs source;

	// Token: 0x040035AE RID: 13742
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs target;

	// Token: 0x040035AF RID: 13743
	[PublicizedFrom(EAccessModifier.Private)]
	public bool addAlly;
}
