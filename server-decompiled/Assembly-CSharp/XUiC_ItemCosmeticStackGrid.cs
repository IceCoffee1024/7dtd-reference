using System;
using UnityEngine.Scripting;

// Token: 0x02000F56 RID: 3926
[Preserve]
public class XUiC_ItemCosmeticStackGrid : XUiController
{
	// Token: 0x17000E65 RID: 3685
	// (get) Token: 0x06007B60 RID: 31584 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Backpack;
		}
	}

	// Token: 0x17000E66 RID: 3686
	// (get) Token: 0x06007B61 RID: 31585 RVA: 0x002E9CD7 File Offset: 0x002E7ED7
	// (set) Token: 0x06007B62 RID: 31586 RVA: 0x002E9CDF File Offset: 0x002E7EDF
	public ItemStack CurrentItem { get; set; }

	// Token: 0x17000E67 RID: 3687
	// (get) Token: 0x06007B63 RID: 31587 RVA: 0x002E9CE8 File Offset: 0x002E7EE8
	// (set) Token: 0x06007B64 RID: 31588 RVA: 0x002E9CF0 File Offset: 0x002E7EF0
	public XUiC_AssembleWindow AssembleWindow { get; set; }

	// Token: 0x06007B65 RID: 31589 RVA: 0x002E9CFC File Offset: 0x002E7EFC
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_ItemCosmeticStack>(null);
		this.itemControllers = childrenByType;
		this.IsDirty = false;
	}

	// Token: 0x06007B66 RID: 31590 RVA: 0x002E9A91 File Offset: 0x002E7C91
	public override void Update(float _dt)
	{
		if (GameManager.Instance == null && GameManager.Instance.World == null)
		{
			return;
		}
		base.Update(_dt);
	}

	// Token: 0x06007B67 RID: 31591 RVA: 0x002E9D28 File Offset: 0x002E7F28
	public void SetParts(ItemValue[] stackList)
	{
		if (stackList == null)
		{
			return;
		}
		this.currentItemClass = this.CurrentItem.itemValue.ItemClass;
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_ItemCosmeticStack xuiC_ItemCosmeticStack = (XUiC_ItemCosmeticStack)this.itemControllers[i];
			if (i < this.CurrentItem.itemValue.CosmeticMods.Length)
			{
				ItemValue itemValue = this.CurrentItem.itemValue.CosmeticMods[i];
				if (itemValue != null && itemValue.ItemClass is ItemClassModifier)
				{
					xuiC_ItemCosmeticStack.SlotType = (itemValue.ItemClass as ItemClassModifier).Type.ToStringCached<ItemClassModifier.ModifierTypes>().ToLower();
				}
				xuiC_ItemCosmeticStack.SlotChangedEvent -= this.HandleSlotChangedEvent;
				xuiC_ItemCosmeticStack.ItemValue = ((itemValue != null) ? itemValue : ItemValue.None);
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

	// Token: 0x06007B68 RID: 31592 RVA: 0x002E9E48 File Offset: 0x002E8048
	public void HandleSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		XUiC_ItemCosmeticStack xuiC_ItemCosmeticStack = (XUiC_ItemCosmeticStack)this.itemControllers[slotNumber];
		ItemValue itemValue = (xuiC_ItemCosmeticStack.ItemStack.IsEmpty() ? ItemValue.None : xuiC_ItemCosmeticStack.ItemStack.itemValue);
		if (itemValue.ItemClass != null)
		{
			ItemClassModifier itemClassModifier = itemValue.ItemClass as ItemClassModifier;
			if (itemClassModifier != null)
			{
				if (!itemClassModifier.ModifierTags.Test_AnySet(ItemClassModifier.CosmeticModTypes) && this.CurrentItem.itemValue.Modifications.Length != 0)
				{
					for (int i = 0; i < this.CurrentItem.itemValue.Modifications.Length; i++)
					{
						ItemValue itemValue2 = this.CurrentItem.itemValue.Modifications[i];
						if (itemValue2 == null || itemValue2.IsEmpty())
						{
							this.CurrentItem.itemValue.Modifications[i] = itemValue;
							break;
						}
					}
					goto IL_00ED;
				}
				this.CurrentItem.itemValue.CosmeticMods[slotNumber] = itemValue;
				goto IL_00ED;
			}
		}
		this.CurrentItem.itemValue.CosmeticMods[slotNumber] = itemValue;
		IL_00ED:
		this.AssembleWindow.ItemStack = this.CurrentItem;
		this.AssembleWindow.OnChanged();
		this.xui.AssembleItem.RefreshAssembleItem();
	}

	// Token: 0x06007B69 RID: 31593 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007B6A RID: 31594 RVA: 0x002E146D File Offset: 0x002DF66D
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.IsDirty = true;
	}

	// Token: 0x06007B6B RID: 31595 RVA: 0x002E9CB4 File Offset: 0x002E7EB4
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x04005B5C RID: 23388
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x04005B5D RID: 23389
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x04005B5E RID: 23390
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController[] itemControllers;

	// Token: 0x04005B5F RID: 23391
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack[] items;

	// Token: 0x04005B62 RID: 23394
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass currentItemClass;
}
