using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001660 RID: 5728
	public class PositionXmlWhitespace : XmlWhitespace, IXmlLineInfo
	{
		// Token: 0x0600B358 RID: 45912 RVA: 0x0042E157 File Offset: 0x0042C357
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

		// Token: 0x0600B359 RID: 45913 RVA: 0x0042E183 File Offset: 0x0042C383
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x1700154C RID: 5452
		// (get) Token: 0x0600B35A RID: 45914 RVA: 0x0042E18B File Offset: 0x0042C38B
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x1700154D RID: 5453
		// (get) Token: 0x0600B35B RID: 45915 RVA: 0x0042E193 File Offset: 0x0042C393
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x040086AD RID: 34477
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x040086AE RID: 34478
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x040086AF RID: 34479
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
