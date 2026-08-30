using System;
using XMLData.Exceptions;

namespace XMLData.Parsers
{
	// Token: 0x02001636 RID: 5686
	public static class doubleParser
	{
		// Token: 0x0600B2D8 RID: 45784 RVA: 0x0042EC84 File Offset: 0x0042CE84
		public static double Parse(string _value)
		{
			double num;
			if (StringParsers.TryParseDouble(_value, out num))
			{
				return num;
			}
			throw new InvalidValueException("Expected double value, found \"" + _value + "\"", -1);
		}

		// Token: 0x0600B2D9 RID: 45785 RVA: 0x0042ECB3 File Offset: 0x0042CEB3
		public static string Unparse(double _value)
		{
			return _value.ToCultureInvariantString();
		}
	}
}
