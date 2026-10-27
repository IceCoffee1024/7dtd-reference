using System;
using UnityEngine.Scripting;

// Token: 0x020008FC RID: 2300
[Preserve]
public class NetPackageAllyResponse : NetPackage
{
	// Token: 0x17000715 RID: 1813
	// (get) Token: 0x060041F9 RID: 16889 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x060041FA RID: 16890 RVA: 0x0019EAD4 File Offset: 0x0019CCD4
	public NetPackageAllyResponse Setup(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target, AllyStore.AllyStatus _newStatus, AllyStore.AllyEvent _allyEventSource, AllyStore.AllyEvent _allyEventTarget)
	{
		this.source = _source;
		this.target = _target;
		this.newStatus = _newStatus;
		this.allyEventSource = _allyEventSource;
		this.allyEventTarget = _allyEventTarget;
		return this;
	}

	// Token: 0x060041FB RID: 16891 RVA: 0x0019EAFC File Offset: 0x0019CCFC
	public override void read(PooledBinaryReader _br)
	{
		this.source = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.target = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.newStatus = (AllyStore.AllyStatus)_br.ReadByte();
		this.allyEventSource = (AllyStore.AllyEvent)_br.ReadByte();
		this.allyEventTarget = (AllyStore.AllyEvent)_br.ReadByte();
	}

	// Token: 0x060041FC RID: 16892 RVA: 0x0019EB4C File Offset: 0x0019CD4C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.source.ToStream(_bw, false);
		this.target.ToStream(_bw, false);
		_bw.Write((byte)this.newStatus);
		_bw.Write((byte)this.allyEventSource);
		_bw.Write((byte)this.allyEventTarget);
	}

	// Token: 0x060041FD RID: 16893 RVA: 0x0019EB9E File Offset: 0x0019CD9E
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.persistentPlayers.Allies.AllyUpdateResponse(this.source, this.target, this.newStatus, this.allyEventSource, this.allyEventTarget);
	}

	// Token: 0x040035C4 RID: 13764
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs source;

	// Token: 0x040035C5 RID: 13765
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs target;

	// Token: 0x040035C6 RID: 13766
	[PublicizedFrom(EAccessModifier.Private)]
	public AllyStore.AllyStatus newStatus;

	// Token: 0x040035C7 RID: 13767
	[PublicizedFrom(EAccessModifier.Private)]
	public AllyStore.AllyEvent allyEventSource;

	// Token: 0x040035C8 RID: 13768
	[PublicizedFrom(EAccessModifier.Private)]
	public AllyStore.AllyEvent allyEventTarget;
}
