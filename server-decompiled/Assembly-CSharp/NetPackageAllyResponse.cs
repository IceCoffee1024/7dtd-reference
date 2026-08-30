using System;
using UnityEngine.Scripting;

// Token: 0x020008ED RID: 2285
[Preserve]
public class NetPackageAllyResponse : NetPackage
{
	// Token: 0x170006F0 RID: 1776
	// (get) Token: 0x06004241 RID: 16961 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06004242 RID: 16962 RVA: 0x0019D918 File Offset: 0x0019BB18
	public NetPackageAllyResponse Setup(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target, AllyStore.AllyStatus _newStatus, AllyStore.AllyEvent _allyEventSource, AllyStore.AllyEvent _allyEventTarget)
	{
		this.source = _source;
		this.target = _target;
		this.newStatus = _newStatus;
		this.allyEventSource = _allyEventSource;
		this.allyEventTarget = _allyEventTarget;
		return this;
	}

	// Token: 0x06004243 RID: 16963 RVA: 0x0019D940 File Offset: 0x0019BB40
	public override void read(PooledBinaryReader _br)
	{
		this.source = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.target = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.newStatus = (AllyStore.AllyStatus)_br.ReadByte();
		this.allyEventSource = (AllyStore.AllyEvent)_br.ReadByte();
		this.allyEventTarget = (AllyStore.AllyEvent)_br.ReadByte();
	}

	// Token: 0x06004244 RID: 16964 RVA: 0x0019D990 File Offset: 0x0019BB90
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.source.ToStream(_bw, false);
		this.target.ToStream(_bw, false);
		_bw.Write((byte)this.newStatus);
		_bw.Write((byte)this.allyEventSource);
		_bw.Write((byte)this.allyEventTarget);
	}

	// Token: 0x06004245 RID: 16965 RVA: 0x0019D9E2 File Offset: 0x0019BBE2
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.persistentPlayers.Allies.AllyUpdateResponse(this.source, this.target, this.newStatus, this.allyEventSource, this.allyEventTarget);
	}

	// Token: 0x06004246 RID: 16966 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x040035B0 RID: 13744
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs source;

	// Token: 0x040035B1 RID: 13745
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs target;

	// Token: 0x040035B2 RID: 13746
	[PublicizedFrom(EAccessModifier.Private)]
	public AllyStore.AllyStatus newStatus;

	// Token: 0x040035B3 RID: 13747
	[PublicizedFrom(EAccessModifier.Private)]
	public AllyStore.AllyEvent allyEventSource;

	// Token: 0x040035B4 RID: 13748
	[PublicizedFrom(EAccessModifier.Private)]
	public AllyStore.AllyEvent allyEventTarget;
}
