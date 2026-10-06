using System;

namespace XMLData.Parsers
{
	// Token: 0x02001671 RID: 5745
	public static class EnumParser
	{
		// Token: 0x0600B469 RID: 46185 RVA: 0x003E486F File Offset: 0x003E2A6F
		public static TEnum Parse<TEnum>(string _value) where TEnum : struct, IConvertible
		{
			return EnumUtils.Parse<TEnum>(_value, false);
		}

		// Token: 0x0600B46A RID: 46186 RVA: 0x00437CD7 File Offset: 0x00435ED7
		public static string Unparse<TEnum>(TEnum _value) where TEnum : struct, IConvertible
		{
			return _value.ToStringCached<TEnum>();
		}
	}
}
