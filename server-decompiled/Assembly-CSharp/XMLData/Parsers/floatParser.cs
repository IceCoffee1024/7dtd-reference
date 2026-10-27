using System;
using XMLData.Exceptions;

namespace XMLData.Parsers
{
	// Token: 0x02001684 RID: 5764
	public static class floatParser
	{
		// Token: 0x0600B4C3 RID: 46275 RVA: 0x0043A0E4 File Offset: 0x004382E4
		public static float Parse(string _value)
		{
			float num;
			if (StringParsers.TryParseFloat(_value, out num))
			{
				return num;
			}
			throw new InvalidValueException("Expected float value, found \"" + _value + "\"", -1);
		}

		// Token: 0x0600B4C4 RID: 46276 RVA: 0x0043A113 File Offset: 0x00438313
		public static string Unparse(float _value)
		{
			return _value.ToCultureInvariantString();
		}
	}
}
