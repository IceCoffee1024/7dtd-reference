using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001611 RID: 5649
	public class PositionXmlProcessingInstruction : XmlProcessingInstruction, IXmlLineInfo
	{
		// Token: 0x0600B165 RID: 45413 RVA: 0x00422C67 File Offset: 0x00420E67
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

		// Token: 0x0600B166 RID: 45414 RVA: 0x00422C97 File Offset: 0x00420E97
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014BE RID: 5310
		// (get) Token: 0x0600B167 RID: 45415 RVA: 0x00422C9F File Offset: 0x00420E9F
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014BF RID: 5311
		// (get) Token: 0x0600B168 RID: 45416 RVA: 0x00422CA7 File Offset: 0x00420EA7
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008534 RID: 34100
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008535 RID: 34101
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008536 RID: 34102
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
