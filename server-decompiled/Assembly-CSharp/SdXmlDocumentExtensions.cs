using System;
using System.IO;
using System.Xml;

// Token: 0x02000A22 RID: 2594
public static class SdXmlDocumentExtensions
{
	// Token: 0x06004D5D RID: 19805 RVA: 0x001DD124 File Offset: 0x001DB324
	public static void SdLoad(this XmlDocument xmlDoc, string filename)
	{
		using (Stream stream = SdFile.OpenRead(filename))
		{
			xmlDoc.Load(stream);
		}
	}

	// Token: 0x06004D5E RID: 19806 RVA: 0x001DD15C File Offset: 0x001DB35C
	public static void SdSave(this XmlDocument xmlDoc, string filename)
	{
		using (Stream stream = SdFile.Open(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
		{
			xmlDoc.Save(stream);
		}
	}
}
