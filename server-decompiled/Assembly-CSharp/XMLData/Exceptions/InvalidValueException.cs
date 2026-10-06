using System;

namespace XMLData.Exceptions
{
	// Token: 0x0200167B RID: 5755
	public class InvalidValueException : XmlParserException
	{
		// Token: 0x0600B47C RID: 46204 RVA: 0x00437D9D File Offset: 0x00435F9D
		public InvalidValueException(string _msg, int _line)
			: base(_msg, _line)
		{
		}

		// Token: 0x0600B47D RID: 46205 RVA: 0x00437DA7 File Offset: 0x00435FA7
		public InvalidValueException(string _msg, int _line, Exception _innerException)
			: base(_msg, _line, _innerException)
		{
		}
	}
}
