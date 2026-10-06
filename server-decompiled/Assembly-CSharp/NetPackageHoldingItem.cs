using System;
using UnityEngine.Scripting;

// Token: 0x020007FF RID: 2047
[Preserve]
public class NetPackageHoldingItem : NetPackage
{
	// Token: 0x06003C41 RID: 15425 RVA: 0x00182470 File Offset: 0x00180670
	public NetPackageHoldingItem Setup(EntityAlive _entity)
	{
		this.entityId = _entity.entityId;
		this.holdingItemStack = _entity.inventory.holdingItemStack;
		this.holdingItemIndex = (byte)_entity.inventory.holdingItemIdx;
		return this;
	}

	// Token: 0x06003C42 RID: 15426 RVA: 0x001824A2 File Offset: 0x001806A2
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.holdingItemStack = new ItemStack();
		this.holdingItemStack.Read(_reader);
		this.holdingItemIndex = _reader.ReadByte();
	}

	// Token: 0x06003C43 RID: 15427 RVA: 0x001824D4 File Offset: 0x001806D4
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		this.holdingItemStack.Write(_writer);
		_writer.Write(this.holdingItemIndex);
	}

	// Token: 0x06003C44 RID: 15428 RVA: 0x00182504 File Offset: 0x00180704
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

	// Token: 0x04003105 RID: 12549
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003106 RID: 12550
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack holdingItemStack;

	// Token: 0x04003107 RID: 12551
	[PublicizedFrom(EAccessModifier.Private)]
	public byte holdingItemIndex;
}
