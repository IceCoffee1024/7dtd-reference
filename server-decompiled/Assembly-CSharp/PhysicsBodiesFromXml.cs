using System;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

// Token: 0x02000D12 RID: 3346
public class PhysicsBodiesFromXml
{
	// Token: 0x060065FC RID: 26108 RVA: 0x00282A35 File Offset: 0x00280C35
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

	// Token: 0x060065FD RID: 26109 RVA: 0x00282A44 File Offset: 0x00280C44
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
