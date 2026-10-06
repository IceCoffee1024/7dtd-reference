using System;

namespace XMLData.Parsers
{
	// Token: 0x02001673 RID: 5747
	public static class intParser
	{
		// Token: 0x0600B46D RID: 46189 RVA: 0x00437D17 File Offset: 0x00435F17
		public static int Parse(string _value)
		{
			return int.Parse(_value);
		}

		// Token: 0x0600B46E RID: 46190 RVA: 0x003C38EB File Offset: 0x003C1AEB
		public static string Unparse(int _value)
		{
			return _value.ToString();
		}
	}
}
