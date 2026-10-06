using System;
using UnityEngine.Scripting;

// Token: 0x020010E0 RID: 4320
[Preserve]
public class XUiC_WorkstationGrid : XUiC_ItemStackGrid
{
	// Token: 0x17001022 RID: 4130
	// (get) Token: 0x06008956 RID: 35158 RVA: 0x00348303 File Offset: 0x00346503
	public XUiM_Workstation WorkstationData
	{
		get
		{
			return this.workstationData;
		}
	}

	// Token: 0x17001023 RID: 4131
	// (get) Token: 0x06008957 RID: 35159 RVA: 0x000601CD File Offset: 0x0005E3CD
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x06008958 RID: 35160 RVA: 0x0034830B File Offset: 0x0034650B
	public virtual void SetSlots(ItemStack[] stacks)
	{
		base.SetStacks(stacks);
	}

	// Token: 0x06008959 RID: 35161 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool HasRequirement(Recipe recipe)
	{
		return true;
	}

	// Token: 0x0600895A RID: 35162 RVA: 0x00348314 File Offset: 0x00346514
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

	// Token: 0x0600895B RID: 35163 RVA: 0x0034836F File Offset: 0x0034656F
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.OnClose();
			base.ViewComponent.IsVisible = false;
		}
		this.IsDirty = true;
	}

	// Token: 0x0600895C RID: 35164 RVA: 0x003483A4 File Offset: 0x003465A4
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

	// Token: 0x0600895D RID: 35165 RVA: 0x0034842C File Offset: 0x0034662C
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

	// Token: 0x0600895E RID: 35166 RVA: 0x00348468 File Offset: 0x00346668
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

	// Token: 0x0600895F RID: 35167 RVA: 0x00348514 File Offset: 0x00346714
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

	// Token: 0x04006642 RID: 26178
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiM_Workstation workstationData;
}
