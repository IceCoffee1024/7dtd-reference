using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001626 RID: 5670
	public class PositionXmlAttribute : XmlAttribute, IXmlLineInfo
	{
		// Token: 0x0600B24A RID: 45642 RVA: 0x00427E02 File Offset: 0x00426002
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

		// Token: 0x0600B24B RID: 45643 RVA: 0x00427E34 File Offset: 0x00426034
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014EB RID: 5355
		// (get) Token: 0x0600B24C RID: 45644 RVA: 0x00427E3C File Offset: 0x0042603C
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014EC RID: 5356
		// (get) Token: 0x0600B24D RID: 45645 RVA: 0x00427E44 File Offset: 0x00426044
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x040085FB RID: 34299
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x040085FC RID: 34300
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x040085FD RID: 34301
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
