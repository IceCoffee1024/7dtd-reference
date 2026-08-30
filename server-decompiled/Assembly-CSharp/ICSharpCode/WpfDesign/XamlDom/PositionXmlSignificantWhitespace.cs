using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200162D RID: 5677
	public class PositionXmlSignificantWhitespace : XmlSignificantWhitespace, IXmlLineInfo
	{
		// Token: 0x0600B266 RID: 45670 RVA: 0x00427FEF File Offset: 0x004261EF
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlSignificantWhitespace(string text, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(text, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B267 RID: 45671 RVA: 0x0042801B File Offset: 0x0042621B
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014F9 RID: 5369
		// (get) Token: 0x0600B268 RID: 45672 RVA: 0x00428023 File Offset: 0x00426223
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014FA RID: 5370
		// (get) Token: 0x0600B269 RID: 45673 RVA: 0x0042802B File Offset: 0x0042622B
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008610 RID: 34320
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008611 RID: 34321
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008612 RID: 34322
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
