using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib.Internal.Util;
using HarmonyLib.Public.Patching;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace HarmonyLib.Internal.RuntimeFixes
{
	// Token: 0x0200005F RID: 95
	internal static class StackTraceFixes
	{
		// Token: 0x0600041F RID: 1055 RVA: 0x00013808 File Offset: 0x00011A08
		public static void Install()
		{
			if (StackTraceFixes._applied)
			{
				return;
			}
			Action<ILHookInfo> action;
			if ((action = StackTraceFixes.<>O.<0>__OnILChainRefresh) == null)
			{
				action = (StackTraceFixes.<>O.<0>__OnILChainRefresh = new Action<ILHookInfo>(StackTraceFixes.OnILChainRefresh));
			}
			DetourManager.ILHookApplied += action;
			Action<ILHookInfo> action2;
			if ((action2 = StackTraceFixes.<>O.<0>__OnILChainRefresh) == null)
			{
				action2 = (StackTraceFixes.<>O.<0>__OnILChainRefresh = new Action<ILHookInfo>(StackTraceFixes.OnILChainRefresh));
			}
			DetourManager.ILHookUndone += action2;
			StackTraceFixes._applied = true;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00013863 File Offset: 0x00011A63
		private static void OnILChainRefresh(ILHookInfo self)
		{
			PatchManager.AddReplacementOriginal(self.Method.Method, self.Method.GetEndOfChain());
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00013880 File Offset: 0x00011A80
		private static Assembly GetExecutingAssemblyReplacement()
		{
			StackFrame[] frames = new StackTrace().GetFrames();
			StackFrame stackFrame = ((frames != null) ? frames.Skip<StackFrame>(1).FirstOrDefault<StackFrame>() : null);
			if (stackFrame != null)
			{
				MethodBase methodFromStackframe = Harmony.GetMethodFromStackframe(stackFrame);
				if (methodFromStackframe != null)
				{
					return methodFromStackframe.Module.Assembly;
				}
			}
			return Assembly.GetExecutingAssembly();
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x000138C9 File Offset: 0x00011AC9
		private static MethodBase GetMethodReplacement(StackFrame self)
		{
			return Harmony.GetMethodFromStackframe(self) ?? self.GetMethod();
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x000138DC File Offset: 0x00011ADC
		internal static void FixStackTrace(ILContext il)
		{
			MethodReference methodReference = null;
			MethodReference methodReference2 = null;
			ILCursor ilcursor = new ILCursor(il);
			foreach (Instruction instruction in ilcursor.Instrs)
			{
				if (instruction.MatchCall(StackTraceFixes.GetExecutingAssembly_MethodInfo))
				{
					if (methodReference == null)
					{
						methodReference = il.Import(StackTraceFixes.GetExecutingAssemblyReplacement_MethodInfo);
					}
					instruction.Operand = methodReference;
				}
				else if (instruction.MatchCallvirt(StackTraceFixes.GetMethod_MethodInfo))
				{
					if (methodReference2 == null)
					{
						methodReference2 = il.Import(StackTraceFixes.GetMethodReplacement_MethodInfo);
					}
					instruction.OpCode = OpCodes.Call;
					instruction.Operand = methodReference2;
				}
			}
		}

		// Token: 0x04000106 RID: 262
		private static bool _applied;

		// Token: 0x04000107 RID: 263
		private static readonly MethodInfo GetExecutingAssembly_MethodInfo = SymbolExtensions.GetMethodInfo(() => Assembly.GetExecutingAssembly());

		// Token: 0x04000108 RID: 264
		private static readonly MethodInfo GetExecutingAssemblyReplacement_MethodInfo = SymbolExtensions.GetMethodInfo(() => StackTraceFixes.GetExecutingAssemblyReplacement());

		// Token: 0x04000109 RID: 265
		private static readonly MethodInfo GetMethod_MethodInfo = AccessTools.DeclaredMethod(typeof(StackFrame), "GetMethod", Type.EmptyTypes, null);

		// Token: 0x0400010A RID: 266
		private static readonly MethodInfo GetMethodReplacement_MethodInfo = SymbolExtensions.GetMethodInfo(() => StackTraceFixes.GetMethodReplacement(null));

		// Token: 0x020001D7 RID: 471
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000284 RID: 644
			[Nullable(new byte[] { 0, 1 })]
			public static Action<ILHookInfo> <0>__OnILChainRefresh;
		}
	}
}
