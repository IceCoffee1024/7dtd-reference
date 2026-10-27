using System;
using UnityEngine.Scripting;

// Token: 0x02000809 RID: 2057
[Preserve]
public class NetPackageHoldingItem : NetPackage
{
	// Token: 0x06003C64 RID: 15460 RVA: 0x00183574 File Offset: 0x00181774
	public NetPackageHoldingItem Setup(EntityAlive _entity)
	{
		this.entityId = _entity.entityId;
		this.holdingItemStack = _entity.inventory.holdingItemStack;
		this.holdingItemIndex = (byte)_entity.inventory.holdingItemIdx;
		return this;
	}

	// Token: 0x06003C65 RID: 15461 RVA: 0x001835A6 File Offset: 0x001817A6
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.holdingItemStack = new ItemStack();
		this.holdingItemStack.Read(_reader);
		this.holdingItemIndex = _reader.ReadByte();
	}

	// Token: 0x06003C66 RID: 15462 RVA: 0x001835D8 File Offset: 0x001817D8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		this.holdingItemStack.Write(_writer);
		_writer.Write(this.holdingItemIndex);
	}

	// Token: 0x06003C67 RID: 15463 RVA: 0x00183608 File Offset: 0x00181808
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!base.ValidEntityIdForSender(this.entityId, false))
		{
			return;
		}
		EntityAlive entityAlive = _world.GetEntity(this.entityId) as EntityAlive;
		if (entityAlive)
		{
			entityAlive.EnqueueNetworkHoldingData(this.holdingItemStack, this.holdingItemIndex);
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageHoldingItem>().Setup(entityAlive), false, -1, base.Sender.entityId, -1, null, 192, false);
		}
	}

	// Token: 0x04003125 RID: 12581
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003126 RID: 12582
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack holdingItemStack;

	// Token: 0x04003127 RID: 12583
	[PublicizedFrom(EAccessModifier.Private)]
	public byte holdingItemIndex;
}
