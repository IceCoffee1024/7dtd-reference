using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200162F RID: 5679
	public class PositionXmlWhitespace : XmlWhitespace, IXmlLineInfo
	{
		// Token: 0x0600B26E RID: 45678 RVA: 0x00428077 File Offset: 0x00426277
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlWhitespace(string text, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(text, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B26F RID: 45679 RVA: 0x004280A3 File Offset: 0x004262A3
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014FD RID: 5373
		// (get) Token: 0x0600B270 RID: 45680 RVA: 0x004280AB File Offset: 0x004262AB
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014FE RID: 5374
		// (get) Token: 0x0600B271 RID: 45681 RVA: 0x004280B3 File Offset: 0x004262B3
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008616 RID: 34326
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008617 RID: 34327
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008618 RID: 34328
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
