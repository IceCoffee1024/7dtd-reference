using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200162A RID: 5674
	public class PositionXmlDocumentType : XmlDocumentType, IXmlLineInfo
	{
		// Token: 0x0600B25A RID: 45658 RVA: 0x00427F17 File Offset: 0x00426117
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

		// Token: 0x0600B25B RID: 45659 RVA: 0x00427F4B File Offset: 0x0042614B
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014F3 RID: 5363
		// (get) Token: 0x0600B25C RID: 45660 RVA: 0x00427F53 File Offset: 0x00426153
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014F4 RID: 5364
		// (get) Token: 0x0600B25D RID: 45661 RVA: 0x00427F5B File Offset: 0x0042615B
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008607 RID: 34311
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008608 RID: 34312
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008609 RID: 34313
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
