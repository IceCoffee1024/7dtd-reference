using System;
using System.IO;
using System.Xml.Linq;

// Token: 0x02000A16 RID: 2582
public static class SdXDocument
{
	// Token: 0x06004D37 RID: 19767 RVA: 0x001DBFC8 File Offset: 0x001DA1C8
	public static XDocument Load(string filename)
	{
		XDocument xdocument;
		using (Stream stream = SdFile.OpenRead(filename))
		{
			xdocument = XDocument.Load(stream);
		}
		return xdocument;
	}
}
