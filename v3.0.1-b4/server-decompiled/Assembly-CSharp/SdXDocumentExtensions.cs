using System;
using System.IO;
using System.Xml.Linq;

// Token: 0x020009FC RID: 2556
public static class SdXDocumentExtensions
{
	// Token: 0x06004CE3 RID: 19683 RVA: 0x001D82B8 File Offset: 0x001D64B8
	public static void SdSave(this XDocument xmlDoc, string filename)
	{
		using (Stream stream = SdFile.Open(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
		{
			xmlDoc.Save(stream);
		}
	}
}
