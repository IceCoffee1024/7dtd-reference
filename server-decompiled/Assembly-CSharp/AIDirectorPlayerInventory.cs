using System;
using System.Collections.Generic;

// Token: 0x0200041B RID: 1051
public struct AIDirectorPlayerInventory : IEquatable<AIDirectorPlayerInventory>
{
	// Token: 0x06002051 RID: 8273 RVA: 0x000C41F4 File Offset: 0x000C23F4
	public static AIDirectorPlayerInventory FromEntity(EntityAlive entity)
	{
		AIDirectorPlayerInventory aidirectorPlayerInventory;
		aidirectorPlayerInventory.bag = AIDirectorPlayerInventory.TrackedItemsFromBag(entity.bag);
		aidirectorPlayerInventory.belt = AIDirectorPlayerInventory.TrackedItemsFromInventory(entity.inventory);
		return aidirectorPlayerInventory;
	}

	// Token: 0x06002052 RID: 8274 RVA: 0x000C4228 File Offset: 0x000C2428
	public bool Equals(AIDirectorPlayerInventory other)
	{
		return this.bag != null == (other.bag != null) && this.belt != null == (other.belt != null) && AIDirectorPlayerInventory.OrderIndependantEquals(this.bag, other.bag) && AIDirectorPlayerInventory.OrderIndependantEquals(this.belt, other.belt);
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x000200DA File Offset: 0x0001E2DA
	public static List<AIDirectorPlayerInventory.ItemId> TrackedItemsFromBag(Bag bag)
	{
		return null;
	}

	// Token: 0x06002054 RID: 8276 RVA: 0x000200DA File Offset: 0x0001E2DA
	public static List<AIDirectorPlayerInventory.ItemId> TrackedItemsFromInventory(Inventory inv)
	{
		return null;
	}

	// Token: 0x06002055 RID: 8277 RVA: 0x000C4288 File Offset: 0x000C2488
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

	// Token: 0x06002056 RID: 8278 RVA: 0x000C42E0 File Offset: 0x000C24E0
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

	// Token: 0x0400162B RID: 5675
	public List<AIDirectorPlayerInventory.ItemId> bag;

	// Token: 0x0400162C RID: 5676
	public List<AIDirectorPlayerInventory.ItemId> belt;

	// Token: 0x0200041C RID: 1052
	public struct ItemId : IEquatable<AIDirectorPlayerInventory.ItemId>
	{
		// Token: 0x06002057 RID: 8279 RVA: 0x000C4334 File Offset: 0x000C2534
		public static AIDirectorPlayerInventory.ItemId FromStack(ItemStack stack)
		{
			AIDirectorPlayerInventory.ItemId itemId;
			itemId.id = stack.itemValue.type;
			itemId.count = stack.count;
			return itemId;
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x000C4364 File Offset: 0x000C2564
		public static AIDirectorPlayerInventory.ItemId Read(PooledBinaryReader stream)
		{
			AIDirectorPlayerInventory.ItemId itemId;
			itemId.id = (int)stream.ReadInt16();
			itemId.count = (int)stream.ReadInt16();
			return itemId;
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x000C438C File Offset: 0x000C258C
		public void Write(PooledBinaryWriter stream)
		{
			stream.Write((short)this.id);
			stream.Write((short)this.count);
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x000C43A8 File Offset: 0x000C25A8
		public bool Equals(AIDirectorPlayerInventory.ItemId other)
		{
			return this.id == other.id && this.count == other.count;
		}

		// Token: 0x0400162D RID: 5677
		public const int kNetworkSize = 4;

		// Token: 0x0400162E RID: 5678
		public int id;

		// Token: 0x0400162F RID: 5679
		public int count;
	}
}
