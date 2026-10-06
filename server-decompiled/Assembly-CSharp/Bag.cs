using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000570 RID: 1392
public class Bag : InventoryBase
{
	// Token: 0x14000036 RID: 54
	// (add) Token: 0x06002CEF RID: 11503 RVA: 0x001202A4 File Offset: 0x0011E4A4
	// (remove) Token: 0x06002CF0 RID: 11504 RVA: 0x001202DC File Offset: 0x0011E4DC
	public event XUiEvent_BackpackItemsChangedInternal OnBackpackItemsChangedInternal;

	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x06002CF1 RID: 11505 RVA: 0x00120311 File Offset: 0x0011E511
	public ItemStackGrid ItemGrid
	{
		get
		{
			return this.itemGrid;
		}
	}

	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x06002CF2 RID: 11506 RVA: 0x00120319 File Offset: 0x0011E519
	public XUiC_ItemStack.StackLocationTypes StackLocation
	{
		get
		{
			return this.itemGrid.StackLocation;
		}
	}

	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x06002CF3 RID: 11507 RVA: 0x00120326 File Offset: 0x0011E526
	public PackedBoolArray LockedSlots
	{
		get
		{
			return this.itemGrid.SlotLocks;
		}
	}

	// Token: 0x170004C4 RID: 1220
	// (get) Token: 0x06002CF4 RID: 11508 RVA: 0x00120333 File Offset: 0x0011E533
	public PreferenceTracker preferences
	{
		get
		{
			return this.itemGrid.Preferences;
		}
	}

	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x06002CF5 RID: 11509 RVA: 0x00120340 File Offset: 0x0011E540
	public bool Touched
	{
		get
		{
			return this.itemGrid.Touched;
		}
	}

	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x06002CF6 RID: 11510 RVA: 0x0012034D File Offset: 0x0011E54D
	public int SlotCount
	{
		get
		{
			return this.itemGrid.Length;
		}
	}

	// Token: 0x06002CF7 RID: 11511 RVA: 0x0012035A File Offset: 0x0011E55A
	public Bag(Vector2i _containerSize, XUiC_ItemStack.StackLocationTypes _stackLocation, IItemStackGridHolder _holder)
	{
		this.itemGrid = ItemStackGrid.Create(_containerSize, _stackLocation, true, true, _holder);
		this.itemGrid.SlotChanged += this.onSlotChanged;
	}

	// Token: 0x06002CF8 RID: 11512 RVA: 0x00120389 File Offset: 0x0011E589
	public void Write(PooledBinaryWriter bw, StreamModeWrite _eStreamMode)
	{
		bw.Write(2);
		this.itemGrid.Write(bw, _eStreamMode);
	}

	// Token: 0x06002CF9 RID: 11513 RVA: 0x0012039F File Offset: 0x0011E59F
	public static Bag Read(PooledBinaryReader br, XUiC_ItemStack.StackLocationTypes stackLocation, StreamModeRead _eStreamMode, IItemStackGridHolder _holder)
	{
		return new Bag(new Vector2i(0, 0), stackLocation, _holder).ReadInto(br, _eStreamMode);
	}

	// Token: 0x06002CFA RID: 11514 RVA: 0x001203B8 File Offset: 0x0011E5B8
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

	// Token: 0x06002CFB RID: 11515 RVA: 0x001203F0 File Offset: 0x0011E5F0
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

	// Token: 0x06002CFC RID: 11516 RVA: 0x001204DC File Offset: 0x0011E6DC
	public void Clear()
	{
		this.itemGrid.ClearItems();
	}

	// Token: 0x06002CFD RID: 11517 RVA: 0x001204EC File Offset: 0x0011E6EC
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

	// Token: 0x06002CFE RID: 11518 RVA: 0x00120525 File Offset: 0x0011E725
	public void onSlotChanged(int _index)
	{
		if (this.isReading)
		{
			return;
		}
		this.onBackpackChanged();
	}

	// Token: 0x06002CFF RID: 11519 RVA: 0x00120536 File Offset: 0x0011E736
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

	// Token: 0x06002D00 RID: 11520 RVA: 0x00120548 File Offset: 0x0011E748
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

	// Token: 0x06002D01 RID: 11521 RVA: 0x00120584 File Offset: 0x0011E784
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

	// Token: 0x06002D02 RID: 11522 RVA: 0x001205C0 File Offset: 0x0011E7C0
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

	// Token: 0x06002D03 RID: 11523 RVA: 0x00120610 File Offset: 0x0011E810
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

	// Token: 0x06002D04 RID: 11524 RVA: 0x001206D0 File Offset: 0x0011E8D0
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

	// Token: 0x06002D05 RID: 11525 RVA: 0x00120721 File Offset: 0x0011E921
	public void SetSlots(ItemStack[] _slots, bool _resize = true)
	{
		if (_resize)
		{
			this.itemGrid.Resize(new Vector2i(_slots.Length, 1));
		}
		this.itemGrid.SetItems(_slots, false);
		this.onBackpackChanged();
	}

	// Token: 0x06002D06 RID: 11526 RVA: 0x0012074E File Offset: 0x0011E94E
	public void SetSlot(int index, ItemStack _stack)
	{
		if (index >= this.SlotCount)
		{
			return;
		}
		this.itemGrid.SetItem(index, _stack);
	}

	// Token: 0x06002D07 RID: 11527 RVA: 0x00120768 File Offset: 0x0011E968
	public override void PerformActionOnSlots(Action<ItemStack> _action)
	{
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			_action(this.itemGrid[i]);
		}
	}

	// Token: 0x06002D08 RID: 11528 RVA: 0x0012079D File Offset: 0x0011E99D
	public override bool AddItem(ItemStack _itemStack)
	{
		return _itemStack.CanMoveTo(this.StackLocation, -1) && this.itemGrid.AddItem(_itemStack, -1) >= 0;
	}

	// Token: 0x06002D09 RID: 11529 RVA: 0x001207C4 File Offset: 0x0011E9C4
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

	// Token: 0x06002D0A RID: 11530 RVA: 0x001208C0 File Offset: 0x0011EAC0
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

	// Token: 0x06002D0B RID: 11531 RVA: 0x00120900 File Offset: 0x0011EB00
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

	// Token: 0x06002D0C RID: 11532 RVA: 0x00120994 File Offset: 0x0011EB94
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

	// Token: 0x06002D0D RID: 11533 RVA: 0x00120A3D File Offset: 0x0011EC3D
	public override bool HasItem(ItemValue _item)
	{
		return this.GetItemCount(_item, -1, -1, true) > 0;
	}

	// Token: 0x04002367 RID: 9063
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte Version = 2;

	// Token: 0x04002369 RID: 9065
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStackGrid itemGrid;

	// Token: 0x0400236A RID: 9066
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isReading;
}
