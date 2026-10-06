using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001650 RID: 5712
	public class PositionXmlImplementation : XmlImplementation
	{
		// Token: 0x0600B308 RID: 45832 RVA: 0x0042BDE1 File Offset: 0x00429FE1
		public override XmlDocument CreateDocument()
		{
			return new PositionXmlDocument();
		}
	}
}
