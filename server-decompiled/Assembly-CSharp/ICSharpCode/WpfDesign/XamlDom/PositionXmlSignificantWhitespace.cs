using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200164C RID: 5708
	public class PositionXmlSignificantWhitespace : XmlSignificantWhitespace, IXmlLineInfo
	{
		// Token: 0x0600B2F8 RID: 45816 RVA: 0x0042BCCB File Offset: 0x00429ECB
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

		// Token: 0x0600B2F9 RID: 45817 RVA: 0x0042BCF7 File Offset: 0x00429EF7
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x1700153C RID: 5436
		// (get) Token: 0x0600B2FA RID: 45818 RVA: 0x0042BCFF File Offset: 0x00429EFF
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x1700153D RID: 5437
		// (get) Token: 0x0600B2FB RID: 45819 RVA: 0x0042BD07 File Offset: 0x00429F07
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008666 RID: 34406
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008667 RID: 34407
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008668 RID: 34408
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
