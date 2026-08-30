using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200160E RID: 5646
	public class PositionXmlDocumentFragment : XmlDocumentFragment, IXmlLineInfo
	{
		// Token: 0x0600B159 RID: 45401 RVA: 0x00422B94 File Offset: 0x00420D94
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

		// Token: 0x0600B15A RID: 45402 RVA: 0x00422BBF File Offset: 0x00420DBF
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014B8 RID: 5304
		// (get) Token: 0x0600B15B RID: 45403 RVA: 0x00422BC7 File Offset: 0x00420DC7
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014B9 RID: 5305
		// (get) Token: 0x0600B15C RID: 45404 RVA: 0x00422BCF File Offset: 0x00420DCF
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x0400852B RID: 34091
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x0400852C RID: 34092
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x0400852D RID: 34093
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
