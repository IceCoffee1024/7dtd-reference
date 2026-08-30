using System;

namespace XMLData.Exceptions
{
	// Token: 0x0200165E RID: 5726
	public class RedefinedElementException : XmlParserException
	{
		// Token: 0x0600B3ED RID: 46061 RVA: 0x004340C1 File Offset: 0x004322C1
		public RedefinedElementException(string _msg, int _line)
			: base(_msg, _line)
		{
		}
	}
}
