using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200164A RID: 5706
	public class PositionXmlEntityReference : XmlEntityReference, IXmlLineInfo
	{
		// Token: 0x0600B2F0 RID: 45808 RVA: 0x0042BC3F File Offset: 0x00429E3F
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlEntityReference(string name, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(name, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B2F1 RID: 45809 RVA: 0x0042BC6B File Offset: 0x00429E6B
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x17001538 RID: 5432
		// (get) Token: 0x0600B2F2 RID: 45810 RVA: 0x0042BC73 File Offset: 0x00429E73
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x17001539 RID: 5433
		// (get) Token: 0x0600B2F3 RID: 45811 RVA: 0x0042BC7B File Offset: 0x00429E7B
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008660 RID: 34400
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008661 RID: 34401
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008662 RID: 34402
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
