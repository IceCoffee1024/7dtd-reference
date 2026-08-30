using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001612 RID: 5650
	public class PositionXmlSignificantWhitespace : XmlSignificantWhitespace, IXmlLineInfo
	{
		// Token: 0x0600B169 RID: 45417 RVA: 0x00422CAF File Offset: 0x00420EAF
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlSignificantWhitespace(string text, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(text, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B16A RID: 45418 RVA: 0x00422CDB File Offset: 0x00420EDB
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014C0 RID: 5312
		// (get) Token: 0x0600B16B RID: 45419 RVA: 0x00422CE3 File Offset: 0x00420EE3
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014C1 RID: 5313
		// (get) Token: 0x0600B16C RID: 45420 RVA: 0x00422CEB File Offset: 0x00420EEB
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008537 RID: 34103
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008538 RID: 34104
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008539 RID: 34105
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
