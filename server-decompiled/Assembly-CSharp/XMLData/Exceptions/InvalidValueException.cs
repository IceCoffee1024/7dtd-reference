using System;

namespace XMLData.Exceptions
{
	// Token: 0x02001641 RID: 5697
	public class InvalidValueException : XmlParserException
	{
		// Token: 0x0600B2ED RID: 45805 RVA: 0x0042ED81 File Offset: 0x0042CF81
		public InvalidValueException(string _msg, int _line)
			: base(_msg, _line)
		{
		}

		// Token: 0x0600B2EE RID: 45806 RVA: 0x0042ED8B File Offset: 0x0042CF8B
		public InvalidValueException(string _msg, int _line, Exception _innerException)
			: base(_msg, _line, _innerException)
		{
		}
	}
}
