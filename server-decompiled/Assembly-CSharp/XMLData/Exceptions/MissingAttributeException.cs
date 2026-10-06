using System;

namespace XMLData.Exceptions
{
	// Token: 0x0200167C RID: 5756
	public class MissingAttributeException : XmlParserException
	{
		// Token: 0x0600B47E RID: 46206 RVA: 0x00437D9D File Offset: 0x00435F9D
		public MissingAttributeException(string _msg, int _line)
			: base(_msg, _line)
		{
		}
	}
}
