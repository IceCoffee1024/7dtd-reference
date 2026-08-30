using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x0200056A RID: 1386
public class Bag : InventoryBase
{
	// Token: 0x14000036 RID: 54
	// (add) Token: 0x06002CF9 RID: 11513 RVA: 0x0011F420 File Offset: 0x0011D620
	// (remove) Token: 0x06002CFA RID: 11514 RVA: 0x0011F458 File Offset: 0x0011D658
	public event XUiEvent_BackpackItemsChangedInternal OnBackpackItemsChangedInternal;

	// Token: 0x06002CFB RID: 11515 RVA: 0x0011F48D File Offset: 0x0011D68D
	[PublicizedFrom(EAccessModifier.Private)]
	public Bag()
	{
	}

	// Token: 0x06002CFC RID: 11516 RVA: 0x0011F495 File Offset: 0x0011D695
	public Bag(int _size)
	{
		this.items = ItemStack.CreateArray(_size);
	}

	// Token: 0x170004CB RID: 1227
	// (get) Token: 0x06002CFD RID: 11517 RVA: 0x0011F4A9 File Offset: 0x0011D6A9
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

	// Token: 0x06002CFE RID: 11518 RVA: 0x0011F4C0 File Offset: 0x0011D6C0
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

	// Token: 0x06002CFF RID: 11519 RVA: 0x0011F561 File Offset: 0x0011D761
	public static Bag Read(BinaryReader br)
	{
		return new Bag().ReadInto(br);
	}

	// Token: 0x06002D00 RID: 11520 RVA: 0x0011F570 File Offset: 0x0011D770
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

	// Token: 0x06002D01 RID: 11521 RVA: 0x0011F658 File Offset: 0x0011D858
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

	// Token: 0x06002D02 RID: 11522 RVA: 0x0011F6B8 File Offset: 0x0011D8B8
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

	// Token: 0x06002D03 RID: 11523 RVA: 0x0011F6EB File Offset: 0x0011D8EB
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

	// Token: 0x06002D04 RID: 11524 RVA: 0x0011F700 File Offset: 0x0011D900
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

	// Token: 0x06002D05 RID: 11525 RVA: 0x0011F734 File Offset: 0x0011D934
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

	// Token: 0x06002D06 RID: 11526 RVA: 0x0011F764 File Offset: 0x0011D964
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

	// Token: 0x06002D07 RID: 11527 RVA: 0x0011F7A0 File Offset: 0x0011D9A0
	[return: TupleElementNames(new string[] { "anyMoved", "allMoved" })]
	public override ValueTuple<bool, bool> TryStackItem(int startIndex, ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Backpack, -1))
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

	// Token: 0x06002D08 RID: 11528 RVA: 0x0011F844 File Offset: 0x0011DA44
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

	// Token: 0x06002D09 RID: 11529 RVA: 0x0011F882 File Offset: 0x0011DA82
	public ItemStack[] GetSlots()
	{
		return this.items;
	}

	// Token: 0x06002D0A RID: 11530 RVA: 0x0011F88A File Offset: 0x0011DA8A
	public void SetSlots(ItemStack[] _slots)
	{
		this.items = _slots;
		this.onBackpackChanged();
	}

	// Token: 0x06002D0B RID: 11531 RVA: 0x0011F899 File Offset: 0x0011DA99
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

	// Token: 0x06002D0C RID: 11532 RVA: 0x0011F8BC File Offset: 0x0011DABC
	public override void PerformActionOnSlots(Action<ItemStack> _action)
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			_action(this.items[i]);
		}
	}

	// Token: 0x06002D0D RID: 11533 RVA: 0x0011F8EA File Offset: 0x0011DAEA
	public override bool AddItem(ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Backpack, -1))
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

	// Token: 0x06002D0E RID: 11534 RVA: 0x0011F914 File Offset: 0x0011DB14
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

	// Token: 0x06002D0F RID: 11535 RVA: 0x0011FA24 File Offset: 0x0011DC24
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

	// Token: 0x06002D10 RID: 11536 RVA: 0x0011FA58 File Offset: 0x0011DC58
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

	// Token: 0x06002D11 RID: 11537 RVA: 0x0011FAE8 File Offset: 0x0011DCE8
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

	// Token: 0x06002D12 RID: 11538 RVA: 0x0011FB91 File Offset: 0x0011DD91
	public override bool HasItem(ItemValue _item)
	{
		return this.GetItemCount(_item, -1, -1, true) > 0;
	}

	// Token: 0x0400235D RID: 9053
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte Version = 1;

	// Token: 0x0400235F RID: 9055
	public bool Touched;

	// Token: 0x04002360 RID: 9056
	public PackedBoolArray LockedSlots;

	// Token: 0x04002361 RID: 9057
	public PreferenceTracker preferences;

	// Token: 0x04002362 RID: 9058
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] items;
}
