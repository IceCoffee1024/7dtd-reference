using System;
using System.Reflection;
using System.Reflection.Emit;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x0200000F RID: 15
	public static class FastAccess
	{
		// Token: 0x0600002A RID: 42 RVA: 0x000025D4 File Offset: 0x000007D4
		public static InstantiationHandler<T> CreateInstantiationHandler<T>()
		{
			ConstructorInfo constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
			if (constructor == null)
			{
				throw new ApplicationException(string.Format("The type {0} must declare an empty constructor (the constructor may be private, internal, protected, protected internal, or public).", typeof(T)));
			}
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("InstantiateObject_" + typeof(T).Name, typeof(T), null);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Newobj, constructor);
			ilgenerator.Emit(OpCodes.Ret);
			return (InstantiationHandler<T>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(InstantiationHandler<T>));
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000267C File Offset: 0x0000087C
		[Obsolete("Use AccessTools.MethodDelegate<Func<T, S>>(PropertyInfo.GetGetMethod(true))")]
		public static GetterHandler<T, S> CreateGetterHandler<T, S>(PropertyInfo propertyInfo)
		{
			MethodInfo getMethod = propertyInfo.GetGetMethod(true);
			DynamicMethodDefinition dynamicMethodDefinition = FastAccess.CreateGetDynamicMethod<T, S>(propertyInfo.DeclaringType);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Call, getMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (GetterHandler<T, S>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(GetterHandler<T, S>));
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000026E0 File Offset: 0x000008E0
		[Obsolete("Use AccessTools.FieldRefAccess<T, S>(fieldInfo)")]
		public static GetterHandler<T, S> CreateGetterHandler<T, S>(FieldInfo fieldInfo)
		{
			DynamicMethodDefinition dynamicMethodDefinition = FastAccess.CreateGetDynamicMethod<T, S>(fieldInfo.DeclaringType);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return (GetterHandler<T, S>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(GetterHandler<T, S>));
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000273C File Offset: 0x0000093C
		[Obsolete("Use AccessTools.FieldRefAccess<T, S>(name) for fields and AccessTools.MethodDelegate<Func<T, S>>(AccessTools.PropertyGetter(typeof(T), name)) for properties")]
		public static GetterHandler<T, S> CreateFieldGetter<T, S>(params string[] names)
		{
			foreach (string text in names)
			{
				FieldInfo field = typeof(T).GetField(text, AccessTools.all);
				if (field != null)
				{
					return FastAccess.CreateGetterHandler<T, S>(field);
				}
				PropertyInfo property = typeof(T).GetProperty(text, AccessTools.all);
				if (property != null)
				{
					return FastAccess.CreateGetterHandler<T, S>(property);
				}
			}
			return null;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000027A4 File Offset: 0x000009A4
		[Obsolete("Use AccessTools.MethodDelegate<Action<T, S>>(PropertyInfo.GetSetMethod(true))")]
		public static SetterHandler<T, S> CreateSetterHandler<T, S>(PropertyInfo propertyInfo)
		{
			MethodInfo setMethod = propertyInfo.GetSetMethod(true);
			DynamicMethodDefinition dynamicMethodDefinition = FastAccess.CreateSetDynamicMethod<T, S>(propertyInfo.DeclaringType);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Call, setMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (SetterHandler<T, S>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(SetterHandler<T, S>));
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002814 File Offset: 0x00000A14
		[Obsolete("Use AccessTools.FieldRefAccess<T, S>(fieldInfo)")]
		public static SetterHandler<T, S> CreateSetterHandler<T, S>(FieldInfo fieldInfo)
		{
			DynamicMethodDefinition dynamicMethodDefinition = FastAccess.CreateSetDynamicMethod<T, S>(fieldInfo.DeclaringType);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Stfld, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return (SetterHandler<T, S>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(SetterHandler<T, S>));
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000287B File Offset: 0x00000A7B
		private static DynamicMethodDefinition CreateGetDynamicMethod<T, S>(Type type)
		{
			return new DynamicMethodDefinition("DynamicGet_" + type.Name, typeof(S), new Type[] { typeof(T) });
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000028B0 File Offset: 0x00000AB0
		private static DynamicMethodDefinition CreateSetDynamicMethod<T, S>(Type type)
		{
			return new DynamicMethodDefinition("DynamicSet_" + type.Name, typeof(void), new Type[]
			{
				typeof(T),
				typeof(S)
			});
		}
	}
}
