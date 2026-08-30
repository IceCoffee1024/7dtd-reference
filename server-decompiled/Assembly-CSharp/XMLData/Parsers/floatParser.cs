using System;
using XMLData.Exceptions;

namespace XMLData.Parsers
{
	// Token: 0x02001653 RID: 5715
	public static class floatParser
	{
		// Token: 0x0600B3D9 RID: 46041 RVA: 0x00434004 File Offset: 0x00432204
		public static float Parse(string _value)
		{
			float num;
			if (StringParsers.TryParseFloat(_value, out num))
			{
				return num;
			}
			throw new InvalidValueException("Expected float value, found \"" + _value + "\"", -1);
		}

		// Token: 0x0600B3DA RID: 46042 RVA: 0x00434033 File Offset: 0x00432233
		public static string Unparse(float _value)
		{
			return _value.ToCultureInvariantString();
		}
	}
}
