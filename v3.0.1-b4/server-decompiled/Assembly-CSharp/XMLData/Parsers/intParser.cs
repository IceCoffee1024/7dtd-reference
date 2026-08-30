using System;

namespace XMLData.Parsers
{
	// Token: 0x02001639 RID: 5689
	public static class intParser
	{
		// Token: 0x0600B2DE RID: 45790 RVA: 0x0042ECFB File Offset: 0x0042CEFB
		public static int Parse(string _value)
		{
			return int.Parse(_value);
		}

		// Token: 0x0600B2DF RID: 45791 RVA: 0x003B9A0B File Offset: 0x003B7C0B
		public static string Unparse(int _value)
		{
			return _value.ToString();
		}
	}
}
