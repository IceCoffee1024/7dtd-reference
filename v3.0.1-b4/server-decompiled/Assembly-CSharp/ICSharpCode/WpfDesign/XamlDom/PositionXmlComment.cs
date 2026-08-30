using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200160D RID: 5645
	public class PositionXmlComment : XmlComment, IXmlLineInfo
	{
		// Token: 0x0600B155 RID: 45397 RVA: 0x00422B50 File Offset: 0x00420D50
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlComment(string data, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(data, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B156 RID: 45398 RVA: 0x00422B7C File Offset: 0x00420D7C
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014B6 RID: 5302
		// (get) Token: 0x0600B157 RID: 45399 RVA: 0x00422B84 File Offset: 0x00420D84
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014B7 RID: 5303
		// (get) Token: 0x0600B158 RID: 45400 RVA: 0x00422B8C File Offset: 0x00420D8C
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008528 RID: 34088
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008529 RID: 34089
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x0400852A RID: 34090
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
