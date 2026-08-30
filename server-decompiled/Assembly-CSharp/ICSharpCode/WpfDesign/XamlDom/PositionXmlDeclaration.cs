using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001615 RID: 5653
	public class PositionXmlDeclaration : XmlDeclaration, IXmlLineInfo
	{
		// Token: 0x0600B175 RID: 45429 RVA: 0x00422D7B File Offset: 0x00420F7B
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlDeclaration(string version, string encoding, string standalone, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(version, encoding, standalone, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B176 RID: 45430 RVA: 0x00422DAD File Offset: 0x00420FAD
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014C6 RID: 5318
		// (get) Token: 0x0600B177 RID: 45431 RVA: 0x00422DB5 File Offset: 0x00420FB5
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014C7 RID: 5319
		// (get) Token: 0x0600B178 RID: 45432 RVA: 0x00422DBD File Offset: 0x00420FBD
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008540 RID: 34112
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008541 RID: 34113
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008542 RID: 34114
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
