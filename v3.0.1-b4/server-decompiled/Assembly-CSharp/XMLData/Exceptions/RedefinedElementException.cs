using System;

namespace XMLData.Exceptions
{
	// Token: 0x02001643 RID: 5699
	public class RedefinedElementException : XmlParserException
	{
		// Token: 0x0600B2F0 RID: 45808 RVA: 0x0042ED81 File Offset: 0x0042CF81
		public RedefinedElementException(string _msg, int _line)
			: base(_msg, _line)
		{
		}
	}
}
