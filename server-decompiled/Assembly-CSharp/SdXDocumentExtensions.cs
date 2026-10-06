using System;
using System.IO;
using System.Xml.Linq;

// Token: 0x02000A17 RID: 2583
public static class SdXDocumentExtensions
{
	// Token: 0x06004D38 RID: 19768 RVA: 0x001DC000 File Offset: 0x001DA200
	public static void SdSave(this XDocument xmlDoc, string filename)
	{
		using (Stream stream = SdFile.Open(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
		{
			xmlDoc.Save(stream);
		}
	}
}
