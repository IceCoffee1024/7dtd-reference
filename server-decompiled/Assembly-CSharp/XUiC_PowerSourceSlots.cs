using System;
using UnityEngine.Scripting;

// Token: 0x02000FD8 RID: 4056
[Preserve]
public class XUiC_PowerSourceSlots : XUiC_ItemStackGrid
{
	// Token: 0x17000F0B RID: 3851
	// (get) Token: 0x06007F90 RID: 32656 RVA: 0x00308776 File Offset: 0x00306976
	// (set) Token: 0x06007F91 RID: 32657 RVA: 0x0030877E File Offset: 0x0030697E
	public TileEntityPowerSource TileEntity
	{
		get
		{
			return this.tileEntity;
		}
		set
		{
			this.tileEntity = value;
			this.SetSlots(this.tileEntity.ItemSlots);
		}
	}

	// Token: 0x06007F92 RID: 32658 RVA: 0x00308798 File Offset: 0x00306998
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRequirements()
	{
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_RequiredItemStack xuiC_RequiredItemStack = this.itemControllers[i] as XUiC_RequiredItemStack;
			if (xuiC_RequiredItemStack != null)
			{
				xuiC_RequiredItemStack.RequiredType = XUiC_RequiredItemStack.RequiredTypes.ItemClass;
				xuiC_RequiredItemStack.SetAllowedItemClassSingle(this.tileEntity.SlotItem);
			}
		}
	}

	// Token: 0x17000F0C RID: 3852
	// (get) Token: 0x06007F93 RID: 32659 RVA: 0x00060491 File Offset: 0x0005E691
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x17000F0D RID: 3853
	// (get) Token: 0x06007F94 RID: 32660 RVA: 0x003087E1 File Offset: 0x003069E1
	// (set) Token: 0x06007F95 RID: 32661 RVA: 0x003087E9 File Offset: 0x003069E9
	public XUiC_PowerSourceWindowGroup Owner { get; set; }

	// Token: 0x06007F96 RID: 32662 RVA: 0x00307B2B File Offset: 0x00305D2B
	public virtual void SetSlots(ItemStack[] stacks)
	{
		this.items = stacks;
		base.SetStacks(stacks);
	}

	// Token: 0x06007F97 RID: 32663 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool HasRequirement(Recipe recipe)
	{
		return true;
	}

	// Token: 0x06007F98 RID: 32664 RVA: 0x003087F4 File Offset: 0x003069F4
	public override void OnOpen()
	{
		base.OnOpen();
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.OnOpen();
			base.ViewComponent.IsVisible = true;
		}
		this.IsDirty = true;
		this.SetRequirements();
		this.xui.CurrentPowerSourceSlots = this;
		XUiC_PowerSourceSlots.Current = this;
	}

	// Token: 0x06007F99 RID: 32665 RVA: 0x00308854 File Offset: 0x00306A54
	public override void OnClose()
	{
		base.OnClose();
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.OnClose();
			base.ViewComponent.IsVisible = false;
		}
		this.IsDirty = true;
		XUiC_PowerSourceSlots.Current = (this.xui.CurrentPowerSourceSlots = null);
	}

	// Token: 0x06007F9A RID: 32666 RVA: 0x003088B0 File Offset: 0x00306AB0
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetOn(bool isOn)
	{
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_RequiredItemStack xuiC_RequiredItemStack = this.itemControllers[i] as XUiC_RequiredItemStack;
			if (xuiC_RequiredItemStack != null)
			{
				xuiC_RequiredItemStack.ToolLock = isOn;
			}
		}
	}

	// Token: 0x06007F9B RID: 32667 RVA: 0x003088E8 File Offset: 0x00306AE8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.tileEntity.ItemSlots = stackList;
		this.tileEntity.SetSendSlots();
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06007F9C RID: 32668 RVA: 0x00308918 File Offset: 0x00306B18
	public override void Update(float _dt)
	{
		if (GameManager.Instance == null && GameManager.Instance.World == null)
		{
			return;
		}
		if (this.tileEntity == null)
		{
			return;
		}
		base.Update(_dt);
		if (this.tileEntity.IsOn)
		{
			this.SetSlots(this.tileEntity.ItemSlots);
		}
		base.RefreshBindings();
	}

	// Token: 0x06007F9D RID: 32669 RVA: 0x00308973 File Offset: 0x00306B73
	public void Refresh()
	{
		this.SetSlots(this.tileEntity.ItemSlots);
	}

	// Token: 0x06007F9E RID: 32670 RVA: 0x00308986 File Offset: 0x00306B86
	[PublicizedFrom(EAccessModifier.Internal)]
	public bool TryAddItemToSlot(ItemClass itemClass, ItemStack itemStack)
	{
		if (itemClass != this.tileEntity.SlotItem)
		{
			return false;
		}
		bool flag = this.tileEntity.TryAddItemToSlot(itemClass, itemStack);
		if (flag)
		{
			this.SetSlots(this.tileEntity.ItemSlots);
		}
		return flag;
	}

	// Token: 0x04005EB9 RID: 24249
	public static XUiC_PowerSourceSlots Current;

	// Token: 0x04005EBA RID: 24250
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowerSource tileEntity;
}
