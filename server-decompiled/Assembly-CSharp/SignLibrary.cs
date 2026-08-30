using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

// Token: 0x02000AB4 RID: 2740
public class SignLibrary
{
	// Token: 0x0600523B RID: 21051 RVA: 0x001F5B0C File Offset: 0x001F3D0C
	public void WriteXml(string filePath)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.CreateXmlDeclaration();
		XmlElement xmlElement = xmlDocument.AddXmlElement("signs");
		xmlElement.SetAttribute("version", 2.ToString());
		foreach (KeyValuePair<Guid, SignData> keyValuePair in this.signs)
		{
			keyValuePair.Value.WriteXml(xmlElement);
		}
		xmlDocument.SdSave(filePath);
	}

	// Token: 0x0600523C RID: 21052 RVA: 0x001F5B9C File Offset: 0x001F3D9C
	public void ReadXml(XmlFile _xmlFile)
	{
		this.signs.Clear();
		XElement root = _xmlFile.XmlDoc.Root;
		int num = this.ReadVersion(root);
		foreach (XElement xelement in this.MigrateToCurrentVersion(root, num, _xmlFile).Elements(XNames.sign))
		{
			SignData signData = SignData.ReadXML(xelement);
			if (this.signs.ContainsKey(signData.guid))
			{
				Log.Error(string.Format("Duplicate sign guid '{0}' found in sign library: '{1}/{2}'", signData.guid, _xmlFile.Directory, _xmlFile.Filename));
			}
			else
			{
				this.signs[signData.guid] = signData;
			}
		}
		if (num < 2)
		{
			Log.Error("Out-of-date sign library loaded in build: " + _xmlFile.Directory + "/" + _xmlFile.Filename);
		}
	}

	// Token: 0x0600523D RID: 21053 RVA: 0x001F5C84 File Offset: 0x001F3E84
	[PublicizedFrom(EAccessModifier.Private)]
	public int ReadVersion(XElement root)
	{
		XAttribute xattribute = root.Attribute("version");
		if (xattribute == null)
		{
			Log.Warning("Sign library has no version attribute, assuming legacy format (v0)");
			return 0;
		}
		int num;
		if (!int.TryParse(xattribute.Value, out num))
		{
			Log.Error("Invalid version attribute value: '" + xattribute.Value + "', assuming legacy format (v0)");
			return 0;
		}
		return num;
	}

	// Token: 0x0600523E RID: 21054 RVA: 0x001F5CE0 File Offset: 0x001F3EE0
	[PublicizedFrom(EAccessModifier.Private)]
	public XElement MigrateToCurrentVersion(XElement root, int fromVersion, XmlFile xmlFile)
	{
		if (fromVersion == 2)
		{
			return root;
		}
		if (fromVersion > 2)
		{
			Log.Error(string.Concat(new string[]
			{
				string.Format("Sign library version {0} is newer than supported version {1}. ", fromVersion, 2),
				"File: '",
				xmlFile.Directory,
				"/",
				xmlFile.Filename,
				"'. Data loss may occur!"
			}));
			return root;
		}
		Log.Out(string.Format("Migrating sign library from v{0} to v{1}: '{2}/{3}'", new object[] { fromVersion, 2, xmlFile.Directory, xmlFile.Filename }));
		XElement xelement = root;
		for (int i = fromVersion; i < 2; i++)
		{
			xelement = SignLibraryMigrations.Migrate(xelement, i, i + 1);
		}
		return xelement;
	}

	// Token: 0x04003FC9 RID: 16329
	public const int CurrentVersion = 2;

	// Token: 0x04003FCA RID: 16330
	[PublicizedFrom(EAccessModifier.Private)]
	public const string VersionAttributeName = "version";

	// Token: 0x04003FCB RID: 16331
	public readonly Dictionary<Guid, SignData> signs = new Dictionary<Guid, SignData>();
}
