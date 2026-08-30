using System;

namespace HarmonyLib
{
	// Token: 0x02000025 RID: 37
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyPriority : HarmonyAttribute
	{
		// Token: 0x0600009D RID: 157 RVA: 0x0000474C File Offset: 0x0000294C
		public HarmonyPriority(int priority)
		{
			this.info.priority = priority;
		}
	}
}
