using System;

namespace XMLData.Parsers
{
	// Token: 0x0200163A RID: 5690
	public static class MaterialBlockParser
	{
		// Token: 0x0600B2E0 RID: 45792 RVA: 0x0042ED03 File Offset: 0x0042CF03
		public static MaterialBlock Parse(string _value)
		{
			return MaterialBlock.materials[_value];
		}

		// Token: 0x0600B2E1 RID: 45793 RVA: 0x0042ED10 File Offset: 0x0042CF10
		public static string Unparse(MaterialBlock _value)
		{
			return _value.id;
		}
	}
}
