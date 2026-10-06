using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200164B RID: 5707
	public class PositionXmlProcessingInstruction : XmlProcessingInstruction, IXmlLineInfo
	{
		// Token: 0x0600B2F4 RID: 45812 RVA: 0x0042BC83 File Offset: 0x00429E83
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlProcessingInstruction(string target, string data, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(target, data, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B2F5 RID: 45813 RVA: 0x0042BCB3 File Offset: 0x00429EB3
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x1700153A RID: 5434
		// (get) Token: 0x0600B2F6 RID: 45814 RVA: 0x0042BCBB File Offset: 0x00429EBB
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x1700153B RID: 5435
		// (get) Token: 0x0600B2F7 RID: 45815 RVA: 0x0042BCC3 File Offset: 0x00429EC3
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008663 RID: 34403
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008664 RID: 34404
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008665 RID: 34405
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
