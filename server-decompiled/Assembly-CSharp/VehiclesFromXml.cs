using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x02000D3F RID: 3391
public class VehiclesFromXml
{
	// Token: 0x06006743 RID: 26435 RVA: 0x0028D5AD File Offset: 0x0028B7AD
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

	// Token: 0x06006744 RID: 26436 RVA: 0x0028D5BC File Offset: 0x0028B7BC
	public static void Reload(XmlFile xmlFile)
	{
		ThreadManager.RunCoroutineSync(VehiclesFromXml.Load(xmlFile));
	}
}
