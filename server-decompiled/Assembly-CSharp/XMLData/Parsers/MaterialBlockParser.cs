using System;

namespace XMLData.Parsers
{
	// Token: 0x02001686 RID: 5766
	public static class MaterialBlockParser
	{
		// Token: 0x0600B4C7 RID: 46279 RVA: 0x0043A123 File Offset: 0x00438323
		public static MaterialBlock Parse(string _value)
		{
			return MaterialBlock.materials[_value];
		}

		// Token: 0x0600B4C8 RID: 46280 RVA: 0x0043A130 File Offset: 0x00438330
		public static string Unparse(MaterialBlock _value)
		{
			return _value.id;
		}
	}
}
