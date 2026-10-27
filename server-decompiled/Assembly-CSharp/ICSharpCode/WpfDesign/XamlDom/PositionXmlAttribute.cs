using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001657 RID: 5719
	public class PositionXmlAttribute : XmlAttribute, IXmlLineInfo
	{
		// Token: 0x0600B334 RID: 45876 RVA: 0x0042DEE2 File Offset: 0x0042C0E2
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

		// Token: 0x0600B335 RID: 45877 RVA: 0x0042DF14 File Offset: 0x0042C114
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x1700153A RID: 5434
		// (get) Token: 0x0600B336 RID: 45878 RVA: 0x0042DF1C File Offset: 0x0042C11C
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x1700153B RID: 5435
		// (get) Token: 0x0600B337 RID: 45879 RVA: 0x0042DF24 File Offset: 0x0042C124
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008692 RID: 34450
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008693 RID: 34451
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008694 RID: 34452
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
