using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001648 RID: 5704
	public class PositionXmlDocumentFragment : XmlDocumentFragment, IXmlLineInfo
	{
		// Token: 0x0600B2E8 RID: 45800 RVA: 0x0042BBB0 File Offset: 0x00429DB0
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

		// Token: 0x0600B2E9 RID: 45801 RVA: 0x0042BBDB File Offset: 0x00429DDB
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x17001534 RID: 5428
		// (get) Token: 0x0600B2EA RID: 45802 RVA: 0x0042BBE3 File Offset: 0x00429DE3
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x17001535 RID: 5429
		// (get) Token: 0x0600B2EB RID: 45803 RVA: 0x0042BBEB File Offset: 0x00429DEB
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x0400865A RID: 34394
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x0400865B RID: 34395
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x0400865C RID: 34396
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
