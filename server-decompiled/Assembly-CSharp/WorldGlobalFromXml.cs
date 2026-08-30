using System;
using System.Collections;
using System.Xml.Linq;

// Token: 0x02000D44 RID: 3396
public class WorldGlobalFromXml
{
	// Token: 0x06006757 RID: 26455 RVA: 0x0028DAB4 File Offset: 0x0028BCB4
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

	// Token: 0x06006758 RID: 26456 RVA: 0x0028DAC3 File Offset: 0x0028BCC3
	public static void Reload(XmlFile xmlFile)
	{
		ThreadManager.RunCoroutineSync(WorldGlobalFromXml.Load(xmlFile));
	}
}
