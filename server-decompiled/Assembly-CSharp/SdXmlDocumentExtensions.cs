using System;
using System.IO;
using System.Xml;

// Token: 0x02000A18 RID: 2584
public static class SdXmlDocumentExtensions
{
	// Token: 0x06004D39 RID: 19769 RVA: 0x001DC03C File Offset: 0x001DA23C
	public static void SdLoad(this XmlDocument xmlDoc, string filename)
	{
		using (Stream stream = SdFile.OpenRead(filename))
		{
			xmlDoc.Load(stream);
		}
	}

	// Token: 0x06004D3A RID: 19770 RVA: 0x001DC074 File Offset: 0x001DA274
	public static void SdSave(this XmlDocument xmlDoc, string filename)
	{
		using (Stream stream = SdFile.Open(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
		{
			xmlDoc.Save(stream);
		}
	}
}
