using System;
using System.Collections.Generic;

// Token: 0x02000419 RID: 1049
public struct AIDirectorPlayerInventory : IEquatable<AIDirectorPlayerInventory>
{
	// Token: 0x0600204C RID: 8268 RVA: 0x000C3E48 File Offset: 0x000C2048
	public static AIDirectorPlayerInventory FromEntity(EntityAlive entity)
	{
		AIDirectorPlayerInventory aidirectorPlayerInventory;
		aidirectorPlayerInventory.bag = AIDirectorPlayerInventory.TrackedItemsFromBag(entity.bag);
		aidirectorPlayerInventory.belt = AIDirectorPlayerInventory.TrackedItemsFromInventory(entity.inventory);
		return aidirectorPlayerInventory;
	}

	// Token: 0x0600204D RID: 8269 RVA: 0x000C3E7C File Offset: 0x000C207C
	public bool Equals(AIDirectorPlayerInventory other)
	{
		return this.bag != null == (other.bag != null) && this.belt != null == (other.belt != null) && AIDirectorPlayerInventory.OrderIndependantEquals(this.bag, other.bag) && AIDirectorPlayerInventory.OrderIndependantEquals(this.belt, other.belt);
	}

	// Token: 0x0600204E RID: 8270 RVA: 0x000200DA File Offset: 0x0001E2DA
	public static List<AIDirectorPlayerInventory.ItemId> TrackedItemsFromBag(Bag bag)
	{
		return null;
	}

	// Token: 0x0600204F RID: 8271 RVA: 0x000200DA File Offset: 0x0001E2DA
	public static List<AIDirectorPlayerInventory.ItemId> TrackedItemsFromInventory(Inventory inv)
	{
		return null;
	}

	// Token: 0x06002050 RID: 8272 RVA: 0x000C3EDC File Offset: 0x000C20DC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void AppendId(List<AIDirectorPlayerInventory.ItemId> list, AIDirectorPlayerInventory.ItemId id)
	{
		for (int i = 0; i < list.Count; i++)
		{
			AIDirectorPlayerInventory.ItemId itemId = list[i];
			if (itemId.id == id.id)
			{
				itemId.count += id.count;
				list[i] = itemId;
				return;
			}
		}
		list.Add(id);
	}

	// Token: 0x06002051 RID: 8273 RVA: 0x000C3F34 File Offset: 0x000C2134
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool OrderIndependantEquals(List<AIDirectorPlayerInventory.ItemId> a, List<AIDirectorPlayerInventory.ItemId> b)
	{
		if (a == null && b == null)
		{
			return true;
		}
		if (a == null || b == null)
		{
			return false;
		}
		if (a.Count != b.Count)
		{
			return false;
		}
		for (int i = 0; i < a.Count; i++)
		{
			AIDirectorPlayerInventory.ItemId itemId = a[i];
			if (!b.Contains(itemId))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0400161B RID: 5659
	public List<AIDirectorPlayerInventory.ItemId> bag;

	// Token: 0x0400161C RID: 5660
	public List<AIDirectorPlayerInventory.ItemId> belt;

	// Token: 0x0200041A RID: 1050
	public struct ItemId : IEquatable<AIDirectorPlayerInventory.ItemId>
	{
		// Token: 0x06002052 RID: 8274 RVA: 0x000C3F88 File Offset: 0x000C2188
		public static AIDirectorPlayerInventory.ItemId FromStack(ItemStack stack)
		{
			AIDirectorPlayerInventory.ItemId itemId;
			itemId.id = stack.itemValue.type;
			itemId.count = stack.count;
			return itemId;
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x000C3FB8 File Offset: 0x000C21B8
		public static AIDirectorPlayerInventory.ItemId Read(PooledBinaryReader stream)
		{
			AIDirectorPlayerInventory.ItemId itemId;
			itemId.id = (int)stream.ReadInt16();
			itemId.count = (int)stream.ReadInt16();
			return itemId;
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x000C3FE0 File Offset: 0x000C21E0
		public void Write(PooledBinaryWriter stream)
		{
			stream.Write((short)this.id);
			stream.Write((short)this.count);
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x000C3FFC File Offset: 0x000C21FC
		public bool Equals(AIDirectorPlayerInventory.ItemId other)
		{
			return this.id == other.id && this.count == other.count;
		}

		// Token: 0x0400161D RID: 5661
		public const int kNetworkSize = 4;

		// Token: 0x0400161E RID: 5662
		public int id;

		// Token: 0x0400161F RID: 5663
		public int count;
	}
}
