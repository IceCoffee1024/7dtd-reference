using System;

namespace XMLData.Parsers
{
	// Token: 0x02001683 RID: 5763
	public static class EnumParser
	{
		// Token: 0x0600B4C1 RID: 46273 RVA: 0x003E6C73 File Offset: 0x003E4E73
		public static TEnum Parse<TEnum>(string _value) where TEnum : struct, IConvertible
		{
			return EnumUtils.Parse<TEnum>(_value, false);
		}

		// Token: 0x0600B4C2 RID: 46274 RVA: 0x0043A0DB File Offset: 0x004382DB
		public static string Unparse<TEnum>(TEnum _value) where TEnum : struct, IConvertible
		{
			return _value.ToStringCached<TEnum>();
		}
	}
}
