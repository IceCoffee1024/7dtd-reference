using System;

namespace XMLData.Exceptions
{
	// Token: 0x0200168C RID: 5772
	public class IncorrectAttributeOccurrenceException : XmlParserException
	{
		// Token: 0x0600B4D3 RID: 46291 RVA: 0x0043A1A1 File Offset: 0x004383A1
		public IncorrectAttributeOccurrenceException(string _msg, int _line)
			: base(_msg, _line)
		{
		}
	}
}
