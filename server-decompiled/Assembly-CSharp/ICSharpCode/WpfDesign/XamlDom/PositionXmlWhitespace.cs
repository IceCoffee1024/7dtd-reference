using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200164E RID: 5710
	public class PositionXmlWhitespace : XmlWhitespace, IXmlLineInfo
	{
		// Token: 0x0600B300 RID: 45824 RVA: 0x0042BD53 File Offset: 0x00429F53
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlWhitespace(string text, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(text, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B301 RID: 45825 RVA: 0x0042BD7F File Offset: 0x00429F7F
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x17001540 RID: 5440
		// (get) Token: 0x0600B302 RID: 45826 RVA: 0x0042BD87 File Offset: 0x00429F87
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x17001541 RID: 5441
		// (get) Token: 0x0600B303 RID: 45827 RVA: 0x0042BD8F File Offset: 0x00429F8F
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x0400866C RID: 34412
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x0400866D RID: 34413
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x0400866E RID: 34414
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
