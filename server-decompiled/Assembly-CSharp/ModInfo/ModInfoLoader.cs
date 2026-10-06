using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using ICSharpCode.WpfDesign.XamlDom;
using XMLData;

namespace ModInfo
{
	// Token: 0x02001642 RID: 5698
	[XmlParser]
	public static class ModInfoLoader
	{
		// Token: 0x0600B2C2 RID: 45762 RVA: 0x0042B810 File Offset: 0x00429A10
		public static List<ModInfo> ParseXml(string _filename, XmlFile _xml)
		{
			List<ModInfo> list = new List<ModInfo>();
			Dictionary<PositionXmlElement, DataItem<ModInfo>> dictionary = new Dictionary<PositionXmlElement, DataItem<ModInfo>>();
			XElement root = _xml.XmlDoc.Root;
			if (root == null || !root.HasElements)
			{
				Log.Error("No document root or no children found!");
				return list;
			}
			foreach (XElement xelement in root.Elements())
			{
				if (xelement.Name.LocalName == "ModInfo")
				{
					ModInfo modInfo = ModInfo.Parser.Parse(xelement, dictionary);
					if (modInfo != null)
					{
						list.Add(modInfo);
					}
				}
				else
				{
					Log.Warning(string.Format("Unknown element found: {0} (file {1}, line {2})", xelement.Name, _filename, ((IXmlLineInfo)xelement).LineNumber));
				}
			}
			return list;
		}
	}
}
