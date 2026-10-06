using System;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

// Token: 0x02000D32 RID: 3378
public class PhysicsBodiesFromXml
{
	// Token: 0x0600667A RID: 26234 RVA: 0x00288CE9 File Offset: 0x00286EE9
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

	// Token: 0x0600667B RID: 26235 RVA: 0x00288CF8 File Offset: 0x00286EF8
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
