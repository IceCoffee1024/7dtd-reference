using System;

namespace XMLData.Exceptions
{
	// Token: 0x0200165F RID: 5727
	public class UnexpectedElementException : XmlParserException
	{
		// Token: 0x0600B3EE RID: 46062 RVA: 0x004340C1 File Offset: 0x004322C1
		public UnexpectedElementException(string _msg, int _line)
			: base(_msg, _line)
		{
		}
	}
}
