using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200160A RID: 5642
	public class PositionXmlElement : XmlElement, IXmlLineInfo
	{
		// Token: 0x0600B149 RID: 45385 RVA: 0x00422A78 File Offset: 0x00420C78
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

		// Token: 0x0600B14A RID: 45386 RVA: 0x00422AAA File Offset: 0x00420CAA
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014B0 RID: 5296
		// (get) Token: 0x0600B14B RID: 45387 RVA: 0x00422AB2 File Offset: 0x00420CB2
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014B1 RID: 5297
		// (get) Token: 0x0600B14C RID: 45388 RVA: 0x00422ABA File Offset: 0x00420CBA
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x0400851F RID: 34079
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008520 RID: 34080
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008521 RID: 34081
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
