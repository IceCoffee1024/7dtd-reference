using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x02000D2D RID: 3373
public class VehiclesFromXml
{
	// Token: 0x060066B7 RID: 26295 RVA: 0x0028A6C9 File Offset: 0x002888C9
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

	// Token: 0x060066B8 RID: 26296 RVA: 0x0028A6D8 File Offset: 0x002888D8
	public static void Reload(XmlFile xmlFile)
	{
		ThreadManager.RunCoroutineSync(VehiclesFromXml.Load(xmlFile));
	}
}
