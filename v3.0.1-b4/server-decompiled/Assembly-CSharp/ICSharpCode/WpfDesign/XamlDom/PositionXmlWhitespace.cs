using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001614 RID: 5652
	public class PositionXmlWhitespace : XmlWhitespace, IXmlLineInfo
	{
		// Token: 0x0600B171 RID: 45425 RVA: 0x00422D37 File Offset: 0x00420F37
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

		// Token: 0x0600B172 RID: 45426 RVA: 0x00422D63 File Offset: 0x00420F63
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014C4 RID: 5316
		// (get) Token: 0x0600B173 RID: 45427 RVA: 0x00422D6B File Offset: 0x00420F6B
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014C5 RID: 5317
		// (get) Token: 0x0600B174 RID: 45428 RVA: 0x00422D73 File Offset: 0x00420F73
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x0400853D RID: 34109
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x0400853E RID: 34110
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x0400853F RID: 34111
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
