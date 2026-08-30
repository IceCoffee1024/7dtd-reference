using System;
using System.Collections.Generic;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000013 RID: 19
	internal class AccessCache
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00002C60 File Offset: 0x00000E60
		private static T Get<T>(Dictionary<Type, Dictionary<string, T>> dict, Type type, string name, Func<T> fetcher)
		{
			T t2;
			lock (dict)
			{
				Dictionary<string, T> dictionary;
				if (!dict.TryGetValue(type, out dictionary))
				{
					dictionary = new Dictionary<string, T>();
					dict[type] = dictionary;
				}
				T t;
				if (!dictionary.TryGetValue(name, out t))
				{
					t = fetcher();
					dictionary[name] = t;
				}
				t2 = t;
			}
			return t2;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002CD0 File Offset: 0x00000ED0
		private static T Get<T>(Dictionary<Type, Dictionary<string, Dictionary<int, T>>> dict, Type type, string name, Type[] arguments, Func<T> fetcher)
		{
			T t2;
			lock (dict)
			{
				Dictionary<string, Dictionary<int, T>> dictionary;
				if (!dict.TryGetValue(type, out dictionary))
				{
					dictionary = new Dictionary<string, Dictionary<int, T>>();
					dict[type] = dictionary;
				}
				Dictionary<int, T> dictionary2;
				if (!dictionary.TryGetValue(name, out dictionary2))
				{
					dictionary2 = new Dictionary<int, T>();
					dictionary[name] = dictionary2;
				}
				int num = AccessTools.CombinedHashCode(arguments);
				T t;
				if (!dictionary2.TryGetValue(num, out t))
				{
					t = fetcher();
					dictionary2[num] = t;
				}
				t2 = t;
			}
			return t2;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002D64 File Offset: 0x00000F64
		internal FieldInfo GetFieldInfo(Type type, string name, AccessCache.MemberType memberType = AccessCache.MemberType.Any, bool declaredOnly = false)
		{
			FieldInfo fieldInfo = AccessCache.Get<FieldInfo>(this.declaredFields, type, name, () => type.GetField(name, AccessCache.declaredOnlyBindingFlags[memberType]));
			if (fieldInfo == null && !declaredOnly)
			{
				Func<Type, FieldInfo> <>9__2;
				fieldInfo = AccessCache.Get<FieldInfo>(this.inheritedFields, type, name, delegate
				{
					Type type2 = type;
					Func<Type, FieldInfo> func;
					if ((func = <>9__2) == null)
					{
						func = (<>9__2 = (Type t) => t.GetField(name, AccessTools.all));
					}
					return AccessTools.FindIncludingBaseTypes<FieldInfo>(type2, func);
				});
			}
			return fieldInfo;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002DDC File Offset: 0x00000FDC
		internal PropertyInfo GetPropertyInfo(Type type, string name, AccessCache.MemberType memberType = AccessCache.MemberType.Any, bool declaredOnly = false)
		{
			PropertyInfo propertyInfo = AccessCache.Get<PropertyInfo>(this.declaredProperties, type, name, () => type.GetProperty(name, AccessCache.declaredOnlyBindingFlags[memberType]));
			if (propertyInfo == null && !declaredOnly)
			{
				Func<Type, PropertyInfo> <>9__2;
				propertyInfo = AccessCache.Get<PropertyInfo>(this.inheritedProperties, type, name, delegate
				{
					Type type2 = type;
					Func<Type, PropertyInfo> func;
					if ((func = <>9__2) == null)
					{
						func = (<>9__2 = (Type t) => t.GetProperty(name, AccessTools.all));
					}
					return AccessTools.FindIncludingBaseTypes<PropertyInfo>(type2, func);
				});
			}
			return propertyInfo;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002E54 File Offset: 0x00001054
		internal MethodBase GetMethodInfo(Type type, string name, Type[] arguments, AccessCache.MemberType memberType = AccessCache.MemberType.Any, bool declaredOnly = false)
		{
			MethodBase methodBase = AccessCache.Get<MethodBase>(this.declaredMethods, type, name, arguments, () => type.GetMethod(name, AccessCache.declaredOnlyBindingFlags[memberType], null, arguments, null));
			if (methodBase == null && !declaredOnly)
			{
				methodBase = AccessCache.Get<MethodBase>(this.inheritedMethods, type, name, arguments, () => AccessTools.Method(type, name, arguments, null));
			}
			return methodBase;
		}

		// Token: 0x0400001A RID: 26
		private const BindingFlags BasicFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;

		// Token: 0x0400001B RID: 27
		private static readonly Dictionary<AccessCache.MemberType, BindingFlags> declaredOnlyBindingFlags = new Dictionary<AccessCache.MemberType, BindingFlags>
		{
			{
				AccessCache.MemberType.Any,
				BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty
			},
			{
				AccessCache.MemberType.Instance,
				BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty
			},
			{
				AccessCache.MemberType.Static,
				BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty
			}
		};

		// Token: 0x0400001C RID: 28
		private readonly Dictionary<Type, Dictionary<string, FieldInfo>> declaredFields = new Dictionary<Type, Dictionary<string, FieldInfo>>();

		// Token: 0x0400001D RID: 29
		private readonly Dictionary<Type, Dictionary<string, PropertyInfo>> declaredProperties = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

		// Token: 0x0400001E RID: 30
		private readonly Dictionary<Type, Dictionary<string, Dictionary<int, MethodBase>>> declaredMethods = new Dictionary<Type, Dictionary<string, Dictionary<int, MethodBase>>>();

		// Token: 0x0400001F RID: 31
		private readonly Dictionary<Type, Dictionary<string, FieldInfo>> inheritedFields = new Dictionary<Type, Dictionary<string, FieldInfo>>();

		// Token: 0x04000020 RID: 32
		private readonly Dictionary<Type, Dictionary<string, PropertyInfo>> inheritedProperties = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

		// Token: 0x04000021 RID: 33
		private readonly Dictionary<Type, Dictionary<string, Dictionary<int, MethodBase>>> inheritedMethods = new Dictionary<Type, Dictionary<string, Dictionary<int, MethodBase>>>();

		// Token: 0x0200006B RID: 107
		internal enum MemberType
		{
			// Token: 0x0400012B RID: 299
			Any,
			// Token: 0x0400012C RID: 300
			Static,
			// Token: 0x0400012D RID: 301
			Instance
		}
	}
}
