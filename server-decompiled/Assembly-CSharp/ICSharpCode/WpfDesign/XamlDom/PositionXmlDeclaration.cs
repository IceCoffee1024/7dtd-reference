using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200164F RID: 5711
	public class PositionXmlDeclaration : XmlDeclaration, IXmlLineInfo
	{
		// Token: 0x0600B304 RID: 45828 RVA: 0x0042BD97 File Offset: 0x00429F97
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

		// Token: 0x0600B305 RID: 45829 RVA: 0x0042BDC9 File Offset: 0x00429FC9
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x17001542 RID: 5442
		// (get) Token: 0x0600B306 RID: 45830 RVA: 0x0042BDD1 File Offset: 0x00429FD1
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x17001543 RID: 5443
		// (get) Token: 0x0600B307 RID: 45831 RVA: 0x0042BDD9 File Offset: 0x00429FD9
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x0400866F RID: 34415
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008670 RID: 34416
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008671 RID: 34417
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
