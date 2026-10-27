using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200165D RID: 5725
	public class PositionXmlProcessingInstruction : XmlProcessingInstruction, IXmlLineInfo
	{
		// Token: 0x0600B34C RID: 45900 RVA: 0x0042E087 File Offset: 0x0042C287
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlProcessingInstruction(string target, string data, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(target, data, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B34D RID: 45901 RVA: 0x0042E0B7 File Offset: 0x0042C2B7
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x17001546 RID: 5446
		// (get) Token: 0x0600B34E RID: 45902 RVA: 0x0042E0BF File Offset: 0x0042C2BF
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x17001547 RID: 5447
		// (get) Token: 0x0600B34F RID: 45903 RVA: 0x0042E0C7 File Offset: 0x0042C2C7
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x040086A4 RID: 34468
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x040086A5 RID: 34469
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x040086A6 RID: 34470
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
