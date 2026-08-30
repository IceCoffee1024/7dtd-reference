using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Mono.Cecil.Cil;
using MonoMod.Utils;
using MonoMod.Utils.Cil;

namespace HarmonyLib.Internal.Util
{
	// Token: 0x02000061 RID: 97
	internal static class EmitterExtensions
	{
		// Token: 0x06000428 RID: 1064 RVA: 0x000141B4 File Offset: 0x000123B4
		[MethodImpl(MethodImplOptions.Synchronized)]
		static EmitterExtensions()
		{
			Dictionary<Type, Mono.Cecil.Cil.OpCode> dictionary = new Dictionary<Type, Mono.Cecil.Cil.OpCode>();
			Type type = typeof(sbyte);
			dictionary[type] = Mono.Cecil.Cil.OpCodes.Stind_I1;
			Type type2 = typeof(byte);
			dictionary[type2] = Mono.Cecil.Cil.OpCodes.Stind_I1;
			Type type3 = typeof(short);
			dictionary[type3] = Mono.Cecil.Cil.OpCodes.Stind_I2;
			Type type4 = typeof(ushort);
			dictionary[type4] = Mono.Cecil.Cil.OpCodes.Stind_I2;
			Type type5 = typeof(int);
			dictionary[type5] = Mono.Cecil.Cil.OpCodes.Stind_I4;
			Type type6 = typeof(uint);
			dictionary[type6] = Mono.Cecil.Cil.OpCodes.Stind_I4;
			Type type7 = typeof(long);
			dictionary[type7] = Mono.Cecil.Cil.OpCodes.Stind_I8;
			Type type8 = typeof(ulong);
			dictionary[type8] = Mono.Cecil.Cil.OpCodes.Stind_I8;
			Type type9 = typeof(float);
			dictionary[type9] = Mono.Cecil.Cil.OpCodes.Stind_R4;
			Type type10 = typeof(double);
			dictionary[type10] = Mono.Cecil.Cil.OpCodes.Stind_R8;
			EmitterExtensions.storeOpCodes = dictionary;
			dictionary = new Dictionary<Type, Mono.Cecil.Cil.OpCode>();
			type10 = typeof(sbyte);
			dictionary[type10] = Mono.Cecil.Cil.OpCodes.Ldind_I1;
			type9 = typeof(byte);
			dictionary[type9] = Mono.Cecil.Cil.OpCodes.Ldind_I1;
			type8 = typeof(short);
			dictionary[type8] = Mono.Cecil.Cil.OpCodes.Ldind_I2;
			type7 = typeof(ushort);
			dictionary[type7] = Mono.Cecil.Cil.OpCodes.Ldind_I2;
			type6 = typeof(int);
			dictionary[type6] = Mono.Cecil.Cil.OpCodes.Ldind_I4;
			type5 = typeof(uint);
			dictionary[type5] = Mono.Cecil.Cil.OpCodes.Ldind_I4;
			type4 = typeof(long);
			dictionary[type4] = Mono.Cecil.Cil.OpCodes.Ldind_I8;
			type3 = typeof(ulong);
			dictionary[type3] = Mono.Cecil.Cil.OpCodes.Ldind_I8;
			type2 = typeof(float);
			dictionary[type2] = Mono.Cecil.Cil.OpCodes.Ldind_R4;
			type = typeof(double);
			dictionary[type] = Mono.Cecil.Cil.OpCodes.Ldind_R8;
			EmitterExtensions.loadOpCodes = dictionary;
			EmitterExtensions.c_LocalBuilder = (from c in typeof(LocalBuilder).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				orderby c.GetParameters().Length descending
				select c).First<ConstructorInfo>();
			EmitterExtensions.f_LocalBuilder_position = typeof(LocalBuilder).GetField("position", BindingFlags.Instance | BindingFlags.NonPublic);
			EmitterExtensions.f_LocalBuilder_is_pinned = typeof(LocalBuilder).GetField("is_pinned", BindingFlags.Instance | BindingFlags.NonPublic);
			EmitterExtensions.c_LocalBuilder_params = EmitterExtensions.c_LocalBuilder.GetParameters().Length;
			if (EmitterExtensions.emitDMD != null)
			{
				return;
			}
			EmitterExtensions.InitEmitterHelperDMD();
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00014448 File Offset: 0x00012648
		public static Mono.Cecil.Cil.OpCode GetCecilStoreOpCode(this Type t)
		{
			if (t.IsEnum)
			{
				return Mono.Cecil.Cil.OpCodes.Stind_I4;
			}
			Mono.Cecil.Cil.OpCode opCode;
			if (!EmitterExtensions.storeOpCodes.TryGetValue(t, out opCode))
			{
				return Mono.Cecil.Cil.OpCodes.Stind_Ref;
			}
			return opCode;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001447C File Offset: 0x0001267C
		public static Mono.Cecil.Cil.OpCode GetCecilLoadOpCode(this Type t)
		{
			if (t.IsEnum)
			{
				return Mono.Cecil.Cil.OpCodes.Ldind_I4;
			}
			Mono.Cecil.Cil.OpCode opCode;
			if (!EmitterExtensions.loadOpCodes.TryGetValue(t, out opCode))
			{
				return Mono.Cecil.Cil.OpCodes.Ldind_Ref;
			}
			return opCode;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x000144AD File Offset: 0x000126AD
		public static Type OpenRefType(this Type t)
		{
			if (t.IsByRef)
			{
				return t.GetElementType();
			}
			return t;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x000144C0 File Offset: 0x000126C0
		private static void InitEmitterHelperDMD()
		{
			EmitterExtensions.emitDMD = new DynamicMethodDefinition("EmitOpcodeWithOperand", typeof(void), new Type[]
			{
				typeof(CecilILGenerator),
				typeof(global::System.Reflection.Emit.OpCode),
				typeof(object)
			});
			ILGenerator ilgenerator = EmitterExtensions.emitDMD.GetILGenerator();
			Label label = ilgenerator.DefineLabel();
			ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Ldarg_2);
			ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Brtrue, label);
			ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Ldstr, "Provided operand is null!");
			ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Newobj, typeof(Exception).GetConstructor(new Type[] { typeof(string) }));
			ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Throw);
			foreach (MethodInfo methodInfo in from m in typeof(CecilILGenerator).GetMethods()
				where m.Name == "Emit"
				select m)
			{
				ParameterInfo[] parameters = methodInfo.GetParameters();
				if (parameters.Length == 2)
				{
					Type[] array = parameters.Select<ParameterInfo, Type>((ParameterInfo p) => p.ParameterType).ToArray<Type>();
					if (!(array[0] != typeof(global::System.Reflection.Emit.OpCode)))
					{
						Type type = array[1];
						ilgenerator.MarkLabel(label);
						label = ilgenerator.DefineLabel();
						ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Ldarg_2);
						ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Isinst, type);
						ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Brfalse, label);
						ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Ldarg_2);
						if (type.IsValueType)
						{
							ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Unbox_Any, type);
						}
						LocalBuilder localBuilder = ilgenerator.DeclareLocal(type);
						ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Stloc, localBuilder);
						ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Ldarg_0);
						ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Ldarg_1);
						ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Ldloc, localBuilder);
						ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Callvirt, methodInfo);
						ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Ret);
					}
				}
			}
			ilgenerator.MarkLabel(label);
			ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Ldstr, "The operand is none of the supported types!");
			ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Newobj, typeof(Exception).GetConstructor(new Type[] { typeof(string) }));
			ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Throw);
			ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Ret);
			EmitterExtensions.emitDMDMethod = EmitterExtensions.emitDMD.Generate();
			EmitterExtensions.emitCodeDelegate = EmitterExtensions.emitDMDMethod.CreateDelegate<Action<CecilILGenerator, global::System.Reflection.Emit.OpCode, object>>();
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00014778 File Offset: 0x00012978
		public static void Emit(this CecilILGenerator il, global::System.Reflection.Emit.OpCode opcode, object operand)
		{
			EmitterExtensions.emitCodeDelegate(il, opcode, operand);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00014788 File Offset: 0x00012988
		public static void MarkBlockBefore(this CecilILGenerator il, ExceptionBlock block)
		{
			switch (block.blockType)
			{
			case ExceptionBlockType.BeginExceptionBlock:
				il.BeginExceptionBlock();
				return;
			case ExceptionBlockType.BeginCatchBlock:
				il.BeginCatchBlock(block.catchType);
				return;
			case ExceptionBlockType.BeginExceptFilterBlock:
				il.BeginExceptFilterBlock();
				return;
			case ExceptionBlockType.BeginFaultBlock:
				il.BeginFaultBlock();
				return;
			case ExceptionBlockType.BeginFinallyBlock:
				il.BeginFinallyBlock();
				return;
			case ExceptionBlockType.EndExceptionBlock:
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x000147EC File Offset: 0x000129EC
		public static void MarkBlockAfter(this CecilILGenerator il, ExceptionBlock block)
		{
			if (block.blockType == ExceptionBlockType.EndExceptionBlock)
			{
				il.EndExceptionBlock();
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00014800 File Offset: 0x00012A00
		public static LocalBuilder GetLocal(this CecilILGenerator il, VariableDefinition varDef)
		{
			Dictionary<LocalBuilder, VariableDefinition> dictionary = (Dictionary<LocalBuilder, VariableDefinition>)AccessTools.Field(typeof(CecilILGenerator), "_Variables").GetValue(il);
			LocalBuilder localBuilder = dictionary.FirstOrDefault<KeyValuePair<LocalBuilder, VariableDefinition>>((KeyValuePair<LocalBuilder, VariableDefinition> kv) => kv.Value == varDef).Key;
			if (localBuilder != null)
			{
				return localBuilder;
			}
			Type type = varDef.VariableType.ResolveReflection();
			bool isPinned = varDef.VariableType.IsPinned;
			int index = varDef.Index;
			object obj;
			if (EmitterExtensions.c_LocalBuilder_params != 4)
			{
				if (EmitterExtensions.c_LocalBuilder_params != 3)
				{
					if (EmitterExtensions.c_LocalBuilder_params != 2)
					{
						if (EmitterExtensions.c_LocalBuilder_params != 0)
						{
							throw new NotSupportedException();
						}
						obj = EmitterExtensions.c_LocalBuilder.Invoke(new object[0]);
					}
					else
					{
						ConstructorInfo constructorInfo = EmitterExtensions.c_LocalBuilder;
						object[] array = new object[2];
						array[0] = type;
						obj = constructorInfo.Invoke(array);
					}
				}
				else
				{
					ConstructorInfo constructorInfo2 = EmitterExtensions.c_LocalBuilder;
					object[] array2 = new object[3];
					array2[0] = index;
					array2[1] = type;
					obj = constructorInfo2.Invoke(array2);
				}
			}
			else
			{
				obj = EmitterExtensions.c_LocalBuilder.Invoke(new object[] { index, type, null, isPinned });
			}
			localBuilder = (LocalBuilder)obj;
			FieldInfo fieldInfo = EmitterExtensions.f_LocalBuilder_position;
			if (fieldInfo != null)
			{
				fieldInfo.SetValue(localBuilder, (ushort)index);
			}
			FieldInfo fieldInfo2 = EmitterExtensions.f_LocalBuilder_is_pinned;
			if (fieldInfo2 != null)
			{
				fieldInfo2.SetValue(localBuilder, isPinned);
			}
			dictionary[localBuilder] = varDef;
			return localBuilder;
		}

		// Token: 0x0400010C RID: 268
		private static DynamicMethodDefinition emitDMD;

		// Token: 0x0400010D RID: 269
		private static MethodInfo emitDMDMethod;

		// Token: 0x0400010E RID: 270
		private static Action<CecilILGenerator, global::System.Reflection.Emit.OpCode, object> emitCodeDelegate;

		// Token: 0x0400010F RID: 271
		private static Dictionary<Type, Mono.Cecil.Cil.OpCode> storeOpCodes;

		// Token: 0x04000110 RID: 272
		private static Dictionary<Type, Mono.Cecil.Cil.OpCode> loadOpCodes;

		// Token: 0x04000111 RID: 273
		private static readonly ConstructorInfo c_LocalBuilder;

		// Token: 0x04000112 RID: 274
		private static readonly FieldInfo f_LocalBuilder_position;

		// Token: 0x04000113 RID: 275
		private static readonly FieldInfo f_LocalBuilder_is_pinned;

		// Token: 0x04000114 RID: 276
		private static int c_LocalBuilder_params;
	}
}
