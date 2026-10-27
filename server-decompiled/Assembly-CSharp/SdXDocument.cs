using System;
using System.IO;
using System.Xml.Linq;

// Token: 0x02000A20 RID: 2592
public static class SdXDocument
{
	// Token: 0x06004D5B RID: 19803 RVA: 0x001DD0B0 File Offset: 0x001DB2B0
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
