using System;
using System.IO;
using System.Xml;

// Token: 0x02000A0E RID: 2574
public static class SdXmlDocumentExtensions
{
	// Token: 0x06004D6A RID: 19818 RVA: 0x001DAD8C File Offset: 0x001D8F8C
	public static void SdLoad(this XmlDocument xmlDoc, string filename)
	{
		using (Stream stream = SdFile.OpenRead(filename))
		{
			xmlDoc.Load(stream);
		}
	}

	// Token: 0x06004D6B RID: 19819 RVA: 0x001DADC4 File Offset: 0x001D8FC4
	public static void SdSave(this XmlDocument xmlDoc, string filename)
	{
		using (Stream stream = SdFile.Open(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
		{
			xmlDoc.Save(stream);
		}
	}
}
