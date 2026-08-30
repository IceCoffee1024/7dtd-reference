using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonoMod.Cil;

namespace HarmonyLib
{
	// Token: 0x02000047 RID: 71
	public class ReversePatcher
	{
		// Token: 0x06000170 RID: 368 RVA: 0x000080C7 File Offset: 0x000062C7
		public ReversePatcher(Harmony instance, MethodBase original, HarmonyMethod standin)
		{
		}

		// Token: 0x06000171 RID: 369 RVA: 0x000080E4 File Offset: 0x000062E4
		public MethodInfo Patch(HarmonyReversePatchType type = HarmonyReversePatchType.Original)
		{
			if (this.original == null)
			{
				throw new NullReferenceException("Null method for " + this.instance.Id);
			}
			this.standin.reversePatchType = new HarmonyReversePatchType?(type);
			MethodInfo transpiler = ReversePatcher.GetTranspiler(this.standin.method);
			MethodInfo manipulator = ReversePatcher.GetManipulator(this.standin.method);
			return PatchFunctions.ReversePatch(this.standin, this.original, transpiler, manipulator);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000815C File Offset: 0x0000635C
		internal static MethodInfo GetTranspiler(MethodInfo method)
		{
			string methodName = method.Name;
			Type declaringType = method.DeclaringType;
			List<MethodInfo> declaredMethods = AccessTools.GetDeclaredMethods(declaringType);
			Type ici = typeof(IEnumerable<CodeInstruction>);
			return declaredMethods.FirstOrDefault<MethodInfo>((MethodInfo m) => !(m.ReturnType != ici) && m.Name.StartsWith("<" + methodName + ">"));
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000081AC File Offset: 0x000063AC
		internal static MethodInfo GetManipulator(MethodInfo method)
		{
			string methodName = method.Name;
			Type declaringType = method.DeclaringType;
			List<MethodInfo> declaredMethods = AccessTools.GetDeclaredMethods(declaringType);
			Type ctxType = typeof(ILContext);
			return declaredMethods.FirstOrDefault<MethodInfo>((MethodInfo m) => (from p in m.GetParameters()
				select p.ParameterType).Contains(ctxType) && m.Name.StartsWith("<" + methodName + ">"));
		}

		// Token: 0x040000A1 RID: 161
		private readonly Harmony instance = instance;

		// Token: 0x040000A2 RID: 162
		private readonly MethodBase original = original;

		// Token: 0x040000A3 RID: 163
		private readonly HarmonyMethod standin = standin;
	}
}
