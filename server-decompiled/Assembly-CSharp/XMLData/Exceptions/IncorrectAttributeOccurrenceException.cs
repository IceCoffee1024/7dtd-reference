using System;

namespace XMLData.Exceptions
{
	// Token: 0x0200167A RID: 5754
	public class IncorrectAttributeOccurrenceException : XmlParserException
	{
		// Token: 0x0600B47B RID: 46203 RVA: 0x00437D9D File Offset: 0x00435F9D
		public IncorrectAttributeOccurrenceException(string _msg, int _line)
			: base(_msg, _line)
		{
		}
	}
}
