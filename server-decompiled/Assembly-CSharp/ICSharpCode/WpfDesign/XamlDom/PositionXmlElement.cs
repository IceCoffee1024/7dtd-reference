using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001656 RID: 5718
	public class PositionXmlElement : XmlElement, IXmlLineInfo
	{
		// Token: 0x0600B330 RID: 45872 RVA: 0x0042DE98 File Offset: 0x0042C098
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlElement(string prefix, string localName, string namespaceURI, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(prefix, localName, namespaceURI, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B331 RID: 45873 RVA: 0x0042DECA File Offset: 0x0042C0CA
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x17001538 RID: 5432
		// (get) Token: 0x0600B332 RID: 45874 RVA: 0x0042DED2 File Offset: 0x0042C0D2
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x17001539 RID: 5433
		// (get) Token: 0x0600B333 RID: 45875 RVA: 0x0042DEDA File Offset: 0x0042C0DA
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x0400868F RID: 34447
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008690 RID: 34448
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008691 RID: 34449
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
