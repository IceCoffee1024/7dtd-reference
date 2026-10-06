using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001191 RID: 4497
[Preserve]
public class XUiBindingHelper
{
	// Token: 0x06008FFB RID: 36859 RVA: 0x00361B64 File Offset: 0x0035FD64
	public static string GetItemTypeIconValue(XUi xui, ItemStack itemStack)
	{
		ItemValue itemValue = itemStack.itemValue;
		ItemClass itemClass = ((itemValue != null) ? itemValue.ItemClass : null);
		if (itemStack.IsEmpty() || itemClass == null)
		{
			return string.Empty;
		}
		if (itemClass.IsBlock() && itemStack.itemValue.TextureFullArray.IsDefault)
		{
			return Block.list[itemStack.itemValue.type].ItemTypeIcon;
		}
		if (itemStack.itemValue.HasMods())
		{
			return "modded";
		}
		if (itemStack.itemValue.WasCombined)
		{
			return "add";
		}
		if (itemStack.itemValue.HasAnyBoostedStats())
		{
			return "star";
		}
		if (itemClass.AltItemTypeIcon != null && !string.IsNullOrEmpty(itemClass.Unlocks) && XUiM_ItemStack.CheckKnown(xui.playerUI.entityPlayer, itemClass, itemStack.itemValue))
		{
			return itemClass.AltItemTypeIcon;
		}
		return itemClass.ItemTypeIcon;
	}

	// Token: 0x06008FFC RID: 36860 RVA: 0x00361C40 File Offset: 0x0035FE40
	public static string GetHasItemTypeIconValue(ItemStack itemStack)
	{
		return XUiBindingHelper.GetHasItemTypeIconValueInternal(itemStack).ToString();
	}

	// Token: 0x06008FFD RID: 36861 RVA: 0x00361C5C File Offset: 0x0035FE5C
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool GetHasItemTypeIconValueInternal(ItemStack itemStack)
	{
		ItemValue itemValue = itemStack.itemValue;
		ItemClass itemClass = ((itemValue != null) ? itemValue.ItemClass : null);
		if (itemStack.IsEmpty() || itemClass == null)
		{
			return false;
		}
		ItemValue itemValue2 = itemStack.itemValue;
		if (itemClass.IsBlock())
		{
			return !string.IsNullOrEmpty(Block.list[itemValue2.type].ItemTypeIcon);
		}
		return itemValue2.HasAnyBoostedStats() || itemValue2.HasMods() || itemValue2.WasCombined || !string.IsNullOrEmpty(itemClass.ItemTypeIcon);
	}

	// Token: 0x06008FFE RID: 36862 RVA: 0x00361CDA File Offset: 0x0035FEDA
	public static string GetItemTypeIconTintValue(XUi xui, ItemStack itemStack, CachedStringFormatterXuiRgbaColor colorFormatter)
	{
		return colorFormatter.Format(XUiBindingHelper.GetItemTypeIconTintColor(xui, itemStack));
	}

	// Token: 0x06008FFF RID: 36863 RVA: 0x00361CEC File Offset: 0x0035FEEC
	public static Color32 GetItemTypeIconTintColor(XUi xui, ItemStack itemStack)
	{
		Color32 color = Color.white;
		ItemClass itemClass = itemStack.itemValue.ItemClass;
		if (!itemStack.IsEmpty() && itemClass != null && !string.IsNullOrEmpty(itemClass.Unlocks) && XUiM_ItemStack.CheckKnown(xui.playerUI.entityPlayer, itemClass, itemStack.itemValue))
		{
			color = itemClass.AltItemTypeIconColor;
		}
		else if (itemStack.itemValue.HasAnyBoostedStats())
		{
			color = XUiBindingHelper.StatBoostedColor;
		}
		else if (itemStack.itemValue.WasCombined)
		{
			color = XUiBindingHelper.CombinedIconColor;
		}
		else if (itemStack.itemValue.HasMods())
		{
			color = Color.white;
		}
		return color;
	}

	// Token: 0x04006968 RID: 26984
	public const string BoostedItemTypeIcon = "star";

	// Token: 0x04006969 RID: 26985
	public const string ModdedItemTypeIcon = "modded";

	// Token: 0x0400696A RID: 26986
	public const string CombinedItemTypeIcon = "add";

	// Token: 0x0400696B RID: 26987
	public static readonly Color32 StatBoostedColor = new Color32(byte.MaxValue, 175, 11, byte.MaxValue);

	// Token: 0x0400696C RID: 26988
	public static readonly Color32 CombinedIconColor = new Color(0f, 194f, 153f, 255f);
}
