using System;

namespace XMLData.Exceptions
{
	// Token: 0x0200167E RID: 5758
	public class UnexpectedElementException : XmlParserException
	{
		// Token: 0x0600B480 RID: 46208 RVA: 0x00437D9D File Offset: 0x00435F9D
		public UnexpectedElementException(string _msg, int _line)
			: base(_msg, _line)
		{
		}
	}
}
