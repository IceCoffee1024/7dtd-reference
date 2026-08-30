using System;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000039 RID: 57
	public class InvalidHarmonyPatchArgumentException : Exception
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000CE RID: 206 RVA: 0x000051C2 File Offset: 0x000033C2
		public MethodBase Original { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000CF RID: 207 RVA: 0x000051CA File Offset: 0x000033CA
		public MethodInfo Patch { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x000051D2 File Offset: 0x000033D2
		public override string Message
		{
			get
			{
				return "(" + this.Patch.FullDescription() + "): " + base.Message;
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000051F4 File Offset: 0x000033F4
		public InvalidHarmonyPatchArgumentException(string message, MethodBase original, MethodInfo patch)
			: base(message)
		{
			this.Original = original;
			this.Patch = patch;
		}
	}
}
