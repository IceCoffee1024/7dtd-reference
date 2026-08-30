using System;

namespace XMLData.Parsers
{
	// Token: 0x02001654 RID: 5716
	public static class intParser
	{
		// Token: 0x0600B3DB RID: 46043 RVA: 0x0043403B File Offset: 0x0043223B
		public static int Parse(string _value)
		{
			return int.Parse(_value);
		}

		// Token: 0x0600B3DC RID: 46044 RVA: 0x003BED4F File Offset: 0x003BCF4F
		public static string Unparse(int _value)
		{
			return _value.ToString();
		}
	}
}
