using System;

namespace XMLData.Parsers
{
	// Token: 0x02001685 RID: 5765
	public static class intParser
	{
		// Token: 0x0600B4C5 RID: 46277 RVA: 0x0043A11B File Offset: 0x0043831B
		public static int Parse(string _value)
		{
			return int.Parse(_value);
		}

		// Token: 0x0600B4C6 RID: 46278 RVA: 0x003C5CEF File Offset: 0x003C3EEF
		public static string Unparse(int _value)
		{
			return _value.ToString();
		}
	}
}
