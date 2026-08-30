using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001625 RID: 5669
	public class PositionXmlElement : XmlElement, IXmlLineInfo
	{
		// Token: 0x0600B246 RID: 45638 RVA: 0x00427DB8 File Offset: 0x00425FB8
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

		// Token: 0x0600B247 RID: 45639 RVA: 0x00427DEA File Offset: 0x00425FEA
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014E9 RID: 5353
		// (get) Token: 0x0600B248 RID: 45640 RVA: 0x00427DF2 File Offset: 0x00425FF2
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014EA RID: 5354
		// (get) Token: 0x0600B249 RID: 45641 RVA: 0x00427DFA File Offset: 0x00425FFA
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x040085F8 RID: 34296
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x040085F9 RID: 34297
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x040085FA RID: 34298
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
