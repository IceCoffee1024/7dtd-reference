using System;
using UnityEngine.Scripting;

// Token: 0x02000FB9 RID: 4025
[Preserve]
public class XUiC_PowerSourceSlots : XUiC_ItemStackGrid
{
	// Token: 0x17000ED9 RID: 3801
	// (get) Token: 0x06007F32 RID: 32562 RVA: 0x00303C82 File Offset: 0x00301E82
	// (set) Token: 0x06007F33 RID: 32563 RVA: 0x00303C8A File Offset: 0x00301E8A
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

	// Token: 0x06007F34 RID: 32564 RVA: 0x00303CA4 File Offset: 0x00301EA4
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

	// Token: 0x17000EDA RID: 3802
	// (get) Token: 0x06007F35 RID: 32565 RVA: 0x000817FC File Offset: 0x0007F9FC
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x17000EDB RID: 3803
	// (get) Token: 0x06007F36 RID: 32566 RVA: 0x00303CED File Offset: 0x00301EED
	// (set) Token: 0x06007F37 RID: 32567 RVA: 0x00303CF5 File Offset: 0x00301EF5
	public XUiC_PowerSourceWindowGroup Owner { get; set; }

	// Token: 0x06007F38 RID: 32568 RVA: 0x00303037 File Offset: 0x00301237
	public virtual void SetSlots(ItemStack[] stacks)
	{
		this.items = stacks;
		base.SetStacks(stacks);
	}

	// Token: 0x06007F39 RID: 32569 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool HasRequirement(Recipe recipe)
	{
		return true;
	}

	// Token: 0x06007F3A RID: 32570 RVA: 0x00303D00 File Offset: 0x00301F00
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

	// Token: 0x06007F3B RID: 32571 RVA: 0x00303D60 File Offset: 0x00301F60
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

	// Token: 0x06007F3C RID: 32572 RVA: 0x00303DBC File Offset: 0x00301FBC
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

	// Token: 0x06007F3D RID: 32573 RVA: 0x00303DF4 File Offset: 0x00301FF4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.tileEntity.ItemSlots = stackList;
		this.tileEntity.SetSendSlots();
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06007F3E RID: 32574 RVA: 0x00303E24 File Offset: 0x00302024
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

	// Token: 0x06007F3F RID: 32575 RVA: 0x00303E7F File Offset: 0x0030207F
	public void Refresh()
	{
		this.SetSlots(this.tileEntity.ItemSlots);
	}

	// Token: 0x06007F40 RID: 32576 RVA: 0x00303E92 File Offset: 0x00302092
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

	// Token: 0x04005E55 RID: 24149
	public static XUiC_PowerSourceSlots Current;

	// Token: 0x04005E56 RID: 24150
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowerSource tileEntity;
}
