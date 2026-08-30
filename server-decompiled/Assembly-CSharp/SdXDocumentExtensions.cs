using System;
using System.IO;
using System.Xml.Linq;

// Token: 0x02000A0D RID: 2573
public static class SdXDocumentExtensions
{
	// Token: 0x06004D69 RID: 19817 RVA: 0x001DAD50 File Offset: 0x001D8F50
	public static void SdSave(this XDocument xmlDoc, string filename)
	{
		using (Stream stream = SdFile.Open(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
		{
			xmlDoc.Save(stream);
		}
	}
}
