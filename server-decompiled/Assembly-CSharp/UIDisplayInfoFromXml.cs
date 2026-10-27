using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x02000D58 RID: 3416
public class UIDisplayInfoFromXml
{
	// Token: 0x06006754 RID: 26452 RVA: 0x002919AC File Offset: 0x0028FBAC
	public static IEnumerator Load(XmlFile xmlFile)
	{
		XElement root = xmlFile.XmlDoc.Root;
		if (!root.HasElements)
		{
			throw new Exception("No element <ui_display_info> found!");
		}
		UIDisplayInfoFromXml.ParseNode(root);
		yield break;
	}

	// Token: 0x06006755 RID: 26453 RVA: 0x002919BC File Offset: 0x0028FBBC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseNode(XElement root)
	{
		foreach (XElement xelement in root.Elements())
		{
			if (xelement.Name == "item_display")
			{
				using (IEnumerator<XElement> enumerator2 = xelement.Elements().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						XElement xelement2 = enumerator2.Current;
						UIDisplayInfoFromXml.ParseItemDisplayInfo(xelement2);
					}
					continue;
				}
			}
			if (xelement.Name == "character_stat_display")
			{
				using (IEnumerator<XElement> enumerator2 = xelement.Elements().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						XElement xelement3 = enumerator2.Current;
						DisplayInfoEntry displayInfoEntry = UIDisplayInfoFromXml.ParseDisplayInfoEntry(xelement3);
						if (displayInfoEntry != null)
						{
							UIDisplayInfoManager.Current.AddCharacterDisplayInfo(displayInfoEntry);
						}
					}
					continue;
				}
			}
			if (xelement.Name == "crafting_category_display")
			{
				using (IEnumerator<XElement> enumerator2 = xelement.Elements().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						XElement xelement4 = enumerator2.Current;
						UIDisplayInfoFromXml.ParseCraftingCategoryList(xelement4);
					}
					continue;
				}
			}
			if (xelement.Name == "trader_category_display")
			{
				UIDisplayInfoFromXml.ParseTraderCategoryList(xelement);
			}
		}
	}

	// Token: 0x06006756 RID: 26454 RVA: 0x00291B58 File Offset: 0x0028FD58
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseItemDisplayInfo(XElement e)
	{
		if (!e.HasAttribute("display_type"))
		{
			throw new Exception("item_display_info must have an display_type attribute");
		}
		string attribute = e.GetAttribute("display_type");
		if (UIDisplayInfoManager.Current.ContainsItemDisplayStats(attribute))
		{
			throw new Exception("Duplicate item_display_info entry with tag " + attribute);
		}
		string text = attribute;
		if (e.HasAttribute("display_group"))
		{
			text = e.GetAttribute("display_group");
		}
		UIDisplayInfoManager.Current.AddItemDisplayStats(attribute, text);
		foreach (XElement xelement in e.Elements("display_entry"))
		{
			DisplayInfoEntry displayInfoEntry = UIDisplayInfoFromXml.ParseDisplayInfoEntry(xelement);
			if (displayInfoEntry != null)
			{
				UIDisplayInfoManager.Current.AddItemDisplayInfo(attribute, displayInfoEntry);
			}
		}
		if (!e.HasAttribute("extends"))
		{
			return;
		}
		string attribute2 = e.GetAttribute("extends");
		if (UIDisplayInfoManager.Current.ContainsItemDisplayStats(attribute2))
		{
			ItemDisplayEntry displayStatsForTag = UIDisplayInfoManager.Current.GetDisplayStatsForTag(attribute2);
			for (int i = 0; i < displayStatsForTag.DisplayStats.Count; i++)
			{
				UIDisplayInfoManager.Current.AddItemDisplayInfo(attribute, displayStatsForTag.DisplayStats[i]);
			}
			return;
		}
		throw new Exception(string.Format("Extends item_display_info {0} is not specified.'", attribute2));
	}

	// Token: 0x06006757 RID: 26455 RVA: 0x00291CC0 File Offset: 0x0028FEC0
	[PublicizedFrom(EAccessModifier.Private)]
	public static DisplayInfoEntry ParseDisplayInfoEntry(XElement node)
	{
		DisplayInfoEntry displayInfoEntry = new DisplayInfoEntry();
		if (node.HasAttribute("name"))
		{
			string attribute = node.GetAttribute("name");
			try
			{
				displayInfoEntry.StatType = EnumUtils.Parse<PassiveEffects>(attribute, true);
			}
			catch
			{
				displayInfoEntry.CustomName = attribute;
			}
		}
		if (node.HasAttribute("display_type"))
		{
			displayInfoEntry.DisplayType = EnumUtils.Parse<DisplayInfoEntry.DisplayTypes>(node.GetAttribute("display_type"), true);
		}
		if (node.HasAttribute("show_inverted"))
		{
			displayInfoEntry.ShowInverted = Convert.ToBoolean(node.GetAttribute("show_inverted"));
		}
		if (node.HasAttribute("title_key"))
		{
			displayInfoEntry.TitleOverride = Localization.Get(node.GetAttribute("title_key"), false, null);
		}
		if (node.HasAttribute("negative_preferred"))
		{
			displayInfoEntry.NegativePreferred = Convert.ToBoolean(node.GetAttribute("negative_preferred"));
		}
		if (node.HasAttribute("display_leading_plus"))
		{
			displayInfoEntry.DisplayLeadingPlus = Convert.ToBoolean(node.GetAttribute("display_leading_plus"));
		}
		if (node.HasAttribute("tags"))
		{
			displayInfoEntry.Tags = FastTags<TagGroup.Global>.Parse(node.GetAttribute("tags"));
		}
		return displayInfoEntry;
	}

	// Token: 0x06006758 RID: 26456 RVA: 0x00291E34 File Offset: 0x00290034
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseCraftingCategoryList(XElement e)
	{
		if (!e.HasAttribute("display_type"))
		{
			throw new Exception("crafting_category_list must have an display_type attribute");
		}
		string attribute = e.GetAttribute("display_type");
		if (UIDisplayInfoManager.Current.ContainsCraftingCategoryList(attribute))
		{
			throw new Exception("Duplicate crafting_category_list entry with tag " + attribute);
		}
		foreach (XElement xelement in e.Elements("crafting_category"))
		{
			CraftingCategoryDisplayEntry craftingCategoryDisplayEntry = UIDisplayInfoFromXml.ParseCraftingCategory(xelement);
			if (craftingCategoryDisplayEntry != null)
			{
				UIDisplayInfoManager.Current.AddCraftingCategoryDisplayItem(attribute, craftingCategoryDisplayEntry);
			}
		}
	}

	// Token: 0x06006759 RID: 26457 RVA: 0x00291EE4 File Offset: 0x002900E4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseTraderCategoryList(XElement e)
	{
		foreach (XElement xelement in e.Elements("trader_category"))
		{
			CraftingCategoryDisplayEntry craftingCategoryDisplayEntry = UIDisplayInfoFromXml.ParseCraftingCategory(xelement);
			if (craftingCategoryDisplayEntry != null)
			{
				UIDisplayInfoManager.Current.AddTraderCategoryDIsplayItem(craftingCategoryDisplayEntry);
			}
		}
	}

	// Token: 0x0600675A RID: 26458 RVA: 0x00291F48 File Offset: 0x00290148
	[PublicizedFrom(EAccessModifier.Private)]
	public static CraftingCategoryDisplayEntry ParseCraftingCategory(XElement node)
	{
		CraftingCategoryDisplayEntry craftingCategoryDisplayEntry = new CraftingCategoryDisplayEntry();
		if (node.HasAttribute("name"))
		{
			craftingCategoryDisplayEntry.Name = node.GetAttribute("name");
		}
		if (node.HasAttribute("icon"))
		{
			craftingCategoryDisplayEntry.Icon = node.GetAttribute("icon");
		}
		if (node.HasAttribute("display_name"))
		{
			craftingCategoryDisplayEntry.DisplayName = Localization.Get(node.GetAttribute("display_name"), false, null);
		}
		return craftingCategoryDisplayEntry;
	}
}
