using System;

namespace HarmonyLib
{
	// Token: 0x02000026 RID: 38
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyBefore : HarmonyAttribute
	{
		// Token: 0x0600009E RID: 158 RVA: 0x00004760 File Offset: 0x00002960
		public HarmonyBefore(params string[] before)
		{
			this.info.before = before;
		}
	}
}
