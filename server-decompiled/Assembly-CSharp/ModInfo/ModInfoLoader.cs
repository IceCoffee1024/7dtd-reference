using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using ICSharpCode.WpfDesign.XamlDom;
using XMLData;

namespace ModInfo
{
	// Token: 0x02001608 RID: 5640
	[XmlParser]
	public static class ModInfoLoader
	{
		// Token: 0x0600B133 RID: 45363 RVA: 0x004227F4 File Offset: 0x004209F4
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
