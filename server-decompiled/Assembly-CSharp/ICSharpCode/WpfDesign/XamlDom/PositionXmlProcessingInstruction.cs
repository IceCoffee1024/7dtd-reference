using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200162C RID: 5676
	public class PositionXmlProcessingInstruction : XmlProcessingInstruction, IXmlLineInfo
	{
		// Token: 0x0600B262 RID: 45666 RVA: 0x00427FA7 File Offset: 0x004261A7
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

		// Token: 0x0600B263 RID: 45667 RVA: 0x00427FD7 File Offset: 0x004261D7
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014F7 RID: 5367
		// (get) Token: 0x0600B264 RID: 45668 RVA: 0x00427FDF File Offset: 0x004261DF
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014F8 RID: 5368
		// (get) Token: 0x0600B265 RID: 45669 RVA: 0x00427FE7 File Offset: 0x004261E7
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x0400860D RID: 34317
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x0400860E RID: 34318
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x0400860F RID: 34319
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
