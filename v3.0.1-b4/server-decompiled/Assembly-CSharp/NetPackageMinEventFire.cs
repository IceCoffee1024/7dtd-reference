using System;
using UnityEngine.Scripting;

// Token: 0x02000806 RID: 2054
[Preserve]
public class NetPackageMinEventFire : NetPackage
{
	// Token: 0x06003C78 RID: 15480 RVA: 0x001816D7 File Offset: 0x0017F8D7
	public NetPackageMinEventFire Setup(int _selfEntityID, int _otherEntityID, MinEventTypes _eventType, ItemValue _itemValue)
	{
		this.selfEntityID = _selfEntityID;
		this.otherEntityID = _otherEntityID;
		this.eventType = _eventType;
		this.itemValue = _itemValue;
		this.eventPackageType = NetPackageMinEventFire.EventPackageTypes.ItemEvent;
		return this;
	}

	// Token: 0x06003C79 RID: 15481 RVA: 0x001816FE File Offset: 0x0017F8FE
	public NetPackageMinEventFire Setup(int _selfEntityID, int _otherEntityID, MinEventTypes _eventType, BlockValue _blockValue)
	{
		this.selfEntityID = _selfEntityID;
		this.otherEntityID = _otherEntityID;
		this.eventType = _eventType;
		this.blockValue = _blockValue;
		this.eventPackageType = NetPackageMinEventFire.EventPackageTypes.BlockEvent;
		return this;
	}

	// Token: 0x06003C7A RID: 15482 RVA: 0x00181728 File Offset: 0x0017F928
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

	// Token: 0x06003C7B RID: 15483 RVA: 0x001817AC File Offset: 0x0017F9AC
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

	// Token: 0x06003C7C RID: 15484 RVA: 0x00181818 File Offset: 0x0017FA18
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

	// Token: 0x1700064E RID: 1614
	// (get) Token: 0x06003C7D RID: 15485 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C7E RID: 15486 RVA: 0x001818A1 File Offset: 0x0017FAA1
	public override int GetLength()
	{
		return 32;
	}

	// Token: 0x040030D7 RID: 12503
	[PublicizedFrom(EAccessModifier.Private)]
	public int selfEntityID;

	// Token: 0x040030D8 RID: 12504
	[PublicizedFrom(EAccessModifier.Private)]
	public int otherEntityID;

	// Token: 0x040030D9 RID: 12505
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventTypes eventType;

	// Token: 0x040030DA RID: 12506
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValue;

	// Token: 0x040030DB RID: 12507
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockValue;

	// Token: 0x040030DC RID: 12508
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageMinEventFire.EventPackageTypes eventPackageType;

	// Token: 0x02000807 RID: 2055
	public enum EventPackageTypes
	{
		// Token: 0x040030DE RID: 12510
		ItemEvent,
		// Token: 0x040030DF RID: 12511
		BlockEvent
	}
}
