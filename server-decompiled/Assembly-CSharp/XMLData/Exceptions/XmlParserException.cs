using System;

namespace XMLData.Exceptions
{
	// Token: 0x0200167F RID: 5759
	public class XmlParserException : Exception
	{
		// Token: 0x170015D3 RID: 5587
		// (get) Token: 0x0600B481 RID: 46209 RVA: 0x00437DB2 File Offset: 0x00435FB2
		// (set) Token: 0x0600B482 RID: 46210 RVA: 0x00437DBA File Offset: 0x00435FBA
		public int Line
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600B483 RID: 46211 RVA: 0x00437DC3 File Offset: 0x00435FC3
		public XmlParserException(string _msg, int _line)
			: base(_msg)
		{
			this.Line = _line;
		}

		// Token: 0x0600B484 RID: 46212 RVA: 0x00437DD3 File Offset: 0x00435FD3
		public XmlParserException(string _msg, int _line, Exception _innerException)
			: base(_msg, _innerException)
		{
			this.Line = _line;
		}

		// Token: 0x170015D4 RID: 5588
		// (get) Token: 0x0600B485 RID: 46213 RVA: 0x00437DE4 File Offset: 0x00435FE4
		public override string Message
		{
			get
			{
				return string.Format("{0} (line {1})", base.Message, this.Line);
			}
		}

		// Token: 0x0600B486 RID: 46214 RVA: 0x00437E01 File Offset: 0x00436001
		public override string ToString()
		{
			return this.Message;
		}
	}
}
