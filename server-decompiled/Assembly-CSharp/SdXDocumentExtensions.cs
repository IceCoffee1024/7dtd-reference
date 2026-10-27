using System;
using System.IO;
using System.Xml.Linq;

// Token: 0x02000A21 RID: 2593
public static class SdXDocumentExtensions
{
	// Token: 0x06004D5C RID: 19804 RVA: 0x001DD0E8 File Offset: 0x001DB2E8
	public static void SdSave(this XDocument xmlDoc, string filename)
	{
		using (Stream stream = SdFile.Open(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
		{
			xmlDoc.Save(stream);
		}
	}
}
