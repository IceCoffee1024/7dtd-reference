using System;
using System.Collections;
using System.Xml.Linq;
using UnityEngine;
using XMLData;

// Token: 0x02000D0E RID: 3342
public class ColorMappingsFromXml : MonoBehaviour
{
	// Token: 0x060065AD RID: 26029 RVA: 0x0028003E File Offset: 0x0027E23E
	public static IEnumerator Load(XmlFile _xmlFile)
	{
		XElement root = _xmlFile.XmlDoc.Root;
		if (!root.HasElements)
		{
			throw new Exception("No element <root> found!");
		}
		ColorMappingData.Instance.ColorFromID.Clear();
		ColorMappingData.Instance.IDFromName.Clear();
		ColorMappingData.Instance.NameFromID.Clear();
		foreach (XElement xelement in root.Elements("color"))
		{
			int num = int.Parse(xelement.GetAttribute("id"));
			string attribute = xelement.GetAttribute("name");
			Color color;
			if (ColorUtility.TryParseHtmlString(xelement.GetAttribute("value"), out color))
			{
				ColorMappingData.Instance.ColorFromID.Add(num, color);
				ColorMappingData.Instance.IDFromName.Add(attribute, num);
				ColorMappingData.Instance.NameFromID.Add(num, attribute);
			}
			else
			{
				Log.Warning(string.Format("No color value for {0} and {1}", num, attribute));
			}
		}
		yield return null;
		yield break;
	}
}
