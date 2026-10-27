using System;
using UnityEngine.Scripting;

// Token: 0x02000819 RID: 2073
[Preserve]
public class NetPackageMinEventFire : NetPackage
{
	// Token: 0x06003CC5 RID: 15557 RVA: 0x00184793 File Offset: 0x00182993
	public NetPackageMinEventFire Setup(int _selfEntityID, int _otherEntityID, MinEventTypes _eventType, ItemValue _itemValue)
	{
		this.selfEntityID = _selfEntityID;
		this.otherEntityID = _otherEntityID;
		this.eventType = _eventType;
		this.itemValue = _itemValue;
		this.eventPackageType = NetPackageMinEventFire.EventPackageTypes.ItemEvent;
		return this;
	}

	// Token: 0x06003CC6 RID: 15558 RVA: 0x001847BA File Offset: 0x001829BA
	public NetPackageMinEventFire Setup(int _selfEntityID, int _otherEntityID, MinEventTypes _eventType, BlockValue _blockValue)
	{
		this.selfEntityID = _selfEntityID;
		this.otherEntityID = _otherEntityID;
		this.eventType = _eventType;
		this.blockValue = _blockValue;
		this.eventPackageType = NetPackageMinEventFire.EventPackageTypes.BlockEvent;
		return this;
	}

	// Token: 0x06003CC7 RID: 15559 RVA: 0x001847E4 File Offset: 0x001829E4
	public override void read(PooledBinaryReader _br)
	{
		this.selfEntityID = _br.ReadInt32();
		this.otherEntityID = _br.ReadInt32();
		this.eventType = (MinEventTypes)_br.ReadByte();
		this.eventPackageType = (NetPackageMinEventFire.EventPackageTypes)_br.ReadByte();
		if (this.eventPackageType == NetPackageMinEventFire.EventPackageTypes.ItemEvent)
		{
			this.itemValue = new ItemValue();
			this.itemValue.Read(_br);
			this.blockValue = BlockValue.Air;
			return;
		}
		this.blockValue = new BlockValue(_br.ReadUInt32());
		this.itemValue = ItemValue.None;
	}

	// Token: 0x06003CC8 RID: 15560 RVA: 0x00184868 File Offset: 0x00182A68
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.selfEntityID);
		_bw.Write(this.otherEntityID);
		_bw.Write((byte)this.eventType);
		_bw.Write((byte)this.eventPackageType);
		if (this.eventPackageType == NetPackageMinEventFire.EventPackageTypes.ItemEvent)
		{
			this.itemValue.Write(_bw);
			return;
		}
		_bw.Write(this.blockValue.rawData);
	}

	// Token: 0x06003CC9 RID: 15561 RVA: 0x001848D4 File Offset: 0x00182AD4
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityAlive entityAlive = _world.GetEntity(this.selfEntityID) as EntityAlive;
		if (entityAlive != null)
		{
			entityAlive.MinEventContext.Self = entityAlive;
			entityAlive.MinEventContext.Other = ((this.otherEntityID == -1) ? null : (_world.GetEntity(this.otherEntityID) as EntityAlive));
			entityAlive.MinEventContext.ItemValue = this.itemValue;
			entityAlive.MinEventContext.BlockValue = this.blockValue;
			entityAlive.FireEvent(this.eventType, true);
		}
	}

	// Token: 0x1700067E RID: 1662
	// (get) Token: 0x06003CCA RID: 15562 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x0400315A RID: 12634
	[PublicizedFrom(EAccessModifier.Private)]
	public int selfEntityID;

	// Token: 0x0400315B RID: 12635
	[PublicizedFrom(EAccessModifier.Private)]
	public int otherEntityID;

	// Token: 0x0400315C RID: 12636
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventTypes eventType;

	// Token: 0x0400315D RID: 12637
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValue;

	// Token: 0x0400315E RID: 12638
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockValue;

	// Token: 0x0400315F RID: 12639
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageMinEventFire.EventPackageTypes eventPackageType;

	// Token: 0x0200081A RID: 2074
	public enum EventPackageTypes
	{
		// Token: 0x04003161 RID: 12641
		ItemEvent,
		// Token: 0x04003162 RID: 12642
		BlockEvent
	}
}
