using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using System.Threading;
using HarmonyLib.Internal.Util;
using HarmonyLib.Tools;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Core.Platforms;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x02000049 RID: 73
	public static class AccessTools
	{
		// Token: 0x0600017A RID: 378 RVA: 0x00008565 File Offset: 0x00006765
		public static IEnumerable<Assembly> AllAssemblies()
		{
			return from a in AppDomain.CurrentDomain.GetAssemblies()
				where !a.FullName.StartsWith("Microsoft.VisualStudio")
				select a;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00008598 File Offset: 0x00006798
		public static Type TypeByName(string name)
		{
			Type type = Type.GetType(name, false);
			if (type == null)
			{
				type = AccessTools.AllTypes().FirstOrDefault<Type>((Type t) => t.FullName == name);
			}
			if (type == null)
			{
				type = AccessTools.AllTypes().FirstOrDefault<Type>((Type t) => t.Name == name);
			}
			if (type == null)
			{
				Logger.Log(Logger.LogChannel.Warn, () => "AccessTools.TypeByName: Could not find type named " + name, false);
			}
			return type;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000860C File Offset: 0x0000680C
		public static Type[] GetTypesFromAssembly(Assembly assembly)
		{
			Type[] array;
			try
			{
				Type[] types = assembly.GetTypes();
				for (int i = 0; i < types.Length; i++)
				{
					if (types[i] == null)
					{
						return types.Where<Type>((Type type) => type != null).ToArray<Type>();
					}
				}
				array = types;
			}
			catch (ReflectionTypeLoadException ex)
			{
				ReflectionTypeLoadException ex2;
				ex = ex2;
				Logger.Log(Logger.LogChannel.Warn, delegate
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 2);
					defaultInterpolatedStringHandler.AppendLiteral("AccessTools.GetTypesFromAssembly: assembly ");
					defaultInterpolatedStringHandler.AppendFormatted<Assembly>(assembly);
					defaultInterpolatedStringHandler.AppendLiteral(" => ");
					defaultInterpolatedStringHandler.AppendFormatted<ReflectionTypeLoadException>(ex);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}, false);
				array = ex.Types.Where<Type>((Type type) => type != null).ToArray<Type>();
			}
			return array;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000086E4 File Offset: 0x000068E4
		public static IEnumerable<Type> AllTypes()
		{
			IEnumerable<Assembly> enumerable = AccessTools.AllAssemblies();
			Func<Assembly, IEnumerable<Type>> func;
			if ((func = AccessTools.<>O.<0>__GetTypesFromAssembly) == null)
			{
				func = (AccessTools.<>O.<0>__GetTypesFromAssembly = new Func<Assembly, IEnumerable<Type>>(AccessTools.GetTypesFromAssembly));
			}
			return enumerable.SelectMany<Assembly, Type>(func);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000870B File Offset: 0x0000690B
		public static IEnumerable<Type> InnerTypes(Type type)
		{
			return type.GetNestedTypes(AccessTools.all);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00008718 File Offset: 0x00006918
		public static T FindIncludingBaseTypes<T>(Type type, Func<Type, T> func) where T : class
		{
			T t;
			for (;;)
			{
				t = func(type);
				if (t != null)
				{
					break;
				}
				type = type.BaseType;
				if (type == null)
				{
					goto Block_1;
				}
			}
			return t;
			Block_1:
			return default(T);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000874C File Offset: 0x0000694C
		public static T FindIncludingInnerTypes<T>(Type type, Func<Type, T> func) where T : class
		{
			T t = func(type);
			if (t != null)
			{
				return t;
			}
			foreach (Type type2 in type.GetNestedTypes(AccessTools.all))
			{
				t = AccessTools.FindIncludingInnerTypes<T>(type2, func);
				if (t != null)
				{
					break;
				}
			}
			return t;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000879A File Offset: 0x0000699A
		public static MethodInfo Identifiable(this MethodInfo method)
		{
			return (PlatformTriple.Current.GetIdentifiable(method) as MethodInfo) ?? method;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000087B4 File Offset: 0x000069B4
		public static FieldInfo DeclaredField(Type type, string name)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.DeclaredField: type is null", false);
				return null;
			}
			if (name == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.DeclaredField: name is null", false);
				return null;
			}
			FieldInfo field = type.GetField(name, AccessTools.allDeclared);
			if (field == null)
			{
				Logger.Log(Logger.LogChannel.Warn, delegate
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 2);
					defaultInterpolatedStringHandler.AppendLiteral("AccessTools.DeclaredField: Could not find field for type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
					defaultInterpolatedStringHandler.AppendLiteral(" and name ");
					defaultInterpolatedStringHandler.AppendFormatted(name);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}, false);
			}
			return field;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00008830 File Offset: 0x00006A30
		public static FieldInfo DeclaredField(string typeColonName)
		{
			ReflectionTools.TypeAndName info = ReflectionTools.TypColonName(typeColonName);
			FieldInfo field = info.type.GetField(info.name, AccessTools.allDeclared);
			if (field == null)
			{
				Logger.Log(Logger.LogChannel.Warn, delegate
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 2);
					defaultInterpolatedStringHandler.AppendLiteral("AccessTools.DeclaredField: Could not find field for type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(info.type);
					defaultInterpolatedStringHandler.AppendLiteral(" and name ");
					defaultInterpolatedStringHandler.AppendFormatted(info.name);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}, false);
			}
			return field;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00008888 File Offset: 0x00006A88
		public static FieldInfo Field(Type type, string name)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.Field: type is null", false);
				return null;
			}
			if (name == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.Field: name is null", false);
				return null;
			}
			FieldInfo fieldInfo = AccessTools.FindIncludingBaseTypes<FieldInfo>(type, (Type t) => t.GetField(name, AccessTools.all));
			if (fieldInfo == null)
			{
				Logger.Log(Logger.LogChannel.Warn, delegate
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 2);
					defaultInterpolatedStringHandler.AppendLiteral("AccessTools.Field: Could not find field for type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
					defaultInterpolatedStringHandler.AppendLiteral(" and name ");
					defaultInterpolatedStringHandler.AppendFormatted(name);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}, false);
			}
			return fieldInfo;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00008904 File Offset: 0x00006B04
		public static FieldInfo Field(string typeColonName)
		{
			ReflectionTools.TypeAndName info = ReflectionTools.TypColonName(typeColonName);
			FieldInfo fieldInfo = AccessTools.FindIncludingBaseTypes<FieldInfo>(info.type, (Type t) => t.GetField(info.name, AccessTools.all));
			if (fieldInfo == null)
			{
				Logger.Log(Logger.LogChannel.Warn, delegate
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 2);
					defaultInterpolatedStringHandler.AppendLiteral("AccessTools.Field: Could not find field for type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(info.type);
					defaultInterpolatedStringHandler.AppendLiteral(" and name ");
					defaultInterpolatedStringHandler.AppendFormatted(info.name);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}, false);
			}
			return fieldInfo;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00008958 File Offset: 0x00006B58
		public static FieldInfo DeclaredField(Type type, int idx)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.DeclaredField: type is null", false);
				return null;
			}
			FieldInfo fieldInfo = AccessTools.GetDeclaredFields(type).ElementAtOrDefault<FieldInfo>(idx);
			if (fieldInfo == null)
			{
				Logger.Log(Logger.LogChannel.Warn, delegate
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 2);
					defaultInterpolatedStringHandler.AppendLiteral("AccessTools.DeclaredField: Could not find field for type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
					defaultInterpolatedStringHandler.AppendLiteral(" and idx ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(idx);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}, false);
			}
			return fieldInfo;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000089C0 File Offset: 0x00006BC0
		public static PropertyInfo DeclaredProperty(Type type, string name)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.DeclaredProperty: type is null", false);
				return null;
			}
			if (name == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.DeclaredProperty: name is null", false);
				return null;
			}
			PropertyInfo property = type.GetProperty(name, AccessTools.allDeclared);
			if (property == null)
			{
				Logger.Log(Logger.LogChannel.Warn, delegate
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(73, 2);
					defaultInterpolatedStringHandler.AppendLiteral("AccessTools.DeclaredProperty: Could not find property for type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
					defaultInterpolatedStringHandler.AppendLiteral(" and name ");
					defaultInterpolatedStringHandler.AppendFormatted(name);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}, false);
			}
			return property;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00008A3C File Offset: 0x00006C3C
		public static PropertyInfo DeclaredProperty(string typeColonName)
		{
			ReflectionTools.TypeAndName info = ReflectionTools.TypColonName(typeColonName);
			PropertyInfo property = info.type.GetProperty(info.name, AccessTools.allDeclared);
			if (property == null)
			{
				Logger.Log(Logger.LogChannel.Warn, delegate
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(73, 2);
					defaultInterpolatedStringHandler.AppendLiteral("AccessTools.DeclaredProperty: Could not find property for type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(info.type);
					defaultInterpolatedStringHandler.AppendLiteral(" and name ");
					defaultInterpolatedStringHandler.AppendFormatted(info.name);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}, false);
			}
			return property;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008A94 File Offset: 0x00006C94
		public static PropertyInfo DeclaredIndexer(Type type, Type[] parameters = null)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Debug, "AccessTools.DeclaredIndexer: type is null", false);
				return null;
			}
			PropertyInfo propertyInfo3;
			try
			{
				PropertyInfo propertyInfo;
				if (parameters != null)
				{
					propertyInfo = type.GetProperties(AccessTools.allDeclared).FirstOrDefault<PropertyInfo>((PropertyInfo property) => (from param in property.GetIndexParameters()
						select param.ParameterType).SequenceEqual<Type>(parameters));
				}
				else
				{
					propertyInfo = type.GetProperties(AccessTools.allDeclared).SingleOrDefault<PropertyInfo>((PropertyInfo property) => property.GetIndexParameters().Length != 0);
				}
				PropertyInfo propertyInfo2 = propertyInfo;
				if (propertyInfo2 == null)
				{
					Logger.Log(Logger.LogChannel.Debug, delegate
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(77, 2);
						defaultInterpolatedStringHandler.AppendLiteral("AccessTools.DeclaredIndexer: Could not find indexer for type ");
						defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
						defaultInterpolatedStringHandler.AppendLiteral(" and parameters ");
						Type[] parameters2 = parameters;
						defaultInterpolatedStringHandler.AppendFormatted((parameters2 != null) ? parameters2.Description() : null);
						return defaultInterpolatedStringHandler.ToStringAndClear();
					}, false);
				}
				propertyInfo3 = propertyInfo2;
			}
			catch (InvalidOperationException ex)
			{
				throw new AmbiguousMatchException("Multiple possible indexers were found.", ex);
			}
			return propertyInfo3;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00008B68 File Offset: 0x00006D68
		public static MethodInfo DeclaredPropertyGetter(Type type, string name)
		{
			PropertyInfo propertyInfo = AccessTools.DeclaredProperty(type, name);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetGetMethod(true);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00008B7D File Offset: 0x00006D7D
		public static MethodInfo DeclaredPropertyGetter(string typeColonName)
		{
			PropertyInfo propertyInfo = AccessTools.DeclaredProperty(typeColonName);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetGetMethod(true);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00008B91 File Offset: 0x00006D91
		public static MethodInfo DeclaredIndexerGetter(Type type, Type[] parameters = null)
		{
			PropertyInfo propertyInfo = AccessTools.DeclaredIndexer(type, parameters);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetGetMethod(true);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00008BA6 File Offset: 0x00006DA6
		public static MethodInfo DeclaredPropertySetter(Type type, string name)
		{
			PropertyInfo propertyInfo = AccessTools.DeclaredProperty(type, name);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetSetMethod(true);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00008BBB File Offset: 0x00006DBB
		public static MethodInfo DeclaredPropertySetter(string typeColonName)
		{
			PropertyInfo propertyInfo = AccessTools.DeclaredProperty(typeColonName);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetSetMethod(true);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00008BCF File Offset: 0x00006DCF
		public static MethodInfo DeclaredIndexerSetter(Type type, Type[] parameters)
		{
			PropertyInfo propertyInfo = AccessTools.DeclaredIndexer(type, parameters);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetSetMethod(true);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00008BE4 File Offset: 0x00006DE4
		public static PropertyInfo Property(Type type, string name)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.Property: type is null", false);
				return null;
			}
			if (name == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.Property: name is null", false);
				return null;
			}
			PropertyInfo propertyInfo = AccessTools.FindIncludingBaseTypes<PropertyInfo>(type, (Type t) => t.GetProperty(name, AccessTools.all));
			if (propertyInfo == null)
			{
				Logger.Log(Logger.LogChannel.Warn, delegate
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 2);
					defaultInterpolatedStringHandler.AppendLiteral("AccessTools.Property: Could not find property for type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
					defaultInterpolatedStringHandler.AppendLiteral(" and name ");
					defaultInterpolatedStringHandler.AppendFormatted(name);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}, false);
			}
			return propertyInfo;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00008C60 File Offset: 0x00006E60
		public static PropertyInfo Property(string typeColonName)
		{
			ReflectionTools.TypeAndName info = ReflectionTools.TypColonName(typeColonName);
			PropertyInfo propertyInfo = AccessTools.FindIncludingBaseTypes<PropertyInfo>(info.type, (Type t) => t.GetProperty(info.name, AccessTools.all));
			if (propertyInfo == null)
			{
				Logger.Log(Logger.LogChannel.Debug, delegate
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 2);
					defaultInterpolatedStringHandler.AppendLiteral("AccessTools.Property: Could not find property for type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(info.type);
					defaultInterpolatedStringHandler.AppendLiteral(" and name ");
					defaultInterpolatedStringHandler.AppendFormatted(info.name);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}, false);
			}
			return propertyInfo;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00008CB4 File Offset: 0x00006EB4
		public static PropertyInfo Indexer(Type type, Type[] parameters = null)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Debug, "AccessTools.Indexer: type is null", false);
				return null;
			}
			Func<Type, PropertyInfo> func;
			if (parameters != null)
			{
				Func<PropertyInfo, bool> <>9__3;
				func = delegate(Type t)
				{
					IEnumerable<PropertyInfo> properties = t.GetProperties(AccessTools.all);
					Func<PropertyInfo, bool> func3;
					if ((func3 = <>9__3) == null)
					{
						func3 = (<>9__3 = (PropertyInfo property) => (from param in property.GetIndexParameters()
							select param.ParameterType).SequenceEqual<Type>(parameters));
					}
					return properties.FirstOrDefault<PropertyInfo>(func3);
				};
			}
			else
			{
				func = (Type t) => t.GetProperties(AccessTools.all).SingleOrDefault<PropertyInfo>((PropertyInfo property) => property.GetIndexParameters().Length != 0);
			}
			Func<Type, PropertyInfo> func2 = func;
			PropertyInfo propertyInfo2;
			try
			{
				PropertyInfo propertyInfo = AccessTools.FindIncludingBaseTypes<PropertyInfo>(type, func2);
				if (propertyInfo == null)
				{
					Logger.Log(Logger.LogChannel.Debug, delegate
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(69, 2);
						defaultInterpolatedStringHandler.AppendLiteral("AccessTools.Indexer: Could not find indexer for type ");
						defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
						defaultInterpolatedStringHandler.AppendLiteral(" and parameters ");
						Type[] parameters2 = parameters;
						defaultInterpolatedStringHandler.AppendFormatted((parameters2 != null) ? parameters2.Description() : null);
						return defaultInterpolatedStringHandler.ToStringAndClear();
					}, false);
				}
				propertyInfo2 = propertyInfo;
			}
			catch (InvalidOperationException ex)
			{
				throw new AmbiguousMatchException("Multiple possible indexers were found.", ex);
			}
			return propertyInfo2;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00008D6C File Offset: 0x00006F6C
		public static MethodInfo PropertyGetter(Type type, string name)
		{
			PropertyInfo propertyInfo = AccessTools.Property(type, name);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetGetMethod(true);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00008D81 File Offset: 0x00006F81
		public static MethodInfo PropertyGetter(string typeColonName)
		{
			PropertyInfo propertyInfo = AccessTools.Property(typeColonName);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetGetMethod(true);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00008D95 File Offset: 0x00006F95
		public static MethodInfo IndexerGetter(Type type, Type[] parameters = null)
		{
			PropertyInfo propertyInfo = AccessTools.Indexer(type, parameters);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetGetMethod(true);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00008DAA File Offset: 0x00006FAA
		public static MethodInfo PropertySetter(Type type, string name)
		{
			PropertyInfo propertyInfo = AccessTools.Property(type, name);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetSetMethod(true);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00008DBF File Offset: 0x00006FBF
		public static MethodInfo PropertySetter(string typeColonName)
		{
			PropertyInfo propertyInfo = AccessTools.Property(typeColonName);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetSetMethod(true);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00008DD3 File Offset: 0x00006FD3
		public static MethodInfo IndexerSetter(Type type, Type[] parameters = null)
		{
			PropertyInfo propertyInfo = AccessTools.Indexer(type, parameters);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetSetMethod(true);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00008DE8 File Offset: 0x00006FE8
		public static MethodInfo DeclaredMethod(Type type, string name, Type[] parameters = null, Type[] generics = null)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.DeclaredMethod: type is null", false);
				return null;
			}
			if (name == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.DeclaredMethod: name is null", false);
				return null;
			}
			ParameterModifier[] array = new ParameterModifier[0];
			MethodInfo methodInfo;
			if (parameters == null)
			{
				methodInfo = type.GetMethod(name, AccessTools.allDeclared);
			}
			else
			{
				methodInfo = type.GetMethod(name, AccessTools.allDeclared, null, parameters, array);
			}
			if (methodInfo == null)
			{
				Logger.Log(Logger.LogChannel.Warn, delegate
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(85, 3);
					defaultInterpolatedStringHandler.AppendLiteral("AccessTools.DeclaredMethod: Could not find method for type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
					defaultInterpolatedStringHandler.AppendLiteral(" and name ");
					defaultInterpolatedStringHandler.AppendFormatted(name);
					defaultInterpolatedStringHandler.AppendLiteral(" and parameters ");
					Type[] parameters2 = parameters;
					defaultInterpolatedStringHandler.AppendFormatted((parameters2 != null) ? parameters2.Description() : null);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}, false);
				return null;
			}
			if (generics != null)
			{
				methodInfo = methodInfo.MakeGenericMethod(generics);
			}
			return methodInfo;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00008EA8 File Offset: 0x000070A8
		public static MethodInfo DeclaredMethod(string typeColonName, Type[] parameters = null, Type[] generics = null)
		{
			ReflectionTools.TypeAndName typeAndName = ReflectionTools.TypColonName(typeColonName);
			return AccessTools.DeclaredMethod(typeAndName.type, typeAndName.name, parameters, generics);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00008ED0 File Offset: 0x000070D0
		public static MethodInfo Method(Type type, string name, Type[] parameters = null, Type[] generics = null)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.Method: type is null", false);
				return null;
			}
			if (name == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.Method: name is null", false);
				return null;
			}
			ParameterModifier[] modifiers = new ParameterModifier[0];
			MethodInfo methodInfo;
			if (parameters == null)
			{
				try
				{
					methodInfo = AccessTools.FindIncludingBaseTypes<MethodInfo>(type, (Type t) => t.GetMethod(name, AccessTools.all));
					goto IL_00FB;
				}
				catch (AmbiguousMatchException ex)
				{
					methodInfo = AccessTools.FindIncludingBaseTypes<MethodInfo>(type, (Type t) => t.GetMethod(name, AccessTools.all, null, new Type[0], modifiers));
					if (methodInfo == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Ambiguous match in Harmony patch for ");
						defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
						defaultInterpolatedStringHandler.AppendLiteral(":");
						defaultInterpolatedStringHandler.AppendFormatted(name);
						throw new AmbiguousMatchException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
					}
					goto IL_00FB;
				}
			}
			methodInfo = AccessTools.FindIncludingBaseTypes<MethodInfo>(type, (Type t) => t.GetMethod(name, AccessTools.all, null, parameters, modifiers));
			IL_00FB:
			if (methodInfo == null)
			{
				Logger.Log(Logger.LogChannel.Warn, delegate
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(77, 3);
					defaultInterpolatedStringHandler2.AppendLiteral("AccessTools.Method: Could not find method for type ");
					defaultInterpolatedStringHandler2.AppendFormatted<Type>(type);
					defaultInterpolatedStringHandler2.AppendLiteral(" and name ");
					defaultInterpolatedStringHandler2.AppendFormatted(name);
					defaultInterpolatedStringHandler2.AppendLiteral(" and parameters ");
					Type[] parameters2 = parameters;
					defaultInterpolatedStringHandler2.AppendFormatted((parameters2 != null) ? parameters2.Description() : null);
					return defaultInterpolatedStringHandler2.ToStringAndClear();
				}, false);
				return null;
			}
			if (generics != null)
			{
				methodInfo = methodInfo.MakeGenericMethod(generics);
			}
			return methodInfo;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000900C File Offset: 0x0000720C
		public static MethodInfo Method(string typeColonName, Type[] parameters = null, Type[] generics = null)
		{
			ReflectionTools.TypeAndName typeAndName = ReflectionTools.TypColonName(typeColonName);
			return AccessTools.Method(typeAndName.type, typeAndName.name, parameters, generics);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00009034 File Offset: 0x00007234
		public static MethodInfo EnumeratorMoveNext(MethodBase method)
		{
			AccessTools.<>c__DisplayClass37_0 CS$<>8__locals1 = new AccessTools.<>c__DisplayClass37_0();
			CS$<>8__locals1.method = method;
			if (CS$<>8__locals1.method == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "EnumeratorMoveNext.Method: enumerator is null", false);
				return null;
			}
			ILContext ilcontext = new ILContext(new DynamicMethodDefinition(CS$<>8__locals1.method).Definition);
			ILCursor ilcursor = new ILCursor(ilcontext);
			if (ilcontext.Method.ReturnType.Name.StartsWith("UniTask"))
			{
				AccessTools.<>c__DisplayClass37_1 CS$<>8__locals2 = new AccessTools.<>c__DisplayClass37_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				AccessTools.<>c__DisplayClass37_1 CS$<>8__locals3 = CS$<>8__locals2;
				VariableDefinition variableDefinition = ilcontext.Body.Variables.FirstOrDefault<VariableDefinition>();
				CS$<>8__locals3.firstVar = ((variableDefinition != null) ? variableDefinition.VariableType : null);
				if (CS$<>8__locals2.firstVar != null && !CS$<>8__locals2.firstVar.Name.Contains(CS$<>8__locals2.CS$<>8__locals1.method.Name))
				{
					Logger.Log(Logger.LogChannel.Warn, () => "EnumeratorMoveNext.Method: Unexpected type name " + CS$<>8__locals2.firstVar.Name + ", should contain " + CS$<>8__locals2.CS$<>8__locals1.method.Name, false);
					return null;
				}
				CS$<>8__locals2.CS$<>8__locals1.enumeratorType = CS$<>8__locals2.firstVar.ResolveReflection();
			}
			else
			{
				MethodReference enumeratorCtor = null;
				ilcursor.GotoNext(new Func<Instruction, bool>[]
				{
					(Instruction i) => i.MatchNewobj(out enumeratorCtor)
				});
				if (enumeratorCtor == null)
				{
					Logger.Log(Logger.LogChannel.Warn, () => "EnumeratorMoveNext.Method: " + CS$<>8__locals1.method.FullDescription() + " does not create enumerators", false);
					return null;
				}
				if (enumeratorCtor.Name != ".ctor")
				{
					Logger.Log(Logger.LogChannel.Warn, delegate
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 2);
						defaultInterpolatedStringHandler.AppendLiteral("EnumeratorMoveNext.Method: ");
						defaultInterpolatedStringHandler.AppendFormatted(CS$<>8__locals1.method.FullDescription());
						defaultInterpolatedStringHandler.AppendLiteral(" does not create an enumerator (got ");
						defaultInterpolatedStringHandler.AppendFormatted(enumeratorCtor.GetID(null, null, true, true));
						defaultInterpolatedStringHandler.AppendLiteral(")");
						return defaultInterpolatedStringHandler.ToStringAndClear();
					}, false);
					return null;
				}
				CS$<>8__locals1.enumeratorType = enumeratorCtor.DeclaringType.ResolveReflection();
			}
			MethodInfo methodInfo = AccessTools.Method(CS$<>8__locals1.enumeratorType, "MoveNext", null, null);
			if (methodInfo == null)
			{
				Logger.Log(Logger.LogChannel.Warn, delegate
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(75, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("EnumeratorMoveNext.Method: ");
					defaultInterpolatedStringHandler2.AppendFormatted(CS$<>8__locals1.method.FullDescription());
					defaultInterpolatedStringHandler2.AppendLiteral(" creates an object ");
					defaultInterpolatedStringHandler2.AppendFormatted(CS$<>8__locals1.enumeratorType.FullDescription());
					defaultInterpolatedStringHandler2.AppendLiteral(" but it doesn't have MoveNext");
					return defaultInterpolatedStringHandler2.ToStringAndClear();
				}, false);
				return null;
			}
			return methodInfo;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00009204 File Offset: 0x00007404
		public static MethodInfo AsyncMoveNext(MethodBase method)
		{
			if (method == null)
			{
				Logger.LogText(Logger.LogChannel.Debug, "AccessTools.AsyncMoveNext: method is null", false);
				return null;
			}
			AsyncStateMachineAttribute customAttribute = method.GetCustomAttribute<AsyncStateMachineAttribute>();
			if (customAttribute == null)
			{
				Logger.Log(Logger.LogChannel.Debug, () => "AccessTools.AsyncMoveNext: Could not find AsyncStateMachine for " + method.FullDescription(), false);
				return null;
			}
			Type stateMachineType = customAttribute.StateMachineType;
			MethodInfo methodInfo = AccessTools.DeclaredMethod(stateMachineType, "MoveNext", null, null);
			if (methodInfo == null)
			{
				Logger.Log(Logger.LogChannel.Debug, () => "AccessTools.AsyncMoveNext: Could not find async method body for " + method.FullDescription(), false);
				return null;
			}
			return methodInfo;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000928C File Offset: 0x0000748C
		public static List<string> GetMethodNames(Type type)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.GetMethodNames: type is null", false);
				return new List<string>();
			}
			return (from m in AccessTools.GetDeclaredMethods(type)
				select m.Name).ToList<string>();
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000092DD File Offset: 0x000074DD
		public static List<string> GetMethodNames(object instance)
		{
			if (instance == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.GetMethodNames: instance is null", false);
				return new List<string>();
			}
			return AccessTools.GetMethodNames(instance.GetType());
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00009300 File Offset: 0x00007500
		public static List<string> GetFieldNames(Type type)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.GetFieldNames: type is null", false);
				return new List<string>();
			}
			return (from f in AccessTools.GetDeclaredFields(type)
				select f.Name).ToList<string>();
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00009351 File Offset: 0x00007551
		public static List<string> GetFieldNames(object instance)
		{
			if (instance == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.GetFieldNames: instance is null", false);
				return new List<string>();
			}
			return AccessTools.GetFieldNames(instance.GetType());
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00009374 File Offset: 0x00007574
		public static List<string> GetPropertyNames(Type type)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.GetPropertyNames: type is null", false);
				return new List<string>();
			}
			return (from f in AccessTools.GetDeclaredProperties(type)
				select f.Name).ToList<string>();
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000093C5 File Offset: 0x000075C5
		public static List<string> GetPropertyNames(object instance)
		{
			if (instance == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.GetPropertyNames: instance is null", false);
				return new List<string>();
			}
			return AccessTools.GetPropertyNames(instance.GetType());
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x000093E8 File Offset: 0x000075E8
		public static Type GetUnderlyingType(this MemberInfo member)
		{
			MemberTypes memberType = member.MemberType;
			if (memberType <= MemberTypes.Field)
			{
				if (memberType == MemberTypes.Event)
				{
					return ((EventInfo)member).EventHandlerType;
				}
				if (memberType == MemberTypes.Field)
				{
					return ((FieldInfo)member).FieldType;
				}
			}
			else
			{
				if (memberType == MemberTypes.Method)
				{
					return ((MethodInfo)member).ReturnType;
				}
				if (memberType == MemberTypes.Property)
				{
					return ((PropertyInfo)member).PropertyType;
				}
			}
			throw new ArgumentException("Member must be of type EventInfo, FieldInfo, MethodInfo, or PropertyInfo");
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00009459 File Offset: 0x00007659
		public static bool IsDeclaredMember<T>(this T member) where T : MemberInfo
		{
			return member.DeclaringType == member.ReflectedType;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00009478 File Offset: 0x00007678
		public static T GetDeclaredMember<T>(this T member) where T : MemberInfo
		{
			if (member.DeclaringType == null || member.IsDeclaredMember<T>())
			{
				return member;
			}
			int metadataToken = member.MetadataToken;
			Type declaringType = member.DeclaringType;
			MemberInfo[] array = ((declaringType != null) ? declaringType.GetMembers(AccessTools.all) : null) ?? new MemberInfo[0];
			foreach (MemberInfo memberInfo in array)
			{
				if (memberInfo.MetadataToken == metadataToken)
				{
					return (T)((object)memberInfo);
				}
			}
			return member;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x000094F8 File Offset: 0x000076F8
		public static ConstructorInfo DeclaredConstructor(Type type, Type[] parameters = null, bool searchForStatic = false)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.DeclaredConstructor: type is null", false);
				return null;
			}
			if (parameters == null)
			{
				parameters = new Type[0];
			}
			BindingFlags bindingFlags = (searchForStatic ? (AccessTools.allDeclared & ~BindingFlags.Instance) : (AccessTools.allDeclared & ~BindingFlags.Static));
			return type.GetConstructor(bindingFlags, null, parameters, new ParameterModifier[0]);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00009548 File Offset: 0x00007748
		public static ConstructorInfo Constructor(Type type, Type[] parameters = null, bool searchForStatic = false)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.ConstructorInfo: type is null", false);
				return null;
			}
			if (parameters == null)
			{
				parameters = new Type[0];
			}
			BindingFlags flags = (searchForStatic ? (AccessTools.all & ~BindingFlags.Instance) : (AccessTools.all & ~BindingFlags.Static));
			return AccessTools.FindIncludingBaseTypes<ConstructorInfo>(type, (Type t) => t.GetConstructor(flags, null, parameters, new ParameterModifier[0]));
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000095B4 File Offset: 0x000077B4
		public static List<ConstructorInfo> GetDeclaredConstructors(Type type, bool? searchForStatic = null)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.GetDeclaredConstructors: type is null", false);
				return new List<ConstructorInfo>();
			}
			BindingFlags bindingFlags = AccessTools.allDeclared;
			if (searchForStatic != null)
			{
				bindingFlags = (searchForStatic.Value ? (bindingFlags & ~BindingFlags.Instance) : (bindingFlags & ~BindingFlags.Static));
			}
			return (from method in type.GetConstructors(bindingFlags)
				where method.DeclaringType == type
				select method).ToList<ConstructorInfo>();
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00009630 File Offset: 0x00007830
		public static List<MethodInfo> GetDeclaredMethods(Type type)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.GetDeclaredMethods: type is null", false);
				return new List<MethodInfo>();
			}
			MethodInfo[] methods = type.GetMethods(AccessTools.allDeclared);
			List<MethodInfo> list = new List<MethodInfo>(methods.Length);
			list.AddRange(methods);
			return list;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00009670 File Offset: 0x00007870
		public static List<PropertyInfo> GetDeclaredProperties(Type type)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.GetDeclaredProperties: type is null", false);
				return new List<PropertyInfo>();
			}
			PropertyInfo[] properties = type.GetProperties(AccessTools.allDeclared);
			List<PropertyInfo> list = new List<PropertyInfo>(properties.Length);
			list.AddRange(properties);
			return list;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x000096B0 File Offset: 0x000078B0
		public static List<FieldInfo> GetDeclaredFields(Type type)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.GetDeclaredFields: type is null", false);
				return new List<FieldInfo>();
			}
			FieldInfo[] fields = type.GetFields(AccessTools.allDeclared);
			List<FieldInfo> list = new List<FieldInfo>(fields.Length);
			list.AddRange(fields);
			return list;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x000096F0 File Offset: 0x000078F0
		public static Type GetReturnedType(MethodBase methodOrConstructor)
		{
			if (methodOrConstructor == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.GetReturnedType: methodOrConstructor is null", false);
				return null;
			}
			ConstructorInfo constructorInfo = methodOrConstructor as ConstructorInfo;
			if (constructorInfo != null)
			{
				return typeof(void);
			}
			return ((MethodInfo)methodOrConstructor).ReturnType;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00009730 File Offset: 0x00007930
		public static Type Inner(Type type, string name)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.Inner: type is null", false);
				return null;
			}
			if (name == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.Inner: name is null", false);
				return null;
			}
			return AccessTools.FindIncludingBaseTypes<Type>(type, (Type t) => t.GetNestedType(name, AccessTools.all));
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00009784 File Offset: 0x00007984
		public static Type FirstInner(Type type, Func<Type, bool> predicate)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.FirstInner: type is null", false);
				return null;
			}
			if (predicate == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.FirstInner: predicate is null", false);
				return null;
			}
			return type.GetNestedTypes(AccessTools.all).FirstOrDefault<Type>((Type subType) => predicate(subType));
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x000097E4 File Offset: 0x000079E4
		public static MethodInfo FirstMethod(Type type, Func<MethodInfo, bool> predicate)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.FirstMethod: type is null", false);
				return null;
			}
			if (predicate == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.FirstMethod: predicate is null", false);
				return null;
			}
			return type.GetMethods(AccessTools.allDeclared).FirstOrDefault<MethodInfo>((MethodInfo method) => predicate(method));
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00009844 File Offset: 0x00007A44
		public static ConstructorInfo FirstConstructor(Type type, Func<ConstructorInfo, bool> predicate)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.FirstConstructor: type is null", false);
				return null;
			}
			if (predicate == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.FirstConstructor: predicate is null", false);
				return null;
			}
			return type.GetConstructors(AccessTools.allDeclared).FirstOrDefault<ConstructorInfo>((ConstructorInfo constructor) => predicate(constructor));
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000098A4 File Offset: 0x00007AA4
		public static PropertyInfo FirstProperty(Type type, Func<PropertyInfo, bool> predicate)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.FirstProperty: type is null", false);
				return null;
			}
			if (predicate == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.FirstProperty: predicate is null", false);
				return null;
			}
			return type.GetProperties(AccessTools.allDeclared).FirstOrDefault<PropertyInfo>((PropertyInfo property) => predicate(property));
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00009901 File Offset: 0x00007B01
		public static Type[] GetTypes(object[] parameters)
		{
			if (parameters == null)
			{
				return new Type[0];
			}
			return parameters.Select<object, Type>(delegate(object p)
			{
				if (p != null)
				{
					return p.GetType();
				}
				return typeof(object);
			}).ToArray<Type>();
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00009938 File Offset: 0x00007B38
		public static object[] ActualParameters(MethodBase method, object[] inputs)
		{
			List<Type> inputTypes = inputs.Select<object, Type>(delegate(object obj)
			{
				if (obj == null)
				{
					return null;
				}
				return obj.GetType();
			}).ToList<Type>();
			return (from p in method.GetParameters()
				select p.ParameterType).Select<Type, object>(delegate(Type pType)
			{
				int num = inputTypes.FindIndex((Type inType) => inType != null && pType.IsAssignableFrom(inType));
				if (num >= 0)
				{
					return inputs[num];
				}
				return AccessTools.GetDefaultValue(pType);
			}).ToArray<object>();
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000099C8 File Offset: 0x00007BC8
		public static AccessTools.FieldRef<T, F> FieldRefAccess<T, F>(string fieldName)
		{
			if (fieldName == null)
			{
				throw new ArgumentNullException("fieldName");
			}
			AccessTools.FieldRef<T, F> fieldRef;
			try
			{
				Type typeFromHandle = typeof(T);
				if (typeFromHandle.IsValueType)
				{
					throw new ArgumentException("T (FieldRefAccess instance type) must not be a value type");
				}
				fieldRef = ReflectionTools.FieldRefAccess<T, F>(ReflectionTools.GetInstanceField(typeFromHandle, fieldName), false);
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 3);
				defaultInterpolatedStringHandler.AppendLiteral("FieldRefAccess<");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(F));
				defaultInterpolatedStringHandler.AppendLiteral("> for ");
				defaultInterpolatedStringHandler.AppendFormatted(fieldName);
				defaultInterpolatedStringHandler.AppendLiteral(" caused an exception");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
			}
			return fieldRef;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00009A98 File Offset: 0x00007C98
		public static ref F FieldRefAccess<T, F>(T instance, string fieldName)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (fieldName == null)
			{
				throw new ArgumentNullException("fieldName");
			}
			ref F ptr;
			try
			{
				Type typeFromHandle = typeof(T);
				if (typeFromHandle.IsValueType)
				{
					throw new ArgumentException("T (FieldRefAccess instance type) must not be a value type");
				}
				ptr = ReflectionTools.FieldRefAccess<T, F>(ReflectionTools.GetInstanceField(typeFromHandle, fieldName), false)(instance);
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 4);
				defaultInterpolatedStringHandler.AppendLiteral("FieldRefAccess<");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(F));
				defaultInterpolatedStringHandler.AppendLiteral("> for ");
				defaultInterpolatedStringHandler.AppendFormatted<T>(instance);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted(fieldName);
				defaultInterpolatedStringHandler.AppendLiteral(" caused an exception");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
			}
			return ref ptr;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00009B98 File Offset: 0x00007D98
		public static AccessTools.FieldRef<object, F> FieldRefAccess<F>(Type type, string fieldName)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (fieldName == null)
			{
				throw new ArgumentNullException("fieldName");
			}
			AccessTools.FieldRef<object, F> fieldRef;
			try
			{
				FieldInfo fieldInfo = AccessTools.Field(type, fieldName);
				if (fieldInfo == null)
				{
					throw new MissingFieldException(type.Name, fieldName);
				}
				if (!fieldInfo.IsStatic)
				{
					Type declaringType = fieldInfo.DeclaringType;
					if (declaringType != null && declaringType.IsValueType)
					{
						throw new ArgumentException("Either FieldDeclaringType must be a class or field must be static");
					}
				}
				fieldRef = ReflectionTools.FieldRefAccess<object, F>(fieldInfo, true);
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 3);
				defaultInterpolatedStringHandler.AppendLiteral("FieldRefAccess<");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(F));
				defaultInterpolatedStringHandler.AppendLiteral("> for ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted(fieldName);
				defaultInterpolatedStringHandler.AppendLiteral(" caused an exception");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
			}
			return fieldRef;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00009C84 File Offset: 0x00007E84
		public static AccessTools.FieldRef<object, F> FieldRefAccess<F>(string typeColonName)
		{
			ReflectionTools.TypeAndName typeAndName = ReflectionTools.TypColonName(typeColonName);
			return AccessTools.FieldRefAccess<F>(typeAndName.type, typeAndName.name);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00009CAC File Offset: 0x00007EAC
		public static AccessTools.FieldRef<T, F> FieldRefAccess<T, F>(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			AccessTools.FieldRef<T, F> fieldRef;
			try
			{
				Type typeFromHandle = typeof(T);
				if (typeFromHandle.IsValueType)
				{
					throw new ArgumentException("T (FieldRefAccess instance type) must not be a value type");
				}
				bool flag = false;
				if (!fieldInfo.IsStatic)
				{
					Type declaringType = fieldInfo.DeclaringType;
					if (declaringType != null)
					{
						if (declaringType.IsValueType)
						{
							throw new ArgumentException("Either FieldDeclaringType must be a class or field must be static");
						}
						flag = ReflectionTools.FieldRefNeedsClasscast(typeFromHandle, declaringType);
					}
				}
				fieldRef = ReflectionTools.FieldRefAccess<T, F>(fieldInfo, flag);
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 3);
				defaultInterpolatedStringHandler.AppendLiteral("FieldRefAccess<");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(F));
				defaultInterpolatedStringHandler.AppendLiteral("> for ");
				defaultInterpolatedStringHandler.AppendFormatted<FieldInfo>(fieldInfo);
				defaultInterpolatedStringHandler.AppendLiteral(" caused an exception");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
			}
			return fieldRef;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00009DA8 File Offset: 0x00007FA8
		public static ref F FieldRefAccess<T, F>(T instance, FieldInfo fieldInfo)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			ref F ptr;
			try
			{
				Type typeFromHandle = typeof(T);
				if (typeFromHandle.IsValueType)
				{
					throw new ArgumentException("T (FieldRefAccess instance type) must not be a value type");
				}
				if (fieldInfo.IsStatic)
				{
					throw new ArgumentException("Field must not be static");
				}
				bool flag = false;
				Type declaringType = fieldInfo.DeclaringType;
				if (declaringType != null)
				{
					if (declaringType.IsValueType)
					{
						throw new ArgumentException("FieldDeclaringType must be a class");
					}
					flag = ReflectionTools.FieldRefNeedsClasscast(typeFromHandle, declaringType);
				}
				ptr = ReflectionTools.FieldRefAccess<T, F>(fieldInfo, flag)(instance);
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 4);
				defaultInterpolatedStringHandler.AppendLiteral("FieldRefAccess<");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(F));
				defaultInterpolatedStringHandler.AppendLiteral("> for ");
				defaultInterpolatedStringHandler.AppendFormatted<T>(instance);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<FieldInfo>(fieldInfo);
				defaultInterpolatedStringHandler.AppendLiteral(" caused an exception");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
			}
			return ref ptr;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00009EDC File Offset: 0x000080DC
		public static AccessTools.StructFieldRef<T, F> StructFieldRefAccess<T, F>(string fieldName) where T : struct
		{
			if (fieldName == null)
			{
				throw new ArgumentNullException("fieldName");
			}
			AccessTools.StructFieldRef<T, F> structFieldRef;
			try
			{
				structFieldRef = ReflectionTools.StructFieldRefAccess<T, F>(ReflectionTools.GetInstanceField(typeof(T), fieldName));
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 3);
				defaultInterpolatedStringHandler.AppendLiteral("StructFieldRefAccess<");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(F));
				defaultInterpolatedStringHandler.AppendLiteral("> for ");
				defaultInterpolatedStringHandler.AppendFormatted(fieldName);
				defaultInterpolatedStringHandler.AppendLiteral(" caused an exception");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
			}
			return structFieldRef;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00009F94 File Offset: 0x00008194
		public static ref F StructFieldRefAccess<T, F>(ref T instance, string fieldName) where T : struct
		{
			if (fieldName == null)
			{
				throw new ArgumentNullException("fieldName");
			}
			ref F ptr;
			try
			{
				ptr = ReflectionTools.StructFieldRefAccess<T, F>(ReflectionTools.GetInstanceField(typeof(T), fieldName))(ref instance);
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 4);
				defaultInterpolatedStringHandler.AppendLiteral("StructFieldRefAccess<");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(F));
				defaultInterpolatedStringHandler.AppendLiteral("> for ");
				defaultInterpolatedStringHandler.AppendFormatted<T>(instance);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted(fieldName);
				defaultInterpolatedStringHandler.AppendLiteral(" caused an exception");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
			}
			return ref ptr;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000A070 File Offset: 0x00008270
		public static AccessTools.StructFieldRef<T, F> StructFieldRefAccess<T, F>(FieldInfo fieldInfo) where T : struct
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			AccessTools.StructFieldRef<T, F> structFieldRef;
			try
			{
				ReflectionTools.ValidateStructField<T, F>(fieldInfo);
				structFieldRef = ReflectionTools.StructFieldRefAccess<T, F>(fieldInfo);
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 3);
				defaultInterpolatedStringHandler.AppendLiteral("StructFieldRefAccess<");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(F));
				defaultInterpolatedStringHandler.AppendLiteral("> for ");
				defaultInterpolatedStringHandler.AppendFormatted<FieldInfo>(fieldInfo);
				defaultInterpolatedStringHandler.AppendLiteral(" caused an exception");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
			}
			return structFieldRef;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000A120 File Offset: 0x00008320
		public static ref F StructFieldRefAccess<T, F>(ref T instance, FieldInfo fieldInfo) where T : struct
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			ref F ptr;
			try
			{
				ReflectionTools.ValidateStructField<T, F>(fieldInfo);
				ptr = ReflectionTools.StructFieldRefAccess<T, F>(fieldInfo)(ref instance);
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 4);
				defaultInterpolatedStringHandler.AppendLiteral("StructFieldRefAccess<");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(F));
				defaultInterpolatedStringHandler.AppendLiteral("> for ");
				defaultInterpolatedStringHandler.AppendFormatted<T>(instance);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<FieldInfo>(fieldInfo);
				defaultInterpolatedStringHandler.AppendLiteral(" caused an exception");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
			}
			return ref ptr;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000A1F4 File Offset: 0x000083F4
		public static ref F StaticFieldRefAccess<T, F>(string fieldName)
		{
			return AccessTools.StaticFieldRefAccess<F>(typeof(T), fieldName);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000A208 File Offset: 0x00008408
		public static ref F StaticFieldRefAccess<F>(Type type, string fieldName)
		{
			ref F ptr;
			try
			{
				FieldInfo fieldInfo = AccessTools.Field(type, fieldName);
				if (fieldInfo == null)
				{
					throw new MissingFieldException(type.Name, fieldName);
				}
				ptr = ReflectionTools.StaticFieldRefAccess<F>(fieldInfo)();
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 3);
				defaultInterpolatedStringHandler.AppendLiteral("StaticFieldRefAccess<");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(F));
				defaultInterpolatedStringHandler.AppendLiteral("> for ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted(fieldName);
				defaultInterpolatedStringHandler.AppendLiteral(" caused an exception");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
			}
			return ref ptr;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000A2B8 File Offset: 0x000084B8
		public static ref F StaticFieldRefAccess<F>(string typeColonName)
		{
			ReflectionTools.TypeAndName typeAndName = ReflectionTools.TypColonName(typeColonName);
			return AccessTools.StaticFieldRefAccess<F>(typeAndName.type, typeAndName.name);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000A2E0 File Offset: 0x000084E0
		public static ref F StaticFieldRefAccess<T, F>(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			ref F ptr;
			try
			{
				ptr = ReflectionTools.StaticFieldRefAccess<F>(fieldInfo)();
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 3);
				defaultInterpolatedStringHandler.AppendLiteral("StaticFieldRefAccess<");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(F));
				defaultInterpolatedStringHandler.AppendLiteral("> for ");
				defaultInterpolatedStringHandler.AppendFormatted<FieldInfo>(fieldInfo);
				defaultInterpolatedStringHandler.AppendLiteral(" caused an exception");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
			}
			return ref ptr;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000A390 File Offset: 0x00008590
		public static AccessTools.FieldRef<F> StaticFieldRefAccess<F>(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			AccessTools.FieldRef<F> fieldRef;
			try
			{
				fieldRef = ReflectionTools.StaticFieldRefAccess<F>(fieldInfo);
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 2);
				defaultInterpolatedStringHandler.AppendLiteral("StaticFieldRefAccess<");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(F));
				defaultInterpolatedStringHandler.AppendLiteral("> for ");
				defaultInterpolatedStringHandler.AppendFormatted<FieldInfo>(fieldInfo);
				defaultInterpolatedStringHandler.AppendLiteral(" caused an exception");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
			}
			return fieldRef;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000A41C File Offset: 0x0000861C
		public static DelegateType MethodDelegate<DelegateType>(MethodInfo method, object instance = null, bool virtualCall = true) where DelegateType : Delegate
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			Type typeFromHandle = typeof(DelegateType);
			if (method.IsStatic)
			{
				return (DelegateType)((object)Delegate.CreateDelegate(typeFromHandle, method));
			}
			Type type = method.DeclaringType;
			if (type != null && type.IsInterface && !virtualCall)
			{
				throw new ArgumentException("Interface methods must be called virtually");
			}
			if (instance == null)
			{
				ParameterInfo[] parameters = typeFromHandle.GetMethod("Invoke").GetParameters();
				if (parameters.Length == 0)
				{
					Delegate.CreateDelegate(typeof(DelegateType), method);
					throw new ArgumentException("Invalid delegate type");
				}
				Type parameterType = parameters[0].ParameterType;
				if (type != null && type.IsInterface && parameterType.IsValueType)
				{
					InterfaceMapping interfaceMap = parameterType.GetInterfaceMap(type);
					method = interfaceMap.TargetMethods[Array.IndexOf<MethodInfo>(interfaceMap.InterfaceMethods, method)];
					type = parameterType;
				}
				if (type != null && virtualCall)
				{
					if (type.IsInterface)
					{
						return (DelegateType)((object)Delegate.CreateDelegate(typeFromHandle, method));
					}
					if (parameterType.IsInterface)
					{
						InterfaceMapping interfaceMap2 = type.GetInterfaceMap(parameterType);
						MethodInfo methodInfo = interfaceMap2.InterfaceMethods[Array.IndexOf<MethodInfo>(interfaceMap2.TargetMethods, method)];
						return (DelegateType)((object)Delegate.CreateDelegate(typeFromHandle, methodInfo));
					}
					if (!type.IsValueType)
					{
						return (DelegateType)((object)Delegate.CreateDelegate(typeFromHandle, method.GetBaseDefinition()));
					}
				}
				ParameterInfo[] parameters2 = method.GetParameters();
				int num = parameters2.Length;
				Type[] array = new Type[num + 1];
				array[0] = type;
				for (int i = 0; i < num; i++)
				{
					array[i + 1] = parameters2[i].ParameterType;
				}
				DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("OpenInstanceDelegate_" + method.Name, method.ReturnType, array);
				ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
				if (type != null && type.IsValueType)
				{
					ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Ldarga_S, 0);
				}
				else
				{
					ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Ldarg_0);
				}
				for (int j = 1; j < array.Length; j++)
				{
					ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Ldarg, j);
				}
				ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Call, method);
				ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Ret);
				return (DelegateType)((object)dynamicMethodDefinition.Generate().CreateDelegate(typeFromHandle));
			}
			else
			{
				if (virtualCall)
				{
					return (DelegateType)((object)Delegate.CreateDelegate(typeFromHandle, instance, method.GetBaseDefinition()));
				}
				if (type != null && !type.IsInstanceOfType(instance))
				{
					Delegate.CreateDelegate(typeof(DelegateType), instance, method);
					throw new ArgumentException("Invalid delegate type");
				}
				if (AccessTools.IsMonoRuntime)
				{
					DynamicMethodDefinition dynamicMethodDefinition2 = new DynamicMethodDefinition("LdftnDelegate_" + method.Name, typeFromHandle, new Type[] { typeof(object) });
					ILGenerator ilgenerator2 = dynamicMethodDefinition2.GetILGenerator();
					ilgenerator2.Emit(global::System.Reflection.Emit.OpCodes.Ldarg_0);
					ilgenerator2.Emit(global::System.Reflection.Emit.OpCodes.Ldftn, method);
					ilgenerator2.Emit(global::System.Reflection.Emit.OpCodes.Newobj, typeFromHandle.GetConstructor(new Type[]
					{
						typeof(object),
						typeof(IntPtr)
					}));
					ilgenerator2.Emit(global::System.Reflection.Emit.OpCodes.Ret);
					return (DelegateType)((object)dynamicMethodDefinition2.Generate().Invoke(null, new object[] { instance }));
				}
				return (DelegateType)((object)Activator.CreateInstance(typeFromHandle, new object[]
				{
					instance,
					method.MethodHandle.GetFunctionPointer()
				}));
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000A76C File Offset: 0x0000896C
		public static DelegateType MethodDelegate<DelegateType>(string typeColonName, object instance = null, bool virtualCall = true) where DelegateType : Delegate
		{
			MethodInfo methodInfo = AccessTools.DeclaredMethod(typeColonName, null, null);
			return AccessTools.MethodDelegate<DelegateType>(methodInfo, instance, virtualCall);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000A78C File Offset: 0x0000898C
		public static DelegateType HarmonyDelegate<DelegateType>(object instance = null) where DelegateType : Delegate
		{
			HarmonyMethod mergedFromType = HarmonyMethodExtensions.GetMergedFromType(typeof(DelegateType));
			HarmonyMethod harmonyMethod = mergedFromType;
			MethodType methodType = harmonyMethod.methodType.GetValueOrDefault();
			if (harmonyMethod.methodType == null)
			{
				methodType = MethodType.Normal;
				harmonyMethod.methodType = new MethodType?(methodType);
			}
			MethodInfo methodInfo = mergedFromType.GetOriginalMethod() as MethodInfo;
			if (methodInfo == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Delegate ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(DelegateType));
				defaultInterpolatedStringHandler.AppendLiteral(" has no defined original method");
				throw new NullReferenceException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return AccessTools.MethodDelegate<DelegateType>(methodInfo, instance, !mergedFromType.nonVirtualDelegate);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000A834 File Offset: 0x00008A34
		public static MethodBase GetOutsideCaller()
		{
			StackTrace stackTrace = new StackTrace(true);
			foreach (StackFrame stackFrame in stackTrace.GetFrames())
			{
				MethodBase method = stackFrame.GetMethod();
				Type declaringType = method.DeclaringType;
				if (((declaringType != null) ? declaringType.Namespace : null) != typeof(Harmony).Namespace)
				{
					return method;
				}
			}
			throw new Exception("Unexpected end of stack trace");
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000A89F File Offset: 0x00008A9F
		public static void RethrowException(Exception exception)
		{
			ExceptionDispatchInfo.Capture(exception).Throw();
			throw exception;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000A8AD File Offset: 0x00008AAD
		public static bool IsMonoRuntime { get; } = Type.GetType("Mono.Runtime") != null;

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060001CB RID: 459 RVA: 0x0000A8B4 File Offset: 0x00008AB4
		public static bool IsNetFrameworkRuntime { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060001CC RID: 460 RVA: 0x0000A8BB File Offset: 0x00008ABB
		public static bool IsNetCoreRuntime { get; }

		// Token: 0x060001CD RID: 461 RVA: 0x0000A8C4 File Offset: 0x00008AC4
		public static void ThrowMissingMemberException(Type type, params string[] names)
		{
			string text = string.Join(",", AccessTools.GetFieldNames(type).ToArray());
			string text2 = string.Join(",", AccessTools.GetPropertyNames(type).ToArray());
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 3);
			defaultInterpolatedStringHandler.AppendFormatted(string.Join(",", names));
			defaultInterpolatedStringHandler.AppendLiteral("; available fields: ");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral("; available properties: ");
			defaultInterpolatedStringHandler.AppendFormatted(text2);
			throw new MissingMemberException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000A94D File Offset: 0x00008B4D
		public static object GetDefaultValue(Type type)
		{
			if (type == null)
			{
				Logger.LogText(Logger.LogChannel.Warn, "AccessTools.GetDefaultValue: type is null", false);
				return null;
			}
			if (type == typeof(void))
			{
				return null;
			}
			if (type.IsValueType)
			{
				return Activator.CreateInstance(type);
			}
			return null;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000A984 File Offset: 0x00008B84
		public static object CreateInstance(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, CallingConventions.Any, new Type[0], null);
			if (constructor != null)
			{
				return constructor.Invoke(null);
			}
			return FormatterServices.GetUninitializedObject(type);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000A9C4 File Offset: 0x00008BC4
		public static T CreateInstance<T>()
		{
			object obj = AccessTools.CreateInstance(typeof(T));
			if (obj is T)
			{
				return (T)((object)obj);
			}
			return default(T);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000A9FB File Offset: 0x00008BFB
		public static T MakeDeepCopy<T>(object source) where T : class
		{
			return AccessTools.MakeDeepCopy(source, typeof(T), null, "") as T;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000AA1D File Offset: 0x00008C1D
		public static void MakeDeepCopy<T>(object source, out T result, Func<string, Traverse, Traverse, object> processor = null, string pathRoot = "")
		{
			result = (T)((object)AccessTools.MakeDeepCopy(source, typeof(T), processor, pathRoot));
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000AA3C File Offset: 0x00008C3C
		public static object MakeDeepCopy(object source, Type resultType, Func<string, Traverse, Traverse, object> processor = null, string pathRoot = "")
		{
			if (source == null || resultType == null)
			{
				return null;
			}
			resultType = Nullable.GetUnderlyingType(resultType) ?? resultType;
			Type type = source.GetType();
			if (type.IsPrimitive)
			{
				return source;
			}
			if (type.IsEnum)
			{
				return Enum.ToObject(resultType, (int)source);
			}
			if (type.IsGenericType && resultType.IsGenericType)
			{
				AccessTools.addHandlerCacheLock.EnterUpgradeableReadLock();
				try
				{
					FastInvokeHandler handler;
					if (!AccessTools.addHandlerCache.TryGetValue(resultType, out handler))
					{
						MethodInfo methodInfo = AccessTools.FirstMethod(resultType, (MethodInfo m) => m.Name == "Add" && m.GetParameters().Length == 1);
						if (methodInfo != null)
						{
							handler = MethodInvoker.GetHandler(methodInfo, false);
						}
						AccessTools.addHandlerCacheLock.EnterWriteLock();
						try
						{
							AccessTools.addHandlerCache[resultType] = handler;
						}
						finally
						{
							AccessTools.addHandlerCacheLock.ExitWriteLock();
						}
					}
					if (handler != null)
					{
						object obj = Activator.CreateInstance(resultType);
						Type type2 = resultType.GetGenericArguments()[0];
						int num = 0;
						foreach (object obj2 in (source as IEnumerable))
						{
							string text = num++.ToString();
							string text2 = ((pathRoot.Length > 0) ? (pathRoot + "." + text) : text);
							object obj3 = AccessTools.MakeDeepCopy(obj2, type2, processor, text2);
							handler(obj, new object[] { obj3 });
						}
						return obj;
					}
				}
				finally
				{
					AccessTools.addHandlerCacheLock.ExitUpgradeableReadLock();
				}
			}
			if (type.IsArray && resultType.IsArray)
			{
				Type elementType = resultType.GetElementType();
				int length = ((Array)source).Length;
				object[] array = Activator.CreateInstance(resultType, new object[] { length }) as object[];
				object[] array2 = source as object[];
				for (int i = 0; i < length; i++)
				{
					string text3 = i.ToString();
					string text4 = ((pathRoot.Length > 0) ? (pathRoot + "." + text3) : text3);
					array[i] = AccessTools.MakeDeepCopy(array2[i], elementType, processor, text4);
				}
				return array;
			}
			string @namespace = type.Namespace;
			if (@namespace == "System" || (@namespace != null && @namespace.StartsWith("System.")))
			{
				return source;
			}
			object obj4 = AccessTools.CreateInstance((resultType == typeof(object)) ? type : resultType);
			Traverse.IterateFields(source, obj4, delegate(string name, Traverse src, Traverse dst)
			{
				string text5 = ((pathRoot.Length > 0) ? (pathRoot + "." + name) : name);
				object obj5 = ((processor != null) ? processor(text5, src, dst) : src.GetValue());
				dst.SetValue(AccessTools.MakeDeepCopy(obj5, dst.GetValueType(), processor, text5));
			});
			return obj4;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000AD3C File Offset: 0x00008F3C
		public static bool IsStruct(Type type)
		{
			return !(type == null) && (type.IsValueType && !AccessTools.IsValue(type)) && !AccessTools.IsVoid(type);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000AD64 File Offset: 0x00008F64
		public static bool IsClass(Type type)
		{
			return !(type == null) && !type.IsValueType;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000AD7A File Offset: 0x00008F7A
		public static bool IsValue(Type type)
		{
			return !(type == null) && (type.IsPrimitive || type.IsEnum);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000AD98 File Offset: 0x00008F98
		public static bool IsInteger(Type type)
		{
			if (type == null)
			{
				return false;
			}
			TypeCode typeCode = Type.GetTypeCode(type);
			return typeCode - TypeCode.SByte <= 7;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000ADC4 File Offset: 0x00008FC4
		public static bool IsFloatingPoint(Type type)
		{
			if (type == null)
			{
				return false;
			}
			TypeCode typeCode = Type.GetTypeCode(type);
			return typeCode - TypeCode.Single <= 2;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000ADF1 File Offset: 0x00008FF1
		public static bool IsNumber(Type type)
		{
			return AccessTools.IsInteger(type) || AccessTools.IsFloatingPoint(type);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000AE03 File Offset: 0x00009003
		public static bool IsVoid(Type type)
		{
			return type == typeof(void);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000AE15 File Offset: 0x00009015
		public static bool IsOfNullableType<T>(T instance)
		{
			return Nullable.GetUnderlyingType(typeof(T)) != null;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000AE2C File Offset: 0x0000902C
		public static bool IsStatic(MemberInfo member)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			MemberTypes memberType = member.MemberType;
			if (memberType <= MemberTypes.Method)
			{
				switch (memberType)
				{
				case MemberTypes.Constructor:
					break;
				case MemberTypes.Event:
					return AccessTools.IsStatic((EventInfo)member);
				case MemberTypes.Constructor | MemberTypes.Event:
					goto IL_0091;
				case MemberTypes.Field:
					return ((FieldInfo)member).IsStatic;
				default:
					if (memberType != MemberTypes.Method)
					{
						goto IL_0091;
					}
					break;
				}
				return ((MethodBase)member).IsStatic;
			}
			if (memberType == MemberTypes.Property)
			{
				return AccessTools.IsStatic((PropertyInfo)member);
			}
			if (memberType == MemberTypes.TypeInfo || memberType == MemberTypes.NestedType)
			{
				return AccessTools.IsStatic((Type)member);
			}
			IL_0091:
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Unknown member type: ");
			defaultInterpolatedStringHandler.AppendFormatted<MemberTypes>(member.MemberType);
			throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000AEFB File Offset: 0x000090FB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool IsStatic(Type type)
		{
			return type != null && type.IsAbstract && type.IsSealed;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000AF12 File Offset: 0x00009112
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool IsStatic(PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentNullException("propertyInfo");
			}
			return propertyInfo.GetAccessors(true)[0].IsStatic;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000AF30 File Offset: 0x00009130
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool IsStatic(EventInfo eventInfo)
		{
			if (eventInfo == null)
			{
				throw new ArgumentNullException("eventInfo");
			}
			return eventInfo.GetAddMethod(true).IsStatic;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000AF4C File Offset: 0x0000914C
		public static int CombinedHashCode(IEnumerable<object> objects)
		{
			int num = 352654597;
			int num2 = num;
			int num3 = 0;
			foreach (object obj in objects)
			{
				if (num3 % 2 == 0)
				{
					num = ((num << 5) + num + (num >> 27)) ^ obj.GetHashCode();
				}
				else
				{
					num2 = ((num2 << 5) + num2 + (num2 >> 27)) ^ obj.GetHashCode();
				}
				num3++;
			}
			return num + num2 * 1566083941;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000AFD4 File Offset: 0x000091D4
		// Note: this type is marked as 'beforefieldinit'.
		static AccessTools()
		{
			Type type = Type.GetType("System.Runtime.InteropServices.RuntimeInformation", false);
			AccessTools.IsNetFrameworkRuntime = ((type != null) ? type.GetProperty("FrameworkDescription").GetValue(null, null).ToString()
				.StartsWith(".NET Framework") : (!AccessTools.IsMonoRuntime));
			Type type2 = Type.GetType("System.Runtime.InteropServices.RuntimeInformation", false);
			AccessTools.IsNetCoreRuntime = type2 != null && type2.GetProperty("FrameworkDescription").GetValue(null, null).ToString()
				.StartsWith(".NET Core");
			AccessTools.addHandlerCache = new Dictionary<Type, FastInvokeHandler>();
			AccessTools.addHandlerCacheLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		}

		// Token: 0x040000A6 RID: 166
		public static readonly BindingFlags all = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;

		// Token: 0x040000A7 RID: 167
		public static readonly BindingFlags allDeclared = AccessTools.all | BindingFlags.DeclaredOnly;

		// Token: 0x040000AB RID: 171
		private static readonly Dictionary<Type, FastInvokeHandler> addHandlerCache;

		// Token: 0x040000AC RID: 172
		private static readonly ReaderWriterLockSlim addHandlerCacheLock;

		// Token: 0x020000AB RID: 171
		// (Invoke) Token: 0x0600054F RID: 1359
		public delegate ref F FieldRef<in T, F>(T instance = default(T));

		// Token: 0x020000AC RID: 172
		// (Invoke) Token: 0x06000553 RID: 1363
		public delegate ref F StructFieldRef<T, F>(ref T instance) where T : struct;

		// Token: 0x020000AD RID: 173
		// (Invoke) Token: 0x06000557 RID: 1367
		public delegate ref F FieldRef<F>();

		// Token: 0x020000AE RID: 174
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040001E3 RID: 483
			public static Func<Assembly, IEnumerable<Type>> <0>__GetTypesFromAssembly;
		}
	}
}
