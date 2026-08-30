using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HarmonyLib
{
	// Token: 0x02000017 RID: 23
	internal class AttributePatch
	{
		// Token: 0x0600005B RID: 91 RVA: 0x000036D8 File Offset: 0x000018D8
		internal static IEnumerable<AttributePatch> Create(MethodInfo patch, bool collectIncomplete = false)
		{
			if (patch == null)
			{
				throw new NullReferenceException("Patch method cannot be null");
			}
			object[] customAttributes = patch.GetCustomAttributes(true);
			string name = patch.Name;
			HarmonyPatchType? type = AttributePatch.GetPatchType(name, customAttributes);
			if (type == null)
			{
				return Enumerable.Empty<AttributePatch>();
			}
			if (type.GetValueOrDefault() != HarmonyPatchType.ReversePatch && !patch.IsStatic)
			{
				throw new ArgumentException("Patch method " + patch.FullDescription() + " must be static");
			}
			IEnumerable<object> enumerable = customAttributes.Where<object>((object attr) => attr.GetType().BaseType.FullName == PatchTools.harmonyAttributeFullName).Select<object, object>(delegate(object attr)
			{
				FieldInfo fieldInfo = AccessTools.Field(attr.GetType(), "info");
				return fieldInfo.GetValue(attr);
			});
			Func<object, HarmonyMethod> func;
			if ((func = AttributePatch.<>O.<0>__MakeDeepCopy) == null)
			{
				func = (AttributePatch.<>O.<0>__MakeDeepCopy = new Func<object, HarmonyMethod>(AccessTools.MakeDeepCopy<HarmonyMethod>));
			}
			List<HarmonyMethod> list = enumerable.Select<object, HarmonyMethod>(func).ToList<HarmonyMethod>();
			List<HarmonyMethod> list2 = new List<HarmonyMethod>();
			ILookup<bool, HarmonyMethod> lookup = list.ToLookup<HarmonyMethod, bool>((HarmonyMethod m) => AttributePatch.<Create>g__IsComplete|3_3(m, collectIncomplete));
			List<HarmonyMethod> incomplete = lookup[false].ToList<HarmonyMethod>();
			HarmonyMethod info = HarmonyMethod.Merge(incomplete);
			List<HarmonyMethod> list3 = lookup[true].Where<HarmonyMethod>((HarmonyMethod m) => !AttributePatch.<Create>g__Same|3_2(m, info)).ToList<HarmonyMethod>();
			if (list3.Count > 1)
			{
				list2.AddRange(list3.Select<HarmonyMethod, HarmonyMethod>((HarmonyMethod m) => HarmonyMethod.Merge(incomplete.AddItem(m))));
			}
			else
			{
				list2.Add(HarmonyMethod.Merge(list));
			}
			foreach (HarmonyMethod harmonyMethod in list2)
			{
				harmonyMethod.method = patch;
			}
			return list2.Select<HarmonyMethod, AttributePatch>((HarmonyMethod i) => new AttributePatch
			{
				info = i,
				type = type
			}).ToList<AttributePatch>();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000038C0 File Offset: 0x00001AC0
		private static HarmonyPatchType? GetPatchType(string methodName, object[] allAttributes)
		{
			HashSet<string> hashSet = new HashSet<string>(from attr in allAttributes
				select attr.GetType().FullName into name
				where name.StartsWith("Harmony")
				select name);
			HarmonyPatchType? harmonyPatchType = null;
			foreach (HarmonyPatchType harmonyPatchType2 in AttributePatch.allPatchTypes)
			{
				string text = harmonyPatchType2.ToString();
				if (text == methodName || hashSet.Contains("HarmonyLib.Harmony" + text))
				{
					harmonyPatchType = new HarmonyPatchType?(harmonyPatchType2);
					break;
				}
			}
			return harmonyPatchType;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003996 File Offset: 0x00001B96
		[CompilerGenerated]
		internal static bool <Create>g__Same|3_2(HarmonyMethod m1, HarmonyMethod m2)
		{
			return m1.declaringType == m2.declaringType && m1.methodName == m2.methodName && m1.GetArgumentList().SequenceEqual<Type>(m2.GetArgumentList());
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000039D1 File Offset: 0x00001BD1
		[CompilerGenerated]
		internal static bool <Create>g__IsComplete|3_3(HarmonyMethod m, bool collectIncomplete)
		{
			return (collectIncomplete || m.declaringType != null) && m.methodName != null;
		}

		// Token: 0x04000023 RID: 35
		private static readonly HarmonyPatchType[] allPatchTypes = new HarmonyPatchType[]
		{
			HarmonyPatchType.Prefix,
			HarmonyPatchType.Postfix,
			HarmonyPatchType.Transpiler,
			HarmonyPatchType.Finalizer,
			HarmonyPatchType.ReversePatch,
			HarmonyPatchType.ILManipulator
		};

		// Token: 0x04000024 RID: 36
		internal HarmonyMethod info;

		// Token: 0x04000025 RID: 37
		internal HarmonyPatchType? type;

		// Token: 0x02000077 RID: 119
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000152 RID: 338
			public static Func<object, HarmonyMethod> <0>__MakeDeepCopy;
		}
	}
}
