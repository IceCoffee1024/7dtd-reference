using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001644 RID: 5700
	public class PositionXmlElement : XmlElement, IXmlLineInfo
	{
		// Token: 0x0600B2D8 RID: 45784 RVA: 0x0042BA94 File Offset: 0x00429C94
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

		// Token: 0x0600B2D9 RID: 45785 RVA: 0x0042BAC6 File Offset: 0x00429CC6
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x1700152C RID: 5420
		// (get) Token: 0x0600B2DA RID: 45786 RVA: 0x0042BACE File Offset: 0x00429CCE
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x1700152D RID: 5421
		// (get) Token: 0x0600B2DB RID: 45787 RVA: 0x0042BAD6 File Offset: 0x00429CD6
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x0400864E RID: 34382
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x0400864F RID: 34383
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008650 RID: 34384
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
