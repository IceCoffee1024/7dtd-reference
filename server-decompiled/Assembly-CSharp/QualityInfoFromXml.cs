using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000D3F RID: 3391
public class QualityInfoFromXml : MonoBehaviour
{
	// Token: 0x060066A8 RID: 26280 RVA: 0x0028A320 File Offset: 0x00288520
	public static IEnumerator CreateQualityInfo(XmlFile _xmlFile)
	{
		XElement root = _xmlFile.XmlDoc.Root;
		if (!root.HasElements)
		{
			throw new Exception("No element <quality> found!");
		}
		using (IEnumerator<XElement> enumerator = root.Elements("quality").GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				XElement xelement = enumerator.Current;
				int num = -1;
				if (xelement.HasAttribute("key"))
				{
					num = int.Parse(xelement.GetAttribute("key"));
				}
				string text = "#FFFFFF";
				if (xelement.HasAttribute("color"))
				{
					text = xelement.GetAttribute("color");
				}
				if (num > -1)
				{
					QualityInfo.Add(num, text);
				}
			}
			yield break;
		}
		yield break;
	}
}
