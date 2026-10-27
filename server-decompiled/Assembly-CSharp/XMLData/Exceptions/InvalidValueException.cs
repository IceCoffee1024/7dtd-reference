using System;

namespace XMLData.Exceptions
{
	// Token: 0x0200168D RID: 5773
	public class InvalidValueException : XmlParserException
	{
		// Token: 0x0600B4D4 RID: 46292 RVA: 0x0043A1A1 File Offset: 0x004383A1
		public InvalidValueException(string _msg, int _line)
			: base(_msg, _line)
		{
		}

		// Token: 0x0600B4D5 RID: 46293 RVA: 0x0043A1AB File Offset: 0x004383AB
		public InvalidValueException(string _msg, int _line, Exception _innerException)
			: base(_msg, _line, _innerException)
		{
		}
	}
}
