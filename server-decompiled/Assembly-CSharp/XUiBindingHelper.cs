using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020011A1 RID: 4513
[Preserve]
public class XUiBindingHelper
{
	// Token: 0x0600904F RID: 36943 RVA: 0x00363C54 File Offset: 0x00361E54
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

	// Token: 0x06009050 RID: 36944 RVA: 0x00363D30 File Offset: 0x00361F30
	public static string GetHasItemTypeIconValue(ItemStack itemStack)
	{
		return XUiBindingHelper.GetHasItemTypeIconValueInternal(itemStack).ToString();
	}

	// Token: 0x06009051 RID: 36945 RVA: 0x00363D4C File Offset: 0x00361F4C
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

	// Token: 0x06009052 RID: 36946 RVA: 0x00363DCA File Offset: 0x00361FCA
	public static string GetItemTypeIconTintValue(XUi xui, ItemStack itemStack, CachedStringFormatterXuiRgbaColor colorFormatter)
	{
		return colorFormatter.Format(XUiBindingHelper.GetItemTypeIconTintColor(xui, itemStack));
	}

	// Token: 0x06009053 RID: 36947 RVA: 0x00363DDC File Offset: 0x00361FDC
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

	// Token: 0x040069A8 RID: 27048
	public const string BoostedItemTypeIcon = "star";

	// Token: 0x040069A9 RID: 27049
	public const string ModdedItemTypeIcon = "modded";

	// Token: 0x040069AA RID: 27050
	public const string CombinedItemTypeIcon = "add";

	// Token: 0x040069AB RID: 27051
	public static readonly Color32 StatBoostedColor = new Color32(byte.MaxValue, 175, 11, byte.MaxValue);

	// Token: 0x040069AC RID: 27052
	public static readonly Color32 CombinedIconColor = new Color(0f, 194f, 153f, 255f);
}
