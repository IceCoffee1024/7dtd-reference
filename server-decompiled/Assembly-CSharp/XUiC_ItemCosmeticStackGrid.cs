using System;
using UnityEngine.Scripting;

// Token: 0x02000F3F RID: 3903
[Preserve]
public class XUiC_ItemCosmeticStackGrid : XUiController
{
	// Token: 0x17000E4A RID: 3658
	// (get) Token: 0x06007AA7 RID: 31399 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Backpack;
		}
	}

	// Token: 0x17000E4B RID: 3659
	// (get) Token: 0x06007AA8 RID: 31400 RVA: 0x002E5E4B File Offset: 0x002E404B
	// (set) Token: 0x06007AA9 RID: 31401 RVA: 0x002E5E53 File Offset: 0x002E4053
	public ItemStack CurrentItem { get; set; }

	// Token: 0x17000E4C RID: 3660
	// (get) Token: 0x06007AAA RID: 31402 RVA: 0x002E5E5C File Offset: 0x002E405C
	// (set) Token: 0x06007AAB RID: 31403 RVA: 0x002E5E64 File Offset: 0x002E4064
	public XUiC_AssembleWindow AssembleWindow { get; set; }

	// Token: 0x06007AAC RID: 31404 RVA: 0x002E5E70 File Offset: 0x002E4070
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_ItemCosmeticStack>(null);
		this.itemControllers = childrenByType;
		this.IsDirty = false;
	}

	// Token: 0x06007AAD RID: 31405 RVA: 0x002E5C05 File Offset: 0x002E3E05
	public override void Update(float _dt)
	{
		if (GameManager.Instance == null && GameManager.Instance.World == null)
		{
			return;
		}
		base.Update(_dt);
	}

	// Token: 0x06007AAE RID: 31406 RVA: 0x002E5E9C File Offset: 0x002E409C
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

	// Token: 0x06007AAF RID: 31407 RVA: 0x002E5FBC File Offset: 0x002E41BC
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

	// Token: 0x06007AB0 RID: 31408 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007AB1 RID: 31409 RVA: 0x002DD4B9 File Offset: 0x002DB6B9
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.IsDirty = true;
	}

	// Token: 0x06007AB2 RID: 31410 RVA: 0x002E5E28 File Offset: 0x002E4028
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x04005AA9 RID: 23209
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x04005AAA RID: 23210
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x04005AAB RID: 23211
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController[] itemControllers;

	// Token: 0x04005AAC RID: 23212
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack[] items;

	// Token: 0x04005AAF RID: 23215
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass currentItemClass;
}
