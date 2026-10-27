using System;
using System.Collections;
using System.Xml.Linq;

// Token: 0x02000D51 RID: 3409
public static class ToolbeltFromXml
{
	// Token: 0x06006712 RID: 26386 RVA: 0x0028DC40 File Offset: 0x0028BE40
	public static IEnumerator Load(XmlFile _xmlFile)
	{
		XElement root = _xmlFile.XmlDoc.Root;
		if (root == null)
		{
			yield break;
		}
		ToolbeltFromXml.ParseConfig(root, "editorconfig", ToolbeltConfig.EditorConfig);
		yield break;
	}

	// Token: 0x06006713 RID: 26387 RVA: 0x0028DC50 File Offset: 0x0028BE50
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseConfig(XElement root, string configId, ToolbeltConfig config)
	{
		XElement xelement = root.Element(configId);
		XElement xelement2 = ((xelement != null) ? xelement.Element("slots") : null);
		if (xelement2 == null)
		{
			return;
		}
		int num;
		if (xelement2.HasAttribute("slot_per_bar") && int.TryParse(xelement2.GetAttribute("slot_per_bar"), out num))
		{
			if (num < 1 && num > 10)
			{
				Log.Warning(string.Format("Invalid slot_per_bar amount in toolbelt.xml value must be between {0} and {1}.", 1, 10));
			}
			config.SlotsPerBar = Math.Clamp(num, 1, 10);
		}
		int num2;
		if (xelement2.HasAttribute("bar_per_page") && int.TryParse(xelement2.GetAttribute("bar_per_page"), out num2))
		{
			if (num2 < 1 && num2 > 2)
			{
				Log.Warning(string.Format("Invalid bar_per_page amount in toolbelt.xml value must be between {0} and {1}.", 1, 2));
			}
			config.BarsPerPage = Math.Clamp(num2, 1, 2);
		}
		int num3;
		if (xelement2.HasAttribute("page_count") && int.TryParse(xelement2.GetAttribute("page_count"), out num3))
		{
			if (num3 < 1 && num3 > 10)
			{
				Log.Warning(string.Format("Invalid page_count amount in toolbelt.xml value must be between {0} and {1}.", 1, 10));
			}
			config.PageCount = Math.Clamp(num3, 1, 10);
		}
	}

	// Token: 0x04004FDD RID: 20445
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MinSlotsPerBar = 1;

	// Token: 0x04004FDE RID: 20446
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MaxSlotsPerBar = 10;

	// Token: 0x04004FDF RID: 20447
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MinBarPerPage = 1;

	// Token: 0x04004FE0 RID: 20448
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MaxBarPerPage = 2;

	// Token: 0x04004FE1 RID: 20449
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MinPages = 1;

	// Token: 0x04004FE2 RID: 20450
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MaxPages = 10;
}
