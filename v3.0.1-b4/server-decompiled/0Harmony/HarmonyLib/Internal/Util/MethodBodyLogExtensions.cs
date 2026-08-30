using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using MonoMod.Cil;
using MonoMod.Utils;

namespace HarmonyLib.Internal.Util
{
	// Token: 0x02000063 RID: 99
	internal static class MethodBodyLogExtensions
	{
		// Token: 0x06000446 RID: 1094 RVA: 0x00014F4C File Offset: 0x0001314C
		public static string ToILDasmString(this MethodBody mb)
		{
			MethodBodyLogExtensions.<>c__DisplayClass0_0 CS$<>8__locals1;
			CS$<>8__locals1.sb = new StringBuilder();
			Collection<Instruction> instructions = mb.Instructions;
			Instruction instruction = instructions.First<Instruction>();
			instruction.Offset = 0;
			foreach (Instruction instruction2 in instructions.Skip<Instruction>(1))
			{
				object operand = instruction.Operand;
				ILLabel illabel = operand as ILLabel;
				if (illabel != null)
				{
					instruction.Operand = illabel.Target;
				}
				else
				{
					ILLabel[] array = operand as ILLabel[];
					if (array != null)
					{
						instruction.Operand = array.Select<ILLabel, Instruction>((ILLabel l) => l.Target).ToArray<Instruction>();
					}
				}
				instruction2.Offset = instruction.Offset + instruction.GetSize();
				instruction.Operand = operand;
				instruction = instruction2;
			}
			CS$<>8__locals1.exBlocks = new Dictionary<Instruction, List<ExceptionBlock>>();
			foreach (ExceptionHandler exceptionHandler in mb.ExceptionHandlers)
			{
				MethodBodyLogExtensions.<ToILDasmString>g__AddBlock|0_0(exceptionHandler.TryStart, ExceptionBlockType.BeginExceptionBlock, ref CS$<>8__locals1);
				MethodBodyLogExtensions.<ToILDasmString>g__AddBlock|0_0(exceptionHandler.TryEnd, ExceptionBlockType.EndExceptionBlock, ref CS$<>8__locals1);
				MethodBodyLogExtensions.<ToILDasmString>g__AddBlock|0_0(exceptionHandler.HandlerEnd, ExceptionBlockType.EndExceptionBlock, ref CS$<>8__locals1);
				switch (exceptionHandler.HandlerType)
				{
				case ExceptionHandlerType.Catch:
					MethodBodyLogExtensions.<ToILDasmString>g__AddBlock|0_0(exceptionHandler.HandlerStart, ExceptionBlockType.BeginCatchBlock, ref CS$<>8__locals1).catchType = exceptionHandler.CatchType.ResolveReflection();
					continue;
				case ExceptionHandlerType.Filter:
					MethodBodyLogExtensions.<ToILDasmString>g__AddBlock|0_0(exceptionHandler.FilterStart, ExceptionBlockType.BeginExceptFilterBlock, ref CS$<>8__locals1);
					continue;
				case ExceptionHandlerType.Finally:
					MethodBodyLogExtensions.<ToILDasmString>g__AddBlock|0_0(exceptionHandler.HandlerStart, ExceptionBlockType.BeginFinallyBlock, ref CS$<>8__locals1);
					continue;
				case ExceptionHandlerType.Fault:
					MethodBodyLogExtensions.<ToILDasmString>g__AddBlock|0_0(exceptionHandler.HandlerStart, ExceptionBlockType.BeginFaultBlock, ref CS$<>8__locals1);
					continue;
				}
				throw new ArgumentOutOfRangeException();
			}
			CS$<>8__locals1.indent = 0;
			MethodBodyLogExtensions.<ToILDasmString>g__WriteLine|0_1(".locals init (", ref CS$<>8__locals1);
			CS$<>8__locals1.indent += 4;
			for (int i = 0; i < mb.Variables.Count; i++)
			{
				VariableDefinition variableDefinition = mb.Variables[i];
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler.AppendFormatted(variableDefinition.VariableType.FullName);
				defaultInterpolatedStringHandler.AppendLiteral(" V_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				MethodBodyLogExtensions.<ToILDasmString>g__WriteLine|0_1(defaultInterpolatedStringHandler.ToStringAndClear(), ref CS$<>8__locals1);
			}
			CS$<>8__locals1.indent -= 4;
			MethodBodyLogExtensions.<ToILDasmString>g__WriteLine|0_1(")", ref CS$<>8__locals1);
			Stack<string> stack = new Stack<string>();
			foreach (Instruction instruction3 in instructions)
			{
				List<ExceptionBlock> list = CS$<>8__locals1.exBlocks.GetValueSafe(instruction3) ?? new List<ExceptionBlock>();
				list.Sort(delegate(ExceptionBlock a, ExceptionBlock b)
				{
					if (a.blockType != ExceptionBlockType.EndExceptionBlock)
					{
						return 0;
					}
					return -1;
				});
				foreach (ExceptionBlock exceptionBlock in list)
				{
					switch (exceptionBlock.blockType)
					{
					case ExceptionBlockType.BeginExceptionBlock:
						MethodBodyLogExtensions.<ToILDasmString>g__WriteLine|0_1(".try", ref CS$<>8__locals1);
						MethodBodyLogExtensions.<ToILDasmString>g__WriteLine|0_1("{", ref CS$<>8__locals1);
						CS$<>8__locals1.indent += 2;
						stack.Push(".try");
						break;
					case ExceptionBlockType.BeginCatchBlock:
						MethodBodyLogExtensions.<ToILDasmString>g__WriteLine|0_1("catch " + exceptionBlock.catchType.FullName, ref CS$<>8__locals1);
						MethodBodyLogExtensions.<ToILDasmString>g__WriteLine|0_1("{", ref CS$<>8__locals1);
						CS$<>8__locals1.indent += 2;
						stack.Push("handler (catch)");
						break;
					case ExceptionBlockType.BeginExceptFilterBlock:
						MethodBodyLogExtensions.<ToILDasmString>g__WriteLine|0_1("filter", ref CS$<>8__locals1);
						MethodBodyLogExtensions.<ToILDasmString>g__WriteLine|0_1("{", ref CS$<>8__locals1);
						CS$<>8__locals1.indent += 2;
						stack.Push("handler (filter)");
						break;
					case ExceptionBlockType.BeginFaultBlock:
						MethodBodyLogExtensions.<ToILDasmString>g__WriteLine|0_1("fault", ref CS$<>8__locals1);
						MethodBodyLogExtensions.<ToILDasmString>g__WriteLine|0_1("{", ref CS$<>8__locals1);
						CS$<>8__locals1.indent += 2;
						stack.Push("handler (fault)");
						break;
					case ExceptionBlockType.BeginFinallyBlock:
						MethodBodyLogExtensions.<ToILDasmString>g__WriteLine|0_1("finally", ref CS$<>8__locals1);
						MethodBodyLogExtensions.<ToILDasmString>g__WriteLine|0_1("{", ref CS$<>8__locals1);
						CS$<>8__locals1.indent += 2;
						stack.Push("handler (finally)");
						break;
					case ExceptionBlockType.EndExceptionBlock:
						CS$<>8__locals1.indent -= 2;
						MethodBodyLogExtensions.<ToILDasmString>g__WriteLine|0_1("} // end " + stack.Pop(), ref CS$<>8__locals1);
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
				object operand2 = instruction3.Operand;
				ILLabel illabel2 = operand2 as ILLabel;
				if (illabel2 != null)
				{
					instruction3.Operand = illabel2.Target;
				}
				else
				{
					ILLabel[] array2 = operand2 as ILLabel[];
					if (array2 != null)
					{
						instruction3.Operand = array2.Select<ILLabel, Instruction>((ILLabel l) => l.Target).ToArray<Instruction>();
					}
				}
				MethodBodyLogExtensions.<ToILDasmString>g__WriteLine|0_1(instruction3.ToString(), ref CS$<>8__locals1);
				instruction3.Operand = operand2;
			}
			return CS$<>8__locals1.sb.ToString();
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x000154DC File Offset: 0x000136DC
		[CompilerGenerated]
		internal static ExceptionBlock <ToILDasmString>g__AddBlock|0_0(Instruction ins, ExceptionBlockType t, ref MethodBodyLogExtensions.<>c__DisplayClass0_0 A_2)
		{
			if (ins == null)
			{
				return new ExceptionBlock(ExceptionBlockType.BeginExceptionBlock, null);
			}
			List<ExceptionBlock> list;
			if (!A_2.exBlocks.TryGetValue(ins, out list))
			{
				list = (A_2.exBlocks[ins] = new List<ExceptionBlock>());
			}
			ExceptionBlock exceptionBlock = new ExceptionBlock(t, null);
			list.Add(exceptionBlock);
			return exceptionBlock;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00015527 File Offset: 0x00013727
		[CompilerGenerated]
		internal static void <ToILDasmString>g__WriteLine|0_1(string text, ref MethodBodyLogExtensions.<>c__DisplayClass0_0 A_1)
		{
			A_1.sb.Append(new string(' ', A_1.indent)).AppendLine(text);
		}
	}
}
