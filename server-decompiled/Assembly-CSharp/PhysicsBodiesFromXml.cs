using System;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

// Token: 0x02000D24 RID: 3364
public class PhysicsBodiesFromXml
{
	// Token: 0x06006688 RID: 26248 RVA: 0x00285945 File Offset: 0x00283B45
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

	// Token: 0x06006689 RID: 26249 RVA: 0x00285954 File Offset: 0x00283B54
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
