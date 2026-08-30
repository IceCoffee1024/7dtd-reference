using System;

namespace HarmonyLib
{
	// Token: 0x02000020 RID: 32
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class HarmonyPatchCategory : HarmonyAttribute
	{
		// Token: 0x06000071 RID: 113 RVA: 0x00004245 File Offset: 0x00002445
		public HarmonyPatchCategory(string category)
		{
			this.info.category = category;
		}
	}
}
