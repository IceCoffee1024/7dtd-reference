using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200160F RID: 5647
	public class PositionXmlDocumentType : XmlDocumentType, IXmlLineInfo
	{
		// Token: 0x0600B15D RID: 45405 RVA: 0x00422BD7 File Offset: 0x00420DD7
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

		// Token: 0x0600B15E RID: 45406 RVA: 0x00422C0B File Offset: 0x00420E0B
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014BA RID: 5306
		// (get) Token: 0x0600B15F RID: 45407 RVA: 0x00422C13 File Offset: 0x00420E13
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014BB RID: 5307
		// (get) Token: 0x0600B160 RID: 45408 RVA: 0x00422C1B File Offset: 0x00420E1B
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x0400852E RID: 34094
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x0400852F RID: 34095
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008530 RID: 34096
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
