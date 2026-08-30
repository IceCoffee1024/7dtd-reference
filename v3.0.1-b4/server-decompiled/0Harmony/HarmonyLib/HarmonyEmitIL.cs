using System;

namespace HarmonyLib
{
	// Token: 0x02000029 RID: 41
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyEmitIL : HarmonyAttribute
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x000047A1 File Offset: 0x000029A1
		public HarmonyEmitIL()
		{
			this.info.debugEmitPath = "./";
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000047B9 File Offset: 0x000029B9
		public HarmonyEmitIL(string dir)
		{
			this.info.debugEmitPath = dir;
		}
	}
}
