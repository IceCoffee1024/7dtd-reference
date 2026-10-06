using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001645 RID: 5701
	public class PositionXmlAttribute : XmlAttribute, IXmlLineInfo
	{
		// Token: 0x0600B2DC RID: 45788 RVA: 0x0042BADE File Offset: 0x00429CDE
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlAttribute(string prefix, string localName, string namespaceURI, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(prefix, localName, namespaceURI, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B2DD RID: 45789 RVA: 0x0042BB10 File Offset: 0x00429D10
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x1700152E RID: 5422
		// (get) Token: 0x0600B2DE RID: 45790 RVA: 0x0042BB18 File Offset: 0x00429D18
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x1700152F RID: 5423
		// (get) Token: 0x0600B2DF RID: 45791 RVA: 0x0042BB20 File Offset: 0x00429D20
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008651 RID: 34385
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008652 RID: 34386
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008653 RID: 34387
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
