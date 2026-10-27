using System;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

// Token: 0x02000D3D RID: 3389
public class PhysicsBodiesFromXml
{
	// Token: 0x0600669F RID: 26271 RVA: 0x0028A211 File Offset: 0x00288411
	public static IEnumerator Load(XmlFile xmlFile)
	{
		try
		{
			XElement root = xmlFile.XmlDoc.Root;
			if (!root.HasElements)
			{
				yield break;
			}
			foreach (XElement xelement in root.Elements("body"))
			{
				PhysicsBodyLayout.Read(xelement);
			}
			yield break;
		}
		catch (Exception)
		{
			PhysicsBodyLayout.Reset();
			throw;
		}
		yield break;
	}

	// Token: 0x060066A0 RID: 26272 RVA: 0x0028A220 File Offset: 0x00288420
	public static void Save(string path)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.CreateXmlDeclaration();
		XmlElement xmlElement = xmlDocument.AddXmlElement("bodies");
		PhysicsBodyLayout[] bodyLayouts = PhysicsBodyLayout.BodyLayouts;
		for (int i = 0; i < bodyLayouts.Length; i++)
		{
			bodyLayouts[i].Write(xmlElement);
		}
		xmlDocument.SdSave(path);
	}
}
