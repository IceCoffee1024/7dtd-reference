using System;
using XMLData.Exceptions;

namespace XMLData.Parsers
{
	// Token: 0x0200164F RID: 5711
	public static class boolParser
	{
		// Token: 0x0600B3D0 RID: 46032 RVA: 0x00433DCB File Offset: 0x00431FCB
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

		// Token: 0x0600B3D1 RID: 46033 RVA: 0x00433E01 File Offset: 0x00432001
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
