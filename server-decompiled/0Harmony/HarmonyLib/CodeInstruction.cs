using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace HarmonyLib
{
	// Token: 0x02000036 RID: 54
	public class CodeInstruction
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x000048C8 File Offset: 0x00002AC8
		internal CodeInstruction()
		{
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000048E6 File Offset: 0x00002AE6
		public CodeInstruction(OpCode opcode, object operand = null)
		{
			this.opcode = opcode;
			this.operand = operand;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004914 File Offset: 0x00002B14
		public CodeInstruction(CodeInstruction instruction)
		{
			this.opcode = instruction.opcode;
			this.operand = instruction.operand;
			List<Label> list = instruction.labels;
			List<Label> list2 = new List<Label>(list.Count);
			list2.AddRange(list);
			this.labels = list2;
			List<ExceptionBlock> list3 = instruction.blocks;
			List<ExceptionBlock> list4 = new List<ExceptionBlock>(list3.Count);
			list4.AddRange(list3);
			this.blocks = list4;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004993 File Offset: 0x00002B93
		public CodeInstruction Clone()
		{
			return new CodeInstruction(this)
			{
				labels = new List<Label>(),
				blocks = new List<ExceptionBlock>()
			};
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000049B4 File Offset: 0x00002BB4
		public CodeInstruction Clone(OpCode opcode)
		{
			CodeInstruction codeInstruction = this.Clone();
			codeInstruction.opcode = opcode;
			return codeInstruction;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000049D0 File Offset: 0x00002BD0
		public CodeInstruction Clone(object operand)
		{
			CodeInstruction codeInstruction = this.Clone();
			codeInstruction.operand = operand;
			return codeInstruction;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000049EC File Offset: 0x00002BEC
		public static CodeInstruction Call(Type type, string name, Type[] parameters = null, Type[] generics = null)
		{
			MethodInfo methodInfo = AccessTools.Method(type, name, parameters, generics);
			if (methodInfo == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 4);
				defaultInterpolatedStringHandler.AppendLiteral("No method found for type=");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
				defaultInterpolatedStringHandler.AppendLiteral(", name=");
				defaultInterpolatedStringHandler.AppendFormatted(name);
				defaultInterpolatedStringHandler.AppendLiteral(", parameters=");
				defaultInterpolatedStringHandler.AppendFormatted(parameters.Description());
				defaultInterpolatedStringHandler.AppendLiteral(", generics=");
				defaultInterpolatedStringHandler.AppendFormatted(generics.Description());
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new CodeInstruction(OpCodes.Call, methodInfo);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004A84 File Offset: 0x00002C84
		public static CodeInstruction Call(string typeColonMethodname, Type[] parameters = null, Type[] generics = null)
		{
			MethodInfo methodInfo = AccessTools.Method(typeColonMethodname, parameters, generics);
			if (methodInfo == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 3);
				defaultInterpolatedStringHandler.AppendLiteral("No method found for ");
				defaultInterpolatedStringHandler.AppendFormatted(typeColonMethodname);
				defaultInterpolatedStringHandler.AppendLiteral(", parameters=");
				defaultInterpolatedStringHandler.AppendFormatted(parameters.Description());
				defaultInterpolatedStringHandler.AppendLiteral(", generics=");
				defaultInterpolatedStringHandler.AppendFormatted(generics.Description());
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new CodeInstruction(OpCodes.Call, methodInfo);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004B05 File Offset: 0x00002D05
		public static CodeInstruction Call(Expression<Action> expression)
		{
			return new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo(expression));
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004B17 File Offset: 0x00002D17
		public static CodeInstruction Call<T>(Expression<Action<T>> expression)
		{
			return new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo<T>(expression));
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004B29 File Offset: 0x00002D29
		public static CodeInstruction Call<T, TResult>(Expression<Func<T, TResult>> expression)
		{
			return new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo<T, TResult>(expression));
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004B3B File Offset: 0x00002D3B
		public static CodeInstruction Call(LambdaExpression expression)
		{
			return new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo(expression));
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004B4D File Offset: 0x00002D4D
		public static CodeInstruction CallClosure<T>(T closure) where T : Delegate
		{
			return Transpilers.EmitDelegate<T>(closure);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004B58 File Offset: 0x00002D58
		public static CodeInstruction LoadField(Type type, string name, bool useAddress = false)
		{
			FieldInfo fieldInfo = AccessTools.Field(type, name);
			if (fieldInfo == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 2);
				defaultInterpolatedStringHandler.AppendLiteral("No field found for ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
				defaultInterpolatedStringHandler.AppendLiteral(" and ");
				defaultInterpolatedStringHandler.AppendFormatted(name);
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new CodeInstruction(useAddress ? (fieldInfo.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda) : (fieldInfo.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld), fieldInfo);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004BE4 File Offset: 0x00002DE4
		public static CodeInstruction StoreField(Type type, string name)
		{
			FieldInfo fieldInfo = AccessTools.Field(type, name);
			if (fieldInfo == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 2);
				defaultInterpolatedStringHandler.AppendLiteral("No field found for ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
				defaultInterpolatedStringHandler.AppendLiteral(" and ");
				defaultInterpolatedStringHandler.AppendFormatted(name);
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new CodeInstruction(fieldInfo.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fieldInfo);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004C58 File Offset: 0x00002E58
		public static CodeInstruction LoadLocal(int index, bool useAddress = false)
		{
			if (useAddress)
			{
				if (index < 256)
				{
					return new CodeInstruction(OpCodes.Ldloca_S, Convert.ToByte(index));
				}
				return new CodeInstruction(OpCodes.Ldloca, index);
			}
			else
			{
				if (index == 0)
				{
					return new CodeInstruction(OpCodes.Ldloc_0, null);
				}
				if (index == 1)
				{
					return new CodeInstruction(OpCodes.Ldloc_1, null);
				}
				if (index == 2)
				{
					return new CodeInstruction(OpCodes.Ldloc_2, null);
				}
				if (index == 3)
				{
					return new CodeInstruction(OpCodes.Ldloc_3, null);
				}
				if (index < 256)
				{
					return new CodeInstruction(OpCodes.Ldloc_S, Convert.ToByte(index));
				}
				return new CodeInstruction(OpCodes.Ldloc, index);
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004D04 File Offset: 0x00002F04
		public static CodeInstruction StoreLocal(int index)
		{
			if (index == 0)
			{
				return new CodeInstruction(OpCodes.Stloc_0, null);
			}
			if (index == 1)
			{
				return new CodeInstruction(OpCodes.Stloc_1, null);
			}
			if (index == 2)
			{
				return new CodeInstruction(OpCodes.Stloc_2, null);
			}
			if (index == 3)
			{
				return new CodeInstruction(OpCodes.Stloc_3, null);
			}
			if (index < 256)
			{
				return new CodeInstruction(OpCodes.Stloc_S, Convert.ToByte(index));
			}
			return new CodeInstruction(OpCodes.Stloc, index);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004D80 File Offset: 0x00002F80
		public static CodeInstruction LoadArgument(int index, bool useAddress = false)
		{
			if (useAddress)
			{
				if (index < 256)
				{
					return new CodeInstruction(OpCodes.Ldarga_S, Convert.ToByte(index));
				}
				return new CodeInstruction(OpCodes.Ldarga, index);
			}
			else
			{
				if (index == 0)
				{
					return new CodeInstruction(OpCodes.Ldarg_0, null);
				}
				if (index == 1)
				{
					return new CodeInstruction(OpCodes.Ldarg_1, null);
				}
				if (index == 2)
				{
					return new CodeInstruction(OpCodes.Ldarg_2, null);
				}
				if (index == 3)
				{
					return new CodeInstruction(OpCodes.Ldarg_3, null);
				}
				if (index < 256)
				{
					return new CodeInstruction(OpCodes.Ldarg_S, Convert.ToByte(index));
				}
				return new CodeInstruction(OpCodes.Ldarg, index);
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004E2C File Offset: 0x0000302C
		public static CodeInstruction StoreArgument(int index)
		{
			if (index < 256)
			{
				return new CodeInstruction(OpCodes.Starg_S, Convert.ToByte(index));
			}
			return new CodeInstruction(OpCodes.Starg, index);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004E5C File Offset: 0x0000305C
		public override string ToString()
		{
			List<string> list = new List<string>();
			foreach (Label label in this.labels)
			{
				List<string> list2 = list;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Label");
				defaultInterpolatedStringHandler.AppendFormatted<int>(label.GetHashCode());
				list2.Add(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			foreach (ExceptionBlock exceptionBlock in this.blocks)
			{
				list.Add("EX_" + exceptionBlock.blockType.ToString().Replace("Block", ""));
			}
			string text = ((list.Count > 0) ? (" [" + string.Join(", ", list.ToArray()) + "]") : "");
			string text2 = CodeInstruction.FormatArgument(this.operand, null);
			if (text2.Length > 0)
			{
				text2 = " " + text2;
			}
			OpCode opCode = this.opcode;
			return opCode.ToString() + text2 + text;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004FC4 File Offset: 0x000031C4
		internal static string FormatArgument(object argument, string extra = null)
		{
			if (argument == null)
			{
				return "NULL";
			}
			Type type = argument.GetType();
			MethodBase methodBase = argument as MethodBase;
			if (methodBase != null)
			{
				return methodBase.FullDescription() + ((extra != null) ? (" " + extra) : "");
			}
			FieldInfo fieldInfo = argument as FieldInfo;
			if (fieldInfo != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 3);
				defaultInterpolatedStringHandler.AppendFormatted(fieldInfo.FieldType.FullDescription());
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted(fieldInfo.DeclaringType.FullDescription());
				defaultInterpolatedStringHandler.AppendLiteral("::");
				defaultInterpolatedStringHandler.AppendFormatted(fieldInfo.Name);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (type == typeof(Label))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Label");
				defaultInterpolatedStringHandler.AppendFormatted<int>(((Label)argument).GetHashCode());
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (type == typeof(Label[]))
			{
				return "Labels" + string.Join(",", ((Label[])argument).Select<Label, string>((Label l) => l.GetHashCode().ToString()).ToArray<string>());
			}
			if (type == typeof(LocalBuilder))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>(((LocalBuilder)argument).LocalIndex);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(((LocalBuilder)argument).LocalType);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (type == typeof(string))
			{
				return argument.ToString().ToLiteral("\"");
			}
			return argument.ToString().Trim();
		}

		// Token: 0x0400004F RID: 79
		public OpCode opcode;

		// Token: 0x04000050 RID: 80
		public object operand;

		// Token: 0x04000051 RID: 81
		public List<Label> labels = new List<Label>();

		// Token: 0x04000052 RID: 82
		public List<ExceptionBlock> blocks = new List<ExceptionBlock>();
	}
}
