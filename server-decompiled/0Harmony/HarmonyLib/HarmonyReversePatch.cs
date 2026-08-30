using System;

namespace HarmonyLib
{
	// Token: 0x02000023 RID: 35
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public class HarmonyReversePatch : HarmonyAttribute
	{
		// Token: 0x0600009B RID: 155 RVA: 0x0000472B File Offset: 0x0000292B
		public HarmonyReversePatch(HarmonyReversePatchType type = HarmonyReversePatchType.Original)
		{
			this.info.reversePatchType = new HarmonyReversePatchType?(type);
		}
	}
}
