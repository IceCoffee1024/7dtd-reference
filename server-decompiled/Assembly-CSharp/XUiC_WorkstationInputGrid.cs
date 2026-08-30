using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020010D0 RID: 4304
[Preserve]
public class XUiC_WorkstationInputGrid : XUiC_WorkstationGrid
{
	// Token: 0x06008913 RID: 35091 RVA: 0x003447B5 File Offset: 0x003429B5
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.CurrentWorkstationInputGrid = this;
	}

	// Token: 0x06008914 RID: 35092 RVA: 0x003447C9 File Offset: 0x003429C9
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CurrentWorkstationInputGrid = null;
	}

	// Token: 0x06008915 RID: 35093 RVA: 0x003447E0 File Offset: 0x003429E0
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

	// Token: 0x06008916 RID: 35094 RVA: 0x00344816 File Offset: 0x00342A16
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.workstationData.SetInputStacks(stackList);
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06008917 RID: 35095 RVA: 0x0034483C File Offset: 0x00342A3C
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

	// Token: 0x06008918 RID: 35096 RVA: 0x00344884 File Offset: 0x00342A84
	public void RemoveItems(IList<ItemStack> _itemStacks, int _multiplier = 1, IList<ItemStack> _removedItems = null)
	{
		for (int i = 0; i < _itemStacks.Count; i++)
		{
			int num = _itemStacks[i].count * _multiplier;
			num -= this.DecItem(_itemStacks[i].itemValue, num, _removedItems);
		}
	}

	// Token: 0x06008919 RID: 35097 RVA: 0x003448C8 File Offset: 0x00342AC8
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

	// Token: 0x0600891A RID: 35098 RVA: 0x00344950 File Offset: 0x00342B50
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

	// Token: 0x0600891B RID: 35099 RVA: 0x00344A3C File Offset: 0x00342C3C
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
