using System;

// Token: 0x02000D74 RID: 3444
public class ItemStackGrid
{
	// Token: 0x1400008E RID: 142
	// (add) Token: 0x0600686B RID: 26731 RVA: 0x00293EDC File Offset: 0x002920DC
	// (remove) Token: 0x0600686C RID: 26732 RVA: 0x00293F14 File Offset: 0x00292114
	public event Action<int> SlotChanged;

	// Token: 0x17000B18 RID: 2840
	// (get) Token: 0x0600686D RID: 26733 RVA: 0x00293F49 File Offset: 0x00292149
	public Vector2i ContainerSize { get; }

	// Token: 0x0600686E RID: 26734 RVA: 0x00293F54 File Offset: 0x00292154
	public ItemStackGrid(Vector2i _containerSize, bool _hasLocks)
	{
		this.ContainerSize = _containerSize;
		int num = _containerSize.x * _containerSize.y;
		this.items = new ItemStack[num];
		for (int i = 0; i < num; i++)
		{
			this.items[i] = ItemStack.Empty.Clone();
		}
		this.locks = (_hasLocks ? new PackedBoolArray(num) : null);
	}

	// Token: 0x17000B19 RID: 2841
	// (get) Token: 0x0600686F RID: 26735 RVA: 0x00293FB8 File Offset: 0x002921B8
	public int SlotCount
	{
		get
		{
			return this.items.Length;
		}
	}

	// Token: 0x17000B1A RID: 2842
	// (get) Token: 0x06006870 RID: 26736 RVA: 0x00293FC2 File Offset: 0x002921C2
	public ItemStack[] Items
	{
		get
		{
			return this.items;
		}
	}

	// Token: 0x17000B1B RID: 2843
	public ItemStack this[int _index]
	{
		get
		{
			return this.items[_index];
		}
		set
		{
			this.SetSlot(_index, value);
		}
	}

	// Token: 0x17000B1C RID: 2844
	// (get) Token: 0x06006873 RID: 26739 RVA: 0x00293FDE File Offset: 0x002921DE
	public PackedBoolArray SlotLocks
	{
		get
		{
			return this.locks;
		}
	}

	// Token: 0x06006874 RID: 26740 RVA: 0x00293FE6 File Offset: 0x002921E6
	public ItemStackGrid Clone()
	{
		ItemStackGrid itemStackGrid = new ItemStackGrid(this.ContainerSize, false);
		itemStackGrid.items = ItemStack.Clone(this.items, 0, this.SlotCount);
		PackedBoolArray packedBoolArray = this.locks;
		itemStackGrid.locks = ((packedBoolArray != null) ? packedBoolArray.Clone() : null);
		return itemStackGrid;
	}

	// Token: 0x06006875 RID: 26741 RVA: 0x00294024 File Offset: 0x00292224
	public void SetSlot(int _index, ItemStack _stack)
	{
		this.items[_index] = (_stack ?? ItemStack.Empty).Clone();
		Action<int> slotChanged = this.SlotChanged;
		if (slotChanged == null)
		{
			return;
		}
		slotChanged(_index);
	}

	// Token: 0x06006876 RID: 26742 RVA: 0x0029404E File Offset: 0x0029224E
	public bool IsLocked(int _index)
	{
		return this.locks != null && this.locks[_index];
	}

	// Token: 0x06006877 RID: 26743 RVA: 0x00294066 File Offset: 0x00292266
	public void SetLocked(int _index, bool _value)
	{
		if (this.locks == null || this.locks[_index] == _value)
		{
			return;
		}
		this.locks[_index] = _value;
		Action<int> slotChanged = this.SlotChanged;
		if (slotChanged == null)
		{
			return;
		}
		slotChanged(_index);
	}

	// Token: 0x0400503C RID: 20540
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] items;

	// Token: 0x0400503D RID: 20541
	[PublicizedFrom(EAccessModifier.Private)]
	public PackedBoolArray locks;
}
