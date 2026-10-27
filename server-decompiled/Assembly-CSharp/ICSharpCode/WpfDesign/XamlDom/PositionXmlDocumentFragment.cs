using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200165A RID: 5722
	public class PositionXmlDocumentFragment : XmlDocumentFragment, IXmlLineInfo
	{
		// Token: 0x0600B340 RID: 45888 RVA: 0x0042DFB4 File Offset: 0x0042C1B4
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlDocumentFragment(XmlDocument doc, IXmlLineInfo lineInfo)
			: base(doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B341 RID: 45889 RVA: 0x0042DFDF File Offset: 0x0042C1DF
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x17001540 RID: 5440
		// (get) Token: 0x0600B342 RID: 45890 RVA: 0x0042DFE7 File Offset: 0x0042C1E7
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x17001541 RID: 5441
		// (get) Token: 0x0600B343 RID: 45891 RVA: 0x0042DFEF File Offset: 0x0042C1EF
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x0400869B RID: 34459
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x0400869C RID: 34460
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x0400869D RID: 34461
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
