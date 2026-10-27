using System;
using UnityEngine.Scripting;

// Token: 0x02000F74 RID: 3956
[Preserve]
public class XUiC_ItemCosmeticStackGrid : XUiController
{
	// Token: 0x17000E8C RID: 3724
	// (get) Token: 0x06007BA0 RID: 31648 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Backpack;
		}
	}

	// Token: 0x17000E8D RID: 3725
	// (get) Token: 0x06007BA1 RID: 31649 RVA: 0x002EE8E4 File Offset: 0x002ECAE4
	// (set) Token: 0x06007BA2 RID: 31650 RVA: 0x002EE8EC File Offset: 0x002ECAEC
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

	// Token: 0x17000E8E RID: 3726
	// (get) Token: 0x06007BA3 RID: 31651 RVA: 0x002EE8FB File Offset: 0x002ECAFB
	// (set) Token: 0x06007BA4 RID: 31652 RVA: 0x002EE903 File Offset: 0x002ECB03
	public XUiC_AssembleWindow AssembleWindow { get; set; }

	// Token: 0x06007BA5 RID: 31653 RVA: 0x002EE90C File Offset: 0x002ECB0C
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_ItemCosmeticStack>(null);
		this.itemControllers = childrenByType;
		this.IsDirty = false;
	}

	// Token: 0x06007BA6 RID: 31654 RVA: 0x002EE6C1 File Offset: 0x002EC8C1
	public override void Update(float _dt)
	{
		if (GameManager.Instance == null && GameManager.Instance.World == null)
		{
			return;
		}
		base.Update(_dt);
	}

	// Token: 0x06007BA7 RID: 31655 RVA: 0x002EE938 File Offset: 0x002ECB38
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetParts()
	{
		ItemValue itemValue = this.currentItem.itemValue;
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_ItemCosmeticStack xuiC_ItemCosmeticStack = (XUiC_ItemCosmeticStack)this.itemControllers[i];
			if (i < itemValue.CosmeticModCount)
			{
				ItemValue cosmeticMod = itemValue.GetCosmeticMod(i);
				if (cosmeticMod != null && cosmeticMod.ItemClass is ItemClassModifier)
				{
					xuiC_ItemCosmeticStack.SlotType = (cosmeticMod.ItemClass as ItemClassModifier).Type.ToStringCached<ItemClassModifier.ModifierTypes>().ToLower();
				}
				xuiC_ItemCosmeticStack.SlotChangedEvent -= this.HandleSlotChangedEvent;
				xuiC_ItemCosmeticStack.ItemValue = ((cosmeticMod != null) ? cosmeticMod : ItemValue.None);
				xuiC_ItemCosmeticStack.SlotChangedEvent += this.HandleSlotChangedEvent;
				xuiC_ItemCosmeticStack.SlotNumber = i;
				xuiC_ItemCosmeticStack.InfoWindow = childByType;
				xuiC_ItemCosmeticStack.StackLocation = this.StackLocation;
				xuiC_ItemCosmeticStack.ViewComponent.IsVisible = true;
			}
			else
			{
				xuiC_ItemCosmeticStack.ViewComponent.IsVisible = false;
			}
		}
	}

	// Token: 0x06007BA8 RID: 31656 RVA: 0x002EEA38 File Offset: 0x002ECC38
	public void HandleSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		XUiC_ItemCosmeticStack xuiC_ItemCosmeticStack = (XUiC_ItemCosmeticStack)this.itemControllers[slotNumber];
		ItemValue itemValue = (xuiC_ItemCosmeticStack.ItemStack.IsEmpty() ? ItemValue.None : xuiC_ItemCosmeticStack.ItemStack.itemValue);
		if (itemValue.ItemClass != null)
		{
			ItemClassModifier itemClassModifier = itemValue.ItemClass as ItemClassModifier;
			if (itemClassModifier != null)
			{
				if (!itemClassModifier.ModifierTags.Test_AnySet(ItemClassModifier.CosmeticModTypes) && this.CurrentItem.itemValue.ModificationCount > 0)
				{
					for (int i = 0; i < this.CurrentItem.itemValue.ModificationCount; i++)
					{
						ItemValue modification = this.CurrentItem.itemValue.GetModification(i);
						if (modification == null || modification.IsEmpty())
						{
							this.CurrentItem.itemValue.SetModification(i, itemValue);
							break;
						}
					}
					goto IL_00E7;
				}
				this.CurrentItem.itemValue.SetCosmeticMod(slotNumber, itemValue);
				goto IL_00E7;
			}
		}
		this.CurrentItem.itemValue.SetCosmeticMod(slotNumber, itemValue);
		IL_00E7:
		this.AssembleWindow.ItemStack = this.CurrentItem;
		this.AssembleWindow.OnChanged();
		this.xui.AssembleItem.RefreshAssembleItem();
	}

	// Token: 0x06007BA9 RID: 31657 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007BAA RID: 31658 RVA: 0x002E60CD File Offset: 0x002E42CD
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.IsDirty = true;
	}

	// Token: 0x06007BAB RID: 31659 RVA: 0x002EE8C1 File Offset: 0x002ECAC1
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x04005BC6 RID: 23494
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x04005BC7 RID: 23495
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x04005BC8 RID: 23496
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController[] itemControllers;

	// Token: 0x04005BC9 RID: 23497
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack[] items;

	// Token: 0x04005BCA RID: 23498
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack currentItem;
}
