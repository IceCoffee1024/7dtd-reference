using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000576 RID: 1398
public class ItemStack
{
	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x06002D96 RID: 11670 RVA: 0x00121FD0 File Offset: 0x001201D0
	public static ItemStack Empty
	{
		get
		{
			return new ItemStack(ItemValue.None, 0);
		}
	}

	// Token: 0x06002D97 RID: 11671 RVA: 0x00121FDD File Offset: 0x001201DD
	public ItemStack()
	{
		this.itemValue = ItemValue.None;
		this.count = 0;
	}

	// Token: 0x06002D98 RID: 11672 RVA: 0x00121FF7 File Offset: 0x001201F7
	public ItemStack(ItemValue _itemValue, int _count)
	{
		this.itemValue = _itemValue;
		this.count = _count;
	}

	// Token: 0x06002D99 RID: 11673 RVA: 0x0012200D File Offset: 0x0012020D
	public ItemStack Clone()
	{
		if (this.itemValue != null)
		{
			return new ItemStack(this.itemValue.Clone(), this.count);
		}
		return new ItemStack(ItemValue.None, this.count);
	}

	// Token: 0x06002D9A RID: 11674 RVA: 0x00122040 File Offset: 0x00120240
	public static ItemStack[] CreateArray(int _size)
	{
		ItemStack[] array = new ItemStack[_size];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = ItemStack.Empty;
		}
		return array;
	}

	// Token: 0x06002D9B RID: 11675 RVA: 0x0012206B File Offset: 0x0012026B
	public bool IsEmpty()
	{
		return this.count < 1 || this.itemValue.type == 0;
	}

	// Token: 0x06002D9C RID: 11676 RVA: 0x00122086 File Offset: 0x00120286
	public void Clear()
	{
		this.itemValue.Clear();
		this.count = 0;
	}

	// Token: 0x06002D9D RID: 11677 RVA: 0x0012209C File Offset: 0x0012029C
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

	// Token: 0x06002D9E RID: 11678 RVA: 0x00122112 File Offset: 0x00120312
	public ItemStack ReadOld(BinaryReader _br)
	{
		this.itemValue.ReadOld(_br);
		this.count = (int)_br.ReadInt16();
		return this;
	}

	// Token: 0x06002D9F RID: 11679 RVA: 0x0012212D File Offset: 0x0012032D
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

	// Token: 0x06002DA0 RID: 11680 RVA: 0x00122160 File Offset: 0x00120360
	public ItemStack ReadDelta(BinaryReader _br, ItemStack _last)
	{
		this.itemValue.Read(_br);
		int num = (int)_br.ReadInt16();
		this.count = _last.count + num;
		return this;
	}

	// Token: 0x06002DA1 RID: 11681 RVA: 0x00122190 File Offset: 0x00120390
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

	// Token: 0x06002DA2 RID: 11682 RVA: 0x001221CE File Offset: 0x001203CE
	public void WriteDelta(BinaryWriter _bw, ItemStack _last)
	{
		this.itemValue.Write(_bw);
		_bw.Write((short)(this.count - _last.count));
		_last.count += this.count - _last.count;
	}

	// Token: 0x06002DA3 RID: 11683 RVA: 0x0012220C File Offset: 0x0012040C
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

	// Token: 0x06002DA4 RID: 11684 RVA: 0x00122244 File Offset: 0x00120444
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

	// Token: 0x06002DA5 RID: 11685 RVA: 0x00122280 File Offset: 0x00120480
	public override bool Equals(object _other)
	{
		ItemStack itemStack = _other as ItemStack;
		return itemStack != null && itemStack.count == this.count && itemStack.itemValue.Equals(this.itemValue);
	}

	// Token: 0x06002DA6 RID: 11686 RVA: 0x001222BA File Offset: 0x001204BA
	public override int GetHashCode()
	{
		return this.itemValue.GetHashCode() * 13 + this.count;
	}

	// Token: 0x06002DA7 RID: 11687 RVA: 0x001222D4 File Offset: 0x001204D4
	public int StackTransferCount(ItemStack _other)
	{
		if (_other.itemValue.type != this.itemValue.type)
		{
			return 0;
		}
		return Math.Min(ItemClass.GetForId(this.itemValue.type).Stacknumber.Value - this.count, _other.count);
	}

	// Token: 0x06002DA8 RID: 11688 RVA: 0x00122328 File Offset: 0x00120528
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

	// Token: 0x06002DA9 RID: 11689 RVA: 0x001223BA File Offset: 0x001205BA
	public bool CanStack(int _count)
	{
		return this.itemValue.type == 0 || ItemClass.GetForId(this.itemValue.type).Stacknumber.Value >= _count + this.count;
	}

	// Token: 0x06002DAA RID: 11690 RVA: 0x001223F2 File Offset: 0x001205F2
	public bool CanStackPartlyWith(ItemStack _other, out int _count)
	{
		_count = _other.count;
		return this.CanStackWith(_other, true) && this.CanStackPartly(ref _count);
	}

	// Token: 0x06002DAB RID: 11691 RVA: 0x0012240F File Offset: 0x0012060F
	public bool CanStackPartly(ref int _count)
	{
		if (this.itemValue.type == 0)
		{
			return false;
		}
		_count = Utils.FastMin(ItemClass.GetForId(this.itemValue.type).Stacknumber.Value - this.count, _count);
		return _count > 0;
	}

	// Token: 0x06002DAC RID: 11692 RVA: 0x00122450 File Offset: 0x00120650
	public void Deactivate()
	{
		ItemClass forId = ItemClass.GetForId(this.itemValue.type);
		if (forId != null)
		{
			forId.Deactivate(this.itemValue);
		}
	}

	// Token: 0x06002DAD RID: 11693 RVA: 0x00122480 File Offset: 0x00120680
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

	// Token: 0x06002DAE RID: 11694 RVA: 0x001224C8 File Offset: 0x001206C8
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

	// Token: 0x06002DAF RID: 11695 RVA: 0x00122514 File Offset: 0x00120714
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

	// Token: 0x06002DB0 RID: 11696 RVA: 0x00122573 File Offset: 0x00120773
	public override string ToString()
	{
		ItemValue itemValue = this.itemValue;
		return ((itemValue != null) ? itemValue.ToString() : null) + " cnt=" + this.count.ToString();
	}

	// Token: 0x06002DB1 RID: 11697 RVA: 0x0012259C File Offset: 0x0012079C
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

	// Token: 0x06002DB2 RID: 11698 RVA: 0x001225D0 File Offset: 0x001207D0
	public bool CanMoveTo(XUiC_ItemStack.StackLocationTypes locationType)
	{
		bool flag = true;
		ItemClass itemClass = this.itemValue.ItemClass;
		if (itemClass != null)
		{
			flag = itemClass.CanMoveToLocation(locationType);
		}
		return flag;
	}

	// Token: 0x06002DB3 RID: 11699 RVA: 0x001225F7 File Offset: 0x001207F7
	public void AdjustForSandboxOptions()
	{
		ItemValue itemValue = this.itemValue;
		if (itemValue == null)
		{
			return;
		}
		itemValue.AdjustForSandboxOptions();
	}

	// Token: 0x0400238B RID: 9099
	public ItemValue itemValue;

	// Token: 0x0400238C RID: 9100
	public int count;

	// Token: 0x02000577 RID: 1399
	public enum EnumDragType
	{
		// Token: 0x0400238E RID: 9102
		DragTypeStart,
		// Token: 0x0400238F RID: 9103
		DragTypeAdd,
		// Token: 0x04002390 RID: 9104
		DragTypeExchange,
		// Token: 0x04002391 RID: 9105
		DragTypeOther
	}
}
