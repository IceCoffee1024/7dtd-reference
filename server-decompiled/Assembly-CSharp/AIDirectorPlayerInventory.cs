using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000417 RID: 1047
public struct AIDirectorPlayerInventory : IEquatable<AIDirectorPlayerInventory>
{
	// Token: 0x0600205D RID: 8285 RVA: 0x000C3D94 File Offset: 0x000C1F94
	public static AIDirectorPlayerInventory FromEntity(EntityAlive entity)
	{
		AIDirectorPlayerInventory aidirectorPlayerInventory;
		aidirectorPlayerInventory.bag = AIDirectorPlayerInventory.TrackedItemsFromBag(entity.bag);
		aidirectorPlayerInventory.belt = AIDirectorPlayerInventory.TrackedItemsFromInventory(entity.inventory);
		return aidirectorPlayerInventory;
	}

	// Token: 0x0600205E RID: 8286 RVA: 0x000C3DC8 File Offset: 0x000C1FC8
	public bool Equals(AIDirectorPlayerInventory other)
	{
		return this.bag != null == (other.bag != null) && this.belt != null == (other.belt != null) && AIDirectorPlayerInventory.OrderIndependantEquals(this.bag, other.bag) && AIDirectorPlayerInventory.OrderIndependantEquals(this.belt, other.belt);
	}

	// Token: 0x0600205F RID: 8287 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public static List<AIDirectorPlayerInventory.ItemId> TrackedItemsFromBag(Bag bag)
	{
		return null;
	}

	// Token: 0x06002060 RID: 8288 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public static List<AIDirectorPlayerInventory.ItemId> TrackedItemsFromInventory(Inventory inv)
	{
		return null;
	}

	// Token: 0x06002061 RID: 8289 RVA: 0x000C3E28 File Offset: 0x000C2028
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

	// Token: 0x06002062 RID: 8290 RVA: 0x000C3E80 File Offset: 0x000C2080
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

	// Token: 0x04001624 RID: 5668
	public List<AIDirectorPlayerInventory.ItemId> bag;

	// Token: 0x04001625 RID: 5669
	public List<AIDirectorPlayerInventory.ItemId> belt;

	// Token: 0x02000418 RID: 1048
	public struct ItemId : IEquatable<AIDirectorPlayerInventory.ItemId>
	{
		// Token: 0x06002063 RID: 8291 RVA: 0x000C3ED4 File Offset: 0x000C20D4
		public static AIDirectorPlayerInventory.ItemId FromStack(ItemStack stack)
		{
			AIDirectorPlayerInventory.ItemId itemId;
			itemId.id = stack.itemValue.type;
			itemId.count = stack.count;
			return itemId;
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x000C3F04 File Offset: 0x000C2104
		public static AIDirectorPlayerInventory.ItemId Read(BinaryReader stream)
		{
			AIDirectorPlayerInventory.ItemId itemId;
			itemId.id = (int)stream.ReadInt16();
			itemId.count = (int)stream.ReadInt16();
			return itemId;
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x000C3F2C File Offset: 0x000C212C
		public void Write(BinaryWriter stream)
		{
			stream.Write((short)this.id);
			stream.Write((short)this.count);
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x000C3F48 File Offset: 0x000C2148
		public bool Equals(AIDirectorPlayerInventory.ItemId other)
		{
			return this.id == other.id && this.count == other.count;
		}

		// Token: 0x04001626 RID: 5670
		public const int kNetworkSize = 4;

		// Token: 0x04001627 RID: 5671
		public int id;

		// Token: 0x04001628 RID: 5672
		public int count;
	}
}
