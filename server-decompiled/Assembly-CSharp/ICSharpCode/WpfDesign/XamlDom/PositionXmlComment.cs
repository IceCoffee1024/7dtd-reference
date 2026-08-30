using System;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001628 RID: 5672
	public class PositionXmlComment : XmlComment, IXmlLineInfo
	{
		// Token: 0x0600B252 RID: 45650 RVA: 0x00427E90 File Offset: 0x00426090
		[PublicizedFrom(EAccessModifier.Internal)]
		public PositionXmlComment(string data, XmlDocument doc, IXmlLineInfo lineInfo)
			: base(data, doc)
		{
			if (lineInfo != null)
			{
				this.lineNumber = lineInfo.LineNumber;
				this.linePosition = lineInfo.LinePosition;
				this.hasLineInfo = true;
			}
		}

		// Token: 0x0600B253 RID: 45651 RVA: 0x00427EBC File Offset: 0x004260BC
		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		// Token: 0x170014EF RID: 5359
		// (get) Token: 0x0600B254 RID: 45652 RVA: 0x00427EC4 File Offset: 0x004260C4
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170014F0 RID: 5360
		// (get) Token: 0x0600B255 RID: 45653 RVA: 0x00427ECC File Offset: 0x004260CC
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04008601 RID: 34305
		[PublicizedFrom(EAccessModifier.Private)]
		public int lineNumber;

		// Token: 0x04008602 RID: 34306
		[PublicizedFrom(EAccessModifier.Private)]
		public int linePosition;

		// Token: 0x04008603 RID: 34307
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasLineInfo;
	}
}
