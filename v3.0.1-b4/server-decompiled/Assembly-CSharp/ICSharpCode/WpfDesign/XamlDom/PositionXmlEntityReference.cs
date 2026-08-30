using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001610 RID: 5648
	public class PositionXmlEntityReference : XmlEntityReference, IXmlLineInfo
	{
		// Token: 0x0600B161 RID: 45409 RVA: 0x00422C23 File Offset: 0x00420E23
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

		// Token: 0x0600B162 RID: 45410 RVA: 0x00422C4F File Offset: 0x00420E4F
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014BC RID: 5308
		// (get) Token: 0x0600B163 RID: 45411 RVA: 0x00422C57 File Offset: 0x00420E57
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014BD RID: 5309
		// (get) Token: 0x0600B164 RID: 45412 RVA: 0x00422C5F File Offset: 0x00420E5F
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008531 RID: 34097
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008532 RID: 34098
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008533 RID: 34099
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
