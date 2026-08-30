using System;

namespace HarmonyLib
{
	// Token: 0x02000028 RID: 40
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyDebug : HarmonyAttribute
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00004788 File Offset: 0x00002988
		public HarmonyDebug()
		{
			this.info.debug = new bool?(true);
		}
	}
}
