using System;
using System.IO;
using System.Xml;

// Token: 0x020009FD RID: 2557
public static class SdXmlDocumentExtensions
{
	// Token: 0x06004CE4 RID: 19684 RVA: 0x001D82F4 File Offset: 0x001D64F4
	public static void SdLoad(this XmlDocument xmlDoc, string filename)
	{
		using (Stream stream = SdFile.OpenRead(filename))
		{
			xmlDoc.Load(stream);
		}
	}

	// Token: 0x06004CE5 RID: 19685 RVA: 0x001D832C File Offset: 0x001D652C
	public static void SdSave(this XmlDocument xmlDoc, string filename)
	{
		using (Stream stream = SdFile.Open(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
		{
			xmlDoc.Save(stream);
		}
	}
}
