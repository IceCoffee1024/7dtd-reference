using System;

namespace GearVariants
{
	// Token: 0x02001744 RID: 5956
	public static class GearFitNaming
	{
		// Token: 0x0600B998 RID: 47512 RVA: 0x00446DA4 File Offset: 0x00444FA4
		public static string MeshName(string part, string variant)
		{
			if (!string.IsNullOrEmpty(variant))
			{
				return part + "_" + variant;
			}
			return part;
		}

		// Token: 0x0600B999 RID: 47513 RVA: 0x00446DBC File Offset: 0x00444FBC
		public static string VariantOf(string childName, string part)
		{
			if (string.IsNullOrEmpty(childName) || string.IsNullOrEmpty(part))
			{
				return null;
			}
			if (string.Equals(childName, part, StringComparison.Ordinal))
			{
				return "";
			}
			string text = part + "_";
			if (!childName.StartsWith(text, StringComparison.Ordinal))
			{
				return null;
			}
			int length = text.Length;
			string text2 = childName.Substring(length, childName.Length - length);
			if (!string.IsNullOrEmpty(text2))
			{
				return text2;
			}
			return null;
		}

		// Token: 0x04008A88 RID: 35464
		public const string BodyChild = "body";

		// Token: 0x04008A89 RID: 35465
		public const string HeadChild = "head";

		// Token: 0x04008A8A RID: 35466
		public const string HandsChild = "hands";

		// Token: 0x04008A8B RID: 35467
		public const string FeetChild = "feet";
	}
}
