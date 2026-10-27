using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001661 RID: 5729
	public class PositionXmlDeclaration : XmlDeclaration, IXmlLineInfo
	{
		// Token: 0x0600B35C RID: 45916 RVA: 0x0042E19B File Offset: 0x0042C39B
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

		// Token: 0x0600B35D RID: 45917 RVA: 0x0042E1CD File Offset: 0x0042C3CD
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x1700154E RID: 5454
		// (get) Token: 0x0600B35E RID: 45918 RVA: 0x0042E1D5 File Offset: 0x0042C3D5
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x1700154F RID: 5455
		// (get) Token: 0x0600B35F RID: 45919 RVA: 0x0042E1DD File Offset: 0x0042C3DD
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x040086B0 RID: 34480
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x040086B1 RID: 34481
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x040086B2 RID: 34482
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
