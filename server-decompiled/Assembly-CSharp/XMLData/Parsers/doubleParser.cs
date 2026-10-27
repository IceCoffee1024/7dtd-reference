using System;
using XMLData.Exceptions;

namespace XMLData.Parsers
{
	// Token: 0x02001682 RID: 5762
	public static class doubleParser
	{
		// Token: 0x0600B4BF RID: 46271 RVA: 0x0043A0A4 File Offset: 0x004382A4
		public static double Parse(string _value)
		{
			double num;
			if (StringParsers.TryParseDouble(_value, out num))
			{
				return num;
			}
			throw new InvalidValueException("Expected double value, found \"" + _value + "\"", -1);
		}

		// Token: 0x0600B4C0 RID: 46272 RVA: 0x0043A0D3 File Offset: 0x004382D3
		public static string Unparse(double _value)
		{
			return _value.ToCultureInvariantString();
		}
	}
}
