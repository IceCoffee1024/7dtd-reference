using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200165C RID: 5724
	public class PositionXmlEntityReference : XmlEntityReference, IXmlLineInfo
	{
		// Token: 0x0600B348 RID: 45896 RVA: 0x0042E043 File Offset: 0x0042C243
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

		// Token: 0x0600B349 RID: 45897 RVA: 0x0042E06F File Offset: 0x0042C26F
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x17001544 RID: 5444
		// (get) Token: 0x0600B34A RID: 45898 RVA: 0x0042E077 File Offset: 0x0042C277
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x17001545 RID: 5445
		// (get) Token: 0x0600B34B RID: 45899 RVA: 0x0042E07F File Offset: 0x0042C27F
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x040086A1 RID: 34465
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x040086A2 RID: 34466
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x040086A3 RID: 34467
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
