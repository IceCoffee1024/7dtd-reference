using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x02000019 RID: 25
	internal static class PatchTools
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00003E52 File Offset: 0x00002052
		internal static void RememberObject(object key, object value)
		{
			if (PatchTools.objectReferences == null)
			{
				PatchTools.objectReferences = new Dictionary<object, object>();
			}
			PatchTools.objectReferences[key] = value;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003E74 File Offset: 0x00002074
		public static MethodInfo CreateMethod(string name, Type returnType, List<KeyValuePair<string, Type>> parameters, Action<ILGenerator> generator)
		{
			Type[] array = parameters.Select<KeyValuePair<string, Type>, Type>((KeyValuePair<string, Type> p) => p.Value).ToArray<Type>();
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(name, returnType, array);
			for (int i = 0; i < parameters.Count; i++)
			{
				dynamicMethodDefinition.Definition.Parameters[i].Name = parameters[i].Key;
			}
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			generator(ilgenerator);
			return dynamicMethodDefinition.Generate();
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003F00 File Offset: 0x00002100
		internal static MethodInfo GetPatchMethod(Type patchType, string attributeName)
		{
			Func<object, bool> <>9__1;
			MethodInfo methodInfo = patchType.GetMethods(AccessTools.all).FirstOrDefault<MethodInfo>(delegate(MethodInfo m)
			{
				IEnumerable<object> customAttributes = m.GetCustomAttributes(true);
				Func<object, bool> func;
				if ((func = <>9__1) == null)
				{
					func = (<>9__1 = (object a) => a.GetType().FullName == attributeName);
				}
				return customAttributes.Any<object>(func);
			});
			if (methodInfo == null)
			{
				string text = attributeName.Replace("HarmonyLib.Harmony", "");
				methodInfo = patchType.GetMethod(text, AccessTools.all);
			}
			return methodInfo;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003F60 File Offset: 0x00002160
		internal static AssemblyBuilder DefineDynamicAssembly(string name)
		{
			AssemblyName assemblyName = new AssemblyName(name);
			return AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003F80 File Offset: 0x00002180
		internal static List<AttributePatch> GetPatchMethods(Type type, bool collectIncomplete = false)
		{
			return (from attributePatch in AccessTools.GetDeclaredMethods(type).SelectMany<MethodInfo, AttributePatch>((MethodInfo method) => AttributePatch.Create(method, collectIncomplete))
				where attributePatch != null
				select attributePatch).ToList<AttributePatch>();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003FDC File Offset: 0x000021DC
		internal static MethodBase GetOriginalMethod(this HarmonyMethod attr)
		{
			try
			{
				MethodType? methodType = attr.methodType;
				if (methodType != null)
				{
					switch (methodType.GetValueOrDefault())
					{
					case MethodType.Normal:
						if (attr.methodName == null)
						{
							return null;
						}
						return AccessTools.DeclaredMethod(attr.declaringType, attr.methodName, attr.argumentTypes, null);
					case MethodType.Getter:
					{
						if (attr.methodName == null)
						{
							PropertyInfo propertyInfo = AccessTools.DeclaredIndexer(attr.declaringType, attr.argumentTypes);
							return (propertyInfo != null) ? propertyInfo.GetGetMethod(true) : null;
						}
						PropertyInfo propertyInfo2 = AccessTools.DeclaredProperty(attr.declaringType, attr.methodName);
						return (propertyInfo2 != null) ? propertyInfo2.GetGetMethod(true) : null;
					}
					case MethodType.Setter:
					{
						if (attr.methodName == null)
						{
							PropertyInfo propertyInfo3 = AccessTools.DeclaredIndexer(attr.declaringType, attr.argumentTypes);
							return (propertyInfo3 != null) ? propertyInfo3.GetSetMethod(true) : null;
						}
						PropertyInfo propertyInfo4 = AccessTools.DeclaredProperty(attr.declaringType, attr.methodName);
						return (propertyInfo4 != null) ? propertyInfo4.GetSetMethod(true) : null;
					}
					case MethodType.Constructor:
						return AccessTools.DeclaredConstructor(attr.declaringType, attr.argumentTypes, false);
					case MethodType.StaticConstructor:
						return AccessTools.GetDeclaredConstructors(attr.declaringType, null).FirstOrDefault<ConstructorInfo>((ConstructorInfo c) => c.IsStatic);
					case MethodType.Enumerator:
						if (attr.methodName == null)
						{
							return null;
						}
						return AccessTools.EnumeratorMoveNext(AccessTools.DeclaredMethod(attr.declaringType, attr.methodName, attr.argumentTypes, null));
					case MethodType.Async:
						if (attr.methodName == null)
						{
							return null;
						}
						return AccessTools.AsyncMoveNext(AccessTools.DeclaredMethod(attr.declaringType, attr.methodName, attr.argumentTypes, null));
					}
				}
			}
			catch (AmbiguousMatchException ex)
			{
				throw new HarmonyException("Ambiguous match for HarmonyMethod[" + attr.Description() + "]", ex.InnerException ?? ex);
			}
			return null;
		}

		// Token: 0x0400002C RID: 44
		internal static readonly string harmonyMethodFullName = typeof(HarmonyMethod).FullName;

		// Token: 0x0400002D RID: 45
		internal static readonly string harmonyAttributeFullName = typeof(HarmonyAttribute).FullName;

		// Token: 0x0400002E RID: 46
		internal static readonly string harmonyPatchAllFullName = typeof(HarmonyPatchAll).FullName;

		// Token: 0x0400002F RID: 47
		[ThreadStatic]
		private static Dictionary<object, object> objectReferences;
	}
}
