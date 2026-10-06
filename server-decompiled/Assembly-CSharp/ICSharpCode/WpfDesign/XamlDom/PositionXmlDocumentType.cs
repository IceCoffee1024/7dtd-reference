using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001649 RID: 5705
	public class PositionXmlDocumentType : XmlDocumentType, IXmlLineInfo
	{
		// Token: 0x0600B2EC RID: 45804 RVA: 0x0042BBF3 File Offset: 0x00429DF3
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

		// Token: 0x0600B2ED RID: 45805 RVA: 0x0042BC27 File Offset: 0x00429E27
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x17001536 RID: 5430
		// (get) Token: 0x0600B2EE RID: 45806 RVA: 0x0042BC2F File Offset: 0x00429E2F
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x17001537 RID: 5431
		// (get) Token: 0x0600B2EF RID: 45807 RVA: 0x0042BC37 File Offset: 0x00429E37
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x0400865D RID: 34397
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x0400865E RID: 34398
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x0400865F RID: 34399
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
