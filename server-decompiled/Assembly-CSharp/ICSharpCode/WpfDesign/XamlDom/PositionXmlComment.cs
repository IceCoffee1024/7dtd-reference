using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001647 RID: 5703
	public class PositionXmlComment : XmlComment, IXmlLineInfo
	{
		// Token: 0x0600B2E4 RID: 45796 RVA: 0x0042BB6C File Offset: 0x00429D6C
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlComment(string data, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(data, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B2E5 RID: 45797 RVA: 0x0042BB98 File Offset: 0x00429D98
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x17001532 RID: 5426
		// (get) Token: 0x0600B2E6 RID: 45798 RVA: 0x0042BBA0 File Offset: 0x00429DA0
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x17001533 RID: 5427
		// (get) Token: 0x0600B2E7 RID: 45799 RVA: 0x0042BBA8 File Offset: 0x00429DA8
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008657 RID: 34391
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008658 RID: 34392
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008659 RID: 34393
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
