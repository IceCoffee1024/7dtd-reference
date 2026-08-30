using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000D26 RID: 3366
public class QualityInfoFromXml : MonoBehaviour
{
	// Token: 0x06006691 RID: 26257 RVA: 0x00285A54 File Offset: 0x00283C54
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
