using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001630 RID: 5680
	public class PositionXmlDeclaration : XmlDeclaration, IXmlLineInfo
	{
		// Token: 0x0600B272 RID: 45682 RVA: 0x004280BB File Offset: 0x004262BB
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

		// Token: 0x0600B273 RID: 45683 RVA: 0x004280ED File Offset: 0x004262ED
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014FF RID: 5375
		// (get) Token: 0x0600B274 RID: 45684 RVA: 0x004280F5 File Offset: 0x004262F5
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x17001500 RID: 5376
		// (get) Token: 0x0600B275 RID: 45685 RVA: 0x004280FD File Offset: 0x004262FD
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008619 RID: 34329
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x0400861A RID: 34330
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x0400861B RID: 34331
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
