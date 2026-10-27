using System;

namespace XMLData.Exceptions
{
	// Token: 0x02001690 RID: 5776
	public class UnexpectedElementException : XmlParserException
	{
		// Token: 0x0600B4D8 RID: 46296 RVA: 0x0043A1A1 File Offset: 0x004383A1
		public UnexpectedElementException(string _msg, int _line)
			: base(_msg, _line)
		{
		}
	}
}
