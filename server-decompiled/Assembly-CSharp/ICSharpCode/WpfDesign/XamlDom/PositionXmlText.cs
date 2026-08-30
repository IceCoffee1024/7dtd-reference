using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200162E RID: 5678
	public class PositionXmlText : XmlText, IXmlLineInfo
	{
		// Token: 0x0600B26A RID: 45674 RVA: 0x00428033 File Offset: 0x00426233
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

		// Token: 0x0600B26B RID: 45675 RVA: 0x0042805F File Offset: 0x0042625F
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014FB RID: 5371
		// (get) Token: 0x0600B26C RID: 45676 RVA: 0x00428067 File Offset: 0x00426267
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014FC RID: 5372
		// (get) Token: 0x0600B26D RID: 45677 RVA: 0x0042806F File Offset: 0x0042626F
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008613 RID: 34323
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008614 RID: 34324
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008615 RID: 34325
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
