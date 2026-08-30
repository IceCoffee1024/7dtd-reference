using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000D14 RID: 3348
public class QualityInfoFromXml : MonoBehaviour
{
	// Token: 0x06006605 RID: 26117 RVA: 0x00282B44 File Offset: 0x00280D44
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
