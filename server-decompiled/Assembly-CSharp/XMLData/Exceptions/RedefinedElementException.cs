using System;

namespace XMLData.Exceptions
{
	// Token: 0x0200168F RID: 5775
	public class RedefinedElementException : XmlParserException
	{
		// Token: 0x0600B4D7 RID: 46295 RVA: 0x0043A1A1 File Offset: 0x004383A1
		public RedefinedElementException(string _msg, int _line)
			: base(_msg, _line)
		{
		}
	}
}
