using System;

namespace HarmonyLib
{
	// Token: 0x02000035 RID: 53
	[AttributeUsage(AttributeTargets.Method)]
	public class HarmonyOptional : HarmonyAttribute
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x000048AF File Offset: 0x00002AAF
		public HarmonyOptional()
		{
			this.info.optional = new bool?(true);
		}
	}
}
