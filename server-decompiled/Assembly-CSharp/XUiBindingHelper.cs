using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001179 RID: 4473
[Preserve]
public class XUiBindingHelper
{
	// Token: 0x06008F70 RID: 36720 RVA: 0x0035D3A8 File Offset: 0x0035B5A8
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

	// Token: 0x06008F71 RID: 36721 RVA: 0x0035D480 File Offset: 0x0035B680
	public static string GetHasItemTypeIconValue(ItemStack itemStack)
	{
		return XUiBindingHelper.GetHasItemTypeIconValueInternal(itemStack).ToString();
	}

	// Token: 0x06008F72 RID: 36722 RVA: 0x0035D49C File Offset: 0x0035B69C
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

	// Token: 0x06008F73 RID: 36723 RVA: 0x0035D51C File Offset: 0x0035B71C
	public static string GetItemTypeIconTintValue(XUi xui, ItemStack itemStack, CachedStringFormatterXuiRgbaColor colorFormatter)
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
		return colorFormatter.Format(color);
	}

	// Token: 0x04006902 RID: 26882
	public const string BoostedItemTypeIcon = "star";

	// Token: 0x04006903 RID: 26883
	public const string ModdedItemTypeIcon = "modded";

	// Token: 0x04006904 RID: 26884
	public const string CombinedItemTypeIcon = "add";

	// Token: 0x04006905 RID: 26885
	public static readonly Color32 StatBoostedColor = new Color32(byte.MaxValue, 175, 11, byte.MaxValue);

	// Token: 0x04006906 RID: 26886
	public static readonly Color32 CombinedIconColor = new Color(0f, 194f, 153f, 255f);
}
