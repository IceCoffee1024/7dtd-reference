using System;

namespace XMLData.Parsers
{
	// Token: 0x02001655 RID: 5717
	public static class MaterialBlockParser
	{
		// Token: 0x0600B3DD RID: 46045 RVA: 0x00434043 File Offset: 0x00432243
		public static MaterialBlock Parse(string _value)
		{
			return MaterialBlock.materials[_value];
		}

		// Token: 0x0600B3DE RID: 46046 RVA: 0x00434050 File Offset: 0x00432250
		public static string Unparse(MaterialBlock _value)
		{
			return _value.id;
		}
	}
}
