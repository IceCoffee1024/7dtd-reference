using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x02000014 RID: 20
	internal static class PatchArgumentExtensions
	{
		// Token: 0x06000048 RID: 72 RVA: 0x00002F68 File Offset: 0x00001168
		private static HarmonyArgument[] AllHarmonyArguments(object[] attributes)
		{
			return (from harg in attributes.Select<object, HarmonyArgument>(delegate(object attr)
				{
					if (attr.GetType().Name != "HarmonyArgument")
					{
						return null;
					}
					return AccessTools.MakeDeepCopy<HarmonyArgument>(attr);
				})
				where harg != null
				select harg).ToArray<HarmonyArgument>();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002FC4 File Offset: 0x000011C4
		private static HarmonyArgument GetArgumentAttribute(this ParameterInfo parameter)
		{
			object[] customAttributes = parameter.GetCustomAttributes(false);
			return PatchArgumentExtensions.AllHarmonyArguments(customAttributes).FirstOrDefault<HarmonyArgument>();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002FE4 File Offset: 0x000011E4
		private static HarmonyArgument[] GetArgumentAttributes(this MethodInfo method)
		{
			if (method == null || method is DynamicMethod)
			{
				return null;
			}
			object[] customAttributes = method.GetCustomAttributes(false);
			return PatchArgumentExtensions.AllHarmonyArguments(customAttributes);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000300C File Offset: 0x0000120C
		private static HarmonyArgument[] GetArgumentAttributes(this Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(false);
			return PatchArgumentExtensions.AllHarmonyArguments(customAttributes);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003028 File Offset: 0x00001228
		private static string GetOriginalArgumentName(this ParameterInfo parameter, string[] originalParameterNames)
		{
			HarmonyArgument argumentAttribute = parameter.GetArgumentAttribute();
			if (argumentAttribute == null)
			{
				return null;
			}
			if (!string.IsNullOrEmpty(argumentAttribute.OriginalName))
			{
				return argumentAttribute.OriginalName;
			}
			if (argumentAttribute.Index >= 0 && argumentAttribute.Index < originalParameterNames.Length)
			{
				return originalParameterNames[argumentAttribute.Index];
			}
			return null;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003074 File Offset: 0x00001274
		private static string GetOriginalArgumentName(HarmonyArgument[] attributes, string name, string[] originalParameterNames)
		{
			if (((attributes != null) ? attributes.Length : 0) <= 0)
			{
				return null;
			}
			HarmonyArgument harmonyArgument = attributes.SingleOrDefault<HarmonyArgument>((HarmonyArgument p) => p.NewName == name);
			if (harmonyArgument == null)
			{
				return null;
			}
			if (!string.IsNullOrEmpty(harmonyArgument.OriginalName))
			{
				return harmonyArgument.OriginalName;
			}
			if (originalParameterNames != null && harmonyArgument.Index >= 0 && harmonyArgument.Index < originalParameterNames.Length)
			{
				return originalParameterNames[harmonyArgument.Index];
			}
			return null;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000030EC File Offset: 0x000012EC
		private static string GetOriginalArgumentName(this MethodInfo method, string[] originalParameterNames, string name)
		{
			string text = PatchArgumentExtensions.GetOriginalArgumentName((method != null) ? method.GetArgumentAttributes() : null, name, originalParameterNames);
			if (text != null)
			{
				return text;
			}
			HarmonyArgument[] array;
			if (method == null)
			{
				array = null;
			}
			else
			{
				Type declaringType = method.DeclaringType;
				array = ((declaringType != null) ? declaringType.GetArgumentAttributes() : null);
			}
			text = PatchArgumentExtensions.GetOriginalArgumentName(array, name, originalParameterNames);
			if (text != null)
			{
				return text;
			}
			return name;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003138 File Offset: 0x00001338
		internal static int GetArgumentIndex(this MethodInfo patch, string[] originalParameterNames, ParameterInfo patchParam)
		{
			if (patch is DynamicMethod)
			{
				return Array.IndexOf<string>(originalParameterNames, patchParam.Name);
			}
			string text = patchParam.GetOriginalArgumentName(originalParameterNames);
			if (text != null)
			{
				return Array.IndexOf<string>(originalParameterNames, text);
			}
			text = patch.GetOriginalArgumentName(originalParameterNames, patchParam.Name);
			if (text != null)
			{
				return Array.IndexOf<string>(originalParameterNames, text);
			}
			return -1;
		}
	}
}
