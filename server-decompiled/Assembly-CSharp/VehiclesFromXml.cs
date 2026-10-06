using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x02000D4F RID: 3407
public class VehiclesFromXml
{
	// Token: 0x0600673D RID: 26429 RVA: 0x00290B15 File Offset: 0x0028ED15
	public static IEnumerator Load(XmlFile _xmlFile)
	{
		XElement root = _xmlFile.XmlDoc.Root;
		if (!root.HasElements)
		{
			throw new Exception("No element <vehicles> found!");
		}
		Vehicle.PropertyMap = new Dictionary<string, DynamicProperties>();
		using (IEnumerator<XElement> enumerator = root.Elements("vehicle").GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				XElement xelement = enumerator.Current;
				DynamicProperties dynamicProperties = new DynamicProperties();
				string text = "";
				if (xelement.HasAttribute("name"))
				{
					text = xelement.GetAttribute("name");
				}
				foreach (XElement xelement2 in xelement.Elements("property"))
				{
					dynamicProperties.Add(xelement2, false);
				}
				Vehicle.PropertyMap.Add(text.ToLower(), dynamicProperties);
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x0600673E RID: 26430 RVA: 0x00290B24 File Offset: 0x0028ED24
	public static void Reload(XmlFile xmlFile)
	{
		ThreadManager.RunCoroutineSync(VehiclesFromXml.Load(xmlFile));
	}
}
