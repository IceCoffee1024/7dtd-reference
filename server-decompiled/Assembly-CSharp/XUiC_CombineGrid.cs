using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F13 RID: 3859
[Preserve]
public class XUiC_CombineGrid : XUiC_ItemStackGrid
{
	// Token: 0x0600784C RID: 30796 RVA: 0x002D8EF8 File Offset: 0x002D70F8
	public override void Init()
	{
		base.Init();
		if (this.itemControllers.Length == 2)
		{
			this.merge1 = (XUiC_RequiredItemStack)this.itemControllers[0];
			this.merge2 = (XUiC_RequiredItemStack)this.itemControllers[1];
			this.merge1.StackLocation = (this.merge2.StackLocation = XUiC_ItemStack.StackLocationTypes.Merge);
			this.merge1.RequiredType = (this.merge2.RequiredType = XUiC_RequiredItemStack.RequiredTypes.HasQualityNoParts);
			this.merge1.SlotChangedEvent += this.Merge_SlotChangedEvent;
			this.merge2.SlotChangedEvent += this.Merge_SlotChangedEvent;
			this.merge1.FailedSwap += this.Merge_FailedSwap;
			this.merge2.FailedSwap += this.Merge_FailedSwap;
		}
		this.btnCombine = base.GetChildById("btnCombine").GetChildByType<XUiC_SimpleButton>();
		this.btnCombine.Enabled = false;
		this.btnCombine.OnPressed += this.BtnCombine_OnPressed;
	}

	// Token: 0x0600784D RID: 30797 RVA: 0x002D9008 File Offset: 0x002D7208
	public void SetTileEntity(TEFeatureCombine _te)
	{
		this.te = _te;
	}

	// Token: 0x0600784E RID: 30798 RVA: 0x002D9014 File Offset: 0x002D7214
	[PublicizedFrom(EAccessModifier.Private)]
	public void Merge_FailedSwap(ItemStack stack)
	{
		ItemValue itemValue = stack.itemValue;
		if (!itemValue.HasQuality || itemValue.ItemClass.HasSubItems)
		{
			GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, Localization.Get("ttCombineInvalidItem", false, null), false, false, 0f);
		}
	}

	// Token: 0x0600784F RID: 30799 RVA: 0x002D9068 File Offset: 0x002D7268
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCombine_OnPressed(XUiController _sender, int _mouseButton)
	{
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		if (!this.xui.PlayerInventory.AddItem(this.result))
		{
			GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, string.Format(Localization.Get("xuiInventoryFullForPickup", false, null), 0), string.Empty, "ui_denied", null, false, false, 0f);
			return;
		}
		ItemActionEntryScrap.HandleRemoveAmmo(this.merge1.ItemStack, this.xui);
		ItemActionEntryScrap.HandleRemoveAmmo(this.merge2.ItemStack, this.xui);
		entityPlayer.Progression.AddLevelExp((int)this.experienceFromLastResult, "_xpOther", Progression.XPTypes.Other, true, true, -1, null);
		this.merge1.ItemStack = ItemStack.Empty;
		this.merge2.ItemStack = ItemStack.Empty;
		this.SetResult(ItemStack.Empty);
		TEFeatureCombine tefeatureCombine = this.te;
		if (tefeatureCombine == null)
		{
			return;
		}
		tefeatureCombine.HandlePlayComplete();
	}

	// Token: 0x06007850 RID: 30800 RVA: 0x002D9164 File Offset: 0x002D7364
	[PublicizedFrom(EAccessModifier.Private)]
	public void Merge_SlotChangedEvent(int slotNumber, ItemStack stack)
	{
		this.btnCombine.Enabled = false;
		if (this.merge1.ItemStack.IsEmpty() || this.merge2.ItemStack.IsEmpty() || this.merge1.ItemStack.itemValue.type != this.merge2.ItemStack.itemValue.type)
		{
			this.SetResult(ItemStack.Empty);
			return;
		}
		ItemValue itemValue = this.merge1.ItemStack.itemValue;
		ItemValue itemValue2 = this.merge2.ItemStack.itemValue;
		if ((itemValue.HasMods() && itemValue2.HasMods()) || (itemValue.HasCosmetics() && itemValue2.HasCosmetics()))
		{
			GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, string.Format(Localization.Get("ttCombineRemoveMods", false, null), 0), string.Empty, "ui_denied", null, false, false, 0f);
			this.SetResult(ItemStack.Empty);
			return;
		}
		bool flag = true;
		if (!itemValue.EqualsForMerging(itemValue2))
		{
			ItemStack itemStack = this.merge1.ItemStack.Clone();
			itemStack.itemValue.MergeBest(this.merge2.ItemStack.itemValue);
			if ((itemStack.itemValue.Quality > itemValue.Quality || !itemStack.itemValue.EqualsForMerging(itemValue)) && (itemStack.itemValue.Quality > itemValue2.Quality || !itemStack.itemValue.EqualsForMerging(itemValue2)))
			{
				itemStack.itemValue.Meta = 0;
				this.SetResult(itemStack);
				flag = false;
			}
		}
		if (flag)
		{
			GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, string.Format(Localization.Get("ttCombineSameItem", false, null), 0), string.Empty, "ui_denied", null, false, false, 0f);
			this.SetResult(ItemStack.Empty);
		}
	}

	// Token: 0x06007851 RID: 30801 RVA: 0x002D9344 File Offset: 0x002D7544
	public override void OnOpen()
	{
		base.OnOpen();
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.OnOpen();
			base.ViewComponent.IsVisible = true;
		}
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		this.merge1.InfoWindow = childByType;
		this.merge2.InfoWindow = childByType;
		this.IsDirty = true;
		this.merge1.ItemStack = ItemStack.Empty;
		this.merge2.ItemStack = ItemStack.Empty;
		this.SetResult(ItemStack.Empty);
		this.xui.CurrentCombineGrid = this;
	}

	// Token: 0x06007852 RID: 30802 RVA: 0x002D93E8 File Offset: 0x002D75E8
	public override void OnClose()
	{
		base.OnClose();
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.OnClose();
			base.ViewComponent.IsVisible = false;
		}
		this.IsDirty = true;
		this.AddBackItem(this.merge1.ItemStack);
		this.AddBackItem(this.merge2.ItemStack);
		this.xui.CurrentCombineGrid = null;
	}

	// Token: 0x06007853 RID: 30803 RVA: 0x002D945C File Offset: 0x002D765C
	public bool HasEmptySlot()
	{
		return this.merge1.ItemStack.IsEmpty() || this.merge2.ItemStack.IsEmpty();
	}

	// Token: 0x06007854 RID: 30804 RVA: 0x002D9484 File Offset: 0x002D7684
	[PublicizedFrom(EAccessModifier.Internal)]
	public bool TryAddItemToSlot(ItemClass itemClass, ItemStack itemStack)
	{
		if (this.merge1.ItemStack.IsEmpty())
		{
			if (itemClass.HasQuality && !itemClass.HasSubItems)
			{
				this.merge1.ItemStack = itemStack;
				return true;
			}
		}
		else if (this.merge2.ItemStack.IsEmpty() && itemClass.HasQuality && !itemClass.HasSubItems)
		{
			this.merge2.ItemStack = itemStack;
			return true;
		}
		return false;
	}

	// Token: 0x06007855 RID: 30805 RVA: 0x002D94F4 File Offset: 0x002D76F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetResult(ItemStack newResult)
	{
		this.result = newResult;
		this.btnCombine.Enabled = !this.result.IsEmpty();
		ItemValue itemValue = this.result.itemValue;
		ItemClass itemClass = ((itemValue != null) ? itemValue.ItemClass : null);
		if (itemClass != null)
		{
			this.itemDisplayEntry = UIDisplayInfoManager.Current.GetDisplayStatsForTag(itemClass.DisplayType);
		}
		base.RefreshBindings();
	}

	// Token: 0x06007856 RID: 30806 RVA: 0x002D9558 File Offset: 0x002D7758
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddBackItem(ItemStack stack)
	{
		if (stack.IsEmpty())
		{
			return;
		}
		if (this.xui.PlayerInventory.AddItem(stack))
		{
			return;
		}
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		GameManager.Instance.ItemDropServer(stack, entityPlayer.GetPosition(), Vector3.zero, -1, 60f, false);
		entityPlayer.PlayOneShot("itemdropped", false, false, false, null, 1f);
	}

	// Token: 0x06007857 RID: 30807 RVA: 0x002D95C4 File Offset: 0x002D77C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		XUiC_CombineGrid.<>c__DisplayClass25_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		ItemValue itemValue = this.result.itemValue;
		CS$<>8__locals1.resultItemClass = ((itemValue != null) ? itemValue.ItemClass : null);
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		ValueTuple<string, bool> valueTuple;
		if (num <= 1388578781U)
		{
			if (num <= 847165955U)
			{
				if (num != 143608997U)
				{
					if (num == 847165955U)
					{
						if (bindingName == "itemtypeicon")
						{
							valueTuple = new ValueTuple<string, bool>(XUiBindingHelper.GetItemTypeIconValue(this.xui, this.result), true);
							goto IL_0384;
						}
					}
				}
				else if (bindingName == "removeddurabilityfill")
				{
					valueTuple = new ValueTuple<string, bool>(this.<GetBindingValueInternal>g__GetRemovedDurabilityFillValue|25_3(ref CS$<>8__locals1), true);
					goto IL_0384;
				}
			}
			else if (num != 1022877350U)
			{
				if (num != 1062608009U)
				{
					if (num == 1388578781U)
					{
						if (bindingName == "hasitemtypeicon")
						{
							valueTuple = new ValueTuple<string, bool>(XUiBindingHelper.GetHasItemTypeIconValue(this.result), true);
							goto IL_0384;
						}
					}
				}
				else if (bindingName == "durabilitycolor")
				{
					valueTuple = new ValueTuple<string, bool>(this.<GetBindingValueInternal>g__GetDurabilityColorValue|25_4(ref CS$<>8__locals1), true);
					goto IL_0384;
				}
			}
			else if (bindingName == "durabilityjustify")
			{
				valueTuple = new ValueTuple<string, bool>(this.<GetBindingValueInternal>g__GetDurabilityJustifyValue|25_6(ref CS$<>8__locals1), true);
				goto IL_0384;
			}
		}
		else if (num <= 3708628627U)
		{
			if (num != 1953932597U)
			{
				if (num != 2944858628U)
				{
					if (num == 3708628627U)
					{
						if (bindingName == "itemicon")
						{
							valueTuple = new ValueTuple<string, bool>(this.<GetBindingValueInternal>g__GetItemIconValue|25_0(ref CS$<>8__locals1), true);
							goto IL_0384;
						}
					}
				}
				else if (bindingName == "hasdurability")
				{
					valueTuple = new ValueTuple<string, bool>(this.<GetBindingValueInternal>g__GetHasDurabilityValue|25_2(ref CS$<>8__locals1), true);
					goto IL_0384;
				}
			}
			else if (bindingName == "durabilitytext")
			{
				valueTuple = new ValueTuple<string, bool>(this.<GetBindingValueInternal>g__GetDurabilityTextValue|25_7(ref CS$<>8__locals1), true);
				goto IL_0384;
			}
		}
		else if (num != 4049247086U)
		{
			if (num != 4053908414U)
			{
				if (num == 4172540779U)
				{
					if (bindingName == "durabilityfill")
					{
						valueTuple = new ValueTuple<string, bool>(this.<GetBindingValueInternal>g__GetDurabilityFillValue|25_5(ref CS$<>8__locals1), true);
						goto IL_0384;
					}
				}
			}
			else if (bindingName == "itemicontint")
			{
				valueTuple = new ValueTuple<string, bool>(this.<GetBindingValueInternal>g__GetItemIconTintValue|25_1(ref CS$<>8__locals1), true);
				goto IL_0384;
			}
		}
		else if (bindingName == "itemtypeicontint")
		{
			valueTuple = new ValueTuple<string, bool>(XUiBindingHelper.GetItemTypeIconTintValue(this.xui, this.result, this.iconTypeIconTintFormatter), true);
			goto IL_0384;
		}
		if (bindingName != null)
		{
			if (bindingName.StartsWith("itemstackname"))
			{
				valueTuple = new ValueTuple<string, bool>(this.<GetBindingValueInternal>g__GetItemStackNameValue|25_8((int)char.GetNumericValue(bindingName[bindingName.Length - 1]), ref CS$<>8__locals1), true);
				goto IL_0384;
			}
			if (bindingName.StartsWith("itemstackcolor"))
			{
				valueTuple = new ValueTuple<string, bool>(this.<GetBindingValueInternal>g__GetItemStackColorValue|25_9((int)char.GetNumericValue(bindingName[bindingName.Length - 1]), ref CS$<>8__locals1), true);
				goto IL_0384;
			}
			if (bindingName.StartsWith("itemstattitle"))
			{
				valueTuple = new ValueTuple<string, bool>(this.<GetBindingValueInternal>g__GetItemStatTitleValue|25_10((int)char.GetNumericValue(bindingName[bindingName.Length - 1]), ref CS$<>8__locals1), true);
				goto IL_0384;
			}
			if (bindingName.StartsWith("itemstat"))
			{
				valueTuple = new ValueTuple<string, bool>(this.<GetBindingValueInternal>g__GetItemStatValue|25_11((int)char.GetNumericValue(bindingName[bindingName.Length - 1]), ref CS$<>8__locals1), true);
				goto IL_0384;
			}
		}
		valueTuple = new ValueTuple<string, bool>(value, base.GetBindingValueInternal(ref value, bindingName));
		IL_0384:
		ValueTuple<string, bool> valueTuple2 = valueTuple;
		value = valueTuple2.Item1;
		return valueTuple2.Item2;
	}

	// Token: 0x06007859 RID: 30809 RVA: 0x002D99D5 File Offset: 0x002D7BD5
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public string <GetBindingValueInternal>g__GetItemIconValue|25_0(ref XUiC_CombineGrid.<>c__DisplayClass25_0 A_1)
	{
		if (A_1.resultItemClass == null)
		{
			return string.Empty;
		}
		ItemValue itemValue = this.result.itemValue;
		if (itemValue == null)
		{
			return null;
		}
		return itemValue.GetPropertyOverride("CustomIcon", A_1.resultItemClass.GetIconName());
	}

	// Token: 0x0600785A RID: 30810 RVA: 0x002D9A0C File Offset: 0x002D7C0C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public string <GetBindingValueInternal>g__GetItemIconTintValue|25_1(ref XUiC_CombineGrid.<>c__DisplayClass25_0 A_1)
	{
		Color32 color = Color.white;
		if (A_1.resultItemClass != null)
		{
			color = A_1.resultItemClass.GetIconTint(this.result.itemValue);
		}
		return this.iconTintColorFormatter.Format(color);
	}

	// Token: 0x0600785B RID: 30811 RVA: 0x002D9A54 File Offset: 0x002D7C54
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public string <GetBindingValueInternal>g__GetHasDurabilityValue|25_2(ref XUiC_CombineGrid.<>c__DisplayClass25_0 A_1)
	{
		return (A_1.resultItemClass != null && A_1.resultItemClass.ShowQualityBar).ToString();
	}

	// Token: 0x0600785C RID: 30812 RVA: 0x002D9A7F File Offset: 0x002D7C7F
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public string <GetBindingValueInternal>g__GetRemovedDurabilityFillValue|25_3(ref XUiC_CombineGrid.<>c__DisplayClass25_0 A_1)
	{
		if (this.result.IsEmpty())
		{
			return "1";
		}
		return this.removedDurabilityFillFormatter.Format(this.result.itemValue.MaxDurabilityModifier);
	}

	// Token: 0x0600785D RID: 30813 RVA: 0x002D9AB0 File Offset: 0x002D7CB0
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public string <GetBindingValueInternal>g__GetDurabilityColorValue|25_4(ref XUiC_CombineGrid.<>c__DisplayClass25_0 A_1)
	{
		Color32 color = Color.white;
		if (!this.result.IsEmpty())
		{
			color = QualityInfo.GetTierColor((int)this.result.itemValue.Quality);
		}
		return this.durabilityColorFormatter.Format(color);
	}

	// Token: 0x0600785E RID: 30814 RVA: 0x002D9AFC File Offset: 0x002D7CFC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public string <GetBindingValueInternal>g__GetDurabilityFillValue|25_5(ref XUiC_CombineGrid.<>c__DisplayClass25_0 A_1)
	{
		if (this.result.itemValue == null)
		{
			return "0.0";
		}
		if (this.result.itemValue.MaxUseTimes == 0)
		{
			return "1";
		}
		return this.durabilityFillFormatter.Format(((float)this.result.itemValue.MaxUseTimes - this.result.itemValue.UseTimes) / (float)this.result.itemValue.MaxUseTimesUI);
	}

	// Token: 0x0600785F RID: 30815 RVA: 0x002D9B73 File Offset: 0x002D7D73
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public string <GetBindingValueInternal>g__GetDurabilityJustifyValue|25_6(ref XUiC_CombineGrid.<>c__DisplayClass25_0 A_1)
	{
		if (A_1.resultItemClass == null || A_1.resultItemClass.ShowQualityBar)
		{
			return "center";
		}
		return "right";
	}

	// Token: 0x06007860 RID: 30816 RVA: 0x002D9B98 File Offset: 0x002D7D98
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public string <GetBindingValueInternal>g__GetDurabilityTextValue|25_7(ref XUiC_CombineGrid.<>c__DisplayClass25_0 A_1)
	{
		if (A_1.resultItemClass == null)
		{
			return string.Empty;
		}
		if (A_1.resultItemClass.ShowQualityBar)
		{
			if (this.result.itemValue.Quality <= 0)
			{
				return "-";
			}
			return this.durabilityTextFormatter.Format((int)this.result.itemValue.Quality);
		}
		else
		{
			if (A_1.resultItemClass.MaxCount == 1)
			{
				return string.Empty;
			}
			return this.durabilityTextFormatter.Format(this.result.count);
		}
	}

	// Token: 0x06007861 RID: 30817 RVA: 0x002D9C20 File Offset: 0x002D7E20
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public string <GetBindingValueInternal>g__GetItemStackNameValue|25_8(int itemIndex, ref XUiC_CombineGrid.<>c__DisplayClass25_0 A_2)
	{
		XUiC_RequiredItemStack xuiC_RequiredItemStack = ((itemIndex == 1) ? this.merge1 : this.merge2);
		ItemClass itemClass = ((xuiC_RequiredItemStack != null) ? xuiC_RequiredItemStack.ItemStack.itemValue.ItemClass : null);
		if (itemClass == null)
		{
			return Localization.Get("xuiCombineGridSelectItem", false, null);
		}
		return itemClass.GetLocalizedItemName();
	}

	// Token: 0x06007862 RID: 30818 RVA: 0x002D9C6C File Offset: 0x002D7E6C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public string <GetBindingValueInternal>g__GetItemStackColorValue|25_9(int itemIndex, ref XUiC_CombineGrid.<>c__DisplayClass25_0 A_2)
	{
		XUiC_RequiredItemStack xuiC_RequiredItemStack = ((itemIndex == 1) ? this.merge1 : this.merge2);
		Color color = ((xuiC_RequiredItemStack == null || xuiC_RequiredItemStack.ItemStack.IsEmpty()) ? Color.black : Color.white);
		return this.itemStackColorFormatter.Format(color);
	}

	// Token: 0x06007863 RID: 30819 RVA: 0x002D9CBC File Offset: 0x002D7EBC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public string <GetBindingValueInternal>g__GetItemStatTitleValue|25_10(int index, ref XUiC_CombineGrid.<>c__DisplayClass25_0 A_2)
	{
		if (A_2.resultItemClass == null || this.itemDisplayEntry == null || this.itemDisplayEntry.DisplayStats.Count <= index)
		{
			return string.Empty;
		}
		DisplayInfoEntry displayInfoEntry = this.itemDisplayEntry.DisplayStats[index];
		string text = displayInfoEntry.TitleOverride ?? UIDisplayInfoManager.Current.GetLocalizedName(displayInfoEntry.StatType);
		if (this.result.itemValue.GetStatPercent(displayInfoEntry.StatType, true) != 1f)
		{
			text = "[FFAF0B]" + text;
		}
		return text;
	}

	// Token: 0x06007864 RID: 30820 RVA: 0x002D9D4C File Offset: 0x002D7F4C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public string <GetBindingValueInternal>g__GetItemStatValue|25_11(int index, ref XUiC_CombineGrid.<>c__DisplayClass25_0 A_2)
	{
		if (A_2.resultItemClass == null || this.itemDisplayEntry == null || this.itemDisplayEntry.DisplayStats.Count <= index)
		{
			return string.Empty;
		}
		DisplayInfoEntry displayInfoEntry = this.itemDisplayEntry.DisplayStats[index];
		return XUiM_ItemStack.GetStatItemValueTextWithCompareInfo(this.result.itemValue, ItemStack.Empty.itemValue, this.xui.playerUI.entityPlayer, displayInfoEntry, false, true);
	}

	// Token: 0x0400593A RID: 22842
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RequiredItemStack merge1;

	// Token: 0x0400593B RID: 22843
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RequiredItemStack merge2;

	// Token: 0x0400593C RID: 22844
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnCombine;

	// Token: 0x0400593D RID: 22845
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack result = ItemStack.Empty;

	// Token: 0x0400593E RID: 22846
	[PublicizedFrom(EAccessModifier.Private)]
	public float experienceFromLastResult;

	// Token: 0x0400593F RID: 22847
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureCombine te;

	// Token: 0x04005940 RID: 22848
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemDisplayEntry itemDisplayEntry;

	// Token: 0x04005941 RID: 22849
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor iconTintColorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005942 RID: 22850
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor durabilityColorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005943 RID: 22851
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemStackColorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005944 RID: 22852
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor iconTypeIconTintFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005945 RID: 22853
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat removedDurabilityFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04005946 RID: 22854
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat durabilityFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04005947 RID: 22855
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt durabilityTextFormatter = new CachedStringFormatterInt();
}
