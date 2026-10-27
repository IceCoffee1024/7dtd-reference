using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200165E RID: 5726
	public class PositionXmlSignificantWhitespace : XmlSignificantWhitespace, IXmlLineInfo
	{
		// Token: 0x0600B350 RID: 45904 RVA: 0x0042E0CF File Offset: 0x0042C2CF
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

		// Token: 0x0600B351 RID: 45905 RVA: 0x0042E0FB File Offset: 0x0042C2FB
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x17001548 RID: 5448
		// (get) Token: 0x0600B352 RID: 45906 RVA: 0x0042E103 File Offset: 0x0042C303
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x17001549 RID: 5449
		// (get) Token: 0x0600B353 RID: 45907 RVA: 0x0042E10B File Offset: 0x0042C30B
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x040086A7 RID: 34471
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x040086A8 RID: 34472
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x040086A9 RID: 34473
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
