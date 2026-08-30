using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib.Internal.Util;
using HarmonyLib.Tools;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Utils;
using MonoMod.Utils.Cil;

namespace HarmonyLib.Internal.Patching
{
	// Token: 0x02000066 RID: 102
	internal class ILManipulator
	{
		// Token: 0x06000456 RID: 1110 RVA: 0x000159E8 File Offset: 0x00013BE8
		static ILManipulator()
		{
			Dictionary<global::System.Reflection.Emit.OpCode, global::System.Reflection.Emit.OpCode> dictionary = new Dictionary<global::System.Reflection.Emit.OpCode, global::System.Reflection.Emit.OpCode>();
			global::System.Reflection.Emit.OpCode beq_S = global::System.Reflection.Emit.OpCodes.Beq_S;
			dictionary[beq_S] = global::System.Reflection.Emit.OpCodes.Beq;
			global::System.Reflection.Emit.OpCode bge_S = global::System.Reflection.Emit.OpCodes.Bge_S;
			dictionary[bge_S] = global::System.Reflection.Emit.OpCodes.Bge;
			global::System.Reflection.Emit.OpCode bge_Un_S = global::System.Reflection.Emit.OpCodes.Bge_Un_S;
			dictionary[bge_Un_S] = global::System.Reflection.Emit.OpCodes.Bge_Un;
			global::System.Reflection.Emit.OpCode bgt_S = global::System.Reflection.Emit.OpCodes.Bgt_S;
			dictionary[bgt_S] = global::System.Reflection.Emit.OpCodes.Bgt;
			global::System.Reflection.Emit.OpCode bgt_Un_S = global::System.Reflection.Emit.OpCodes.Bgt_Un_S;
			dictionary[bgt_Un_S] = global::System.Reflection.Emit.OpCodes.Bgt_Un;
			global::System.Reflection.Emit.OpCode ble_S = global::System.Reflection.Emit.OpCodes.Ble_S;
			dictionary[ble_S] = global::System.Reflection.Emit.OpCodes.Ble;
			global::System.Reflection.Emit.OpCode ble_Un_S = global::System.Reflection.Emit.OpCodes.Ble_Un_S;
			dictionary[ble_Un_S] = global::System.Reflection.Emit.OpCodes.Ble_Un;
			global::System.Reflection.Emit.OpCode blt_S = global::System.Reflection.Emit.OpCodes.Blt_S;
			dictionary[blt_S] = global::System.Reflection.Emit.OpCodes.Blt;
			global::System.Reflection.Emit.OpCode blt_Un_S = global::System.Reflection.Emit.OpCodes.Blt_Un_S;
			dictionary[blt_Un_S] = global::System.Reflection.Emit.OpCodes.Blt_Un;
			global::System.Reflection.Emit.OpCode bne_Un_S = global::System.Reflection.Emit.OpCodes.Bne_Un_S;
			dictionary[bne_Un_S] = global::System.Reflection.Emit.OpCodes.Bne_Un;
			global::System.Reflection.Emit.OpCode brfalse_S = global::System.Reflection.Emit.OpCodes.Brfalse_S;
			dictionary[brfalse_S] = global::System.Reflection.Emit.OpCodes.Brfalse;
			global::System.Reflection.Emit.OpCode brtrue_S = global::System.Reflection.Emit.OpCodes.Brtrue_S;
			dictionary[brtrue_S] = global::System.Reflection.Emit.OpCodes.Brtrue;
			global::System.Reflection.Emit.OpCode br_S = global::System.Reflection.Emit.OpCodes.Br_S;
			dictionary[br_S] = global::System.Reflection.Emit.OpCodes.Br;
			global::System.Reflection.Emit.OpCode leave_S = global::System.Reflection.Emit.OpCodes.Leave_S;
			dictionary[leave_S] = global::System.Reflection.Emit.OpCodes.Leave;
			ILManipulator.ShortToLongMap = dictionary;
			foreach (FieldInfo fieldInfo in typeof(global::System.Reflection.Emit.OpCodes).GetFields(BindingFlags.Static | BindingFlags.Public))
			{
				global::System.Reflection.Emit.OpCode opCode = (global::System.Reflection.Emit.OpCode)fieldInfo.GetValue(null);
				ILManipulator.SREOpCodes[opCode.Value] = opCode;
			}
			foreach (FieldInfo fieldInfo2 in typeof(Mono.Cecil.Cil.OpCodes).GetFields(BindingFlags.Static | BindingFlags.Public))
			{
				Mono.Cecil.Cil.OpCode opCode2 = (Mono.Cecil.Cil.OpCode)fieldInfo2.GetValue(null);
				ILManipulator.CecilOpCodes[opCode2.Value] = opCode2;
			}
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00015BC1 File Offset: 0x00013DC1
		public ILManipulator(Mono.Cecil.Cil.MethodBody body, bool debug)
		{
			this.Body = body;
			this.debug = debug;
			this.codeInstructions = this.ReadBody(this.Body);
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x00015BFF File Offset: 0x00013DFF
		public Mono.Cecil.Cil.MethodBody Body { get; }

		// Token: 0x06000459 RID: 1113 RVA: 0x00015C08 File Offset: 0x00013E08
		private int GetTarget(Mono.Cecil.Cil.MethodBody body, object insOp)
		{
			ILLabel illabel = insOp as ILLabel;
			if (illabel != null)
			{
				return body.Instructions.IndexOf(illabel.Target);
			}
			Instruction instruction = insOp as Instruction;
			if (instruction != null)
			{
				return body.Instructions.IndexOf(instruction);
			}
			return -1;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00015C4C File Offset: 0x00013E4C
		private int[] GetTargets(Mono.Cecil.Cil.MethodBody body, object insOp)
		{
			ILManipulator.<>c__DisplayClass13_0 CS$<>8__locals1 = new ILManipulator.<>c__DisplayClass13_0();
			CS$<>8__locals1.body = body;
			ILLabel[] array = insOp as ILLabel[];
			if (array != null)
			{
				return CS$<>8__locals1.<GetTargets>g__Result|0<ILLabel>(array, (ILLabel l) => l.Target);
			}
			Instruction[] array2 = insOp as Instruction[];
			if (array2 != null)
			{
				return CS$<>8__locals1.<GetTargets>g__Result|0<Instruction>(array2, (Instruction i) => i);
			}
			return new int[0];
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00015CD0 File Offset: 0x00013ED0
		private IEnumerable<ILManipulator.RawInstruction> ReadBody(Mono.Cecil.Cil.MethodBody body)
		{
			ILManipulator.<>c__DisplayClass14_0 CS$<>8__locals1 = new ILManipulator.<>c__DisplayClass14_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.body = body;
			CS$<>8__locals1.instructions = new List<ILManipulator.RawInstruction>(CS$<>8__locals1.body.Instructions.Count);
			CS$<>8__locals1.instructions.AddRange(CS$<>8__locals1.body.Instructions.Select<Instruction, ILManipulator.RawInstruction>(new Func<Instruction, ILManipulator.RawInstruction>(CS$<>8__locals1.<ReadBody>g__ReadInstruction|0)));
			foreach (ILManipulator.RawInstruction rawInstruction in CS$<>8__locals1.instructions)
			{
				ILManipulator.RawInstruction rawInstruction2 = rawInstruction;
				global::System.Reflection.Emit.OperandType operandType = rawInstruction.Instruction.opcode.OperandType;
				object obj;
				if (operandType != global::System.Reflection.Emit.OperandType.InlineBrTarget)
				{
					if (operandType != global::System.Reflection.Emit.OperandType.InlineSwitch)
					{
						if (operandType == global::System.Reflection.Emit.OperandType.ShortInlineBrTarget)
						{
							obj = CS$<>8__locals1.instructions[(int)rawInstruction.Operand].Instruction;
						}
						else
						{
							obj = rawInstruction.Operand;
						}
					}
					else
					{
						IEnumerable<int> enumerable = (int[])rawInstruction.Operand;
						Func<int, CodeInstruction> func;
						if ((func = CS$<>8__locals1.<>9__1) == null)
						{
							func = (CS$<>8__locals1.<>9__1 = (int i) => CS$<>8__locals1.instructions[i].Instruction);
						}
						obj = enumerable.Select<int, CodeInstruction>(func).ToArray<CodeInstruction>();
					}
				}
				else
				{
					obj = CS$<>8__locals1.instructions[(int)rawInstruction.Operand].Instruction;
				}
				rawInstruction2.Operand = obj;
			}
			foreach (Mono.Cecil.Cil.ExceptionHandler exceptionHandler in CS$<>8__locals1.body.ExceptionHandlers)
			{
				CodeInstruction instruction = CS$<>8__locals1.instructions[CS$<>8__locals1.body.Instructions.IndexOf(exceptionHandler.TryStart)].Instruction;
				CodeInstruction instruction2 = CS$<>8__locals1.instructions[CS$<>8__locals1.body.Instructions.IndexOf(exceptionHandler.TryEnd)].Instruction;
				CodeInstruction instruction3 = CS$<>8__locals1.instructions[CS$<>8__locals1.body.Instructions.IndexOf(exceptionHandler.HandlerStart)].Instruction;
				int num = ((exceptionHandler.HandlerEnd == null) ? (CS$<>8__locals1.instructions.Count - 1) : CS$<>8__locals1.body.Instructions.IndexOf(exceptionHandler.HandlerEnd.Previous));
				CodeInstruction instruction4 = CS$<>8__locals1.instructions[num].Instruction;
				instruction.blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginExceptionBlock, null));
				instruction4.blocks.Add(new ExceptionBlock(ExceptionBlockType.EndExceptionBlock, null));
				switch (exceptionHandler.HandlerType)
				{
				case ExceptionHandlerType.Catch:
					instruction3.blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginCatchBlock, exceptionHandler.CatchType.ResolveReflection()));
					break;
				case ExceptionHandlerType.Filter:
				{
					CodeInstruction instruction5 = CS$<>8__locals1.instructions[CS$<>8__locals1.body.Instructions.IndexOf(exceptionHandler.FilterStart)].Instruction;
					instruction5.blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginExceptFilterBlock, null));
					break;
				}
				case ExceptionHandlerType.Finally:
					instruction3.blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginFinallyBlock, null));
					break;
				case ExceptionHandlerType.Fault:
					instruction3.blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginFaultBlock, null));
					break;
				}
			}
			return CS$<>8__locals1.instructions;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00016024 File Offset: 0x00014224
		public void AddTranspiler(MethodInfo transpiler)
		{
			this.transpilers.Add(transpiler);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00016034 File Offset: 0x00014234
		private object[] GetTranspilerArguments(ILGenerator il, MethodInfo transpiler, IEnumerable<CodeInstruction> instructions, MethodBase orignal = null)
		{
			List<object> list = new List<object>();
			foreach (Type type in from p in transpiler.GetParameters()
				select p.ParameterType)
			{
				if (type.IsAssignableFrom(typeof(ILGenerator)))
				{
					list.Add(il);
				}
				else if (type.IsAssignableFrom(typeof(MethodBase)) && orignal != null)
				{
					list.Add(orignal);
				}
				else if (type.IsAssignableFrom(typeof(IEnumerable<CodeInstruction>)))
				{
					list.Add(instructions);
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00016104 File Offset: 0x00014304
		public IEnumerable<KeyValuePair<global::System.Reflection.Emit.OpCode, object>> GetRawInstructions()
		{
			return this.codeInstructions.Select<ILManipulator.RawInstruction, KeyValuePair<global::System.Reflection.Emit.OpCode, object>>((ILManipulator.RawInstruction i) => new KeyValuePair<global::System.Reflection.Emit.OpCode, object>(i.Instruction.opcode, i.Operand));
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00016130 File Offset: 0x00014330
		public List<CodeInstruction> GetInstructions(ILGenerator il, MethodBase original = null)
		{
			return ILManipulator.NormalizeInstructions(this.ApplyTranspilers(il, original, (VariableDefinition vDef) => il.DeclareLocal(vDef.VariableType.ResolveReflection()), new Func<Label>(il.DefineLabel))).ToList<CodeInstruction>();
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00016180 File Offset: 0x00014380
		private IEnumerable<CodeInstruction> ApplyTranspilers(ILGenerator il, MethodBase original, Func<VariableDefinition, LocalBuilder> getLocal, Func<Label> defineLabel)
		{
			List<CodeInstruction> list = (from i in this.Prepare(getLocal, defineLabel)
				select i.Instruction).ToList<CodeInstruction>();
			if (this.transpilers.Count == 0)
			{
				return list;
			}
			IEnumerable<CodeInstruction> enumerable = ILManipulator.NormalizeInstructions(list);
			using (List<MethodInfo>.Enumerator enumerator = this.transpilers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MethodInfo transpiler = enumerator.Current;
					object[] transpilerArguments = this.GetTranspilerArguments(il, transpiler, enumerable, original);
					Logger.Log(Logger.LogChannel.Info, () => "Running transpiler " + transpiler.FullDescription(), this.debug);
					IEnumerable<CodeInstruction> enumerable2 = transpiler.Invoke(null, transpilerArguments) as IEnumerable<CodeInstruction>;
					enumerable = ILManipulator.NormalizeInstructions(enumerable2).ToList<CodeInstruction>();
				}
			}
			return enumerable;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0001626C File Offset: 0x0001446C
		public Dictionary<int, CodeInstruction> GetIndexedInstructions(ILGenerator il)
		{
			int size = 0;
			return this.Prepare((VariableDefinition vDef) => il.DeclareLocal(vDef.VariableType.ResolveReflection()), new Func<Label>(il.DefineLabel)).ToDictionary<ILManipulator.RawInstruction, int, CodeInstruction>((ILManipulator.RawInstruction i) => ILManipulator.<GetIndexedInstructions>g__Grow|20_0(ref size, i.CILInstruction.GetSize()), (ILManipulator.RawInstruction i) => i.Instruction);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x000162E4 File Offset: 0x000144E4
		private IEnumerable<ILManipulator.RawInstruction> Prepare(Func<VariableDefinition, LocalBuilder> getLocal, Func<Label> defineLabel)
		{
			this.localsCache.Clear();
			foreach (VariableDefinition variableDefinition in this.Body.Variables)
			{
				this.localsCache[variableDefinition] = getLocal(variableDefinition);
			}
			foreach (ILManipulator.RawInstruction rawInstruction in this.codeInstructions)
			{
				rawInstruction.Instruction.operand = rawInstruction.Operand;
				global::System.Reflection.Emit.OperandType operandType = rawInstruction.Instruction.opcode.OperandType;
				if (operandType != global::System.Reflection.Emit.OperandType.InlineBrTarget)
				{
					switch (operandType)
					{
					case global::System.Reflection.Emit.OperandType.InlineSwitch:
					{
						CodeInstruction[] array = rawInstruction.Operand as CodeInstruction[];
						if (array != null)
						{
							List<Label> list = new List<Label>();
							foreach (CodeInstruction codeInstruction in array)
							{
								Label label = defineLabel();
								codeInstruction.labels.Add(label);
								list.Add(label);
							}
							rawInstruction.Instruction.operand = list.ToArray();
							continue;
						}
						continue;
					}
					case global::System.Reflection.Emit.OperandType.InlineTok:
					case global::System.Reflection.Emit.OperandType.InlineType:
						continue;
					case global::System.Reflection.Emit.OperandType.InlineVar:
						break;
					case global::System.Reflection.Emit.OperandType.ShortInlineBrTarget:
						goto IL_0158;
					default:
						if (operandType != global::System.Reflection.Emit.OperandType.ShortInlineVar)
						{
							continue;
						}
						break;
					}
					VariableDefinition variableDefinition2 = rawInstruction.Operand as VariableDefinition;
					if (variableDefinition2 != null)
					{
						rawInstruction.Instruction.operand = this.localsCache[variableDefinition2];
						continue;
					}
					continue;
				}
				IL_0158:
				CodeInstruction codeInstruction2 = rawInstruction.Instruction.operand as CodeInstruction;
				if (codeInstruction2 != null)
				{
					Label label2 = defineLabel();
					codeInstruction2.labels.Add(label2);
					rawInstruction.Instruction.operand = label2;
				}
			}
			return this.codeInstructions;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000164D8 File Offset: 0x000146D8
		public void WriteTo(Mono.Cecil.Cil.MethodBody body, MethodBase original = null)
		{
			body.Instructions.Clear();
			body.ExceptionHandlers.Clear();
			CecilILGenerator il = new CecilILGenerator(body.GetILProcessor());
			ILGenerator proxy = il.GetProxy();
			il.DefineLabel();
			IEnumerable<CodeInstruction> enumerable = this.ApplyTranspilers(proxy, original, (VariableDefinition vDef) => il.GetLocal(vDef), new Func<Label>(il.DefineLabel));
			Action<Label> <>9__1;
			Action<ExceptionBlock> <>9__2;
			Action<ExceptionBlock> <>9__3;
			foreach (CodeInstruction codeInstruction in enumerable)
			{
				List<Label> labels = codeInstruction.labels;
				Action<Label> action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate(Label l)
					{
						il.MarkLabel(l);
					});
				}
				labels.ForEach(action);
				List<ExceptionBlock> blocks = codeInstruction.blocks;
				Action<ExceptionBlock> action2;
				if ((action2 = <>9__2) == null)
				{
					action2 = (<>9__2 = delegate(ExceptionBlock b)
					{
						il.MarkBlockBefore(b);
					});
				}
				blocks.ForEach(action2);
				if ((!(codeInstruction.opcode == global::System.Reflection.Emit.OpCodes.Endfilter) && !(codeInstruction.opcode == global::System.Reflection.Emit.OpCodes.Endfinally)) || codeInstruction.blocks.Count <= 0)
				{
					global::System.Reflection.Emit.OperandType operandType = codeInstruction.opcode.OperandType;
					if (operandType != global::System.Reflection.Emit.OperandType.InlineNone)
					{
						if (operandType == global::System.Reflection.Emit.OperandType.InlineSig)
						{
							throw new NotSupportedException("Emitting opcodes with CallSites is currently not fully implemented");
						}
						if (codeInstruction.operand == null)
						{
							string text = "operand";
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Invalid argument for ");
							defaultInterpolatedStringHandler.AppendFormatted<CodeInstruction>(codeInstruction);
							throw new ArgumentNullException(text, defaultInterpolatedStringHandler.ToStringAndClear());
						}
						il.Emit(codeInstruction.opcode, codeInstruction.operand);
					}
					else
					{
						il.Emit(codeInstruction.opcode);
					}
				}
				List<ExceptionBlock> blocks2 = codeInstruction.blocks;
				Action<ExceptionBlock> action3;
				if ((action3 = <>9__3) == null)
				{
					action3 = (<>9__3 = delegate(ExceptionBlock b)
					{
						il.MarkBlockAfter(b);
					});
				}
				blocks2.ForEach(action3);
			}
			if (body.Instructions.Count == 0)
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Ret);
			}
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00016704 File Offset: 0x00014904
		private static IEnumerable<CodeInstruction> NormalizeInstructions(IEnumerable<CodeInstruction> instrs)
		{
			foreach (CodeInstruction codeInstruction in instrs)
			{
				CodeInstruction codeInstruction2 = codeInstruction;
				if (codeInstruction2.labels == null)
				{
					codeInstruction2.labels = new List<Label>();
				}
				codeInstruction2 = codeInstruction;
				if (codeInstruction2.blocks == null)
				{
					codeInstruction2.blocks = new List<ExceptionBlock>();
				}
				global::System.Reflection.Emit.OpCode opCode;
				if (ILManipulator.ShortToLongMap.TryGetValue(codeInstruction.opcode, out opCode))
				{
					codeInstruction.opcode = opCode;
				}
				yield return codeInstruction;
			}
			IEnumerator<CodeInstruction> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00016714 File Offset: 0x00014914
		public static Dictionary<int, CodeInstruction> GetInstructions(Mono.Cecil.Cil.MethodBody body)
		{
			if (body == null)
			{
				return null;
			}
			Dictionary<int, CodeInstruction> dictionary;
			try
			{
				dictionary = new ILManipulator(body, false).GetIndexedInstructions(PatchProcessor.CreateILGenerator());
			}
			catch (Exception ex)
			{
				Exception e = ex;
				Logger.Log(Logger.LogChannel.Warn, () => "Could not read instructions of " + body.Method.GetID(null, null, true, false) + ": " + e.Message, false);
				dictionary = null;
			}
			return dictionary;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00016784 File Offset: 0x00014984
		[CompilerGenerated]
		internal static int <GetIndexedInstructions>g__Grow|20_0(ref int i, int s)
		{
			int num = i;
			i += s;
			return num;
		}

		// Token: 0x0400011D RID: 285
		private static readonly Dictionary<short, global::System.Reflection.Emit.OpCode> SREOpCodes = new Dictionary<short, global::System.Reflection.Emit.OpCode>();

		// Token: 0x0400011E RID: 286
		private static readonly Dictionary<short, Mono.Cecil.Cil.OpCode> CecilOpCodes = new Dictionary<short, Mono.Cecil.Cil.OpCode>();

		// Token: 0x0400011F RID: 287
		private static readonly Dictionary<global::System.Reflection.Emit.OpCode, global::System.Reflection.Emit.OpCode> ShortToLongMap;

		// Token: 0x04000120 RID: 288
		private readonly IEnumerable<ILManipulator.RawInstruction> codeInstructions;

		// Token: 0x04000121 RID: 289
		private readonly bool debug;

		// Token: 0x04000122 RID: 290
		private readonly Dictionary<VariableDefinition, LocalBuilder> localsCache = new Dictionary<VariableDefinition, LocalBuilder>();

		// Token: 0x04000123 RID: 291
		private readonly List<MethodInfo> transpilers = new List<MethodInfo>();

		// Token: 0x020001E4 RID: 484
		private class RawInstruction
		{
			// Token: 0x170001FB RID: 507
			// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0001C641 File Offset: 0x0001A841
			// (set) Token: 0x060007F7 RID: 2039 RVA: 0x0001C649 File Offset: 0x0001A849
			public CodeInstruction Instruction { get; set; }

			// Token: 0x170001FC RID: 508
			// (get) Token: 0x060007F8 RID: 2040 RVA: 0x0001C652 File Offset: 0x0001A852
			// (set) Token: 0x060007F9 RID: 2041 RVA: 0x0001C65A File Offset: 0x0001A85A
			public object Operand { get; set; }

			// Token: 0x170001FD RID: 509
			// (get) Token: 0x060007FA RID: 2042 RVA: 0x0001C663 File Offset: 0x0001A863
			// (set) Token: 0x060007FB RID: 2043 RVA: 0x0001C66B File Offset: 0x0001A86B
			public Instruction CILInstruction { get; set; }
		}
	}
}
