using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200162B RID: 5675
	public class PositionXmlEntityReference : XmlEntityReference, IXmlLineInfo
	{
		// Token: 0x0600B25E RID: 45662 RVA: 0x00427F63 File Offset: 0x00426163
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlEntityReference(string name, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(name, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B25F RID: 45663 RVA: 0x00427F8F File Offset: 0x0042618F
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014F5 RID: 5365
		// (get) Token: 0x0600B260 RID: 45664 RVA: 0x00427F97 File Offset: 0x00426197
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014F6 RID: 5366
		// (get) Token: 0x0600B261 RID: 45665 RVA: 0x00427F9F File Offset: 0x0042619F
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x0400860A RID: 34314
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x0400860B RID: 34315
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x0400860C RID: 34316
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
