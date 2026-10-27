using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200165B RID: 5723
	public class PositionXmlDocumentType : XmlDocumentType, IXmlLineInfo
	{
		// Token: 0x0600B344 RID: 45892 RVA: 0x0042DFF7 File Offset: 0x0042C1F7
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlDocumentType(string name, string publicId, string systemId, string internalSubset, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(name, publicId, systemId, internalSubset, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B345 RID: 45893 RVA: 0x0042E02B File Offset: 0x0042C22B
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x17001542 RID: 5442
		// (get) Token: 0x0600B346 RID: 45894 RVA: 0x0042E033 File Offset: 0x0042C233
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x17001543 RID: 5443
		// (get) Token: 0x0600B347 RID: 45895 RVA: 0x0042E03B File Offset: 0x0042C23B
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x0400869E RID: 34462
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x0400869F RID: 34463
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x040086A0 RID: 34464
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
