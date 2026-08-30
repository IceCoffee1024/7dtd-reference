using System;
using UnityEngine.Scripting;

// Token: 0x020008E1 RID: 2273
[Preserve]
public class NetPackageAllyResponse : NetPackage
{
	// Token: 0x170006E4 RID: 1764
	// (get) Token: 0x060041DC RID: 16860 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x060041DD RID: 16861 RVA: 0x0019B840 File Offset: 0x00199A40
	public NetPackageAllyResponse Setup(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target, AllyStore.AllyStatus _newStatus, AllyStore.AllyEvent _allyEventSource, AllyStore.AllyEvent _allyEventTarget)
	{
		this.source = _source;
		this.target = _target;
		this.newStatus = _newStatus;
		this.allyEventSource = _allyEventSource;
		this.allyEventTarget = _allyEventTarget;
		return this;
	}

	// Token: 0x060041DE RID: 16862 RVA: 0x0019B868 File Offset: 0x00199A68
	public override void read(PooledBinaryReader _br)
	{
		this.source = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.target = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.newStatus = (AllyStore.AllyStatus)_br.ReadByte();
		this.allyEventSource = (AllyStore.AllyEvent)_br.ReadByte();
		this.allyEventTarget = (AllyStore.AllyEvent)_br.ReadByte();
	}

	// Token: 0x060041DF RID: 16863 RVA: 0x0019B8B8 File Offset: 0x00199AB8
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.source.ToStream(_bw, false);
		this.target.ToStream(_bw, false);
		_bw.Write((byte)this.newStatus);
		_bw.Write((byte)this.allyEventSource);
		_bw.Write((byte)this.allyEventTarget);
	}

	// Token: 0x060041E0 RID: 16864 RVA: 0x0019B90A File Offset: 0x00199B0A
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.persistentPlayers.Allies.AllyUpdateResponse(this.source, this.target, this.newStatus, this.allyEventSource, this.allyEventTarget);
	}

	// Token: 0x060041E1 RID: 16865 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x0400352F RID: 13615
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs source;

	// Token: 0x04003530 RID: 13616
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs target;

	// Token: 0x04003531 RID: 13617
	[PublicizedFrom(EAccessModifier.Private)]
	public AllyStore.AllyStatus newStatus;

	// Token: 0x04003532 RID: 13618
	[PublicizedFrom(EAccessModifier.Private)]
	public AllyStore.AllyEvent allyEventSource;

	// Token: 0x04003533 RID: 13619
	[PublicizedFrom(EAccessModifier.Private)]
	public AllyStore.AllyEvent allyEventTarget;
}
