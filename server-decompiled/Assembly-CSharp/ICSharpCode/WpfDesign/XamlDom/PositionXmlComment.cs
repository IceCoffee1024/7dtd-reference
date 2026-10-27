using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001659 RID: 5721
	public class PositionXmlComment : XmlComment, IXmlLineInfo
	{
		// Token: 0x0600B33C RID: 45884 RVA: 0x0042DF70 File Offset: 0x0042C170
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

		// Token: 0x0600B33D RID: 45885 RVA: 0x0042DF9C File Offset: 0x0042C19C
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x1700153E RID: 5438
		// (get) Token: 0x0600B33E RID: 45886 RVA: 0x0042DFA4 File Offset: 0x0042C1A4
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x1700153F RID: 5439
		// (get) Token: 0x0600B33F RID: 45887 RVA: 0x0042DFAC File Offset: 0x0042C1AC
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008698 RID: 34456
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008699 RID: 34457
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x0400869A RID: 34458
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
