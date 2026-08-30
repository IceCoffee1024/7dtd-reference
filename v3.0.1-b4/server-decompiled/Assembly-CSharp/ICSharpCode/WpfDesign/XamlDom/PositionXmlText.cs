using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001613 RID: 5651
	public class PositionXmlText : XmlText, IXmlLineInfo
	{
		// Token: 0x0600B16D RID: 45421 RVA: 0x00422CF3 File Offset: 0x00420EF3
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

		// Token: 0x0600B16E RID: 45422 RVA: 0x00422D1F File Offset: 0x00420F1F
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014C2 RID: 5314
		// (get) Token: 0x0600B16F RID: 45423 RVA: 0x00422D27 File Offset: 0x00420F27
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014C3 RID: 5315
		// (get) Token: 0x0600B170 RID: 45424 RVA: 0x00422D2F File Offset: 0x00420F2F
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x0400853A RID: 34106
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x0400853B RID: 34107
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x0400853C RID: 34108
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
