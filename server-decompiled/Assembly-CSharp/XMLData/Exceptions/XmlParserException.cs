using System;

namespace XMLData.Exceptions
{
	// Token: 0x02001691 RID: 5777
	public class XmlParserException : Exception
	{
		// Token: 0x170015DF RID: 5599
		// (get) Token: 0x0600B4D9 RID: 46297 RVA: 0x0043A1B6 File Offset: 0x004383B6
		// (set) Token: 0x0600B4DA RID: 46298 RVA: 0x0043A1BE File Offset: 0x004383BE
		public int Line
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600B4DB RID: 46299 RVA: 0x0043A1C7 File Offset: 0x004383C7
		public XmlParserException(string _msg, int _line)
			: base(_msg)
		{
			this.Line = _line;
		}

		// Token: 0x0600B4DC RID: 46300 RVA: 0x0043A1D7 File Offset: 0x004383D7
		public XmlParserException(string _msg, int _line, Exception _innerException)
			: base(_msg, _innerException)
		{
			this.Line = _line;
		}

		// Token: 0x170015E0 RID: 5600
		// (get) Token: 0x0600B4DD RID: 46301 RVA: 0x0043A1E8 File Offset: 0x004383E8
		public override string Message
		{
			get
			{
				return string.Format("{0} (line {1})", base.Message, this.Line);
			}
		}

		// Token: 0x0600B4DE RID: 46302 RVA: 0x0043A205 File Offset: 0x00438405
		public override string ToString()
		{
			return this.Message;
		}
	}
}
