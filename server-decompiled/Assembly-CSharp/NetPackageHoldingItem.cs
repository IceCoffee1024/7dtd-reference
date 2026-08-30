using System;
using UnityEngine.Scripting;

// Token: 0x020007F1 RID: 2033
[Preserve]
public class NetPackageHoldingItem : NetPackage
{
	// Token: 0x06003BE6 RID: 15334 RVA: 0x0017FB90 File Offset: 0x0017DD90
	public NetPackageHoldingItem Setup(EntityAlive _entity)
	{
		this.entityId = _entity.entityId;
		this.holdingItemStack = _entity.inventory.holdingItemStack;
		this.holdingItemIndex = (byte)_entity.inventory.holdingItemIdx;
		return this;
	}

	// Token: 0x06003BE7 RID: 15335 RVA: 0x0017FBC2 File Offset: 0x0017DDC2
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.holdingItemStack = new ItemStack();
		this.holdingItemStack.Read(_reader);
		this.holdingItemIndex = _reader.ReadByte();
	}

	// Token: 0x06003BE8 RID: 15336 RVA: 0x0017FBF4 File Offset: 0x0017DDF4
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		this.holdingItemStack.Write(_writer);
		_writer.Write(this.holdingItemIndex);
	}

	// Token: 0x06003BE9 RID: 15337 RVA: 0x0017FC24 File Offset: 0x0017DE24
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

	// Token: 0x06003BEA RID: 15338 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x0400308D RID: 12429
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400308E RID: 12430
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack holdingItemStack;

	// Token: 0x0400308F RID: 12431
	[PublicizedFrom(EAccessModifier.Private)]
	public byte holdingItemIndex;
}
