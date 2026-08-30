using System;
using System.Reflection;
using MonoMod.Utils;

namespace HarmonyLib.Public.Patching
{
	// Token: 0x0200005C RID: 92
	public abstract class MethodPatcher
	{
		// Token: 0x06000407 RID: 1031 RVA: 0x00012E8F File Offset: 0x0001108F
		protected MethodPatcher(MethodBase original)
		{
			this.Original = original;
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x00012E9E File Offset: 0x0001109E
		public MethodBase Original { get; }

		// Token: 0x06000409 RID: 1033
		public abstract DynamicMethodDefinition PrepareOriginal();

		// Token: 0x0600040A RID: 1034
		public abstract MethodBase DetourTo(MethodBase replacement);

		// Token: 0x0600040B RID: 1035
		public abstract DynamicMethodDefinition CopyOriginal();
	}
}
