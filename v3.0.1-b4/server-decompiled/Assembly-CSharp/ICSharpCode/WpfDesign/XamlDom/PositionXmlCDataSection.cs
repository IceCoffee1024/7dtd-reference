using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x0200160C RID: 5644
	public class PositionXmlCDataSection : XmlCDataSection, IXmlLineInfo
	{
		// Token: 0x0600B151 RID: 45393 RVA: 0x00422B0C File Offset: 0x00420D0C
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

		// Token: 0x0600B152 RID: 45394 RVA: 0x00422B38 File Offset: 0x00420D38
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014B4 RID: 5300
		// (get) Token: 0x0600B153 RID: 45395 RVA: 0x00422B40 File Offset: 0x00420D40
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014B5 RID: 5301
		// (get) Token: 0x0600B154 RID: 45396 RVA: 0x00422B48 File Offset: 0x00420D48
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008525 RID: 34085
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008526 RID: 34086
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008527 RID: 34087
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
