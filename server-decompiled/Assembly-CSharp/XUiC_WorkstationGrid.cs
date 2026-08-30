using System;
using UnityEngine.Scripting;

// Token: 0x020010B6 RID: 4278
[Preserve]
public class XUiC_WorkstationGrid : XUiC_ItemStackGrid
{
	// Token: 0x17000FB4 RID: 4020
	// (get) Token: 0x06008816 RID: 34838 RVA: 0x0033FD2B File Offset: 0x0033DF2B
	public XUiM_Workstation WorkstationData
	{
		get
		{
			return this.workstationData;
		}
	}

	// Token: 0x17000FB5 RID: 4021
	// (get) Token: 0x06008817 RID: 34839 RVA: 0x00081148 File Offset: 0x0007F348
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x06008818 RID: 34840 RVA: 0x0033FD33 File Offset: 0x0033DF33
	public virtual void SetSlots(ItemStack[] stacks)
	{
		base.SetStacks(stacks);
	}

	// Token: 0x06008819 RID: 34841 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool HasRequirement(Recipe recipe)
	{
		return true;
	}

	// Token: 0x0600881A RID: 34842 RVA: 0x0033FD3C File Offset: 0x0033DF3C
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.OnOpen();
			base.ViewComponent.IsVisible = true;
		}
		this.workstationData = ((XUiC_WorkstationWindowGroup)this.windowGroup.Controller).WorkstationData;
		this.IsDirty = true;
	}

	// Token: 0x0600881B RID: 34843 RVA: 0x0033FD97 File Offset: 0x0033DF97
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.OnClose();
			base.ViewComponent.IsVisible = false;
		}
		this.IsDirty = true;
	}

	// Token: 0x0600881C RID: 34844 RVA: 0x0033FDCC File Offset: 0x0033DFCC
	public int AddToItemStackArray(ItemStack _itemStack)
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

	// Token: 0x0600881D RID: 34845 RVA: 0x0033FE54 File Offset: 0x0033E054
	public virtual bool AddItem(ItemClass _itemClass, ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Workstation))
		{
			return false;
		}
		int num = 0;
		this.TryStackItem(num, _itemStack);
		return _itemStack.count > 0 && this.AddItem(_itemStack);
	}

	// Token: 0x0600881E RID: 34846 RVA: 0x0033FE8C File Offset: 0x0033E08C
	public bool TryStackItem(int startIndex, ItemStack _itemStack)
	{
		int num = 0;
		for (int i = startIndex; i < this.itemControllers.Length; i++)
		{
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[i];
			ItemStack itemStack = xuiC_ItemStack.ItemStack;
			num = _itemStack.count;
			if (itemStack != null && _itemStack.itemValue.type == itemStack.itemValue.type && itemStack.CanStackPartly(ref num))
			{
				xuiC_ItemStack.ItemStack.count += num;
				xuiC_ItemStack.ItemStack = xuiC_ItemStack.ItemStack;
				xuiC_ItemStack.ForceRefreshItemStack();
				_itemStack.count -= num;
				if (_itemStack.count == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600881F RID: 34847 RVA: 0x0033FF38 File Offset: 0x0033E138
	public bool AddItem(ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Workstation))
		{
			return false;
		}
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[i];
			ItemStack itemStack = xuiC_ItemStack.ItemStack;
			if (itemStack == null || itemStack.IsEmpty())
			{
				xuiC_ItemStack.ItemStack = _itemStack;
				return true;
			}
		}
		return false;
	}

	// Token: 0x04006539 RID: 25913
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiM_Workstation workstationData;
}
