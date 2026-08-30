using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001629 RID: 5673
	public class PositionXmlDocumentFragment : XmlDocumentFragment, IXmlLineInfo
	{
		// Token: 0x0600B256 RID: 45654 RVA: 0x00427ED4 File Offset: 0x004260D4
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

		// Token: 0x0600B257 RID: 45655 RVA: 0x00427EFF File Offset: 0x004260FF
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014F1 RID: 5361
		// (get) Token: 0x0600B258 RID: 45656 RVA: 0x00427F07 File Offset: 0x00426107
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014F2 RID: 5362
		// (get) Token: 0x0600B259 RID: 45657 RVA: 0x00427F0F File Offset: 0x0042610F
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008604 RID: 34308
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008605 RID: 34309
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008606 RID: 34310
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
