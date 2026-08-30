using System;

namespace XMLData.Exceptions
{
	// Token: 0x02001644 RID: 5700
	public class UnexpectedElementException : XmlParserException
	{
		// Token: 0x0600B2F1 RID: 45809 RVA: 0x0042ED81 File Offset: 0x0042CF81
		public UnexpectedElementException(string _msg, int _line)
			: base(_msg, _line)
		{
		}
	}
}
