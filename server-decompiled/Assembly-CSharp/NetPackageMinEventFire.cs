using System;
using UnityEngine.Scripting;

// Token: 0x0200080F RID: 2063
[Preserve]
public class NetPackageMinEventFire : NetPackage
{
	// Token: 0x06003CA2 RID: 15522 RVA: 0x0018368F File Offset: 0x0018188F
	public NetPackageMinEventFire Setup(int _selfEntityID, int _otherEntityID, MinEventTypes _eventType, ItemValue _itemValue)
	{
		this.selfEntityID = _selfEntityID;
		this.otherEntityID = _otherEntityID;
		this.eventType = _eventType;
		this.itemValue = _itemValue;
		this.eventPackageType = NetPackageMinEventFire.EventPackageTypes.ItemEvent;
		return this;
	}

	// Token: 0x06003CA3 RID: 15523 RVA: 0x001836B6 File Offset: 0x001818B6
	public NetPackageMinEventFire Setup(int _selfEntityID, int _otherEntityID, MinEventTypes _eventType, BlockValue _blockValue)
	{
		this.selfEntityID = _selfEntityID;
		this.otherEntityID = _otherEntityID;
		this.eventType = _eventType;
		this.blockValue = _blockValue;
		this.eventPackageType = NetPackageMinEventFire.EventPackageTypes.BlockEvent;
		return this;
	}

	// Token: 0x06003CA4 RID: 15524 RVA: 0x001836E0 File Offset: 0x001818E0
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

	// Token: 0x06003CA5 RID: 15525 RVA: 0x00183764 File Offset: 0x00181964
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

	// Token: 0x06003CA6 RID: 15526 RVA: 0x001837D0 File Offset: 0x001819D0
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

	// Token: 0x17000678 RID: 1656
	// (get) Token: 0x06003CA7 RID: 15527 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x0400313A RID: 12602
	[PublicizedFrom(EAccessModifier.Private)]
	public int selfEntityID;

	// Token: 0x0400313B RID: 12603
	[PublicizedFrom(EAccessModifier.Private)]
	public int otherEntityID;

	// Token: 0x0400313C RID: 12604
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventTypes eventType;

	// Token: 0x0400313D RID: 12605
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValue;

	// Token: 0x0400313E RID: 12606
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockValue;

	// Token: 0x0400313F RID: 12607
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageMinEventFire.EventPackageTypes eventPackageType;

	// Token: 0x02000810 RID: 2064
	public enum EventPackageTypes
	{
		// Token: 0x04003141 RID: 12609
		ItemEvent,
		// Token: 0x04003142 RID: 12610
		BlockEvent
	}
}
