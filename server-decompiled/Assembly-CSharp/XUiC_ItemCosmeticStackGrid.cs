using System;
using UnityEngine.Scripting;

// Token: 0x02000F65 RID: 3941
[Preserve]
public class XUiC_ItemCosmeticStackGrid : XUiController
{
	// Token: 0x17000E82 RID: 3714
	// (get) Token: 0x06007B5D RID: 31581 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Backpack;
		}
	}

	// Token: 0x17000E83 RID: 3715
	// (get) Token: 0x06007B5E RID: 31582 RVA: 0x002ECFE8 File Offset: 0x002EB1E8
	// (set) Token: 0x06007B5F RID: 31583 RVA: 0x002ECFF0 File Offset: 0x002EB1F0
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

	// Token: 0x17000E84 RID: 3716
	// (get) Token: 0x06007B60 RID: 31584 RVA: 0x002ECFFF File Offset: 0x002EB1FF
	// (set) Token: 0x06007B61 RID: 31585 RVA: 0x002ED007 File Offset: 0x002EB207
	public XUiC_AssembleWindow AssembleWindow { get; set; }

	// Token: 0x06007B62 RID: 31586 RVA: 0x002ED010 File Offset: 0x002EB210
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_ItemCosmeticStack>(null);
		this.itemControllers = childrenByType;
		this.IsDirty = false;
	}

	// Token: 0x06007B63 RID: 31587 RVA: 0x002ECDC5 File Offset: 0x002EAFC5
	public override void Update(float _dt)
	{
		if (GameManager.Instance == null && GameManager.Instance.World == null)
		{
			return;
		}
		base.Update(_dt);
	}

	// Token: 0x06007B64 RID: 31588 RVA: 0x002ED03C File Offset: 0x002EB23C
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

	// Token: 0x06007B65 RID: 31589 RVA: 0x002ED13C File Offset: 0x002EB33C
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

	// Token: 0x06007B66 RID: 31590 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007B67 RID: 31591 RVA: 0x002E47DD File Offset: 0x002E29DD
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.IsDirty = true;
	}

	// Token: 0x06007B68 RID: 31592 RVA: 0x002ECFC5 File Offset: 0x002EB1C5
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x04005B93 RID: 23443
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x04005B94 RID: 23444
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x04005B95 RID: 23445
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController[] itemControllers;

	// Token: 0x04005B96 RID: 23446
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack[] items;

	// Token: 0x04005B97 RID: 23447
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack currentItem;
}
