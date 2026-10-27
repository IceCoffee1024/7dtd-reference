using System;
using UnityEngine.Scripting;

// Token: 0x020010F0 RID: 4336
[Preserve]
public class XUiC_WorkstationGrid : XUiC_ItemStackGrid
{
	// Token: 0x1700102D RID: 4141
	// (get) Token: 0x060089A9 RID: 35241 RVA: 0x0034A387 File Offset: 0x00348587
	public XUiM_Workstation WorkstationData
	{
		get
		{
			return this.workstationData;
		}
	}

	// Token: 0x1700102E RID: 4142
	// (get) Token: 0x060089AA RID: 35242 RVA: 0x00060491 File Offset: 0x0005E691
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x060089AB RID: 35243 RVA: 0x0034A38F File Offset: 0x0034858F
	public virtual void SetSlots(ItemStack[] stacks)
	{
		base.SetStacks(stacks);
	}

	// Token: 0x060089AC RID: 35244 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool HasRequirement(Recipe recipe)
	{
		return true;
	}

	// Token: 0x060089AD RID: 35245 RVA: 0x0034A398 File Offset: 0x00348598
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

	// Token: 0x060089AE RID: 35246 RVA: 0x0034A3F3 File Offset: 0x003485F3
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.OnClose();
			base.ViewComponent.IsVisible = false;
		}
		this.IsDirty = true;
	}

	// Token: 0x060089AF RID: 35247 RVA: 0x0034A428 File Offset: 0x00348628
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

	// Token: 0x060089B0 RID: 35248 RVA: 0x0034A4B0 File Offset: 0x003486B0
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

	// Token: 0x060089B1 RID: 35249 RVA: 0x0034A4EC File Offset: 0x003486EC
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

	// Token: 0x060089B2 RID: 35250 RVA: 0x0034A598 File Offset: 0x00348798
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

	// Token: 0x04006681 RID: 26241
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiM_Workstation workstationData;
}
