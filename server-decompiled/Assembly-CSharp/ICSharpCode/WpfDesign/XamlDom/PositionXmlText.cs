using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200164D RID: 5709
	public class PositionXmlText : XmlText, IXmlLineInfo
	{
		// Token: 0x0600B2FC RID: 45820 RVA: 0x0042BD0F File Offset: 0x00429F0F
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlText(string text, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(text, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B2FD RID: 45821 RVA: 0x0042BD3B File Offset: 0x00429F3B
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x1700153E RID: 5438
		// (get) Token: 0x0600B2FE RID: 45822 RVA: 0x0042BD43 File Offset: 0x00429F43
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x1700153F RID: 5439
		// (get) Token: 0x0600B2FF RID: 45823 RVA: 0x0042BD4B File Offset: 0x00429F4B
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008669 RID: 34409
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x0400866A RID: 34410
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x0400866B RID: 34411
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
