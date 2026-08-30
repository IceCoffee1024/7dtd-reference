using System;
using XMLData.Exceptions;

namespace XMLData.Parsers
{
	// Token: 0x02001634 RID: 5684
	public static class boolParser
	{
		// Token: 0x0600B2D3 RID: 45779 RVA: 0x0042EA8B File Offset: 0x0042CC8B
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

		// Token: 0x0600B2D4 RID: 45780 RVA: 0x0042EAC1 File Offset: 0x0042CCC1
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
