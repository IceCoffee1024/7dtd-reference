using System;
using XMLData.Exceptions;

namespace XMLData.Parsers
{
	// Token: 0x02001638 RID: 5688
	public static class floatParser
	{
		// Token: 0x0600B2DC RID: 45788 RVA: 0x0042ECC4 File Offset: 0x0042CEC4
		public static float Parse(string _value)
		{
			float num;
			if (StringParsers.TryParseFloat(_value, out num))
			{
				return num;
			}
			throw new InvalidValueException("Expected float value, found \"" + _value + "\"", -1);
		}

		// Token: 0x0600B2DD RID: 45789 RVA: 0x0042ECF3 File Offset: 0x0042CEF3
		public static string Unparse(float _value)
		{
			return _value.ToCultureInvariantString();
		}
	}
}
