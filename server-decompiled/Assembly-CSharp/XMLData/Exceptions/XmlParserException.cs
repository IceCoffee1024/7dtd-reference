using System;

namespace XMLData.Exceptions
{
	// Token: 0x02001660 RID: 5728
	public class XmlParserException : Exception
	{
		// Token: 0x17001590 RID: 5520
		// (get) Token: 0x0600B3EF RID: 46063 RVA: 0x004340D6 File Offset: 0x004322D6
		// (set) Token: 0x0600B3F0 RID: 46064 RVA: 0x004340DE File Offset: 0x004322DE
		public int Line
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600B3F1 RID: 46065 RVA: 0x004340E7 File Offset: 0x004322E7
		public XmlParserException(string _msg, int _line)
			: base(_msg)
		{
			this.Line = _line;
		}

		// Token: 0x0600B3F2 RID: 46066 RVA: 0x004340F7 File Offset: 0x004322F7
		public XmlParserException(string _msg, int _line, Exception _innerException)
			: base(_msg, _innerException)
		{
			this.Line = _line;
		}

		// Token: 0x17001591 RID: 5521
		// (get) Token: 0x0600B3F3 RID: 46067 RVA: 0x00434108 File Offset: 0x00432308
		public override string Message
		{
			get
			{
				return string.Format("{0} (line {1})", base.Message, this.Line);
			}
		}

		// Token: 0x0600B3F4 RID: 46068 RVA: 0x00434125 File Offset: 0x00432325
		public override string ToString()
		{
			return this.Message;
		}
	}
}
