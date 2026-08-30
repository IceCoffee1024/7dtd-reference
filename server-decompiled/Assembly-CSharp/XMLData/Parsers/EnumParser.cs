using System;

namespace XMLData.Parsers
{
	// Token: 0x02001637 RID: 5687
	public static class EnumParser
	{
		// Token: 0x0600B2DA RID: 45786 RVA: 0x003DA6DB File Offset: 0x003D88DB
		public static TEnum Parse<TEnum>(string _value) where TEnum : struct, IConvertible
		{
			return EnumUtils.Parse<TEnum>(_value, false);
		}

		// Token: 0x0600B2DB RID: 45787 RVA: 0x0042ECBB File Offset: 0x0042CEBB
		public static string Unparse<TEnum>(TEnum _value) where TEnum : struct, IConvertible
		{
			return _value.ToStringCached<TEnum>();
		}
	}
}
