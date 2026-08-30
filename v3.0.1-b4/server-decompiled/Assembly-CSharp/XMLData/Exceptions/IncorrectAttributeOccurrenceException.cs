using System;

namespace XMLData.Exceptions
{
	// Token: 0x02001640 RID: 5696
	public class IncorrectAttributeOccurrenceException : XmlParserException
	{
		// Token: 0x0600B2EC RID: 45804 RVA: 0x0042ED81 File Offset: 0x0042CF81
		public IncorrectAttributeOccurrenceException(string _msg, int _line)
			: base(_msg, _line)
		{
		}
	}
}
