using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x02000565 RID: 1381
public class Bag : InventoryBase
{
	// Token: 0x14000036 RID: 54
	// (add) Token: 0x06002CCF RID: 11471 RVA: 0x0011E3AC File Offset: 0x0011C5AC
	// (remove) Token: 0x06002CD0 RID: 11472 RVA: 0x0011E3E4 File Offset: 0x0011C5E4
	public event XUiEvent_BackpackItemsChangedInternal OnBackpackItemsChangedInternal;

	// Token: 0x06002CD1 RID: 11473 RVA: 0x0011E419 File Offset: 0x0011C619
	[PublicizedFrom(EAccessModifier.Private)]
	public Bag()
	{
	}

	// Token: 0x06002CD2 RID: 11474 RVA: 0x0011E421 File Offset: 0x0011C621
	public Bag(int _size)
	{
		this.items = ItemStack.CreateArray(_size);
	}

	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x06002CD3 RID: 11475 RVA: 0x0011E435 File Offset: 0x0011C635
	public int SlotCount
	{
		get
		{
			if (this.items != null)
			{
				return this.items.Length;
			}
			return 0;
		}
	}

	// Token: 0x06002CD4 RID: 11476 RVA: 0x0011E44C File Offset: 0x0011C64C
	public void Write(BinaryWriter bw)
	{
		bw.Write(1);
		ItemStack[] slots = this.GetSlots();
		bw.Write((ushort)slots.Length);
		for (int i = 0; i < slots.Length; i++)
		{
			slots[i].Write(bw);
		}
		bool flag = this.LockedSlots != null;
		bw.Write(flag);
		if (flag)
		{
			this.LockedSlots.Write(bw);
		}
		bw.Write(this.Touched);
		bool flag2 = this.preferences != null;
		bw.Write(flag2);
		if (!flag2)
		{
			return;
		}
		PooledBinaryWriter pooledBinaryWriter = bw as PooledBinaryWriter;
		if (pooledBinaryWriter != null)
		{
			this.preferences.Write(pooledBinaryWriter);
			return;
		}
		throw new InvalidOperationException("[Bag] Writing preferences requires PooledBinaryWriter.");
	}

	// Token: 0x06002CD5 RID: 11477 RVA: 0x0011E4ED File Offset: 0x0011C6ED
	public static Bag Read(BinaryReader br)
	{
		return new Bag().ReadInto(br);
	}

	// Token: 0x06002CD6 RID: 11478 RVA: 0x0011E4FC File Offset: 0x0011C6FC
	public Bag ReadInto(BinaryReader br)
	{
		byte b = br.ReadByte();
		int num = (int)br.ReadUInt16();
		if (this.items == null || this.SlotCount != num)
		{
			this.items = ItemStack.CreateArray(num);
		}
		for (int i = 0; i < num; i++)
		{
			this.items[i].Read(br);
		}
		if (br.ReadBoolean())
		{
			this.LockedSlots = this.LockedSlots ?? new PackedBoolArray(0);
			this.LockedSlots.Read(br);
		}
		else
		{
			this.LockedSlots = null;
		}
		if (b >= 1)
		{
			this.Touched = br.ReadBoolean();
			if (br.ReadBoolean())
			{
				if (this.preferences == null)
				{
					this.preferences = new PreferenceTracker(-1);
				}
				PooledBinaryReader pooledBinaryReader = br as PooledBinaryReader;
				if (pooledBinaryReader == null)
				{
					throw new InvalidOperationException("[Bag] Reading preferences requires PooledBinaryReader.");
				}
				this.preferences.Read(pooledBinaryReader);
			}
			else
			{
				this.preferences = null;
			}
		}
		else
		{
			this.preferences = null;
		}
		return this;
	}

	// Token: 0x06002CD7 RID: 11479 RVA: 0x0011E5E4 File Offset: 0x0011C7E4
	public Bag Clone()
	{
		Bag bag = new Bag();
		bag.items = ItemStack.Clone(this.items);
		PackedBoolArray lockedSlots = this.LockedSlots;
		bag.LockedSlots = ((lockedSlots != null) ? lockedSlots.Clone() : null);
		bag.Touched = this.Touched;
		PreferenceTracker preferenceTracker = this.preferences;
		bag.preferences = ((preferenceTracker != null) ? preferenceTracker.Clone() : null);
		return bag;
	}

	// Token: 0x06002CD8 RID: 11480 RVA: 0x0011E644 File Offset: 0x0011C844
	public void Clear()
	{
		ItemStack[] array = this.items;
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Clear();
			}
		}
		this.onBackpackChanged();
	}

	// Token: 0x06002CD9 RID: 11481 RVA: 0x0011E677 File Offset: 0x0011C877
	[PublicizedFrom(EAccessModifier.Private)]
	public void onBackpackChanged()
	{
		XUiEvent_BackpackItemsChangedInternal onBackpackItemsChangedInternal = this.OnBackpackItemsChangedInternal;
		if (onBackpackItemsChangedInternal == null)
		{
			return;
		}
		onBackpackItemsChangedInternal();
	}

	// Token: 0x06002CDA RID: 11482 RVA: 0x0011E68C File Offset: 0x0011C88C
	public bool CanStackNoEmpty(ItemStack _itemStack)
	{
		ItemStack[] slots = this.GetSlots();
		for (int i = 0; i < slots.Length; i++)
		{
			int num;
			if (slots[i].CanStackPartlyWith(_itemStack, out num))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002CDB RID: 11483 RVA: 0x0011E6C0 File Offset: 0x0011C8C0
	public bool IsEmpty()
	{
		ItemStack[] slots = this.GetSlots();
		for (int i = 0; i < slots.Length; i++)
		{
			if (!slots[i].IsEmpty())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002CDC RID: 11484 RVA: 0x0011E6F0 File Offset: 0x0011C8F0
	public bool CanStack(ItemStack _itemStack)
	{
		ItemStack[] slots = this.GetSlots();
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].IsEmpty() || slots[i].CanStackWith(_itemStack, false))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002CDD RID: 11485 RVA: 0x0011E72C File Offset: 0x0011C92C
	[return: TupleElementNames(new string[] { "anyMoved", "allMoved" })]
	public override ValueTuple<bool, bool> TryStackItem(int startIndex, ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Backpack))
		{
			return new ValueTuple<bool, bool>(false, false);
		}
		ItemStack[] slots = this.GetSlots();
		int num = 0;
		bool flag = false;
		for (int i = startIndex; i < slots.Length; i++)
		{
			num = _itemStack.count;
			if (_itemStack.itemValue.type == slots[i].itemValue.type && slots[i].CanStackPartly(ref num))
			{
				slots[i].count += num;
				_itemStack.count -= num;
				this.onBackpackChanged();
				flag = true;
				if (_itemStack.count == 0)
				{
					return new ValueTuple<bool, bool>(true, true);
				}
			}
		}
		return new ValueTuple<bool, bool>(flag, false);
	}

	// Token: 0x06002CDE RID: 11486 RVA: 0x0011E7D0 File Offset: 0x0011C9D0
	public bool CanTakeItem(ItemStack _itemStack)
	{
		ItemStack[] slots = this.GetSlots();
		for (int i = 0; i < slots.Length; i++)
		{
			int num;
			if (slots[i].CanStackPartlyWith(_itemStack, out num))
			{
				return true;
			}
			if (slots[i].IsEmpty())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002CDF RID: 11487 RVA: 0x0011E80E File Offset: 0x0011CA0E
	public ItemStack[] GetSlots()
	{
		return this.items;
	}

	// Token: 0x06002CE0 RID: 11488 RVA: 0x0011E816 File Offset: 0x0011CA16
	public void SetSlots(ItemStack[] _slots)
	{
		this.items = _slots;
		this.onBackpackChanged();
	}

	// Token: 0x06002CE1 RID: 11489 RVA: 0x0011E825 File Offset: 0x0011CA25
	public void SetSlot(int index, ItemStack _stack, bool callChangedEvent = true)
	{
		if (index >= this.items.Length)
		{
			return;
		}
		this.items[index] = _stack;
		if (callChangedEvent)
		{
			this.onBackpackChanged();
		}
	}

	// Token: 0x06002CE2 RID: 11490 RVA: 0x0011E848 File Offset: 0x0011CA48
	public override void PerformActionOnSlots(Action<ItemStack> _action)
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			_action(this.items[i]);
		}
	}

	// Token: 0x06002CE3 RID: 11491 RVA: 0x0011E876 File Offset: 0x0011CA76
	public override bool AddItem(ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Backpack))
		{
			return false;
		}
		bool flag = ItemStack.AddToItemStackArray(this.items, _itemStack, -1) >= 0;
		if (flag)
		{
			this.onBackpackChanged();
		}
		return flag;
	}

	// Token: 0x06002CE4 RID: 11492 RVA: 0x0011E8A0 File Offset: 0x0011CAA0
	public int DecItem(ItemValue _itemValue, int _count, bool _ignoreModdedItems = false, IList<ItemStack> _removedItems = null)
	{
		int num = _count;
		ItemStack[] slots = this.GetSlots();
		int num2 = 0;
		while (_count > 0 && num2 < this.GetSlots().Length)
		{
			if (slots[num2].itemValue.type == _itemValue.type && (!_ignoreModdedItems || !slots[num2].itemValue.HasModSlots || !slots[num2].itemValue.HasMods()))
			{
				if (ItemClass.GetForId(slots[num2].itemValue.type).CanStack())
				{
					int count = slots[num2].count;
					int num3 = ((count >= _count) ? _count : count);
					if (_removedItems != null)
					{
						_removedItems.Add(new ItemStack(slots[num2].itemValue.Clone(), num3));
					}
					slots[num2].count -= num3;
					_count -= num3;
					if (slots[num2].count <= 0)
					{
						slots[num2].Clear();
					}
				}
				else
				{
					if (_removedItems != null)
					{
						_removedItems.Add(slots[num2].Clone());
					}
					slots[num2].Clear();
					_count--;
				}
			}
			num2++;
		}
		this.SetSlots(slots);
		return num - _count;
	}

	// Token: 0x06002CE5 RID: 11493 RVA: 0x0011E9B0 File Offset: 0x0011CBB0
	public int GetUsedSlotCount()
	{
		ItemStack[] slots = this.GetSlots();
		int num = 0;
		for (int i = 0; i < slots.Length; i++)
		{
			if (!slots[i].IsEmpty())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06002CE6 RID: 11494 RVA: 0x0011E9E4 File Offset: 0x0011CBE4
	public int GetItemCount(ItemValue _itemValue, int _seed = -1, int _meta = -1, bool _ignoreModdedItems = true)
	{
		ItemStack[] slots = this.GetSlots();
		int num = 0;
		for (int i = 0; i < slots.Length; i++)
		{
			if ((!_ignoreModdedItems || !slots[i].itemValue.HasModSlots || !slots[i].itemValue.HasMods()) && slots[i].itemValue.type == _itemValue.type && (_seed == -1 || _seed == (int)slots[i].itemValue.Seed) && (_meta == -1 || _meta == slots[i].itemValue.Meta))
			{
				num += slots[i].count;
			}
		}
		return num;
	}

	// Token: 0x06002CE7 RID: 11495 RVA: 0x0011EA74 File Offset: 0x0011CC74
	public int GetItemCount(FastTags<TagGroup.Global> itemTags, int _seed = -1, int _meta = -1, bool _ignoreModdedItems = true)
	{
		ItemStack[] slots = this.GetSlots();
		int num = 0;
		for (int i = 0; i < slots.Length; i++)
		{
			if ((!_ignoreModdedItems || !slots[i].itemValue.HasModSlots || !slots[i].itemValue.HasMods()) && !slots[i].itemValue.IsEmpty() && slots[i].itemValue.ItemClass.ItemTags.Test_AnySet(itemTags) && (_seed == -1 || _seed == (int)slots[i].itemValue.Seed) && (_meta == -1 || _meta == slots[i].itemValue.Meta))
			{
				num += slots[i].count;
			}
		}
		return num;
	}

	// Token: 0x06002CE8 RID: 11496 RVA: 0x0011EB1D File Offset: 0x0011CD1D
	public override bool HasItem(ItemValue _item)
	{
		return this.GetItemCount(_item, -1, -1, true) > 0;
	}

	// Token: 0x0400232B RID: 9003
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte Version = 1;

	// Token: 0x0400232D RID: 9005
	public bool Touched;

	// Token: 0x0400232E RID: 9006
	public PackedBoolArray LockedSlots;

	// Token: 0x0400232F RID: 9007
	public PreferenceTracker preferences;

	// Token: 0x04002330 RID: 9008
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] items;
}
