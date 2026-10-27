using System;
using System.Collections.Generic;

// Token: 0x02000582 RID: 1410
public class ItemStack
{
	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x06002DC8 RID: 11720 RVA: 0x00123D03 File Offset: 0x00121F03
	public static ItemStack Empty
	{
		get
		{
			return new ItemStack(ItemValue.None, 0);
		}
	}

	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x06002DC9 RID: 11721 RVA: 0x00123D10 File Offset: 0x00121F10
	// (set) Token: 0x06002DCA RID: 11722 RVA: 0x00123D18 File Offset: 0x00121F18
	public ItemValue itemValue
	{
		get
		{
			return this.itemValueField;
		}
		set
		{
			if (this.itemValueField == value)
			{
				return;
			}
			ItemValue itemValue = this.itemValueField;
			if (itemValue != null)
			{
				itemValue.BindItemStack(null);
			}
			this.itemValueField = value;
			ItemValue itemValue2 = this.itemValueField;
			if (itemValue2 != null)
			{
				itemValue2.BindItemStack(this);
			}
			this.NotifyChanged();
		}
	}

	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x06002DCB RID: 11723 RVA: 0x00123D55 File Offset: 0x00121F55
	// (set) Token: 0x06002DCC RID: 11724 RVA: 0x00123D5D File Offset: 0x00121F5D
	public int count
	{
		get
		{
			return this.countField;
		}
		set
		{
			if (this.countField == value)
			{
				return;
			}
			this.countField = value;
			this.NotifyChanged();
		}
	}

	// Token: 0x06002DCD RID: 11725 RVA: 0x00123D76 File Offset: 0x00121F76
	public void BindGridSlot(ItemStackGrid _owner, int _slot = -1)
	{
		this.owner = _owner;
		this.ownerSlot = _slot;
	}

	// Token: 0x06002DCE RID: 11726 RVA: 0x00123D86 File Offset: 0x00121F86
	public void OnValueChanged()
	{
		this.NotifyChanged();
	}

	// Token: 0x06002DCF RID: 11727 RVA: 0x00123D8E File Offset: 0x00121F8E
	[PublicizedFrom(EAccessModifier.Private)]
	public void NotifyChanged()
	{
		ItemStackGrid itemStackGrid = this.owner;
		if (itemStackGrid == null)
		{
			return;
		}
		itemStackGrid.OnSlotChanged(this.ownerSlot);
	}

	// Token: 0x06002DD0 RID: 11728 RVA: 0x00123DA6 File Offset: 0x00121FA6
	public ItemStack()
	{
		this.itemValue = ItemValue.None;
		this.count = 0;
	}

	// Token: 0x06002DD1 RID: 11729 RVA: 0x00123DC7 File Offset: 0x00121FC7
	public ItemStack(ItemValue _itemValue, int _count)
	{
		this.itemValue = _itemValue;
		this.count = _count;
	}

	// Token: 0x06002DD2 RID: 11730 RVA: 0x00123DE4 File Offset: 0x00121FE4
	public ItemStack Clone()
	{
		if (this.itemValue != null)
		{
			return new ItemStack(this.itemValue.Clone(), this.count);
		}
		return new ItemStack(ItemValue.None, this.count);
	}

	// Token: 0x06002DD3 RID: 11731 RVA: 0x00123E18 File Offset: 0x00122018
	public static ItemStack[] CreateArray(int _size)
	{
		ItemStack[] array = new ItemStack[_size];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = ItemStack.Empty;
		}
		return array;
	}

	// Token: 0x06002DD4 RID: 11732 RVA: 0x00123E43 File Offset: 0x00122043
	public bool IsEmpty()
	{
		return this.count < 1 || this.itemValue.type == 0;
	}

	// Token: 0x06002DD5 RID: 11733 RVA: 0x00123E5E File Offset: 0x0012205E
	public void Clear()
	{
		this.countField = 0;
		this.itemValue.Clear();
	}

	// Token: 0x06002DD6 RID: 11734 RVA: 0x00123E72 File Offset: 0x00122072
	public void Set(ItemValue _itemValue, int _count)
	{
		this.countField = _count;
		this.itemValue = _itemValue;
		this.NotifyChanged();
	}

	// Token: 0x06002DD7 RID: 11735 RVA: 0x00123E88 File Offset: 0x00122088
	public void SwapWith(ItemStack _other)
	{
		ItemValue itemValue = this.itemValueField;
		int num = this.countField;
		this.itemValueField = _other.itemValueField;
		this.countField = _other.countField;
		_other.itemValueField = itemValue;
		_other.countField = num;
		ItemValue itemValue2 = this.itemValueField;
		if (itemValue2 != null)
		{
			itemValue2.BindItemStack(this);
		}
		ItemValue itemValue3 = _other.itemValueField;
		if (itemValue3 != null)
		{
			itemValue3.BindItemStack(_other);
		}
		this.NotifyChanged();
		_other.NotifyChanged();
	}

	// Token: 0x06002DD8 RID: 11736 RVA: 0x00123EFC File Offset: 0x001220FC
	public static int AddToItemStackArray(ItemStack[] _itemStackArr, ItemStack _itemStack, int maxItemCount = -1)
	{
		int num = -1;
		int num2 = 0;
		while (num == -1 && num2 < _itemStackArr.Length)
		{
			if (_itemStackArr[num2].CanStackWith(_itemStack, false))
			{
				_itemStackArr[num2].count += _itemStack.count;
				_itemStack.count = 0;
				num = num2;
			}
			num2++;
		}
		int num3 = 0;
		while (num == -1 && num3 < _itemStackArr.Length && (maxItemCount == -1 || num3 != maxItemCount))
		{
			if (_itemStackArr[num3].IsEmpty())
			{
				_itemStackArr[num3].Set(_itemStack.itemValue.Clone(), _itemStack.count);
				num = num3;
			}
			num3++;
		}
		return num;
	}

	// Token: 0x06002DD9 RID: 11737 RVA: 0x00123F87 File Offset: 0x00122187
	public ItemStack ReadOld(PooledBinaryReader _br)
	{
		this.itemValue.ReadOld(_br);
		this.count = (int)_br.ReadInt16();
		return this;
	}

	// Token: 0x06002DDA RID: 11738 RVA: 0x00123FA2 File Offset: 0x001221A2
	public ItemStack Read(PooledBinaryReader _br)
	{
		this.countField = (int)_br.ReadUInt16();
		if (this.countField > 0)
		{
			this.itemValue.Read(_br);
		}
		else
		{
			this.itemValue = ItemValue.None;
		}
		return this;
	}

	// Token: 0x06002DDB RID: 11739 RVA: 0x00123FD4 File Offset: 0x001221D4
	public ItemStack ReadDelta(PooledBinaryReader _br, ItemStack _last)
	{
		this.itemValue.Read(_br);
		int num = (int)_br.ReadInt16();
		this.count = _last.count + num;
		return this;
	}

	// Token: 0x06002DDC RID: 11740 RVA: 0x00124004 File Offset: 0x00122204
	public void Write(PooledBinaryWriter _bw)
	{
		int num = this.count;
		if (num > 65535)
		{
			num = 65535;
		}
		_bw.Write((ushort)num);
		if (this.count != 0)
		{
			this.itemValue.Write(_bw);
		}
	}

	// Token: 0x06002DDD RID: 11741 RVA: 0x00124042 File Offset: 0x00122242
	public void WriteDelta(PooledBinaryWriter _bw, ItemStack _last)
	{
		this.itemValue.Write(_bw);
		_bw.Write((short)(this.count - _last.count));
		_last.count += this.count - _last.count;
	}

	// Token: 0x06002DDE RID: 11742 RVA: 0x00124080 File Offset: 0x00122280
	public static ItemStack[] ReadArray(PooledBinaryReader _br)
	{
		int num = (int)_br.ReadInt16();
		if (num == -1)
		{
			return null;
		}
		ItemStack[] array = ItemStack.CreateArray(num);
		for (int i = 0; i < num; i++)
		{
			array[i].Read(_br);
		}
		return array;
	}

	// Token: 0x06002DDF RID: 11743 RVA: 0x001240B8 File Offset: 0x001222B8
	public static void WriteArray(PooledBinaryWriter _bw, ItemStack[] _items)
	{
		if (_items == null)
		{
			_bw.Write(-1);
			return;
		}
		_bw.Write((short)_items.Length);
		for (int i = 0; i < _items.Length; i++)
		{
			_items[i].Write(_bw);
		}
	}

	// Token: 0x06002DE0 RID: 11744 RVA: 0x001240F4 File Offset: 0x001222F4
	public override bool Equals(object _other)
	{
		ItemStack itemStack = _other as ItemStack;
		return itemStack != null && itemStack.count == this.count && itemStack.itemValue.Equals(this.itemValue);
	}

	// Token: 0x06002DE1 RID: 11745 RVA: 0x0012412E File Offset: 0x0012232E
	public override int GetHashCode()
	{
		return this.itemValue.GetHashCode() * 13 + this.count;
	}

	// Token: 0x06002DE2 RID: 11746 RVA: 0x00124148 File Offset: 0x00122348
	public int StackTransferCount(ItemStack _other)
	{
		if (_other.itemValue.type != this.itemValue.type)
		{
			return 0;
		}
		return Math.Min(ItemClass.GetForId(this.itemValue.type).MaxCount - this.count, _other.count);
	}

	// Token: 0x06002DE3 RID: 11747 RVA: 0x00124198 File Offset: 0x00122398
	public bool CanStackWith(ItemStack _other, bool allowPartialStack = false)
	{
		int count = _other.count;
		if (_other.itemValue == null || this.itemValue == null || _other.itemValue.type != this.itemValue.type || (this.itemValue.type < Block.ItemsStartHere && !(_other.itemValue.TextureFullArray == this.itemValue.TextureFullArray) && !this.itemValue.IsShapeHelperBlock))
		{
			return false;
		}
		if (!allowPartialStack)
		{
			return this.CanStack(_other.count);
		}
		return this.CanStackPartly(ref count);
	}

	// Token: 0x06002DE4 RID: 11748 RVA: 0x0012422A File Offset: 0x0012242A
	public bool CanStack(int _count)
	{
		return this.itemValue.type == 0 || ItemClass.GetForId(this.itemValue.type).MaxCount >= _count + this.count;
	}

	// Token: 0x06002DE5 RID: 11749 RVA: 0x0012425D File Offset: 0x0012245D
	public bool CanStackPartlyWith(ItemStack _other, out int _count)
	{
		_count = _other.count;
		return this.CanStackWith(_other, true) && this.CanStackPartly(ref _count);
	}

	// Token: 0x06002DE6 RID: 11750 RVA: 0x0012427A File Offset: 0x0012247A
	public bool CanStackPartly(ref int _count)
	{
		if (this.itemValue.type == 0)
		{
			return false;
		}
		_count = Utils.FastMin(ItemClass.GetForId(this.itemValue.type).MaxCount - this.count, _count);
		return _count > 0;
	}

	// Token: 0x06002DE7 RID: 11751 RVA: 0x001242B8 File Offset: 0x001224B8
	public void Deactivate()
	{
		ItemClass forId = ItemClass.GetForId(this.itemValue.type);
		if (forId != null)
		{
			forId.Deactivate(this.itemValue);
		}
	}

	// Token: 0x06002DE8 RID: 11752 RVA: 0x001242E8 File Offset: 0x001224E8
	public static ItemStack[] Clone(IList<ItemStack> _itemStackArr)
	{
		if (_itemStackArr == null)
		{
			return null;
		}
		ItemStack[] array = new ItemStack[_itemStackArr.Count];
		for (int i = 0; i < array.Length; i++)
		{
			ItemStack itemStack = _itemStackArr[i];
			array[i] = ((itemStack != null) ? itemStack.Clone() : ItemStack.Empty);
		}
		return array;
	}

	// Token: 0x06002DE9 RID: 11753 RVA: 0x00124330 File Offset: 0x00122530
	public static ItemStack[] Clone(ItemStack[] _itemStackArr, int _startIdx, int _length)
	{
		if (_itemStackArr == null)
		{
			return null;
		}
		ItemStack[] array = new ItemStack[_length];
		for (int i = 0; i < _length; i++)
		{
			ItemStack itemStack = ((_startIdx + i < _itemStackArr.Length) ? _itemStackArr[_startIdx + i] : null);
			array[i] = ((itemStack != null) ? itemStack.Clone() : ItemStack.Empty);
		}
		return array;
	}

	// Token: 0x06002DEA RID: 11754 RVA: 0x0012437C File Offset: 0x0012257C
	public static ItemStack FromString(string _s)
	{
		ItemStack empty = ItemStack.Empty;
		int num = _s.IndexOf("=");
		if (num >= 0)
		{
			int num2 = 0;
			if (int.TryParse(_s.Substring(num + 1), out num2))
			{
				empty.count = num2;
			}
			_s = _s.Substring(0, num);
		}
		else
		{
			empty.count = 1;
		}
		empty.itemValue = ItemClass.GetItem(_s, false);
		return empty;
	}

	// Token: 0x06002DEB RID: 11755 RVA: 0x001243DC File Offset: 0x001225DC
	public override string ToString()
	{
		ItemValue itemValue = this.itemValue;
		return ((itemValue != null) ? itemValue.ToString() : null) + " cnt=" + this.count.ToString();
	}

	// Token: 0x06002DEC RID: 11756 RVA: 0x00124414 File Offset: 0x00122614
	public static bool IsEmpty(ItemStack[] _slots)
	{
		if (_slots == null || _slots.Length == 0)
		{
			return true;
		}
		for (int i = 0; i < _slots.Length; i++)
		{
			if (!_slots[i].IsEmpty())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002DED RID: 11757 RVA: 0x00124448 File Offset: 0x00122648
	public bool CanMoveTo(XUiC_ItemStack.StackLocationTypes locationType, int slotNumber = -1)
	{
		bool flag = true;
		ItemClass itemClass = this.itemValue.ItemClass;
		if (itemClass != null)
		{
			flag = itemClass.CanMoveToLocation(locationType, slotNumber);
		}
		return flag;
	}

	// Token: 0x06002DEE RID: 11758 RVA: 0x00124470 File Offset: 0x00122670
	public void AdjustForSandboxOptions()
	{
		ItemValue itemValue = this.itemValue;
		if (itemValue == null)
		{
			return;
		}
		itemValue.AdjustForSandboxOptions();
	}

	// Token: 0x040023CB RID: 9163
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStackGrid owner;

	// Token: 0x040023CC RID: 9164
	[PublicizedFrom(EAccessModifier.Private)]
	public int ownerSlot = -1;

	// Token: 0x040023CD RID: 9165
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValueField;

	// Token: 0x040023CE RID: 9166
	[PublicizedFrom(EAccessModifier.Private)]
	public int countField;

	// Token: 0x02000583 RID: 1411
	public enum EnumDragType
	{
		// Token: 0x040023D0 RID: 9168
		DragTypeStart,
		// Token: 0x040023D1 RID: 9169
		DragTypeAdd,
		// Token: 0x040023D2 RID: 9170
		DragTypeExchange,
		// Token: 0x040023D3 RID: 9171
		DragTypeOther
	}
}
