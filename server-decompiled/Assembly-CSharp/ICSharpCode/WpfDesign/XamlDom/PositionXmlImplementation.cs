using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001631 RID: 5681
	public class PositionXmlImplementation : XmlImplementation
	{
		// Token: 0x0600B276 RID: 45686 RVA: 0x00428105 File Offset: 0x00426305
		public override XmlDocument CreateDocument()
		{
			return new PositionXmlDocument();
		}
	}
}
