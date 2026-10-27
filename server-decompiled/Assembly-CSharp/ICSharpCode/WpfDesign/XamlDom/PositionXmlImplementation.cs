using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001662 RID: 5730
	public class PositionXmlImplementation : XmlImplementation
	{
		// Token: 0x0600B360 RID: 45920 RVA: 0x0042E1E5 File Offset: 0x0042C3E5
		public override XmlDocument CreateDocument()
		{
			return new PositionXmlDocument();
		}
	}
}
