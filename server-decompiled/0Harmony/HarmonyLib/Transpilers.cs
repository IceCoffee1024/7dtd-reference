using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib.Tools;
using MonoMod.Utils;
using MonoMod.Utils.Cil;

namespace HarmonyLib
{
	// Token: 0x02000048 RID: 72
	public static class Transpilers
	{
		// Token: 0x06000174 RID: 372 RVA: 0x000081FC File Offset: 0x000063FC
		public static CodeInstruction EmitDelegate<T>(T action) where T : Delegate
		{
			if (action.Method.IsStatic && action.Target == null)
			{
				return new CodeInstruction(OpCodes.Call, action.Method);
			}
			Type[] array = (from x in action.Method.GetParameters()
				select x.ParameterType).ToArray<Type>();
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(action.Method.Name, action.Method.ReturnType, array);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			Type type = action.Target.GetType();
			bool flag;
			if (action.Target != null)
			{
				flag = type.GetFields().Any<FieldInfo>((FieldInfo x) => !x.IsStatic);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				int num = Transpilers.delegateCounter++;
				Transpilers.DelegateCache[num] = action;
				FieldInfo fieldInfo = AccessTools.Field(typeof(Transpilers), "DelegateCache");
				MethodInfo methodInfo = AccessTools.Method(typeof(Dictionary<int, Delegate>), "get_Item", null, null);
				ilgenerator.Emit(OpCodes.Ldsfld, fieldInfo);
				ilgenerator.Emit(OpCodes.Ldc_I4, num);
				ilgenerator.Emit(OpCodes.Callvirt, methodInfo);
			}
			else
			{
				if (action.Target == null)
				{
					ilgenerator.Emit(OpCodes.Ldnull);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Newobj, AccessTools.FirstConstructor(type, (ConstructorInfo x) => x.GetParameters().Length == 0 && !x.IsStatic));
				}
				ilgenerator.Emit(OpCodes.Ldftn, action.Method);
				ilgenerator.Emit(OpCodes.Newobj, AccessTools.Constructor(typeof(T), new Type[]
				{
					typeof(object),
					typeof(IntPtr)
				}, false));
			}
			for (int i = 0; i < array.Length; i++)
			{
				ilgenerator.Emit(OpCodes.Ldarg_S, (short)i);
			}
			ilgenerator.Emit(OpCodes.Callvirt, AccessTools.Method(typeof(T), "Invoke", null, null));
			ilgenerator.Emit(OpCodes.Ret);
			return new CodeInstruction(OpCodes.Call, dynamicMethodDefinition.Generate());
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00008469 File Offset: 0x00006669
		public static IEnumerable<CodeInstruction> MethodReplacer(this IEnumerable<CodeInstruction> instructions, MethodBase from, MethodBase to)
		{
			if (from == null)
			{
				throw new ArgumentException("Unexpected null argument", "from");
			}
			if (to == null)
			{
				throw new ArgumentException("Unexpected null argument", "to");
			}
			foreach (CodeInstruction codeInstruction in instructions)
			{
				MethodBase methodBase = codeInstruction.operand as MethodBase;
				if (methodBase == from)
				{
					codeInstruction.opcode = (to.IsConstructor ? OpCodes.Newobj : OpCodes.Call);
					codeInstruction.operand = to;
				}
				yield return codeInstruction;
			}
			IEnumerator<CodeInstruction> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00008488 File Offset: 0x00006688
		public static IEnumerable<CodeInstruction> Manipulator(this IEnumerable<CodeInstruction> instructions, Func<CodeInstruction, bool> predicate, Action<CodeInstruction> action)
		{
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			return instructions.Select<CodeInstruction, CodeInstruction>(delegate(CodeInstruction instruction)
			{
				if (predicate(instruction))
				{
					action(instruction);
				}
				return instruction;
			}).AsEnumerable<CodeInstruction>();
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000084E6 File Offset: 0x000066E6
		public static IEnumerable<CodeInstruction> DebugLogger(this IEnumerable<CodeInstruction> instructions, string text)
		{
			yield return new CodeInstruction(OpCodes.Ldc_I4, Logger.LogChannel.Debug);
			yield return new CodeInstruction(OpCodes.Ldstr, text);
			yield return new CodeInstruction(OpCodes.Ldc_I4_1, null);
			yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Logger), "LogText", null, null));
			foreach (CodeInstruction codeInstruction in instructions)
			{
				yield return codeInstruction;
			}
			IEnumerator<CodeInstruction> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00008500 File Offset: 0x00006700
		public static IEnumerable<CodeInstruction> ReplaceWith(MethodBase replacement, ILGenerator ilGenerator)
		{
			if (replacement.GetMethodBody() == null)
			{
				throw new ArgumentException("Replacement method must be a managed method", "replacement");
			}
			CecilILGenerator value = Traverse.Create(ilGenerator).Field("Target").GetValue<CecilILGenerator>();
			value.IL.Body.Variables.Clear();
			return PatchProcessor.GetOriginalInstructions(replacement, ilGenerator);
		}

		// Token: 0x040000A4 RID: 164
		private static readonly Dictionary<int, Delegate> DelegateCache = new Dictionary<int, Delegate>();

		// Token: 0x040000A5 RID: 165
		private static int delegateCounter;
	}
}
