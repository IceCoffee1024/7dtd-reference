using System;

namespace XMLData.Parsers
{
	// Token: 0x02001674 RID: 5748
	public static class MaterialBlockParser
	{
		// Token: 0x0600B46F RID: 46191 RVA: 0x00437D1F File Offset: 0x00435F1F
		public static MaterialBlock Parse(string _value)
		{
			return MaterialBlock.materials[_value];
		}

		// Token: 0x0600B470 RID: 46192 RVA: 0x00437D2C File Offset: 0x00435F2C
		public static string Unparse(MaterialBlock _value)
		{
			return _value.id;
		}
	}
}
