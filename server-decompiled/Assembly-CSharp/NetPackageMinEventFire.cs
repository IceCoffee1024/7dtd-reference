using System;
using UnityEngine.Scripting;

// Token: 0x02000811 RID: 2065
[Preserve]
public class NetPackageMinEventFire : NetPackage
{
	// Token: 0x06003CD7 RID: 15575 RVA: 0x00183AAF File Offset: 0x00181CAF
	public NetPackageMinEventFire Setup(int _selfEntityID, int _otherEntityID, MinEventTypes _eventType, ItemValue _itemValue)
	{
		this.selfEntityID = _selfEntityID;
		this.otherEntityID = _otherEntityID;
		this.eventType = _eventType;
		this.itemValue = _itemValue;
		this.eventPackageType = NetPackageMinEventFire.EventPackageTypes.ItemEvent;
		return this;
	}

	// Token: 0x06003CD8 RID: 15576 RVA: 0x00183AD6 File Offset: 0x00181CD6
	public NetPackageMinEventFire Setup(int _selfEntityID, int _otherEntityID, MinEventTypes _eventType, BlockValue _blockValue)
	{
		this.selfEntityID = _selfEntityID;
		this.otherEntityID = _otherEntityID;
		this.eventType = _eventType;
		this.blockValue = _blockValue;
		this.eventPackageType = NetPackageMinEventFire.EventPackageTypes.BlockEvent;
		return this;
	}

	// Token: 0x06003CD9 RID: 15577 RVA: 0x00183B00 File Offset: 0x00181D00
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

	// Token: 0x06003CDA RID: 15578 RVA: 0x00183B84 File Offset: 0x00181D84
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

	// Token: 0x06003CDB RID: 15579 RVA: 0x00183BF0 File Offset: 0x00181DF0
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

	// Token: 0x17000659 RID: 1625
	// (get) Token: 0x06003CDC RID: 15580 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003CDD RID: 15581 RVA: 0x00183C79 File Offset: 0x00181E79
	public override int GetLength()
	{
		return 32;
	}

	// Token: 0x04003156 RID: 12630
	[PublicizedFrom(EAccessModifier.Private)]
	public int selfEntityID;

	// Token: 0x04003157 RID: 12631
	[PublicizedFrom(EAccessModifier.Private)]
	public int otherEntityID;

	// Token: 0x04003158 RID: 12632
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventTypes eventType;

	// Token: 0x04003159 RID: 12633
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValue;

	// Token: 0x0400315A RID: 12634
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockValue;

	// Token: 0x0400315B RID: 12635
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageMinEventFire.EventPackageTypes eventPackageType;

	// Token: 0x02000812 RID: 2066
	public enum EventPackageTypes
	{
		// Token: 0x0400315D RID: 12637
		ItemEvent,
		// Token: 0x0400315E RID: 12638
		BlockEvent
	}
}
