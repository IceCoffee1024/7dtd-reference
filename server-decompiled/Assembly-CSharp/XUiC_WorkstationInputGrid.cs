using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020010B7 RID: 4279
[Preserve]
public class XUiC_WorkstationInputGrid : XUiC_WorkstationGrid
{
	// Token: 0x06008821 RID: 34849 RVA: 0x0033FF88 File Offset: 0x0033E188
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.CurrentWorkstationInputGrid = this;
	}

	// Token: 0x06008822 RID: 34850 RVA: 0x0033FF9C File Offset: 0x0033E19C
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CurrentWorkstationInputGrid = null;
	}

	// Token: 0x06008823 RID: 34851 RVA: 0x0033FFB0 File Offset: 0x0033E1B0
	public bool AcceptsMaterial(MaterialBlock material)
	{
		bool flag = false;
		XUiC_WorkstationWindowGroup xuiC_WorkstationWindowGroup = this.windowGroup.Controller as XUiC_WorkstationWindowGroup;
		if (xuiC_WorkstationWindowGroup != null)
		{
			flag = xuiC_WorkstationWindowGroup.WorkstationData.TileEntity.AcceptsMaterial(material);
		}
		return flag;
	}

	// Token: 0x06008824 RID: 34852 RVA: 0x0033FFE6 File Offset: 0x0033E1E6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.workstationData.SetInputStacks(stackList);
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06008825 RID: 34853 RVA: 0x0034000C File Offset: 0x0033E20C
	public bool HasItems(IList<ItemStack> _itemStacks, int _multiplier = 1)
	{
		for (int i = 0; i < _itemStacks.Count; i++)
		{
			if (_itemStacks[i].count * _multiplier - this.GetItemCount(_itemStacks[i].itemValue) > 0)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06008826 RID: 34854 RVA: 0x00340054 File Offset: 0x0033E254
	public void RemoveItems(IList<ItemStack> _itemStacks, int _multiplier = 1, IList<ItemStack> _removedItems = null)
	{
		for (int i = 0; i < _itemStacks.Count; i++)
		{
			int num = _itemStacks[i].count * _multiplier;
			num -= this.DecItem(_itemStacks[i].itemValue, num, _removedItems);
		}
	}

	// Token: 0x06008827 RID: 34855 RVA: 0x00340098 File Offset: 0x0033E298
	public new int AddToItemStackArray(ItemStack _itemStack)
	{
		ItemStack[] slots = this.GetSlots();
		int num = -1;
		int num2 = 0;
		while (num == -1 && num2 < slots.Length)
		{
			if (slots[num2].CanStackWith(_itemStack, false))
			{
				slots[num2].count += _itemStack.count;
				_itemStack.count = 0;
				num = num2;
			}
			num2++;
		}
		int num3 = 0;
		while (num == -1 && num3 < slots.Length)
		{
			if (slots[num3].IsEmpty())
			{
				slots[num3] = _itemStack;
				num = num3;
			}
			num3++;
		}
		if (num != -1)
		{
			this.SetSlots(slots);
			this.UpdateBackend(slots);
		}
		return num;
	}

	// Token: 0x06008828 RID: 34856 RVA: 0x00340120 File Offset: 0x0033E320
	public int DecItem(ItemValue _itemValue, int _count, IList<ItemStack> _removedItems = null)
	{
		int num = _count;
		ItemStack[] slots = this.GetSlots();
		int num2 = 0;
		while (_count > 0 && num2 < this.GetSlots().Length)
		{
			if (slots[num2].itemValue.type == _itemValue.type)
			{
				if (slots[num2].itemValue.ItemClass.CanStack())
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
		this.UpdateBackend(slots);
		return num - _count;
	}

	// Token: 0x06008829 RID: 34857 RVA: 0x0034020C File Offset: 0x0033E40C
	public int GetItemCount(ItemValue _itemValue)
	{
		ItemStack[] slots = this.GetSlots();
		int num = 0;
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].itemValue.type == _itemValue.type)
			{
				num += slots[i].count;
			}
		}
		return num;
	}
}
