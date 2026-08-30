using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001616 RID: 5654
	public class PositionXmlImplementation : XmlImplementation
	{
		// Token: 0x0600B179 RID: 45433 RVA: 0x00422DC5 File Offset: 0x00420FC5
		public override XmlDocument CreateDocument()
		{
			return new PositionXmlDocument();
		}
	}
}
