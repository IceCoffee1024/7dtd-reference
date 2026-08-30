using System;

namespace XMLData.Exceptions
{
	// Token: 0x0200165C RID: 5724
	public class InvalidValueException : XmlParserException
	{
		// Token: 0x0600B3EA RID: 46058 RVA: 0x004340C1 File Offset: 0x004322C1
		public InvalidValueException(string _msg, int _line)
			: base(_msg, _line)
		{
		}

		// Token: 0x0600B3EB RID: 46059 RVA: 0x004340CB File Offset: 0x004322CB
		public InvalidValueException(string _msg, int _line, Exception _innerException)
			: base(_msg, _line, _innerException)
		{
		}
	}
}
