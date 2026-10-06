using System;
using System.Collections.Generic;

// Token: 0x0200057B RID: 1403
public class ItemStack
{
	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x06002DBC RID: 11708 RVA: 0x00123187 File Offset: 0x00121387
	public static ItemStack Empty
	{
		get
		{
			return new ItemStack(ItemValue.None, 0);
		}
	}

	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x06002DBD RID: 11709 RVA: 0x00123194 File Offset: 0x00121394
	// (set) Token: 0x06002DBE RID: 11710 RVA: 0x0012319C File Offset: 0x0012139C
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
	// (get) Token: 0x06002DBF RID: 11711 RVA: 0x001231D9 File Offset: 0x001213D9
	// (set) Token: 0x06002DC0 RID: 11712 RVA: 0x001231E1 File Offset: 0x001213E1
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

	// Token: 0x06002DC1 RID: 11713 RVA: 0x001231FA File Offset: 0x001213FA
	public void BindGridSlot(ItemStackGrid _owner, int _slot = -1)
	{
		this.owner = _owner;
		this.ownerSlot = _slot;
	}

	// Token: 0x06002DC2 RID: 11714 RVA: 0x0012320A File Offset: 0x0012140A
	public void OnValueChanged()
	{
		this.NotifyChanged();
	}

	// Token: 0x06002DC3 RID: 11715 RVA: 0x00123212 File Offset: 0x00121412
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

	// Token: 0x06002DC4 RID: 11716 RVA: 0x0012322A File Offset: 0x0012142A
	public ItemStack()
	{
		this.itemValue = ItemValue.None;
		this.count = 0;
	}

	// Token: 0x06002DC5 RID: 11717 RVA: 0x0012324B File Offset: 0x0012144B
	public ItemStack(ItemValue _itemValue, int _count)
	{
		this.itemValue = _itemValue;
		this.count = _count;
	}

	// Token: 0x06002DC6 RID: 11718 RVA: 0x00123268 File Offset: 0x00121468
	public ItemStack Clone()
	{
		if (this.itemValue != null)
		{
			return new ItemStack(this.itemValue.Clone(), this.count);
		}
		return new ItemStack(ItemValue.None, this.count);
	}

	// Token: 0x06002DC7 RID: 11719 RVA: 0x0012329C File Offset: 0x0012149C
	public static ItemStack[] CreateArray(int _size)
	{
		ItemStack[] array = new ItemStack[_size];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = ItemStack.Empty;
		}
		return array;
	}

	// Token: 0x06002DC8 RID: 11720 RVA: 0x001232C7 File Offset: 0x001214C7
	public bool IsEmpty()
	{
		return this.count < 1 || this.itemValue.type == 0;
	}

	// Token: 0x06002DC9 RID: 11721 RVA: 0x001232E2 File Offset: 0x001214E2
	public void Clear()
	{
		this.countField = 0;
		this.itemValue.Clear();
	}

	// Token: 0x06002DCA RID: 11722 RVA: 0x001232F6 File Offset: 0x001214F6
	public void Set(ItemValue _itemValue, int _count)
	{
		this.countField = _count;
		this.itemValue = _itemValue;
		this.NotifyChanged();
	}

	// Token: 0x06002DCB RID: 11723 RVA: 0x0012330C File Offset: 0x0012150C
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

	// Token: 0x06002DCC RID: 11724 RVA: 0x00123380 File Offset: 0x00121580
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

	// Token: 0x06002DCD RID: 11725 RVA: 0x0012340B File Offset: 0x0012160B
	public ItemStack ReadOld(PooledBinaryReader _br)
	{
		this.itemValue.ReadOld(_br);
		this.count = (int)_br.ReadInt16();
		return this;
	}

	// Token: 0x06002DCE RID: 11726 RVA: 0x00123426 File Offset: 0x00121626
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

	// Token: 0x06002DCF RID: 11727 RVA: 0x00123458 File Offset: 0x00121658
	public ItemStack ReadDelta(PooledBinaryReader _br, ItemStack _last)
	{
		this.itemValue.Read(_br);
		int num = (int)_br.ReadInt16();
		this.count = _last.count + num;
		return this;
	}

	// Token: 0x06002DD0 RID: 11728 RVA: 0x00123488 File Offset: 0x00121688
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

	// Token: 0x06002DD1 RID: 11729 RVA: 0x001234C6 File Offset: 0x001216C6
	public void WriteDelta(PooledBinaryWriter _bw, ItemStack _last)
	{
		this.itemValue.Write(_bw);
		_bw.Write((short)(this.count - _last.count));
		_last.count += this.count - _last.count;
	}

	// Token: 0x06002DD2 RID: 11730 RVA: 0x00123504 File Offset: 0x00121704
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

	// Token: 0x06002DD3 RID: 11731 RVA: 0x0012353C File Offset: 0x0012173C
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

	// Token: 0x06002DD4 RID: 11732 RVA: 0x00123578 File Offset: 0x00121778
	public override bool Equals(object _other)
	{
		ItemStack itemStack = _other as ItemStack;
		return itemStack != null && itemStack.count == this.count && itemStack.itemValue.Equals(this.itemValue);
	}

	// Token: 0x06002DD5 RID: 11733 RVA: 0x001235B2 File Offset: 0x001217B2
	public override int GetHashCode()
	{
		return this.itemValue.GetHashCode() * 13 + this.count;
	}

	// Token: 0x06002DD6 RID: 11734 RVA: 0x001235CC File Offset: 0x001217CC
	public int StackTransferCount(ItemStack _other)
	{
		if (_other.itemValue.type != this.itemValue.type)
		{
			return 0;
		}
		return Math.Min(ItemClass.GetForId(this.itemValue.type).MaxCount - this.count, _other.count);
	}

	// Token: 0x06002DD7 RID: 11735 RVA: 0x0012361C File Offset: 0x0012181C
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

	// Token: 0x06002DD8 RID: 11736 RVA: 0x001236AE File Offset: 0x001218AE
	public bool CanStack(int _count)
	{
		return this.itemValue.type == 0 || ItemClass.GetForId(this.itemValue.type).MaxCount >= _count + this.count;
	}

	// Token: 0x06002DD9 RID: 11737 RVA: 0x001236E1 File Offset: 0x001218E1
	public bool CanStackPartlyWith(ItemStack _other, out int _count)
	{
		_count = _other.count;
		return this.CanStackWith(_other, true) && this.CanStackPartly(ref _count);
	}

	// Token: 0x06002DDA RID: 11738 RVA: 0x001236FE File Offset: 0x001218FE
	public bool CanStackPartly(ref int _count)
	{
		if (this.itemValue.type == 0)
		{
			return false;
		}
		_count = Utils.FastMin(ItemClass.GetForId(this.itemValue.type).MaxCount - this.count, _count);
		return _count > 0;
	}

	// Token: 0x06002DDB RID: 11739 RVA: 0x0012373C File Offset: 0x0012193C
	public void Deactivate()
	{
		ItemClass forId = ItemClass.GetForId(this.itemValue.type);
		if (forId != null)
		{
			forId.Deactivate(this.itemValue);
		}
	}

	// Token: 0x06002DDC RID: 11740 RVA: 0x0012376C File Offset: 0x0012196C
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

	// Token: 0x06002DDD RID: 11741 RVA: 0x001237B4 File Offset: 0x001219B4
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

	// Token: 0x06002DDE RID: 11742 RVA: 0x00123800 File Offset: 0x00121A00
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

	// Token: 0x06002DDF RID: 11743 RVA: 0x00123860 File Offset: 0x00121A60
	public override string ToString()
	{
		ItemValue itemValue = this.itemValue;
		return ((itemValue != null) ? itemValue.ToString() : null) + " cnt=" + this.count.ToString();
	}

	// Token: 0x06002DE0 RID: 11744 RVA: 0x00123898 File Offset: 0x00121A98
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

	// Token: 0x06002DE1 RID: 11745 RVA: 0x001238CC File Offset: 0x00121ACC
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

	// Token: 0x06002DE2 RID: 11746 RVA: 0x001238F4 File Offset: 0x00121AF4
	public void AdjustForSandboxOptions()
	{
		ItemValue itemValue = this.itemValue;
		if (itemValue == null)
		{
			return;
		}
		itemValue.AdjustForSandboxOptions();
	}

	// Token: 0x040023B0 RID: 9136
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStackGrid owner;

	// Token: 0x040023B1 RID: 9137
	[PublicizedFrom(EAccessModifier.Private)]
	public int ownerSlot = -1;

	// Token: 0x040023B2 RID: 9138
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValueField;

	// Token: 0x040023B3 RID: 9139
	[PublicizedFrom(EAccessModifier.Private)]
	public int countField;

	// Token: 0x0200057C RID: 1404
	public enum EnumDragType
	{
		// Token: 0x040023B5 RID: 9141
		DragTypeStart,
		// Token: 0x040023B6 RID: 9142
		DragTypeAdd,
		// Token: 0x040023B7 RID: 9143
		DragTypeExchange,
		// Token: 0x040023B8 RID: 9144
		DragTypeOther
	}
}
