using System;
using XMLData.Exceptions;

namespace XMLData.Parsers
{
	// Token: 0x02001651 RID: 5713
	public static class doubleParser
	{
		// Token: 0x0600B3D5 RID: 46037 RVA: 0x00433FC4 File Offset: 0x004321C4
		public static double Parse(string _value)
		{
			double num;
			if (StringParsers.TryParseDouble(_value, out num))
			{
				return num;
			}
			throw new InvalidValueException("Expected double value, found \"" + _value + "\"", -1);
		}

		// Token: 0x0600B3D6 RID: 46038 RVA: 0x00433FF3 File Offset: 0x004321F3
		public static string Unparse(double _value)
		{
			return _value.ToCultureInvariantString();
		}
	}
}
