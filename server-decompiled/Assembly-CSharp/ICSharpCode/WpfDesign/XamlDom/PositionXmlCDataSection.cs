using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001627 RID: 5671
	public class PositionXmlCDataSection : XmlCDataSection, IXmlLineInfo
	{
		// Token: 0x0600B24E RID: 45646 RVA: 0x00427E4C File Offset: 0x0042604C
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

		// Token: 0x0600B24F RID: 45647 RVA: 0x00427E78 File Offset: 0x00426078
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014ED RID: 5357
		// (get) Token: 0x0600B250 RID: 45648 RVA: 0x00427E80 File Offset: 0x00426080
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014EE RID: 5358
		// (get) Token: 0x0600B251 RID: 45649 RVA: 0x00427E88 File Offset: 0x00426088
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x040085FE RID: 34302
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x040085FF RID: 34303
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008600 RID: 34304
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
