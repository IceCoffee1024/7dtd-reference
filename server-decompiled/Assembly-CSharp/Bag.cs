using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000577 RID: 1399
public class Bag : InventoryBase
{
	// Token: 0x14000036 RID: 54
	// (add) Token: 0x06002CFA RID: 11514 RVA: 0x00120864 File Offset: 0x0011EA64
	// (remove) Token: 0x06002CFB RID: 11515 RVA: 0x0012089C File Offset: 0x0011EA9C
	public event XUiEvent_BackpackItemsChangedInternal OnBackpackItemsChangedInternal;

	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x06002CFC RID: 11516 RVA: 0x001208D1 File Offset: 0x0011EAD1
	public ItemStackGrid ItemGrid
	{
		get
		{
			return this.itemGrid;
		}
	}

	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x06002CFD RID: 11517 RVA: 0x001208D9 File Offset: 0x0011EAD9
	public XUiC_ItemStack.StackLocationTypes StackLocation
	{
		get
		{
			return this.itemGrid.StackLocation;
		}
	}

	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x06002CFE RID: 11518 RVA: 0x001208E6 File Offset: 0x0011EAE6
	public PackedBoolArray LockedSlots
	{
		get
		{
			return this.itemGrid.SlotLocks;
		}
	}

	// Token: 0x170004C4 RID: 1220
	// (get) Token: 0x06002CFF RID: 11519 RVA: 0x001208F3 File Offset: 0x0011EAF3
	public PreferenceTracker preferences
	{
		get
		{
			return this.itemGrid.Preferences;
		}
	}

	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x06002D00 RID: 11520 RVA: 0x00120900 File Offset: 0x0011EB00
	public bool Touched
	{
		get
		{
			return this.itemGrid.Touched;
		}
	}

	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x06002D01 RID: 11521 RVA: 0x0012090D File Offset: 0x0011EB0D
	public int SlotCount
	{
		get
		{
			return this.itemGrid.Length;
		}
	}

	// Token: 0x06002D02 RID: 11522 RVA: 0x0012091A File Offset: 0x0011EB1A
	public Bag(Vector2i _containerSize, XUiC_ItemStack.StackLocationTypes _stackLocation, IItemStackGridHolder _holder)
	{
		this.itemGrid = ItemStackGrid.Create(_containerSize, _stackLocation, true, true, _holder);
		this.itemGrid.SlotChanged += this.onSlotChanged;
	}

	// Token: 0x06002D03 RID: 11523 RVA: 0x00120949 File Offset: 0x0011EB49
	public void Write(PooledBinaryWriter bw, StreamModeWrite _eStreamMode)
	{
		bw.Write(2);
		this.itemGrid.Write(bw, _eStreamMode);
	}

	// Token: 0x06002D04 RID: 11524 RVA: 0x0012095F File Offset: 0x0011EB5F
	public static Bag Read(PooledBinaryReader br, XUiC_ItemStack.StackLocationTypes stackLocation, StreamModeRead _eStreamMode, IItemStackGridHolder _holder)
	{
		return new Bag(new Vector2i(0, 0), stackLocation, _holder).ReadInto(br, _eStreamMode);
	}

	// Token: 0x06002D05 RID: 11525 RVA: 0x00120978 File Offset: 0x0011EB78
	public Bag ReadInto(PooledBinaryReader br, StreamModeRead _eStreamMode)
	{
		this.isReading = true;
		Bag bag;
		try
		{
			bag = this.readInto(br, _eStreamMode);
		}
		finally
		{
			this.isReading = false;
		}
		return bag;
	}

	// Token: 0x06002D06 RID: 11526 RVA: 0x001209B0 File Offset: 0x0011EBB0
	[PublicizedFrom(EAccessModifier.Private)]
	public Bag readInto(PooledBinaryReader br, StreamModeRead _eStreamMode)
	{
		byte b = br.ReadByte();
		if (b >= 2)
		{
			this.itemGrid.ReadInto(br, _eStreamMode);
		}
		else
		{
			int num = (int)br.ReadUInt16();
			ItemStack[] array = ItemStack.CreateArray(num);
			for (int i = 0; i < num; i++)
			{
				array[i].Read(br);
			}
			PackedBoolArray packedBoolArray;
			if (br.ReadBoolean())
			{
				packedBoolArray = new PackedBoolArray(0);
				packedBoolArray.Read(br);
				packedBoolArray.Length = num;
			}
			else
			{
				packedBoolArray = new PackedBoolArray(num);
			}
			bool flag = false;
			PreferenceTracker preferenceTracker = null;
			if (b >= 1)
			{
				flag = br.ReadBoolean();
				if (br.ReadBoolean())
				{
					preferenceTracker = new PreferenceTracker(-1);
					preferenceTracker.Read(br);
				}
			}
			this.itemGrid.Resize(new Vector2i(num, 1));
			this.itemGrid.SetItems(array, false);
			this.itemGrid.SetSlotLocks(packedBoolArray);
			this.itemGrid.SetPreferences(preferenceTracker);
			if (flag)
			{
				this.itemGrid.Touch();
			}
		}
		return this;
	}

	// Token: 0x06002D07 RID: 11527 RVA: 0x00120A9C File Offset: 0x0011EC9C
	public void Clear()
	{
		this.itemGrid.ClearItems();
	}

	// Token: 0x06002D08 RID: 11528 RVA: 0x00120AAC File Offset: 0x0011ECAC
	public List<ItemStack> Resize(Vector2i _newSize)
	{
		int length = this.itemGrid.Length;
		List<ItemStack> list = this.itemGrid.Resize(_newSize);
		if (this.itemGrid.Length != length)
		{
			this.onBackpackChanged();
		}
		return list;
	}

	// Token: 0x06002D09 RID: 11529 RVA: 0x00120AE5 File Offset: 0x0011ECE5
	public void onSlotChanged(int _index)
	{
		if (this.isReading)
		{
			return;
		}
		this.onBackpackChanged();
	}

	// Token: 0x06002D0A RID: 11530 RVA: 0x00120AF6 File Offset: 0x0011ECF6
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

	// Token: 0x06002D0B RID: 11531 RVA: 0x00120B08 File Offset: 0x0011ED08
	public bool CanStackNoEmpty(ItemStack _itemStack)
	{
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			int num;
			if (this.itemGrid[i].CanStackPartlyWith(_itemStack, out num))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002D0C RID: 11532 RVA: 0x00120B44 File Offset: 0x0011ED44
	public bool IsEmpty()
	{
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			if (!this.itemGrid[i].IsEmpty())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002D0D RID: 11533 RVA: 0x00120B80 File Offset: 0x0011ED80
	public bool CanStack(ItemStack _itemStack)
	{
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			if (this.itemGrid[i].IsEmpty() || this.itemGrid[i].CanStackWith(_itemStack, false))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002D0E RID: 11534 RVA: 0x00120BD0 File Offset: 0x0011EDD0
	[return: TupleElementNames(new string[] { "anyMoved", "allMoved" })]
	public override ValueTuple<bool, bool> TryStackItem(int startIndex, ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(this.StackLocation, -1))
		{
			return new ValueTuple<bool, bool>(false, false);
		}
		int num = 0;
		bool flag = false;
		for (int i = startIndex; i < this.itemGrid.Length; i++)
		{
			num = _itemStack.count;
			if (_itemStack.itemValue.type == this.itemGrid[i].itemValue.type && this.itemGrid[i].CanStackPartly(ref num))
			{
				this.itemGrid[i].count += num;
				_itemStack.count -= num;
				flag = true;
				if (_itemStack.count == 0)
				{
					return new ValueTuple<bool, bool>(true, true);
				}
			}
		}
		return new ValueTuple<bool, bool>(flag, false);
	}

	// Token: 0x06002D0F RID: 11535 RVA: 0x00120C90 File Offset: 0x0011EE90
	public bool CanTakeItem(ItemStack _itemStack)
	{
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			int num;
			if (this.itemGrid[i].CanStackPartlyWith(_itemStack, out num))
			{
				return true;
			}
			if (this.itemGrid[i].IsEmpty())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002D10 RID: 11536 RVA: 0x00120CE1 File Offset: 0x0011EEE1
	public void SetSlots(ItemStack[] _slots, bool _resize = true)
	{
		if (_resize)
		{
			this.itemGrid.Resize(new Vector2i(_slots.Length, 1));
		}
		this.itemGrid.SetItems(_slots, false);
		this.onBackpackChanged();
	}

	// Token: 0x06002D11 RID: 11537 RVA: 0x00120D0E File Offset: 0x0011EF0E
	public void SetSlot(int index, ItemStack _stack)
	{
		if (index >= this.SlotCount)
		{
			return;
		}
		this.itemGrid.SetItem(index, _stack);
	}

	// Token: 0x06002D12 RID: 11538 RVA: 0x00120D28 File Offset: 0x0011EF28
	public override void PerformActionOnSlots(Action<ItemStack> _action)
	{
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			_action(this.itemGrid[i]);
		}
	}

	// Token: 0x06002D13 RID: 11539 RVA: 0x00120D5D File Offset: 0x0011EF5D
	public override bool AddItem(ItemStack _itemStack)
	{
		return _itemStack.CanMoveTo(this.StackLocation, -1) && this.itemGrid.AddItem(_itemStack, -1) >= 0;
	}

	// Token: 0x06002D14 RID: 11540 RVA: 0x00120D84 File Offset: 0x0011EF84
	public int DecItem(ItemValue _itemValue, int _count, bool _ignoreModdedItems = false, IList<ItemStack> _removedItems = null)
	{
		int num = _count;
		int num2 = 0;
		while (_count > 0 && num2 < this.itemGrid.Length)
		{
			ItemStack itemStack = this.itemGrid[num2];
			if (itemStack.itemValue.type == _itemValue.type && (!_ignoreModdedItems || !itemStack.itemValue.HasModSlots || !itemStack.itemValue.HasMods()))
			{
				if (ItemClass.GetForId(itemStack.itemValue.type).CanStack())
				{
					int count = itemStack.count;
					int num3 = ((count >= _count) ? _count : count);
					if (_removedItems != null)
					{
						_removedItems.Add(new ItemStack(itemStack.itemValue.Clone(), num3));
					}
					itemStack.count -= num3;
					_count -= num3;
					if (itemStack.count <= 0)
					{
						itemStack.Clear();
					}
				}
				else
				{
					if (_removedItems != null)
					{
						_removedItems.Add(itemStack.Clone());
					}
					itemStack.Clear();
					_count--;
				}
			}
			num2++;
		}
		return num - _count;
	}

	// Token: 0x06002D15 RID: 11541 RVA: 0x00120E80 File Offset: 0x0011F080
	public int GetUsedSlotCount()
	{
		int num = 0;
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			if (!this.itemGrid[i].IsEmpty())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06002D16 RID: 11542 RVA: 0x00120EC0 File Offset: 0x0011F0C0
	public int GetItemCount(ItemValue _itemValue, int _seed = -1, int _meta = -1, bool _ignoreModdedItems = true)
	{
		int num = 0;
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			ItemStack itemStack = this.itemGrid[i];
			if ((!_ignoreModdedItems || !itemStack.itemValue.HasModSlots || !itemStack.itemValue.HasMods()) && itemStack.itemValue.type == _itemValue.type && (_seed == -1 || _seed == (int)itemStack.itemValue.Seed) && (_meta == -1 || _meta == itemStack.itemValue.Meta))
			{
				num += itemStack.count;
			}
		}
		return num;
	}

	// Token: 0x06002D17 RID: 11543 RVA: 0x00120F54 File Offset: 0x0011F154
	public int GetItemCount(FastTags<TagGroup.Global> itemTags, int _seed = -1, int _meta = -1, bool _ignoreModdedItems = true)
	{
		int num = 0;
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			ItemStack itemStack = this.itemGrid[i];
			if ((!_ignoreModdedItems || !itemStack.itemValue.HasModSlots || !itemStack.itemValue.HasMods()) && !itemStack.itemValue.IsEmpty() && itemStack.itemValue.ItemClass.ItemTags.Test_AnySet(itemTags) && (_seed == -1 || _seed == (int)itemStack.itemValue.Seed) && (_meta == -1 || _meta == itemStack.itemValue.Meta))
			{
				num += itemStack.count;
			}
		}
		return num;
	}

	// Token: 0x06002D18 RID: 11544 RVA: 0x00120FFD File Offset: 0x0011F1FD
	public override bool HasItem(ItemValue _item)
	{
		return this.GetItemCount(_item, -1, -1, true) > 0;
	}

	// Token: 0x04002382 RID: 9090
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte Version = 2;

	// Token: 0x04002384 RID: 9092
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStackGrid itemGrid;

	// Token: 0x04002385 RID: 9093
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isReading;
}
