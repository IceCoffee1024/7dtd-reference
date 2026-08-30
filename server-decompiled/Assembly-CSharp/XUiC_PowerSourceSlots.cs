using System;
using UnityEngine.Scripting;

// Token: 0x02000FA0 RID: 4000
[Preserve]
public class XUiC_PowerSourceSlots : XUiC_ItemStackGrid
{
	// Token: 0x17000EA0 RID: 3744
	// (get) Token: 0x06007E40 RID: 32320 RVA: 0x002FF4AE File Offset: 0x002FD6AE
	// (set) Token: 0x06007E41 RID: 32321 RVA: 0x002FF4B6 File Offset: 0x002FD6B6
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

	// Token: 0x06007E42 RID: 32322 RVA: 0x002FF4D0 File Offset: 0x002FD6D0
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

	// Token: 0x17000EA1 RID: 3745
	// (get) Token: 0x06007E43 RID: 32323 RVA: 0x00081148 File Offset: 0x0007F348
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x17000EA2 RID: 3746
	// (get) Token: 0x06007E44 RID: 32324 RVA: 0x002FF519 File Offset: 0x002FD719
	// (set) Token: 0x06007E45 RID: 32325 RVA: 0x002FF521 File Offset: 0x002FD721
	public XUiC_PowerSourceWindowGroup Owner { get; set; }

	// Token: 0x06007E46 RID: 32326 RVA: 0x002FE863 File Offset: 0x002FCA63
	public virtual void SetSlots(ItemStack[] stacks)
	{
		this.items = stacks;
		base.SetStacks(stacks);
	}

	// Token: 0x06007E47 RID: 32327 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool HasRequirement(Recipe recipe)
	{
		return true;
	}

	// Token: 0x06007E48 RID: 32328 RVA: 0x002FF52C File Offset: 0x002FD72C
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

	// Token: 0x06007E49 RID: 32329 RVA: 0x002FF58C File Offset: 0x002FD78C
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

	// Token: 0x06007E4A RID: 32330 RVA: 0x002FF5E8 File Offset: 0x002FD7E8
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

	// Token: 0x06007E4B RID: 32331 RVA: 0x002FF620 File Offset: 0x002FD820
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.tileEntity.ItemSlots = stackList;
		this.tileEntity.SetSendSlots();
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06007E4C RID: 32332 RVA: 0x002FF650 File Offset: 0x002FD850
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

	// Token: 0x06007E4D RID: 32333 RVA: 0x002FF6AB File Offset: 0x002FD8AB
	public void Refresh()
	{
		this.SetSlots(this.tileEntity.ItemSlots);
	}

	// Token: 0x06007E4E RID: 32334 RVA: 0x002FF6BE File Offset: 0x002FD8BE
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

	// Token: 0x04005D8E RID: 23950
	public static XUiC_PowerSourceSlots Current;

	// Token: 0x04005D8F RID: 23951
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowerSource tileEntity;
}
