using System;
using System.Collections;
using System.Xml.Linq;

// Token: 0x02000D5F RID: 3423
public class WorldGlobalFromXml
{
	// Token: 0x06006776 RID: 26486 RVA: 0x00292544 File Offset: 0x00290744
	public static IEnumerator Load(XmlFile _xmlFile)
	{
		XElement root = _xmlFile.XmlDoc.Root;
		if (!root.HasElements)
		{
			throw new Exception("No element <world> found!");
		}
		foreach (XContainer xcontainer in root.Elements("environment"))
		{
			DynamicProperties dynamicProperties = new DynamicProperties();
			foreach (XElement xelement in xcontainer.Elements("property"))
			{
				dynamicProperties.Add(xelement, false);
			}
			WorldEnvironment.Properties = dynamicProperties;
		}
		WorldEnvironment.OnXMLChanged();
		yield break;
	}

	// Token: 0x06006777 RID: 26487 RVA: 0x00292553 File Offset: 0x00290753
	public static void Reload(XmlFile xmlFile)
	{
		ThreadManager.RunCoroutineSync(WorldGlobalFromXml.Load(xmlFile));
	}
}
