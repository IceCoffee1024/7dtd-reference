using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using HarmonyLib.Internal.Patching;
using HarmonyLib.Internal.RuntimeFixes;
using HarmonyLib.Internal.Util;
using HarmonyLib.Public.Patching;
using HarmonyLib.Tools;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x02000015 RID: 21
	internal static class PatchFunctions
	{
		// Token: 0x06000050 RID: 80 RVA: 0x00003187 File Offset: 0x00001387
		internal static List<MethodInfo> GetSortedPatchMethods(MethodBase original, Patch[] patches, bool debug)
		{
			return new PatchSorter(patches, debug).Sort(original);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003196 File Offset: 0x00001396
		internal static Patch[] GetSortedPatchMethodsAsPatches(MethodBase original, Patch[] patches, bool debug)
		{
			return new PatchSorter(patches, debug).SortAsPatches(original);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000031A8 File Offset: 0x000013A8
		internal static MethodInfo UpdateWrapper(MethodBase original, PatchInfo patchInfo)
		{
			MethodPatcher methodPatcher = original.GetMethodPatcher();
			DynamicMethodDefinition dynamicMethodDefinition = methodPatcher.PrepareOriginal();
			if (dynamicMethodDefinition != null)
			{
				ILContext ilcontext = new ILContext(dynamicMethodDefinition.Definition);
				HarmonyManipulator.Manipulate(original, patchInfo, ilcontext);
			}
			MethodInfo methodInfo;
			try
			{
				methodInfo = methodPatcher.DetourTo((dynamicMethodDefinition != null) ? dynamicMethodDefinition.Generate() : null) as MethodInfo;
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				Mono.Cecil.Cil.MethodBody methodBody;
				if (dynamicMethodDefinition == null)
				{
					methodBody = null;
				}
				else
				{
					MethodDefinition definition = dynamicMethodDefinition.Definition;
					methodBody = ((definition != null) ? definition.Body : null);
				}
				throw HarmonyException.Create(ex2, methodBody);
			}
			return methodInfo;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003228 File Offset: 0x00001428
		internal static MethodInfo ReversePatch(HarmonyMethod standin, MethodBase original, MethodInfo postTranspiler, MethodInfo postManipulator)
		{
			if (standin == null)
			{
				throw new ArgumentNullException("standin");
			}
			if (standin.method == null)
			{
				throw new ArgumentNullException("standin", "standin.method is NULL");
			}
			if (!standin.method.IsStatic)
			{
				throw new ArgumentException("standin", "standin.method is not static");
			}
			bool debug = standin.debug.GetValueOrDefault();
			List<MethodInfo> transpilers = new List<MethodInfo>();
			List<MethodInfo> ilmanipulators = new List<MethodInfo>();
			if (standin.reversePatchType.GetValueOrDefault() == HarmonyReversePatchType.Snapshot)
			{
				Patches patchInfo = Harmony.GetPatchInfo(original);
				transpilers.AddRange(PatchFunctions.GetSortedPatchMethods(original, patchInfo.Transpilers.ToArray<Patch>(), debug));
				ilmanipulators.AddRange(PatchFunctions.GetSortedPatchMethods(original, patchInfo.ILManipulators.ToArray<Patch>(), debug));
			}
			if (postTranspiler != null)
			{
				transpilers.Add(postTranspiler);
			}
			if (postManipulator != null)
			{
				ilmanipulators.Add(postManipulator);
			}
			Logger.Log(Logger.LogChannel.Info, delegate
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Reverse patching " + standin.method.FullDescription() + " with " + original.FullDescription());
				PatchFunctions.<ReversePatch>g__PrintInfo|3_2(stringBuilder, transpilers, "Transpiler");
				PatchFunctions.<ReversePatch>g__PrintInfo|3_2(stringBuilder, ilmanipulators, "Manipulators");
				return stringBuilder.ToString();
			}, debug);
			Mono.Cecil.Cil.MethodBody patchBody = null;
			ILHook ilhook = new ILHook(standin.method, delegate(ILContext ctx)
			{
				MethodInfo methodInfo2 = original as MethodInfo;
				if (methodInfo2 == null)
				{
					return;
				}
				patchBody = ctx.Body;
				MethodPatcher methodPatcher = methodInfo2.GetMethodPatcher();
				DynamicMethodDefinition dynamicMethodDefinition = methodPatcher.CopyOriginal();
				if (dynamicMethodDefinition == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(72, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Cannot reverse patch ");
					defaultInterpolatedStringHandler.AppendFormatted(methodInfo2.FullDescription());
					defaultInterpolatedStringHandler.AppendLiteral(": method patcher (");
					defaultInterpolatedStringHandler.AppendFormatted(methodPatcher.GetType().FullDescription());
					defaultInterpolatedStringHandler.AppendLiteral(") can't copy original method body");
					throw new NullReferenceException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ILManipulator ilmanipulator = new ILManipulator(dynamicMethodDefinition.Definition.Body, debug);
				ctx.Body.Variables.Clear();
				foreach (VariableDefinition variableDefinition in ilmanipulator.Body.Variables)
				{
					ctx.Body.Variables.Add(new VariableDefinition(ctx.Module.ImportReference(variableDefinition.VariableType)));
				}
				foreach (MethodInfo methodInfo3 in transpilers)
				{
					ilmanipulator.AddTranspiler(methodInfo3);
				}
				ilmanipulator.WriteTo(ctx.Body, standin.method);
				HarmonyManipulator.ApplyManipulators(ctx, original, ilmanipulators, null);
				StackTraceFixes.FixStackTrace(ctx);
				Instruction instruction = null;
				foreach (Instruction instruction2 in ctx.Instrs.Where<Instruction>((Instruction i) => i.OpCode == Mono.Cecil.Cil.OpCodes.Ret))
				{
					if (instruction == null)
					{
						instruction = ctx.IL.Create(Mono.Cecil.Cil.OpCodes.Ret);
					}
					instruction2.OpCode = Mono.Cecil.Cil.OpCodes.Br;
					instruction2.Operand = instruction;
				}
				if (instruction != null)
				{
					ctx.IL.Append(instruction);
				}
				Logger.Log(Logger.LogChannel.IL, () => "Generated reverse patcher (" + ctx.Method.FullName + "):\n" + ctx.Body.ToILDasmString(), debug);
			}, false);
			try
			{
				ilhook.Apply();
			}
			catch (Exception ex)
			{
				throw HarmonyException.Create(ex, patchBody);
			}
			MethodInfo methodInfo = ilhook.Method as MethodInfo;
			PatchTools.RememberObject(standin.method, new ValueTuple<MethodInfo, ILHook>(methodInfo, ilhook));
			return methodInfo;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000033E0 File Offset: 0x000015E0
		internal static IEnumerable<CodeInstruction> ApplyTranspilers(MethodBase methodBase, ILGenerator generator, int maxTranspilers = 0)
		{
			MethodPatcher methodPatcher = methodBase.GetMethodPatcher();
			DynamicMethodDefinition dynamicMethodDefinition = methodPatcher.CopyOriginal();
			if (dynamicMethodDefinition == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(72, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot reverse patch ");
				defaultInterpolatedStringHandler.AppendFormatted(methodBase.FullDescription());
				defaultInterpolatedStringHandler.AppendLiteral(": method patcher (");
				defaultInterpolatedStringHandler.AppendFormatted(methodPatcher.GetType().FullDescription());
				defaultInterpolatedStringHandler.AppendLiteral(") can't copy original method body");
				throw new NullReferenceException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			ILManipulator ilmanipulator = new ILManipulator(dynamicMethodDefinition.Definition.Body, false);
			PatchInfo patchInfo = methodBase.GetPatchInfo();
			if (patchInfo != null)
			{
				List<MethodInfo> sortedPatchMethods = PatchFunctions.GetSortedPatchMethods(methodBase, patchInfo.transpilers, false);
				int num = 0;
				while (num < maxTranspilers && num < sortedPatchMethods.Count)
				{
					ilmanipulator.AddTranspiler(sortedPatchMethods[num]);
					num++;
				}
			}
			return ilmanipulator.GetInstructions(generator, methodBase);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000034B8 File Offset: 0x000016B8
		internal static void UnpatchConditional(Func<Patch, bool> executionCondition)
		{
			List<MethodBase> list = PatchProcessor.GetAllPatchedMethods().ToList<MethodBase>();
			foreach (MethodBase methodBase in list)
			{
				Patches patchInfo2 = PatchProcessor.GetPatchInfo(methodBase);
				PatchProcessor patchProcessor = new PatchProcessor(null, methodBase);
				patchInfo2.Postfixes.DoIf<Patch>(executionCondition, delegate(Patch patchInfo)
				{
					patchProcessor.Unpatch(patchInfo.PatchMethod);
				});
				patchInfo2.Prefixes.DoIf<Patch>(executionCondition, delegate(Patch patchInfo)
				{
					patchProcessor.Unpatch(patchInfo.PatchMethod);
				});
				patchInfo2.ILManipulators.DoIf<Patch>(executionCondition, delegate(Patch patchInfo)
				{
					patchProcessor.Unpatch(patchInfo.PatchMethod);
				});
				patchInfo2.Transpilers.DoIf<Patch>(executionCondition, delegate(Patch patchInfo)
				{
					patchProcessor.Unpatch(patchInfo.PatchMethod);
				});
				patchInfo2.Finalizers.DoIf<Patch>(executionCondition, delegate(Patch patchInfo)
				{
					patchProcessor.Unpatch(patchInfo.PatchMethod);
				});
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000035A8 File Offset: 0x000017A8
		[CompilerGenerated]
		internal static void <ReversePatch>g__PrintInfo|3_2(StringBuilder sb, ICollection<MethodInfo> methods, string name)
		{
			if (methods.Count <= 0)
			{
				return;
			}
			sb.AppendLine(name + ":");
			foreach (MethodInfo methodInfo in methods)
			{
				sb.AppendLine("  * " + methodInfo.FullDescription());
			}
		}
	}
}
