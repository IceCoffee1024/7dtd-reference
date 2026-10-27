using System;
using UnityEngine.Scripting;

// Token: 0x02000F73 RID: 3955
[Preserve]
public class XUiC_ItemPartStackGrid : XUiController
{
	// Token: 0x17000E89 RID: 3721
	// (get) Token: 0x06007B93 RID: 31635 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Backpack;
		}
	}

	// Token: 0x17000E8A RID: 3722
	// (get) Token: 0x06007B94 RID: 31636 RVA: 0x002EE66F File Offset: 0x002EC86F
	// (set) Token: 0x06007B95 RID: 31637 RVA: 0x002EE677 File Offset: 0x002EC877
	public ItemStack CurrentItem
	{
		get
		{
			return this.currentItem;
		}
		set
		{
			this.currentItem = value;
			this.SetParts();
		}
	}

	// Token: 0x17000E8B RID: 3723
	// (get) Token: 0x06007B96 RID: 31638 RVA: 0x002EE686 File Offset: 0x002EC886
	// (set) Token: 0x06007B97 RID: 31639 RVA: 0x002EE68E File Offset: 0x002EC88E
	public XUiC_AssembleWindow AssembleWindow { get; set; }

	// Token: 0x06007B98 RID: 31640 RVA: 0x002EE698 File Offset: 0x002EC898
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_ItemPartStack>(null);
		this.itemControllers = childrenByType;
		this.IsDirty = false;
	}

	// Token: 0x06007B99 RID: 31641 RVA: 0x002EE6C1 File Offset: 0x002EC8C1
	public override void Update(float _dt)
	{
		if (GameManager.Instance == null && GameManager.Instance.World == null)
		{
			return;
		}
		base.Update(_dt);
	}

	// Token: 0x06007B9A RID: 31642 RVA: 0x002EE6E4 File Offset: 0x002EC8E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetParts()
	{
		ItemValue itemValue = this.currentItem.itemValue;
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_ItemPartStack xuiC_ItemPartStack = (XUiC_ItemPartStack)this.itemControllers[i];
			if (i < itemValue.ModificationCount)
			{
				ItemValue modification = itemValue.GetModification(i);
				if (modification != null && modification.ItemClass is ItemClassModifier)
				{
					xuiC_ItemPartStack.SlotType = (modification.ItemClass as ItemClassModifier).Type.ToStringCached<ItemClassModifier.ModifierTypes>().ToLower();
				}
				xuiC_ItemPartStack.SlotChangedEvent -= this.HandleSlotChangedEvent;
				xuiC_ItemPartStack.ItemValue = ((modification != null) ? modification : ItemValue.None);
				xuiC_ItemPartStack.SlotChangedEvent += this.HandleSlotChangedEvent;
				xuiC_ItemPartStack.SlotNumber = i;
				xuiC_ItemPartStack.InfoWindow = childByType;
				xuiC_ItemPartStack.StackLocation = this.StackLocation;
				xuiC_ItemPartStack.ViewComponent.IsVisible = true;
			}
			else
			{
				xuiC_ItemPartStack.ViewComponent.IsVisible = false;
			}
		}
	}

	// Token: 0x06007B9B RID: 31643 RVA: 0x002EE7E4 File Offset: 0x002EC9E4
	public void HandleSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		XUiC_ItemPartStack xuiC_ItemPartStack = (XUiC_ItemPartStack)this.itemControllers[slotNumber];
		ItemValue itemValue = (xuiC_ItemPartStack.ItemStack.IsEmpty() ? ItemValue.None : xuiC_ItemPartStack.ItemStack.itemValue);
		if (itemValue.ItemClass != null)
		{
			ItemClassModifier itemClassModifier = itemValue.ItemClass as ItemClassModifier;
			if (itemClassModifier != null)
			{
				if (itemClassModifier.ModifierTags.Test_AnySet(ItemClassModifier.CosmeticModTypes) && this.CurrentItem.itemValue.CosmeticModCount > 0)
				{
					this.CurrentItem.itemValue.SetCosmeticMod(0, itemValue);
					goto IL_00A4;
				}
				this.CurrentItem.itemValue.SetModification(slotNumber, itemValue);
				goto IL_00A4;
			}
		}
		this.CurrentItem.itemValue.SetModification(slotNumber, itemValue);
		IL_00A4:
		this.AssembleWindow.ItemStack = this.CurrentItem;
		this.AssembleWindow.OnChanged();
		this.xui.AssembleItem.RefreshAssembleItem();
	}

	// Token: 0x06007B9C RID: 31644 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007B9D RID: 31645 RVA: 0x002E60CD File Offset: 0x002E42CD
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.IsDirty = true;
	}

	// Token: 0x06007B9E RID: 31646 RVA: 0x002EE8C1 File Offset: 0x002ECAC1
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x04005BC0 RID: 23488
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x04005BC1 RID: 23489
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x04005BC2 RID: 23490
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController[] itemControllers;

	// Token: 0x04005BC3 RID: 23491
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack[] items;

	// Token: 0x04005BC4 RID: 23492
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack currentItem;
}
