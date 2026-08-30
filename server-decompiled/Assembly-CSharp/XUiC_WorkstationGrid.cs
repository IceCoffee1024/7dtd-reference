using System;
using UnityEngine.Scripting;

// Token: 0x020010CF RID: 4303
[Preserve]
public class XUiC_WorkstationGrid : XUiC_ItemStackGrid
{
	// Token: 0x17000FED RID: 4077
	// (get) Token: 0x06008908 RID: 35080 RVA: 0x00344553 File Offset: 0x00342753
	public XUiM_Workstation WorkstationData
	{
		get
		{
			return this.workstationData;
		}
	}

	// Token: 0x17000FEE RID: 4078
	// (get) Token: 0x06008909 RID: 35081 RVA: 0x000817FC File Offset: 0x0007F9FC
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x0600890A RID: 35082 RVA: 0x0034455B File Offset: 0x0034275B
	public virtual void SetSlots(ItemStack[] stacks)
	{
		base.SetStacks(stacks);
	}

	// Token: 0x0600890B RID: 35083 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool HasRequirement(Recipe recipe)
	{
		return true;
	}

	// Token: 0x0600890C RID: 35084 RVA: 0x00344564 File Offset: 0x00342764
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

	// Token: 0x0600890D RID: 35085 RVA: 0x003445BF File Offset: 0x003427BF
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.OnClose();
			base.ViewComponent.IsVisible = false;
		}
		this.IsDirty = true;
	}

	// Token: 0x0600890E RID: 35086 RVA: 0x003445F4 File Offset: 0x003427F4
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

	// Token: 0x0600890F RID: 35087 RVA: 0x0034467C File Offset: 0x0034287C
	public virtual bool AddItem(ItemClass _itemClass, ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Workstation, -1))
		{
			return false;
		}
		int num = 0;
		this.TryStackItem(num, _itemStack);
		return _itemStack.count > 0 && this.AddItem(_itemStack);
	}

	// Token: 0x06008910 RID: 35088 RVA: 0x003446B8 File Offset: 0x003428B8
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

	// Token: 0x06008911 RID: 35089 RVA: 0x00344764 File Offset: 0x00342964
	public bool AddItem(ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Workstation, -1))
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

	// Token: 0x04006600 RID: 26112
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiM_Workstation workstationData;
}
