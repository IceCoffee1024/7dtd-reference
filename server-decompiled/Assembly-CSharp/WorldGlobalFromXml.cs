using System;
using System.Collections;
using System.Xml.Linq;

// Token: 0x02000D54 RID: 3412
public class WorldGlobalFromXml
{
	// Token: 0x06006751 RID: 26449 RVA: 0x0029101C File Offset: 0x0028F21C
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

	// Token: 0x06006752 RID: 26450 RVA: 0x0029102B File Offset: 0x0028F22B
	public static void Reload(XmlFile xmlFile)
	{
		ThreadManager.RunCoroutineSync(WorldGlobalFromXml.Load(xmlFile));
	}
}
