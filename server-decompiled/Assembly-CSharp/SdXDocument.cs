using System;
using System.IO;
using System.Xml.Linq;

// Token: 0x02000A0C RID: 2572
public static class SdXDocument
{
	// Token: 0x06004D68 RID: 19816 RVA: 0x001DAD18 File Offset: 0x001D8F18
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
