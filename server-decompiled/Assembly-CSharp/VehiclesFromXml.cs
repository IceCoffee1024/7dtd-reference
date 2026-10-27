using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x02000D5A RID: 3418
public class VehiclesFromXml
{
	// Token: 0x06006762 RID: 26466 RVA: 0x0029203D File Offset: 0x0029023D
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

	// Token: 0x06006763 RID: 26467 RVA: 0x0029204C File Offset: 0x0029024C
	public static void Reload(XmlFile xmlFile)
	{
		ThreadManager.RunCoroutineSync(VehiclesFromXml.Load(xmlFile));
	}
}
