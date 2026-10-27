using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001658 RID: 5720
	public class PositionXmlCDataSection : XmlCDataSection, IXmlLineInfo
	{
		// Token: 0x0600B338 RID: 45880 RVA: 0x0042DF2C File Offset: 0x0042C12C
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

		// Token: 0x0600B339 RID: 45881 RVA: 0x0042DF58 File Offset: 0x0042C158
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x1700153C RID: 5436
		// (get) Token: 0x0600B33A RID: 45882 RVA: 0x0042DF60 File Offset: 0x0042C160
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x1700153D RID: 5437
		// (get) Token: 0x0600B33B RID: 45883 RVA: 0x0042DF68 File Offset: 0x0042C168
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008695 RID: 34453
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008696 RID: 34454
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008697 RID: 34455
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
