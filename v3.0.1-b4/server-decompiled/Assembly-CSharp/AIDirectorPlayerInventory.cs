using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000413 RID: 1043
public struct AIDirectorPlayerInventory : IEquatable<AIDirectorPlayerInventory>
{
	// Token: 0x06002040 RID: 8256 RVA: 0x000C35F8 File Offset: 0x000C17F8
	public static AIDirectorPlayerInventory FromEntity(EntityAlive entity)
	{
		AIDirectorPlayerInventory aidirectorPlayerInventory;
		aidirectorPlayerInventory.bag = AIDirectorPlayerInventory.TrackedItemsFromBag(entity.bag);
		aidirectorPlayerInventory.belt = AIDirectorPlayerInventory.TrackedItemsFromInventory(entity.inventory);
		return aidirectorPlayerInventory;
	}

	// Token: 0x06002041 RID: 8257 RVA: 0x000C362C File Offset: 0x000C182C
	public bool Equals(AIDirectorPlayerInventory other)
	{
		return this.bag != null == (other.bag != null) && this.belt != null == (other.belt != null) && AIDirectorPlayerInventory.OrderIndependantEquals(this.bag, other.bag) && AIDirectorPlayerInventory.OrderIndependantEquals(this.belt, other.belt);
	}

	// Token: 0x06002042 RID: 8258 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public static List<AIDirectorPlayerInventory.ItemId> TrackedItemsFromBag(Bag bag)
	{
		return null;
	}

	// Token: 0x06002043 RID: 8259 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public static List<AIDirectorPlayerInventory.ItemId> TrackedItemsFromInventory(Inventory inv)
	{
		return null;
	}

	// Token: 0x06002044 RID: 8260 RVA: 0x000C368C File Offset: 0x000C188C
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

	// Token: 0x06002045 RID: 8261 RVA: 0x000C36E4 File Offset: 0x000C18E4
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

	// Token: 0x0400160F RID: 5647
	public List<AIDirectorPlayerInventory.ItemId> bag;

	// Token: 0x04001610 RID: 5648
	public List<AIDirectorPlayerInventory.ItemId> belt;

	// Token: 0x02000414 RID: 1044
	public struct ItemId : IEquatable<AIDirectorPlayerInventory.ItemId>
	{
		// Token: 0x06002046 RID: 8262 RVA: 0x000C3738 File Offset: 0x000C1938
		public static AIDirectorPlayerInventory.ItemId FromStack(ItemStack stack)
		{
			AIDirectorPlayerInventory.ItemId itemId;
			itemId.id = stack.itemValue.type;
			itemId.count = stack.count;
			return itemId;
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x000C3768 File Offset: 0x000C1968
		public static AIDirectorPlayerInventory.ItemId Read(BinaryReader stream)
		{
			AIDirectorPlayerInventory.ItemId itemId;
			itemId.id = (int)stream.ReadInt16();
			itemId.count = (int)stream.ReadInt16();
			return itemId;
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x000C3790 File Offset: 0x000C1990
		public void Write(BinaryWriter stream)
		{
			stream.Write((short)this.id);
			stream.Write((short)this.count);
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x000C37AC File Offset: 0x000C19AC
		public bool Equals(AIDirectorPlayerInventory.ItemId other)
		{
			return this.id == other.id && this.count == other.count;
		}

		// Token: 0x04001611 RID: 5649
		public const int kNetworkSize = 4;

		// Token: 0x04001612 RID: 5650
		public int id;

		// Token: 0x04001613 RID: 5651
		public int count;
	}
}
