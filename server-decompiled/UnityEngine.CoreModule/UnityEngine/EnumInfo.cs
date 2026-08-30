using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000246 RID: 582
	internal class EnumInfo
	{
		// Token: 0x060018C1 RID: 6337 RVA: 0x00029880 File Offset: 0x00027A80
		[UsedByNativeCode]
		internal static EnumInfo CreateEnumInfoFromNativeEnum(string[] names, int[] values, string[] annotations, bool isFlags)
		{
			return new EnumInfo
			{
				names = names,
				values = values,
				annotations = annotations,
				isFlags = isFlags
			};
		}

		// Token: 0x040008C7 RID: 2247
		public string[] names;

		// Token: 0x040008C8 RID: 2248
		public int[] values;

		// Token: 0x040008C9 RID: 2249
		public string[] annotations;

		// Token: 0x040008CA RID: 2250
		public bool isFlags;
	}
}
