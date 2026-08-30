using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using MonoMod.Utils;

namespace HarmonyLib.Internal.Util
{
	// Token: 0x02000065 RID: 101
	internal class ReflectionTools
	{
		// Token: 0x0600044C RID: 1100 RVA: 0x000155D4 File Offset: 0x000137D4
		internal static ReflectionTools.TypeAndName TypColonName(string typeColonName)
		{
			if (typeColonName == null)
			{
				throw new ArgumentNullException("typeColonName");
			}
			string[] array = typeColonName.Split(new char[] { ':' });
			if (array.Length != 2)
			{
				throw new ArgumentException(" must be specified as 'Namespace.Type1.Type2:MemberName", "typeColonName");
			}
			return new ReflectionTools.TypeAndName
			{
				type = AccessTools.TypeByName(array[0]),
				name = array[1]
			};
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0001563C File Offset: 0x0001383C
		internal static void ValidateFieldType<F>(FieldInfo fieldInfo)
		{
			Type typeFromHandle = typeof(F);
			Type fieldType = fieldInfo.FieldType;
			if (typeFromHandle == fieldType)
			{
				return;
			}
			if (fieldType.IsEnum)
			{
				Type underlyingType = Enum.GetUnderlyingType(fieldType);
				if (typeFromHandle != underlyingType)
				{
					string text = "FieldRefAccess return type must be the same as FieldType or ";
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 1);
					defaultInterpolatedStringHandler.AppendLiteral("FieldType's underlying integral type (");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(underlyingType);
					defaultInterpolatedStringHandler.AppendLiteral(") for enum types");
					throw new ArgumentException(text + defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			else
			{
				if (fieldType.IsValueType)
				{
					throw new ArgumentException("FieldRefAccess return type must be the same as FieldType for value types");
				}
				if (!typeFromHandle.IsAssignableFrom(fieldType))
				{
					throw new ArgumentException("FieldRefAccess return type must be assignable from FieldType for reference types");
				}
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x000156E8 File Offset: 0x000138E8
		internal static AccessTools.FieldRef<T, F> FieldRefAccess<T, F>(FieldInfo fieldInfo, bool needCastclass)
		{
			ReflectionTools.ValidateFieldType<F>(fieldInfo);
			Type typeFromHandle = typeof(T);
			Type declaringType = fieldInfo.DeclaringType;
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("__refget_" + typeFromHandle.Name + "_fi_" + fieldInfo.Name, typeof(F).MakeByRefType(), new Type[] { typeFromHandle });
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			if (fieldInfo.IsStatic)
			{
				ilgenerator.Emit(OpCodes.Ldsflda, fieldInfo);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				if (needCastclass)
				{
					ilgenerator.Emit(OpCodes.Castclass, declaringType);
				}
				ilgenerator.Emit(OpCodes.Ldflda, fieldInfo);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (AccessTools.FieldRef<T, F>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(AccessTools.FieldRef<T, F>));
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x000157B0 File Offset: 0x000139B0
		internal static AccessTools.StructFieldRef<T, F> StructFieldRefAccess<T, F>(FieldInfo fieldInfo) where T : struct
		{
			ReflectionTools.ValidateFieldType<F>(fieldInfo);
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("__refget_" + typeof(T).Name + "_struct_fi_" + fieldInfo.Name, typeof(F).MakeByRefType(), new Type[] { typeof(T).MakeByRefType() });
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldflda, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return (AccessTools.StructFieldRef<T, F>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(AccessTools.StructFieldRef<T, F>));
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00015858 File Offset: 0x00013A58
		internal static AccessTools.FieldRef<F> StaticFieldRefAccess<F>(FieldInfo fieldInfo)
		{
			if (!fieldInfo.IsStatic)
			{
				throw new ArgumentException("Field must be static");
			}
			ReflectionTools.ValidateFieldType<F>(fieldInfo);
			string text = "__refget_";
			Type declaringType = fieldInfo.DeclaringType;
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(text + (((declaringType != null) ? declaringType.Name : null) ?? "null") + "_static_fi_" + fieldInfo.Name, typeof(F).MakeByRefType(), new Type[0]);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldsflda, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return (AccessTools.FieldRef<F>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(AccessTools.FieldRef<F>));
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00015904 File Offset: 0x00013B04
		internal static FieldInfo GetInstanceField(Type type, string fieldName)
		{
			FieldInfo fieldInfo = AccessTools.Field(type, fieldName);
			if (fieldInfo == null)
			{
				throw new MissingFieldException(type.Name, fieldName);
			}
			if (fieldInfo.IsStatic)
			{
				throw new ArgumentException("Field must not be static");
			}
			return fieldInfo;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00015940 File Offset: 0x00013B40
		internal static bool FieldRefNeedsClasscast(Type delegateInstanceType, Type declaringType)
		{
			bool flag = false;
			if (delegateInstanceType != declaringType)
			{
				flag = delegateInstanceType.IsAssignableFrom(declaringType);
				if (!flag && !declaringType.IsAssignableFrom(delegateInstanceType))
				{
					throw new ArgumentException("FieldDeclaringType must be assignable from or to T (FieldRefAccess instance type) - \"instanceOfT is FieldDeclaringType\" must be possible");
				}
			}
			return flag;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00015978 File Offset: 0x00013B78
		internal static void ValidateStructField<T, F>(FieldInfo fieldInfo) where T : struct
		{
			if (fieldInfo.IsStatic)
			{
				throw new ArgumentException("Field must not be static");
			}
			if (fieldInfo.DeclaringType != typeof(T))
			{
				throw new ArgumentException("FieldDeclaringType must be T (StructFieldRefAccess instance type)");
			}
		}

		// Token: 0x0400011C RID: 284
		internal static readonly bool isWindows = Environment.OSVersion.Platform.Equals(PlatformID.Win32NT);

		// Token: 0x020001E3 RID: 483
		internal struct TypeAndName
		{
			// Token: 0x040002A7 RID: 679
			internal Type type;

			// Token: 0x040002A8 RID: 680
			internal string name;
		}
	}
}
