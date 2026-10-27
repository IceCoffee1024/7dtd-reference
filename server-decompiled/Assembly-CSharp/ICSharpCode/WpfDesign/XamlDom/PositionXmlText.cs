using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200165F RID: 5727
	public class PositionXmlText : XmlText, IXmlLineInfo
	{
		// Token: 0x0600B354 RID: 45908 RVA: 0x0042E113 File Offset: 0x0042C313
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlText(string text, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(text, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B355 RID: 45909 RVA: 0x0042E13F File Offset: 0x0042C33F
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x1700154A RID: 5450
		// (get) Token: 0x0600B356 RID: 45910 RVA: 0x0042E147 File Offset: 0x0042C347
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x1700154B RID: 5451
		// (get) Token: 0x0600B357 RID: 45911 RVA: 0x0042E14F File Offset: 0x0042C34F
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x040086AA RID: 34474
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x040086AB RID: 34475
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x040086AC RID: 34476
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
