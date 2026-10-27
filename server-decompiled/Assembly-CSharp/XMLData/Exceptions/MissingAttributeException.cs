using System;

namespace XMLData.Exceptions
{
	// Token: 0x0200168E RID: 5774
	public class MissingAttributeException : XmlParserException
	{
		// Token: 0x0600B4D6 RID: 46294 RVA: 0x0043A1A1 File Offset: 0x004383A1
		public MissingAttributeException(string _msg, int _line)
			: base(_msg, _line)
		{
		}
	}
}
