using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200160B RID: 5643
	public class PositionXmlAttribute : XmlAttribute, IXmlLineInfo
	{
		// Token: 0x0600B14D RID: 45389 RVA: 0x00422AC2 File Offset: 0x00420CC2
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

		// Token: 0x0600B14E RID: 45390 RVA: 0x00422AF4 File Offset: 0x00420CF4
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014B2 RID: 5298
		// (get) Token: 0x0600B14F RID: 45391 RVA: 0x00422AFC File Offset: 0x00420CFC
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014B3 RID: 5299
		// (get) Token: 0x0600B150 RID: 45392 RVA: 0x00422B04 File Offset: 0x00420D04
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008522 RID: 34082
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008523 RID: 34083
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008524 RID: 34084
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
