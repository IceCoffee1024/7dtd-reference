using System;
using UnityEngine.Scripting;

// Token: 0x02000FC8 RID: 4040
[Preserve]
public class XUiC_PowerSourceSlots : XUiC_ItemStackGrid
{
	// Token: 0x17000F00 RID: 3840
	// (get) Token: 0x06007F41 RID: 32577 RVA: 0x00306C5A File Offset: 0x00304E5A
	// (set) Token: 0x06007F42 RID: 32578 RVA: 0x00306C62 File Offset: 0x00304E62
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

	// Token: 0x06007F43 RID: 32579 RVA: 0x00306C7C File Offset: 0x00304E7C
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

	// Token: 0x17000F01 RID: 3841
	// (get) Token: 0x06007F44 RID: 32580 RVA: 0x000601CD File Offset: 0x0005E3CD
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x17000F02 RID: 3842
	// (get) Token: 0x06007F45 RID: 32581 RVA: 0x00306CC5 File Offset: 0x00304EC5
	// (set) Token: 0x06007F46 RID: 32582 RVA: 0x00306CCD File Offset: 0x00304ECD
	public XUiC_PowerSourceWindowGroup Owner { get; set; }

	// Token: 0x06007F47 RID: 32583 RVA: 0x0030600F File Offset: 0x0030420F
	public virtual void SetSlots(ItemStack[] stacks)
	{
		this.items = stacks;
		base.SetStacks(stacks);
	}

	// Token: 0x06007F48 RID: 32584 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool HasRequirement(Recipe recipe)
	{
		return true;
	}

	// Token: 0x06007F49 RID: 32585 RVA: 0x00306CD8 File Offset: 0x00304ED8
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

	// Token: 0x06007F4A RID: 32586 RVA: 0x00306D38 File Offset: 0x00304F38
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

	// Token: 0x06007F4B RID: 32587 RVA: 0x00306D94 File Offset: 0x00304F94
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

	// Token: 0x06007F4C RID: 32588 RVA: 0x00306DCC File Offset: 0x00304FCC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.tileEntity.ItemSlots = stackList;
		this.tileEntity.SetSendSlots();
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06007F4D RID: 32589 RVA: 0x00306DFC File Offset: 0x00304FFC
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

	// Token: 0x06007F4E RID: 32590 RVA: 0x00306E57 File Offset: 0x00305057
	public void Refresh()
	{
		this.SetSlots(this.tileEntity.ItemSlots);
	}

	// Token: 0x06007F4F RID: 32591 RVA: 0x00306E6A File Offset: 0x0030506A
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

	// Token: 0x04005E7E RID: 24190
	public static XUiC_PowerSourceSlots Current;

	// Token: 0x04005E7F RID: 24191
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowerSource tileEntity;
}
