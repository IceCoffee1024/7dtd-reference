using System;

namespace XMLData.Parsers
{
	// Token: 0x02001652 RID: 5714
	public static class EnumParser
	{
		// Token: 0x0600B3D7 RID: 46039 RVA: 0x003DFA1B File Offset: 0x003DDC1B
		public static TEnum Parse<TEnum>(string _value) where TEnum : struct, IConvertible
		{
			return EnumUtils.Parse<TEnum>(_value, false);
		}

		// Token: 0x0600B3D8 RID: 46040 RVA: 0x00433FFB File Offset: 0x004321FB
		public static string Unparse<TEnum>(TEnum _value) where TEnum : struct, IConvertible
		{
			return _value.ToStringCached<TEnum>();
		}
	}
}
