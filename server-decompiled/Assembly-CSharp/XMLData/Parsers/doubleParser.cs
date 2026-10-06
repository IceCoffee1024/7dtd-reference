using System;
using XMLData.Exceptions;

namespace XMLData.Parsers
{
	// Token: 0x02001670 RID: 5744
	public static class doubleParser
	{
		// Token: 0x0600B467 RID: 46183 RVA: 0x00437CA0 File Offset: 0x00435EA0
		public static double Parse(string _value)
		{
			double num;
			if (StringParsers.TryParseDouble(_value, out num))
			{
				return num;
			}
			throw new InvalidValueException("Expected double value, found \"" + _value + "\"", -1);
		}

		// Token: 0x0600B468 RID: 46184 RVA: 0x00437CCF File Offset: 0x00435ECF
		public static string Unparse(double _value)
		{
			return _value.ToCultureInvariantString();
		}
	}
}
