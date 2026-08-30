using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x0200057B RID: 1403
public class ItemStack
{
	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x06002DC1 RID: 11713 RVA: 0x0012308C File Offset: 0x0012128C
	public static ItemStack Empty
	{
		get
		{
			return new ItemStack(ItemValue.None, 0);
		}
	}

	// Token: 0x06002DC2 RID: 11714 RVA: 0x00123099 File Offset: 0x00121299
	public ItemStack()
	{
		this.itemValue = ItemValue.None;
		this.count = 0;
	}

	// Token: 0x06002DC3 RID: 11715 RVA: 0x001230B3 File Offset: 0x001212B3
	public ItemStack(ItemValue _itemValue, int _count)
	{
		this.itemValue = _itemValue;
		this.count = _count;
	}

	// Token: 0x06002DC4 RID: 11716 RVA: 0x001230C9 File Offset: 0x001212C9
	public ItemStack Clone()
	{
		if (this.itemValue != null)
		{
			return new ItemStack(this.itemValue.Clone(), this.count);
		}
		return new ItemStack(ItemValue.None, this.count);
	}

	// Token: 0x06002DC5 RID: 11717 RVA: 0x001230FC File Offset: 0x001212FC
	public static ItemStack[] CreateArray(int _size)
	{
		ItemStack[] array = new ItemStack[_size];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = ItemStack.Empty;
		}
		return array;
	}

	// Token: 0x06002DC6 RID: 11718 RVA: 0x00123127 File Offset: 0x00121327
	public bool IsEmpty()
	{
		return this.count < 1 || this.itemValue.type == 0;
	}

	// Token: 0x06002DC7 RID: 11719 RVA: 0x00123142 File Offset: 0x00121342
	public void Clear()
	{
		this.itemValue.Clear();
		this.count = 0;
	}

	// Token: 0x06002DC8 RID: 11720 RVA: 0x00123158 File Offset: 0x00121358
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
				_itemStackArr[num3] = _itemStack;
				num = num3;
			}
			num3++;
		}
		return num;
	}

	// Token: 0x06002DC9 RID: 11721 RVA: 0x001231CE File Offset: 0x001213CE
	public ItemStack ReadOld(BinaryReader _br)
	{
		this.itemValue.ReadOld(_br);
		this.count = (int)_br.ReadInt16();
		return this;
	}

	// Token: 0x06002DCA RID: 11722 RVA: 0x001231E9 File Offset: 0x001213E9
	public ItemStack Read(BinaryReader _br)
	{
		this.count = (int)_br.ReadUInt16();
		if (this.count > 0)
		{
			this.itemValue.Read(_br);
		}
		else
		{
			this.itemValue = ItemValue.None;
		}
		return this;
	}

	// Token: 0x06002DCB RID: 11723 RVA: 0x0012321C File Offset: 0x0012141C
	public ItemStack ReadDelta(BinaryReader _br, ItemStack _last)
	{
		this.itemValue.Read(_br);
		int num = (int)_br.ReadInt16();
		this.count = _last.count + num;
		return this;
	}

	// Token: 0x06002DCC RID: 11724 RVA: 0x0012324C File Offset: 0x0012144C
	public void Write(BinaryWriter _bw)
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

	// Token: 0x06002DCD RID: 11725 RVA: 0x0012328A File Offset: 0x0012148A
	public void WriteDelta(BinaryWriter _bw, ItemStack _last)
	{
		this.itemValue.Write(_bw);
		_bw.Write((short)(this.count - _last.count));
		_last.count += this.count - _last.count;
	}

	// Token: 0x06002DCE RID: 11726 RVA: 0x001232C8 File Offset: 0x001214C8
	public static ItemStack[] ReadArray(BinaryReader _br)
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

	// Token: 0x06002DCF RID: 11727 RVA: 0x00123300 File Offset: 0x00121500
	public static void WriteArray(BinaryWriter _bw, ItemStack[] _items)
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

	// Token: 0x06002DD0 RID: 11728 RVA: 0x0012333C File Offset: 0x0012153C
	public override bool Equals(object _other)
	{
		ItemStack itemStack = _other as ItemStack;
		return itemStack != null && itemStack.count == this.count && itemStack.itemValue.Equals(this.itemValue);
	}

	// Token: 0x06002DD1 RID: 11729 RVA: 0x00123376 File Offset: 0x00121576
	public override int GetHashCode()
	{
		return this.itemValue.GetHashCode() * 13 + this.count;
	}

	// Token: 0x06002DD2 RID: 11730 RVA: 0x00123390 File Offset: 0x00121590
	public int StackTransferCount(ItemStack _other)
	{
		if (_other.itemValue.type != this.itemValue.type)
		{
			return 0;
		}
		return Math.Min(ItemClass.GetForId(this.itemValue.type).MaxCount - this.count, _other.count);
	}

	// Token: 0x06002DD3 RID: 11731 RVA: 0x001233E0 File Offset: 0x001215E0
	public bool CanStackWith(ItemStack _other, bool allowPartialStack = false)
	{
		int num = _other.count;
		if (_other.itemValue == null || this.itemValue == null || _other.itemValue.type != this.itemValue.type || (this.itemValue.type < Block.ItemsStartHere && !(_other.itemValue.TextureFullArray == this.itemValue.TextureFullArray) && !this.itemValue.IsShapeHelperBlock))
		{
			return false;
		}
		if (!allowPartialStack)
		{
			return this.CanStack(_other.count);
		}
		return this.CanStackPartly(ref num);
	}

	// Token: 0x06002DD4 RID: 11732 RVA: 0x00123472 File Offset: 0x00121672
	public bool CanStack(int _count)
	{
		return this.itemValue.type == 0 || ItemClass.GetForId(this.itemValue.type).MaxCount >= _count + this.count;
	}

	// Token: 0x06002DD5 RID: 11733 RVA: 0x001234A5 File Offset: 0x001216A5
	public bool CanStackPartlyWith(ItemStack _other, out int _count)
	{
		_count = _other.count;
		return this.CanStackWith(_other, true) && this.CanStackPartly(ref _count);
	}

	// Token: 0x06002DD6 RID: 11734 RVA: 0x001234C2 File Offset: 0x001216C2
	public bool CanStackPartly(ref int _count)
	{
		if (this.itemValue.type == 0)
		{
			return false;
		}
		_count = Utils.FastMin(ItemClass.GetForId(this.itemValue.type).MaxCount - this.count, _count);
		return _count > 0;
	}

	// Token: 0x06002DD7 RID: 11735 RVA: 0x00123500 File Offset: 0x00121700
	public void Deactivate()
	{
		ItemClass forId = ItemClass.GetForId(this.itemValue.type);
		if (forId != null)
		{
			forId.Deactivate(this.itemValue);
		}
	}

	// Token: 0x06002DD8 RID: 11736 RVA: 0x00123530 File Offset: 0x00121730
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

	// Token: 0x06002DD9 RID: 11737 RVA: 0x00123578 File Offset: 0x00121778
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

	// Token: 0x06002DDA RID: 11738 RVA: 0x001235C4 File Offset: 0x001217C4
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

	// Token: 0x06002DDB RID: 11739 RVA: 0x00123623 File Offset: 0x00121823
	public override string ToString()
	{
		ItemValue itemValue = this.itemValue;
		return ((itemValue != null) ? itemValue.ToString() : null) + " cnt=" + this.count.ToString();
	}

	// Token: 0x06002DDC RID: 11740 RVA: 0x0012364C File Offset: 0x0012184C
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

	// Token: 0x06002DDD RID: 11741 RVA: 0x00123680 File Offset: 0x00121880
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

	// Token: 0x06002DDE RID: 11742 RVA: 0x001236A8 File Offset: 0x001218A8
	public void AdjustForSandboxOptions()
	{
		ItemValue itemValue = this.itemValue;
		if (itemValue == null)
		{
			return;
		}
		itemValue.AdjustForSandboxOptions();
	}

	// Token: 0x040023BD RID: 9149
	public ItemValue itemValue;

	// Token: 0x040023BE RID: 9150
	public int count;

	// Token: 0x0200057C RID: 1404
	public enum EnumDragType
	{
		// Token: 0x040023C0 RID: 9152
		DragTypeStart,
		// Token: 0x040023C1 RID: 9153
		DragTypeAdd,
		// Token: 0x040023C2 RID: 9154
		DragTypeExchange,
		// Token: 0x040023C3 RID: 9155
		DragTypeOther
	}
}
