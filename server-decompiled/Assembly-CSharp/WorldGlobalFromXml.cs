using System;
using System.Collections;
using System.Xml.Linq;

// Token: 0x02000D32 RID: 3378
public class WorldGlobalFromXml
{
	// Token: 0x060066CB RID: 26315 RVA: 0x0028ABD0 File Offset: 0x00288DD0
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

	// Token: 0x060066CC RID: 26316 RVA: 0x0028ABDF File Offset: 0x00288DDF
	public static void Reload(XmlFile xmlFile)
	{
		ThreadManager.RunCoroutineSync(WorldGlobalFromXml.Load(xmlFile));
	}
}
