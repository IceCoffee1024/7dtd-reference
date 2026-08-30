using System;

namespace XMLData.Exceptions
{
	// Token: 0x0200165B RID: 5723
	public class IncorrectAttributeOccurrenceException : XmlParserException
	{
		// Token: 0x0600B3E9 RID: 46057 RVA: 0x004340C1 File Offset: 0x004322C1
		public IncorrectAttributeOccurrenceException(string _msg, int _line)
			: base(_msg, _line)
		{
		}
	}
}
