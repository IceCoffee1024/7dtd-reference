using System;
using XMLData.Exceptions;

namespace XMLData.Parsers
{
	// Token: 0x02001672 RID: 5746
	public static class floatParser
	{
		// Token: 0x0600B46B RID: 46187 RVA: 0x00437CE0 File Offset: 0x00435EE0
		public static float Parse(string _value)
		{
			float num;
			if (StringParsers.TryParseFloat(_value, out num))
			{
				return num;
			}
			throw new InvalidValueException("Expected float value, found \"" + _value + "\"", -1);
		}

		// Token: 0x0600B46C RID: 46188 RVA: 0x00437D0F File Offset: 0x00435F0F
		public static string Unparse(float _value)
		{
			return _value.ToCultureInvariantString();
		}
	}
}
