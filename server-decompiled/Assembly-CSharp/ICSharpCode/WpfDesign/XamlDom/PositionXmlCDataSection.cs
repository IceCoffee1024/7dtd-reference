using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001646 RID: 5702
	public class PositionXmlCDataSection : XmlCDataSection, IXmlLineInfo
	{
		// Token: 0x0600B2E0 RID: 45792 RVA: 0x0042BB28 File Offset: 0x00429D28
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlCDataSection(string data, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(data, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B2E1 RID: 45793 RVA: 0x0042BB54 File Offset: 0x00429D54
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x17001530 RID: 5424
		// (get) Token: 0x0600B2E2 RID: 45794 RVA: 0x0042BB5C File Offset: 0x00429D5C
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x17001531 RID: 5425
		// (get) Token: 0x0600B2E3 RID: 45795 RVA: 0x0042BB64 File Offset: 0x00429D64
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008654 RID: 34388
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008655 RID: 34389
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008656 RID: 34390
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
