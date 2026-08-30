using System;

namespace HarmonyLib
{
	// Token: 0x0200002A RID: 42
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyWrapSafe : HarmonyAttribute
	{
		// Token: 0x060000A3 RID: 163 RVA: 0x000047CD File Offset: 0x000029CD
		public HarmonyWrapSafe()
		{
			this.info.wrapTryCatch = new bool?(true);
		}
	}
}
