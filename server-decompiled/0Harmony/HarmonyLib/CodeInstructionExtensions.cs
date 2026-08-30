using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x0200004F RID: 79
	public static class CodeInstructionExtensions
	{
		// Token: 0x06000365 RID: 869 RVA: 0x0000DA25 File Offset: 0x0000BC25
		public static bool IsValid(this OpCode code)
		{
			return code.Size > 0;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000DA34 File Offset: 0x0000BC34
		public static bool OperandIs(this CodeInstruction code, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (code.operand == null)
			{
				return false;
			}
			Type type = value.GetType();
			Type type2 = code.operand.GetType();
			if (AccessTools.IsInteger(type) && AccessTools.IsNumber(type2))
			{
				return Convert.ToInt64(code.operand) == Convert.ToInt64(value);
			}
			if (AccessTools.IsFloatingPoint(type) && AccessTools.IsNumber(type2))
			{
				return Convert.ToDouble(code.operand) == Convert.ToDouble(value);
			}
			return object.Equals(code.operand, value);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000DAC0 File Offset: 0x0000BCC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool OperandIs(this CodeInstruction code, MemberInfo value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return object.Equals(code.operand, value);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000DADC File Offset: 0x0000BCDC
		public static bool Is(this CodeInstruction code, OpCode opcode, object operand)
		{
			return code.opcode == opcode && code.OperandIs(operand);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000DAF5 File Offset: 0x0000BCF5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool Is(this CodeInstruction code, OpCode opcode, MemberInfo operand)
		{
			return code.opcode == opcode && code.OperandIs(operand);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000DB10 File Offset: 0x0000BD10
		public static bool IsLdarg(this CodeInstruction code, int? n = null)
		{
			return ((n == null || n.Value == 0) && code.opcode == OpCodes.Ldarg_0) || ((n == null || n.Value == 1) && code.opcode == OpCodes.Ldarg_1) || ((n == null || n.Value == 2) && code.opcode == OpCodes.Ldarg_2) || ((n == null || n.Value == 3) && code.opcode == OpCodes.Ldarg_3) || (code.opcode == OpCodes.Ldarg && (n == null || n.Value == Convert.ToInt32(code.operand))) || (code.opcode == OpCodes.Ldarg_S && (n == null || n.Value == Convert.ToInt32(code.operand)));
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000DC1C File Offset: 0x0000BE1C
		public static bool IsLdarga(this CodeInstruction code, int? n = null)
		{
			return (!(code.opcode != OpCodes.Ldarga) || !(code.opcode != OpCodes.Ldarga_S)) && (n == null || n.Value == Convert.ToInt32(code.operand));
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000DC70 File Offset: 0x0000BE70
		public static bool IsStarg(this CodeInstruction code, int? n = null)
		{
			return (!(code.opcode != OpCodes.Starg) || !(code.opcode != OpCodes.Starg_S)) && (n == null || n.Value == Convert.ToInt32(code.operand));
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000DCC2 File Offset: 0x0000BEC2
		public static bool IsLdloc(this CodeInstruction code, LocalBuilder variable = null)
		{
			return (CodeInstructionExtensions.opcodesLoadingLocalNormal.Contains(code.opcode) || CodeInstructionExtensions.opcodesLoadingLocalByAddress.Contains(code.opcode)) && (variable == null || object.Equals(variable, code.operand));
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000DCFB File Offset: 0x0000BEFB
		public static bool IsStloc(this CodeInstruction code, LocalBuilder variable = null)
		{
			return CodeInstructionExtensions.opcodesStoringLocal.Contains(code.opcode) && (variable == null || object.Equals(variable, code.operand));
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000DD22 File Offset: 0x0000BF22
		public static bool Branches(this CodeInstruction code, out Label? label)
		{
			if (CodeInstructionExtensions.opcodesBranching.Contains(code.opcode))
			{
				label = new Label?((Label)code.operand);
				return true;
			}
			label = null;
			return false;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000DD58 File Offset: 0x0000BF58
		public static bool Calls(this CodeInstruction code, MethodInfo method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			return (!(code.opcode != OpCodes.Call) || !(code.opcode != OpCodes.Callvirt)) && object.Equals(code.operand, method);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000DDA5 File Offset: 0x0000BFA5
		public static bool LoadsConstant(this CodeInstruction code)
		{
			return CodeInstructionExtensions.constantLoadingCodes.Contains(code.opcode);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000DDB8 File Offset: 0x0000BFB8
		public static bool LoadsConstant(this CodeInstruction code, long number)
		{
			OpCode opcode = code.opcode;
			return (number == -1L && opcode == OpCodes.Ldc_I4_M1) || (number == 0L && opcode == OpCodes.Ldc_I4_0) || (number == 1L && opcode == OpCodes.Ldc_I4_1) || (number == 2L && opcode == OpCodes.Ldc_I4_2) || (number == 3L && opcode == OpCodes.Ldc_I4_3) || (number == 4L && opcode == OpCodes.Ldc_I4_4) || (number == 5L && opcode == OpCodes.Ldc_I4_5) || (number == 6L && opcode == OpCodes.Ldc_I4_6) || (number == 7L && opcode == OpCodes.Ldc_I4_7) || (number == 8L && opcode == OpCodes.Ldc_I4_8) || ((!(opcode != OpCodes.Ldc_I4) || !(opcode != OpCodes.Ldc_I4_S) || !(opcode != OpCodes.Ldc_I8)) && Convert.ToInt64(code.operand) == number);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000DECC File Offset: 0x0000C0CC
		public static bool LoadsConstant(this CodeInstruction code, double number)
		{
			if (code.opcode != OpCodes.Ldc_R4 && code.opcode != OpCodes.Ldc_R8)
			{
				return false;
			}
			double num = Convert.ToDouble(code.operand);
			return num == number;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000DF0F File Offset: 0x0000C10F
		public static bool LoadsConstant(this CodeInstruction code, Enum e)
		{
			return code.LoadsConstant(Convert.ToInt64(e));
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000DF20 File Offset: 0x0000C120
		public static bool LoadsConstant(this CodeInstruction code, string str)
		{
			if (code.opcode != OpCodes.Ldstr)
			{
				return false;
			}
			string text = Convert.ToString(code.operand);
			return text == str;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000DF54 File Offset: 0x0000C154
		public static bool LoadsField(this CodeInstruction code, FieldInfo field, bool byAddress = false)
		{
			if (field == null)
			{
				throw new ArgumentNullException("field");
			}
			OpCode opCode = (field.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld);
			if (!byAddress && code.opcode == opCode && object.Equals(code.operand, field))
			{
				return true;
			}
			OpCode opCode2 = (field.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda);
			return byAddress && code.opcode == opCode2 && object.Equals(code.operand, field);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000DFDC File Offset: 0x0000C1DC
		public static bool StoresField(this CodeInstruction code, FieldInfo field)
		{
			if (field == null)
			{
				throw new ArgumentNullException("field");
			}
			OpCode opCode = (field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld);
			return code.opcode == opCode && object.Equals(code.operand, field);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000E028 File Offset: 0x0000C228
		public static int LocalIndex(this CodeInstruction code)
		{
			if (code.opcode == OpCodes.Ldloc_0 || code.opcode == OpCodes.Stloc_0)
			{
				return 0;
			}
			if (code.opcode == OpCodes.Ldloc_1 || code.opcode == OpCodes.Stloc_1)
			{
				return 1;
			}
			if (code.opcode == OpCodes.Ldloc_2 || code.opcode == OpCodes.Stloc_2)
			{
				return 2;
			}
			if (code.opcode == OpCodes.Ldloc_3 || code.opcode == OpCodes.Stloc_3)
			{
				return 3;
			}
			if (code.opcode == OpCodes.Ldloc_S || code.opcode == OpCodes.Ldloc)
			{
				return Convert.ToInt32(code.operand);
			}
			if (code.opcode == OpCodes.Stloc_S || code.opcode == OpCodes.Stloc)
			{
				return Convert.ToInt32(code.operand);
			}
			if (code.opcode == OpCodes.Ldloca_S || code.opcode == OpCodes.Ldloca)
			{
				return Convert.ToInt32(code.operand);
			}
			throw new ArgumentException("Instruction is not a load or store", "code");
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000E16C File Offset: 0x0000C36C
		public static int ArgumentIndex(this CodeInstruction code)
		{
			if (code.opcode == OpCodes.Ldarg_0)
			{
				return 0;
			}
			if (code.opcode == OpCodes.Ldarg_1)
			{
				return 1;
			}
			if (code.opcode == OpCodes.Ldarg_2)
			{
				return 2;
			}
			if (code.opcode == OpCodes.Ldarg_3)
			{
				return 3;
			}
			if (code.opcode == OpCodes.Ldarg_S || code.opcode == OpCodes.Ldarg)
			{
				return Convert.ToInt32(code.operand);
			}
			if (code.opcode == OpCodes.Starg_S || code.opcode == OpCodes.Starg)
			{
				return Convert.ToInt32(code.operand);
			}
			if (code.opcode == OpCodes.Ldarga_S || code.opcode == OpCodes.Ldarga)
			{
				return Convert.ToInt32(code.operand);
			}
			throw new ArgumentException("Instruction is not a load or store", "code");
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000E268 File Offset: 0x0000C468
		public static CodeInstruction WithLabels(this CodeInstruction code, params Label[] labels)
		{
			code.labels.AddRange(labels);
			return code;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000E277 File Offset: 0x0000C477
		public static CodeInstruction WithLabels(this CodeInstruction code, IEnumerable<Label> labels)
		{
			code.labels.AddRange(labels);
			return code;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000E288 File Offset: 0x0000C488
		public static List<Label> ExtractLabels(this CodeInstruction code)
		{
			List<Label> list = new List<Label>(code.labels);
			code.labels.Clear();
			return list;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000E2AD File Offset: 0x0000C4AD
		public static CodeInstruction MoveLabelsTo(this CodeInstruction code, CodeInstruction other)
		{
			other.WithLabels(code.ExtractLabels());
			return code;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000E2BD File Offset: 0x0000C4BD
		public static CodeInstruction MoveLabelsFrom(this CodeInstruction code, CodeInstruction other)
		{
			return code.WithLabels(other.ExtractLabels());
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000E2CB File Offset: 0x0000C4CB
		public static CodeInstruction WithBlocks(this CodeInstruction code, params ExceptionBlock[] blocks)
		{
			code.blocks.AddRange(blocks);
			return code;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000E2DA File Offset: 0x0000C4DA
		public static CodeInstruction WithBlocks(this CodeInstruction code, IEnumerable<ExceptionBlock> blocks)
		{
			code.blocks.AddRange(blocks);
			return code;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000E2EC File Offset: 0x0000C4EC
		public static List<ExceptionBlock> ExtractBlocks(this CodeInstruction code)
		{
			List<ExceptionBlock> list = new List<ExceptionBlock>(code.blocks);
			code.blocks.Clear();
			return list;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000E311 File Offset: 0x0000C511
		public static CodeInstruction MoveBlocksTo(this CodeInstruction code, CodeInstruction other)
		{
			other.WithBlocks(code.ExtractBlocks());
			return code;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000E321 File Offset: 0x0000C521
		public static CodeInstruction MoveBlocksFrom(this CodeInstruction code, CodeInstruction other)
		{
			return code.WithBlocks(other.ExtractBlocks());
		}

		// Token: 0x040000B9 RID: 185
		internal static readonly HashSet<OpCode> opcodesCalling = new HashSet<OpCode>
		{
			OpCodes.Call,
			OpCodes.Callvirt
		};

		// Token: 0x040000BA RID: 186
		internal static readonly HashSet<OpCode> opcodesLoadingLocalByAddress = new HashSet<OpCode>
		{
			OpCodes.Ldloca_S,
			OpCodes.Ldloca
		};

		// Token: 0x040000BB RID: 187
		internal static readonly HashSet<OpCode> opcodesLoadingLocalNormal = new HashSet<OpCode>
		{
			OpCodes.Ldloc_0,
			OpCodes.Ldloc_1,
			OpCodes.Ldloc_2,
			OpCodes.Ldloc_3,
			OpCodes.Ldloc_S,
			OpCodes.Ldloc
		};

		// Token: 0x040000BC RID: 188
		internal static readonly HashSet<OpCode> opcodesStoringLocal = new HashSet<OpCode>
		{
			OpCodes.Stloc_0,
			OpCodes.Stloc_1,
			OpCodes.Stloc_2,
			OpCodes.Stloc_3,
			OpCodes.Stloc_S,
			OpCodes.Stloc
		};

		// Token: 0x040000BD RID: 189
		internal static readonly HashSet<OpCode> opcodesLoadingArgumentByAddress = new HashSet<OpCode>
		{
			OpCodes.Ldarga_S,
			OpCodes.Ldarga
		};

		// Token: 0x040000BE RID: 190
		internal static readonly HashSet<OpCode> opcodesLoadingArgumentNormal = new HashSet<OpCode>
		{
			OpCodes.Ldarg_0,
			OpCodes.Ldarg_1,
			OpCodes.Ldarg_2,
			OpCodes.Ldarg_3,
			OpCodes.Ldarg_S,
			OpCodes.Ldarg
		};

		// Token: 0x040000BF RID: 191
		internal static readonly HashSet<OpCode> opcodesStoringArgument = new HashSet<OpCode>
		{
			OpCodes.Starg_S,
			OpCodes.Starg
		};

		// Token: 0x040000C0 RID: 192
		internal static readonly HashSet<OpCode> opcodesBranching = new HashSet<OpCode>
		{
			OpCodes.Br_S,
			OpCodes.Brfalse_S,
			OpCodes.Brtrue_S,
			OpCodes.Beq_S,
			OpCodes.Bge_S,
			OpCodes.Bgt_S,
			OpCodes.Ble_S,
			OpCodes.Blt_S,
			OpCodes.Bne_Un_S,
			OpCodes.Bge_Un_S,
			OpCodes.Bgt_Un_S,
			OpCodes.Ble_Un_S,
			OpCodes.Blt_Un_S,
			OpCodes.Br,
			OpCodes.Brfalse,
			OpCodes.Brtrue,
			OpCodes.Beq,
			OpCodes.Bge,
			OpCodes.Bgt,
			OpCodes.Ble,
			OpCodes.Blt,
			OpCodes.Bne_Un,
			OpCodes.Bge_Un,
			OpCodes.Bgt_Un,
			OpCodes.Ble_Un,
			OpCodes.Blt_Un
		};

		// Token: 0x040000C1 RID: 193
		private static readonly HashSet<OpCode> constantLoadingCodes = new HashSet<OpCode>
		{
			OpCodes.Ldc_I4_M1,
			OpCodes.Ldc_I4_0,
			OpCodes.Ldc_I4_1,
			OpCodes.Ldc_I4_2,
			OpCodes.Ldc_I4_3,
			OpCodes.Ldc_I4_4,
			OpCodes.Ldc_I4_5,
			OpCodes.Ldc_I4_6,
			OpCodes.Ldc_I4_7,
			OpCodes.Ldc_I4_8,
			OpCodes.Ldc_I4,
			OpCodes.Ldc_I4_S,
			OpCodes.Ldc_I8,
			OpCodes.Ldc_R4,
			OpCodes.Ldc_R8
		};
	}
}
