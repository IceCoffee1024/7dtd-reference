using System;

namespace HarmonyLib
{
	// Token: 0x02000027 RID: 39
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyAfter : HarmonyAttribute
	{
		// Token: 0x0600009F RID: 159 RVA: 0x00004774 File Offset: 0x00002974
		public HarmonyAfter(params string[] after)
		{
			this.info.after = after;
		}
	}
}
