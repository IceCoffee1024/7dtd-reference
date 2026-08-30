using System;

namespace XMLData.Exceptions
{
	// Token: 0x02001642 RID: 5698
	public class MissingAttributeException : XmlParserException
	{
		// Token: 0x0600B2EF RID: 45807 RVA: 0x0042ED81 File Offset: 0x0042CF81
		public MissingAttributeException(string _msg, int _line)
			: base(_msg, _line)
		{
		}
	}
}
