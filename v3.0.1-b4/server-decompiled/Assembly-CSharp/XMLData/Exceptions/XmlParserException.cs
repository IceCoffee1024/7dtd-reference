using System;

namespace XMLData.Exceptions
{
	// Token: 0x02001645 RID: 5701
	public class XmlParserException : Exception
	{
		// Token: 0x17001557 RID: 5463
		// (get) Token: 0x0600B2F2 RID: 45810 RVA: 0x0042ED96 File Offset: 0x0042CF96
		// (set) Token: 0x0600B2F3 RID: 45811 RVA: 0x0042ED9E File Offset: 0x0042CF9E
		public int Line
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600B2F4 RID: 45812 RVA: 0x0042EDA7 File Offset: 0x0042CFA7
		public XmlParserException(string _msg, int _line)
			: base(_msg)
		{
			this.Line = _line;
		}

		// Token: 0x0600B2F5 RID: 45813 RVA: 0x0042EDB7 File Offset: 0x0042CFB7
		public XmlParserException(string _msg, int _line, Exception _innerException)
			: base(_msg, _innerException)
		{
			this.Line = _line;
		}

		// Token: 0x17001558 RID: 5464
		// (get) Token: 0x0600B2F6 RID: 45814 RVA: 0x0042EDC8 File Offset: 0x0042CFC8
		public override string Message
		{
			get
			{
				return string.Format("{0} (line {1})", base.Message, this.Line);
			}
		}

		// Token: 0x0600B2F7 RID: 45815 RVA: 0x0042EDE5 File Offset: 0x0042CFE5
		public override string ToString()
		{
			return this.Message;
		}
	}
}
