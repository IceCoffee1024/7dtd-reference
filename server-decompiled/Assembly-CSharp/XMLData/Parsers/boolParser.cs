using System;
using XMLData.Exceptions;

namespace XMLData.Parsers
{
	// Token: 0x0200166E RID: 5742
	public static class boolParser
	{
		// Token: 0x0600B462 RID: 46178 RVA: 0x00437AA7 File Offset: 0x00435CA7
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

		// Token: 0x0600B463 RID: 46179 RVA: 0x00437ADD File Offset: 0x00435CDD
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
