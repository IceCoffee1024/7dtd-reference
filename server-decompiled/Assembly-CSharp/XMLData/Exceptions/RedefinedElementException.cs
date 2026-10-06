using System;

namespace XMLData.Exceptions
{
	// Token: 0x0200167D RID: 5757
	public class RedefinedElementException : XmlParserException
	{
		// Token: 0x0600B47F RID: 46207 RVA: 0x00437D9D File Offset: 0x00435F9D
		public RedefinedElementException(string _msg, int _line)
			: base(_msg, _line)
		{
		}
	}
}
