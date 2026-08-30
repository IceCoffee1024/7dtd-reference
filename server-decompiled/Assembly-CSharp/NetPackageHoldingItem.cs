using System;
using UnityEngine.Scripting;

// Token: 0x020007FC RID: 2044
[Preserve]
public class NetPackageHoldingItem : NetPackage
{
	// Token: 0x06003C45 RID: 15429 RVA: 0x00181F68 File Offset: 0x00180168
	public NetPackageHoldingItem Setup(EntityAlive _entity)
	{
		this.entityId = _entity.entityId;
		this.holdingItemStack = _entity.inventory.holdingItemStack;
		this.holdingItemIndex = (byte)_entity.inventory.holdingItemIdx;
		return this;
	}

	// Token: 0x06003C46 RID: 15430 RVA: 0x00181F9A File Offset: 0x0018019A
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.holdingItemStack = new ItemStack();
		this.holdingItemStack.Read(_reader);
		this.holdingItemIndex = _reader.ReadByte();
	}

	// Token: 0x06003C47 RID: 15431 RVA: 0x00181FCC File Offset: 0x001801CC
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		this.holdingItemStack.Write(_writer);
		_writer.Write(this.holdingItemIndex);
	}

	// Token: 0x06003C48 RID: 15432 RVA: 0x00181FFC File Offset: 0x001801FC
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

	// Token: 0x06003C49 RID: 15433 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x0400310C RID: 12556
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400310D RID: 12557
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack holdingItemStack;

	// Token: 0x0400310E RID: 12558
	[PublicizedFrom(EAccessModifier.Private)]
	public byte holdingItemIndex;
}
