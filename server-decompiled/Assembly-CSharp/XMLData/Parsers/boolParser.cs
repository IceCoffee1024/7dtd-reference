using System;
using XMLData.Exceptions;

namespace XMLData.Parsers
{
	// Token: 0x02001680 RID: 5760
	public static class boolParser
	{
		// Token: 0x0600B4BA RID: 46266 RVA: 0x00439EAB File Offset: 0x004380AB
		public static bool Parse(string _value)
		{
			if (_value == "true")
			{
				return true;
			}
			if (_value == "false")
			{
				return false;
			}
			throw new InvalidValueException("Expected bool value, found \"" + _value + "\"", -1);
		}

		// Token: 0x0600B4BB RID: 46267 RVA: 0x00439EE1 File Offset: 0x004380E1
		public static string Unparse(bool _value)
		{
			if (!_value)
			{
				return "false";
			}
			return "true";
		}
	}
}
