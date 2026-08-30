using System;

namespace XMLData.Exceptions
{
	// Token: 0x0200165D RID: 5725
	public class MissingAttributeException : XmlParserException
	{
		// Token: 0x0600B3EC RID: 46060 RVA: 0x004340C1 File Offset: 0x004322C1
		public MissingAttributeException(string _msg, int _line)
			: base(_msg, _line)
		{
		}
	}
}
