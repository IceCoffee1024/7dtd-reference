using System;
using UnityEngine.Scripting;

// Token: 0x020008F2 RID: 2290
[Preserve]
public class NetPackageAllyResponse : NetPackage
{
	// Token: 0x1700070F RID: 1807
	// (get) Token: 0x060041D6 RID: 16854 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x060041D7 RID: 16855 RVA: 0x0019DA2C File Offset: 0x0019BC2C
	public NetPackageAllyResponse Setup(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target, AllyStore.AllyStatus _newStatus, AllyStore.AllyEvent _allyEventSource, AllyStore.AllyEvent _allyEventTarget)
	{
		this.source = _source;
		this.target = _target;
		this.newStatus = _newStatus;
		this.allyEventSource = _allyEventSource;
		this.allyEventTarget = _allyEventTarget;
		return this;
	}

	// Token: 0x060041D8 RID: 16856 RVA: 0x0019DA54 File Offset: 0x0019BC54
	public override void read(PooledBinaryReader _br)
	{
		this.source = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.target = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.newStatus = (AllyStore.AllyStatus)_br.ReadByte();
		this.allyEventSource = (AllyStore.AllyEvent)_br.ReadByte();
		this.allyEventTarget = (AllyStore.AllyEvent)_br.ReadByte();
	}

	// Token: 0x060041D9 RID: 16857 RVA: 0x0019DAA4 File Offset: 0x0019BCA4
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.source.ToStream(_bw, false);
		this.target.ToStream(_bw, false);
		_bw.Write((byte)this.newStatus);
		_bw.Write((byte)this.allyEventSource);
		_bw.Write((byte)this.allyEventTarget);
	}

	// Token: 0x060041DA RID: 16858 RVA: 0x0019DAF6 File Offset: 0x0019BCF6
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.persistentPlayers.Allies.AllyUpdateResponse(this.source, this.target, this.newStatus, this.allyEventSource, this.allyEventTarget);
	}

	// Token: 0x040035A4 RID: 13732
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs source;

	// Token: 0x040035A5 RID: 13733
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs target;

	// Token: 0x040035A6 RID: 13734
	[PublicizedFrom(EAccessModifier.Private)]
	public AllyStore.AllyStatus newStatus;

	// Token: 0x040035A7 RID: 13735
	[PublicizedFrom(EAccessModifier.Private)]
	public AllyStore.AllyEvent allyEventSource;

	// Token: 0x040035A8 RID: 13736
	[PublicizedFrom(EAccessModifier.Private)]
	public AllyStore.AllyEvent allyEventTarget;
}
