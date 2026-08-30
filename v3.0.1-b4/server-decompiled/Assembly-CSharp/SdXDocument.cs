using System;
using System.IO;
using System.Xml.Linq;

// Token: 0x020009FB RID: 2555
public static class SdXDocument
{
	// Token: 0x06004CE2 RID: 19682 RVA: 0x001D8280 File Offset: 0x001D6480
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
