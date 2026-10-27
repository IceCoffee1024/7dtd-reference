using System;
using System.Collections.Generic;

// Token: 0x02000586 RID: 1414
public class ItemStackGrid
{
	// Token: 0x06002DF3 RID: 11763 RVA: 0x001244E8 File Offset: 0x001226E8
	public ItemStackGrid.Address GetAddress()
	{
		if (this.holder == null)
		{
			return default(ItemStackGrid.Address);
		}
		return new ItemStackGrid.Address(this.holder.GetAddress(), this.holder.GetItemStackGridKey(this));
	}

	// Token: 0x14000038 RID: 56
	// (add) Token: 0x06002DF4 RID: 11764 RVA: 0x00124524 File Offset: 0x00122724
	// (remove) Token: 0x06002DF5 RID: 11765 RVA: 0x0012455C File Offset: 0x0012275C
	public event Action<int> SlotChanged;

	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x06002DF6 RID: 11766 RVA: 0x00124591 File Offset: 0x00122791
	// (set) Token: 0x06002DF7 RID: 11767 RVA: 0x00124599 File Offset: 0x00122799
	public Vector2i ContainerSize
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x06002DF8 RID: 11768 RVA: 0x001245A2 File Offset: 0x001227A2
	// (set) Token: 0x06002DF9 RID: 11769 RVA: 0x001245AA File Offset: 0x001227AA
	public XUiC_ItemStack.StackLocationTypes StackLocation
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x06002DFA RID: 11770 RVA: 0x001245B3 File Offset: 0x001227B3
	public int Length
	{
		get
		{
			return this.items.Length;
		}
	}

	// Token: 0x170004F9 RID: 1273
	public ItemStack this[int _index]
	{
		get
		{
			return this.items[_index];
		}
		set
		{
			this.SetItem(_index, value);
		}
	}

	// Token: 0x170004FA RID: 1274
	// (get) Token: 0x06002DFD RID: 11773 RVA: 0x001245D1 File Offset: 0x001227D1
	public PackedBoolArray SlotLocks
	{
		get
		{
			return this.slotLocks;
		}
	}

	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x06002DFE RID: 11774 RVA: 0x001245D9 File Offset: 0x001227D9
	public PreferenceTracker Preferences
	{
		get
		{
			return this.preferences;
		}
	}

	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x06002DFF RID: 11775 RVA: 0x001245E1 File Offset: 0x001227E1
	public bool Touched
	{
		get
		{
			return this.worldTimeTouched > 0UL;
		}
	}

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x06002E00 RID: 11776 RVA: 0x001245ED File Offset: 0x001227ED
	// (set) Token: 0x06002E01 RID: 11777 RVA: 0x001245F5 File Offset: 0x001227F5
	public ulong WorldTimeTouched
	{
		get
		{
			return this.worldTimeTouched;
		}
		set
		{
			this.worldTimeTouched = value;
		}
	}

	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x06002E02 RID: 11778 RVA: 0x001245FE File Offset: 0x001227FE
	// (set) Token: 0x06002E03 RID: 11779 RVA: 0x00124606 File Offset: 0x00122806
	public bool PlayerOwned
	{
		get
		{
			return this.playerOwned;
		}
		set
		{
			this.playerOwned = value;
		}
	}

	// Token: 0x06002E04 RID: 11780 RVA: 0x0012460F File Offset: 0x0012280F
	public static ItemStackGrid Create(Vector2i _containerSize, XUiC_ItemStack.StackLocationTypes _stackLocation, bool _hasLocks, bool _hasPreferences, IItemStackGridHolder _holder)
	{
		return new ItemStackGrid(_containerSize, _stackLocation, _hasLocks, _hasPreferences, _holder);
	}

	// Token: 0x06002E05 RID: 11781 RVA: 0x0012461C File Offset: 0x0012281C
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStackGrid(Vector2i _containerSize, XUiC_ItemStack.StackLocationTypes _stackLocation, bool _hasLocks, bool _hasPreferences, IItemStackGridHolder _holder)
	{
		this.holder = _holder;
		this.ContainerSize = _containerSize;
		this.StackLocation = _stackLocation;
		int num = _containerSize.x * _containerSize.y;
		this.items = new ItemStack[num];
		for (int i = 0; i < num; i++)
		{
			this.items[i] = this.CreateSlot(i);
		}
		this.slotLocks = (_hasLocks ? new PackedBoolArray(num) : null);
		this.preferences = (_hasPreferences ? new PreferenceTracker(-1) : null);
		this.worldTimeTouched = 0UL;
	}

	// Token: 0x06002E06 RID: 11782 RVA: 0x001246A7 File Offset: 0x001228A7
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStackGrid()
	{
		this.items = Array.Empty<ItemStack>();
	}

	// Token: 0x06002E07 RID: 11783 RVA: 0x001246BA File Offset: 0x001228BA
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack CreateSlot(int _slotIndex)
	{
		ItemStack itemStack = new ItemStack();
		itemStack.BindGridSlot(this, _slotIndex);
		return itemStack;
	}

	// Token: 0x06002E08 RID: 11784 RVA: 0x001246C9 File Offset: 0x001228C9
	public void OnSlotChanged(int _slotIndex)
	{
		if (!this.suppressChanged)
		{
			Action<int> slotChanged = this.SlotChanged;
			if (slotChanged == null)
			{
				return;
			}
			slotChanged(_slotIndex);
		}
	}

	// Token: 0x06002E09 RID: 11785 RVA: 0x001246E4 File Offset: 0x001228E4
	public void Touch()
	{
		this.worldTimeTouched = GameManager.Instance.World.GetWorldTime();
	}

	// Token: 0x06002E0A RID: 11786 RVA: 0x001246FC File Offset: 0x001228FC
	public void CopyFrom(ItemStackGrid _other, bool _resize = true)
	{
		if (_resize)
		{
			this.Resize(_other.ContainerSize);
		}
		this.suppressChanged = true;
		int num = Utils.FastMin(this.items.Length, _other.items.Length);
		for (int i = 0; i < num; i++)
		{
			ItemStack itemStack = _other.items[i];
			this.items[i].Set(itemStack.itemValue.Clone(), itemStack.count);
		}
		this.suppressChanged = false;
		if (this.slotLocks != null && _other.slotLocks != null && this.slotLocks.Length == _other.slotLocks.Length)
		{
			this.slotLocks.CopyFrom(_other.slotLocks);
		}
		PreferenceTracker preferenceTracker = _other.preferences;
		this.preferences = ((preferenceTracker != null) ? preferenceTracker.Clone() : null);
		this.worldTimeTouched = _other.worldTimeTouched;
		this.playerOwned = _other.playerOwned;
		for (int j = 0; j < num; j++)
		{
			Action<int> slotChanged = this.SlotChanged;
			if (slotChanged != null)
			{
				slotChanged(j);
			}
		}
	}

	// Token: 0x06002E0B RID: 11787 RVA: 0x001247F8 File Offset: 0x001229F8
	public List<ItemStack> Resize(Vector2i _containerSize)
	{
		int num = _containerSize.x * _containerSize.y;
		bool flag = num == this.Length;
		this.ContainerSize = _containerSize;
		if (flag)
		{
			return null;
		}
		ItemStack[] array = new ItemStack[num];
		List<ItemStack> list = null;
		for (int i = 0; i < this.items.Length; i++)
		{
			if (i < num)
			{
				array[i] = this.items[i];
			}
			else
			{
				this.items[i].BindGridSlot(null, -1);
				if (!this.items[i].IsEmpty())
				{
					if (list == null)
					{
						list = new List<ItemStack>();
					}
					list.Add(this.items[i]);
				}
			}
		}
		for (int j = this.items.Length; j < num; j++)
		{
			array[j] = this.CreateSlot(j);
		}
		this.items = array;
		if (this.slotLocks != null)
		{
			this.slotLocks.Length = num;
		}
		return list;
	}

	// Token: 0x06002E0C RID: 11788 RVA: 0x001248C8 File Offset: 0x00122AC8
	public bool Reset()
	{
		if (this.PlayerOwned)
		{
			return false;
		}
		this.Clear();
		this.worldTimeTouched = 0UL;
		return true;
	}

	// Token: 0x06002E0D RID: 11789 RVA: 0x001248E3 File Offset: 0x00122AE3
	public void Clear()
	{
		this.ClearItems();
		PackedBoolArray packedBoolArray = this.slotLocks;
		if (packedBoolArray != null)
		{
			packedBoolArray.Clear();
		}
		PreferenceTracker preferenceTracker = this.preferences;
		if (preferenceTracker == null)
		{
			return;
		}
		preferenceTracker.Clear();
	}

	// Token: 0x06002E0E RID: 11790 RVA: 0x0012490C File Offset: 0x00122B0C
	public void SetSlotLocks(PackedBoolArray _slotLocks)
	{
		if (_slotLocks == null)
		{
			this.slotLocks = null;
			return;
		}
		if (_slotLocks.Length != this.Length)
		{
			Log.Error(string.Format("[{0}] Cannot set slot locks. Expected length {1}, received {2}. {3} should be called first.", new object[]
			{
				"ItemStackGrid",
				this.items.Length,
				_slotLocks.Length,
				"Resize"
			}));
			return;
		}
		this.slotLocks = _slotLocks;
	}

	// Token: 0x06002E0F RID: 11791 RVA: 0x0012497D File Offset: 0x00122B7D
	public void SetPreferences(PreferenceTracker _preferences)
	{
		this.preferences = _preferences;
	}

	// Token: 0x06002E10 RID: 11792 RVA: 0x00124986 File Offset: 0x00122B86
	public bool IsLocked(int _index)
	{
		return this.slotLocks != null && this.slotLocks[_index];
	}

	// Token: 0x06002E11 RID: 11793 RVA: 0x001249A0 File Offset: 0x00122BA0
	public bool IsSlotLocked(int _index)
	{
		PackedBoolArray packedBoolArray = this.slotLocks;
		int? num = ((packedBoolArray != null) ? new int?(packedBoolArray.Length) : null);
		return !((_index >= num.GetValueOrDefault()) & (num != null)) && this.slotLocks != null && this.slotLocks[_index];
	}

	// Token: 0x06002E12 RID: 11794 RVA: 0x001249FC File Offset: 0x00122BFC
	public void SetLocked(int _index, bool _value)
	{
		if (this.slotLocks == null || this.slotLocks[_index] == _value)
		{
			return;
		}
		this.slotLocks[_index] = _value;
		Action<int> slotChanged = this.SlotChanged;
		if (slotChanged == null)
		{
			return;
		}
		slotChanged(_index);
	}

	// Token: 0x06002E13 RID: 11795 RVA: 0x00124A34 File Offset: 0x00122C34
	public static ItemStackGrid Read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		ItemStackGrid itemStackGrid = new ItemStackGrid();
		itemStackGrid.ReadInto(_br, _eStreamMode);
		return itemStackGrid;
	}

	// Token: 0x06002E14 RID: 11796 RVA: 0x00124A44 File Offset: 0x00122C44
	public void ReadInto(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		ushort num = _br.ReadUInt16();
		Vector2i vector2i = StreamUtils.ReadVector2i(_br);
		int num2 = (int)_br.ReadInt16();
		if (vector2i.x < 0 || vector2i.y < 0 || (long)num2 != (long)vector2i.x * (long)vector2i.y)
		{
			throw new InvalidOperationException(string.Format("[{0}] Stream slot count {1} does not match container size {2}", "ItemStackGrid", num2, vector2i));
		}
		this.Resize(vector2i);
		this.suppressChanged = true;
		for (int i = 0; i < num2; i++)
		{
			this.items[i].Read(_br);
		}
		this.suppressChanged = false;
		if (_br.ReadBoolean())
		{
			if (this.slotLocks == null)
			{
				this.slotLocks = new PackedBoolArray(this.items.Length);
			}
			this.slotLocks.Read(_br);
		}
		else
		{
			this.slotLocks = null;
		}
		if (_br.ReadBoolean())
		{
			if (this.preferences == null)
			{
				this.preferences = new PreferenceTracker(-1);
			}
			this.preferences.Read(_br);
		}
		else
		{
			this.preferences = null;
		}
		if (num >= 2)
		{
			this.worldTimeTouched = _br.ReadUInt64();
			this.playerOwned = _br.ReadBoolean();
		}
	}

	// Token: 0x06002E15 RID: 11797 RVA: 0x00124B64 File Offset: 0x00122D64
	public void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		_bw.Write(2);
		StreamUtils.Write(_bw, this.ContainerSize);
		_bw.Write((short)this.items.Length);
		for (int i = 0; i < this.items.Length; i++)
		{
			this.items[i].Write(_bw);
		}
		bool flag = this.slotLocks != null;
		_bw.Write(flag);
		if (flag)
		{
			this.slotLocks.Write(_bw);
		}
		bool flag2 = this.preferences != null;
		_bw.Write(flag2);
		if (flag2)
		{
			this.preferences.Write(_bw);
		}
		_bw.Write(this.worldTimeTouched);
		_bw.Write(this.playerOwned);
	}

	// Token: 0x06002E16 RID: 11798 RVA: 0x001245BD File Offset: 0x001227BD
	public ItemStack GetItem(int _index)
	{
		return this.items[_index];
	}

	// Token: 0x06002E17 RID: 11799 RVA: 0x00124C0C File Offset: 0x00122E0C
	public ItemStack GetItemClone(int _index)
	{
		return this.items[_index].Clone();
	}

	// Token: 0x06002E18 RID: 11800 RVA: 0x00124C1B File Offset: 0x00122E1B
	public void SetItem(int _index, ItemStack _stack)
	{
		if (_stack == null || _stack.count <= 0)
		{
			this.items[_index].Clear();
			return;
		}
		this.items[_index].Set(_stack.itemValue.Clone(), _stack.count);
	}

	// Token: 0x06002E19 RID: 11801 RVA: 0x00124C58 File Offset: 0x00122E58
	public void SetItems(ItemStack[] _items, bool _fireSlotChanged = true)
	{
		if (_items == null || _items.Length != this.Length)
		{
			Log.Error(string.Format("[{0}] Cannot set slots. Expected length {1}, received {2}. {3} should be called first.", new object[]
			{
				"ItemStackGrid",
				this.items.Length,
				(_items != null) ? _items.Length : 0,
				"Resize"
			}));
			return;
		}
		this.suppressChanged = !_fireSlotChanged;
		for (int i = 0; i < _items.Length; i++)
		{
			ItemStack itemStack = _items[i];
			if (itemStack == null)
			{
				this.items[i].Set(ItemValue.None, 0);
			}
			else
			{
				this.items[i].Set(itemStack.itemValue.Clone(), itemStack.count);
			}
		}
		this.suppressChanged = false;
	}

	// Token: 0x06002E1A RID: 11802 RVA: 0x00124D14 File Offset: 0x00122F14
	public int AddItem(ItemStack _itemStack, int _maxIndex = -1)
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			if (this.items[i].CanStackWith(_itemStack, false))
			{
				this.items[i].count += _itemStack.count;
				_itemStack.count = 0;
				return i;
			}
		}
		int num = ((_maxIndex < 0) ? this.items.Length : Utils.FastMin(_maxIndex, this.items.Length));
		for (int j = 0; j < num; j++)
		{
			if (this.items[j].IsEmpty())
			{
				this.items[j].Set(_itemStack.itemValue.Clone(), _itemStack.count);
				return j;
			}
		}
		return -1;
	}

	// Token: 0x06002E1B RID: 11803 RVA: 0x00124DC4 File Offset: 0x00122FC4
	public int ChangeCount(int _index, int _delta)
	{
		if (_delta == 0)
		{
			return 0;
		}
		ItemStack itemStack = this.items[_index];
		if (itemStack.IsEmpty())
		{
			return 0;
		}
		int num;
		if (_delta > 0)
		{
			num = _delta;
			if (!itemStack.CanStackPartly(ref num))
			{
				return 0;
			}
			itemStack.count += num;
		}
		else
		{
			num = -Utils.FastMin(-_delta, itemStack.count);
			if (itemStack.count + num <= 0)
			{
				itemStack.Clear();
			}
			else
			{
				itemStack.count += num;
			}
		}
		return num;
	}

	// Token: 0x06002E1C RID: 11804 RVA: 0x00124E3C File Offset: 0x0012303C
	public IList<ItemStack> CopyItemsFrom(ItemStackGrid _source)
	{
		int num = Utils.FastMin(this.items.Length, _source.items.Length);
		for (int i = 0; i < num; i++)
		{
			this.SetItem(i, _source.items[i]);
		}
		List<ItemStack> list = null;
		for (int j = num; j < _source.items.Length; j++)
		{
			ItemStack itemStack = _source.items[j];
			if (!itemStack.IsEmpty())
			{
				if (list == null)
				{
					list = new List<ItemStack>();
				}
				list.Add(itemStack.Clone());
			}
		}
		return list;
	}

	// Token: 0x06002E1D RID: 11805 RVA: 0x00124EBC File Offset: 0x001230BC
	public ItemStack[] CloneItems()
	{
		ItemStack[] array = new ItemStack[this.items.Length];
		for (int i = 0; i < this.items.Length; i++)
		{
			array[i] = this.items[i].Clone();
		}
		return array;
	}

	// Token: 0x06002E1E RID: 11806 RVA: 0x00124EFC File Offset: 0x001230FC
	public void ClearItems()
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			this.items[i].Clear();
		}
	}

	// Token: 0x06002E1F RID: 11807 RVA: 0x00124F29 File Offset: 0x00123129
	public void Compact()
	{
		this.suppressChanged = true;
		this.CompactSlots();
		this.suppressChanged = false;
		this.NotifyUnlockedSlotsChanged();
	}

	// Token: 0x06002E20 RID: 11808 RVA: 0x00124F48 File Offset: 0x00123148
	public void Sort()
	{
		this.suppressChanged = true;
		this.CompactSlots();
		for (int i = 0; i < this.items.Length; i++)
		{
			if (!this.IsLocked(i))
			{
				int num = i;
				for (int j = i + 1; j < this.items.Length; j++)
				{
					if (!this.IsLocked(j) && ItemStackGrid.CompareForSort(this.items[j], this.items[num]) < 0)
					{
						num = j;
					}
				}
				if (num != i)
				{
					this.items[i].SwapWith(this.items[num]);
				}
			}
		}
		this.suppressChanged = false;
		this.NotifyUnlockedSlotsChanged();
	}

	// Token: 0x06002E21 RID: 11809 RVA: 0x00124FE0 File Offset: 0x001231E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void NotifyUnlockedSlotsChanged()
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			if (!this.IsLocked(i))
			{
				Action<int> slotChanged = this.SlotChanged;
				if (slotChanged != null)
				{
					slotChanged(i);
				}
			}
		}
	}

	// Token: 0x06002E22 RID: 11810 RVA: 0x0012501C File Offset: 0x0012321C
	[PublicizedFrom(EAccessModifier.Private)]
	public void CompactSlots()
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			ItemStack itemStack = this.items[i];
			if (!this.IsLocked(i))
			{
				if (itemStack.IsEmpty())
				{
					for (int j = i + 1; j < this.items.Length; j++)
					{
						if (!this.IsLocked(j) && !this.items[j].IsEmpty())
						{
							itemStack.SwapWith(this.items[j]);
							break;
						}
					}
					if (itemStack.IsEmpty())
					{
						return;
					}
				}
				ItemClass itemClass = itemStack.itemValue.ItemClass;
				if (itemClass != null && !itemClass.HasQuality)
				{
					int num = itemClass.MaxCount - itemStack.count;
					int num2 = i + 1;
					while (num2 < this.items.Length && num > 0)
					{
						ItemStack itemStack2 = this.items[num2];
						if (!this.IsLocked(num2) && !itemStack2.IsEmpty() && itemStack2.itemValue.type == itemStack.itemValue.type)
						{
							int num3 = Utils.FastMin(itemStack2.count, num);
							itemStack.count += num3;
							itemStack2.count -= num3;
							num -= num3;
							if (itemStack2.count == 0)
							{
								itemStack2.Clear();
							}
						}
						num2++;
					}
				}
			}
		}
	}

	// Token: 0x06002E23 RID: 11811 RVA: 0x00125174 File Offset: 0x00123374
	[PublicizedFrom(EAccessModifier.Private)]
	public static int CompareForSort(ItemStack _a, ItemStack _b)
	{
		bool flag = _a.IsEmpty();
		bool flag2 = _b.IsEmpty();
		if (flag || flag2)
		{
			return flag.CompareTo(flag2);
		}
		ItemClass itemClass = _a.itemValue.ItemClass;
		ItemClass itemClass2 = _b.itemValue.ItemClass;
		int num;
		if (itemClass == null || itemClass2 == null)
		{
			num = (itemClass == null).CompareTo(itemClass2 == null);
		}
		else
		{
			bool flag3 = itemClass.IsBlock();
			num = itemClass2.IsBlock().CompareTo(flag3);
			if (num == 0 && !flag3)
			{
				num = string.Compare(itemClass.Groups[0], itemClass2.Groups[0], StringComparison.CurrentCulture);
			}
			if (num == 0)
			{
				num = string.Compare(itemClass.GetLocalizedItemName(), itemClass2.GetLocalizedItemName(), StringComparison.CurrentCulture);
			}
		}
		if (num == 0)
		{
			num = _a.itemValue.Quality.CompareTo(_b.itemValue.Quality);
		}
		if (num == 0)
		{
			num = _b.itemValue.UseTimes.CompareTo(_a.itemValue.UseTimes);
		}
		return num;
	}

	// Token: 0x040023D4 RID: 9172
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IItemStackGridHolder holder;

	// Token: 0x040023D5 RID: 9173
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort Version = 2;

	// Token: 0x040023D6 RID: 9174
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] items;

	// Token: 0x040023D7 RID: 9175
	[PublicizedFrom(EAccessModifier.Private)]
	public PackedBoolArray slotLocks;

	// Token: 0x040023D8 RID: 9176
	[PublicizedFrom(EAccessModifier.Private)]
	public PreferenceTracker preferences;

	// Token: 0x040023D9 RID: 9177
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong worldTimeTouched;

	// Token: 0x040023DA RID: 9178
	[PublicizedFrom(EAccessModifier.Private)]
	public bool suppressChanged;

	// Token: 0x040023DB RID: 9179
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playerOwned;

	// Token: 0x02000587 RID: 1415
	public readonly struct Address
	{
		// Token: 0x06002E24 RID: 11812 RVA: 0x0012526F File Offset: 0x0012346F
		public Address(WorldAddress _holderAddress, byte _key)
		{
			this.HolderAddress = _holderAddress;
			this.Key = _key;
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x0012527F File Offset: 0x0012347F
		public ItemStackGrid Resolve()
		{
			IItemStackGridHolder itemStackGridHolder = this.HolderAddress.Resolve<IItemStackGridHolder>();
			if (itemStackGridHolder == null)
			{
				return null;
			}
			return itemStackGridHolder.GetItemStackGrid(this.Key);
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x0012529D File Offset: 0x0012349D
		public void Write(PooledBinaryWriter _bw)
		{
			this.HolderAddress.Write(_bw);
			_bw.Write(this.Key);
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x001252B7 File Offset: 0x001234B7
		public static ItemStackGrid.Address Read(PooledBinaryReader _br)
		{
			return new ItemStackGrid.Address(WorldAddress.Read(_br), _br.ReadByte());
		}

		// Token: 0x040023DF RID: 9183
		public readonly WorldAddress HolderAddress;

		// Token: 0x040023E0 RID: 9184
		public readonly byte Key;
	}
}
