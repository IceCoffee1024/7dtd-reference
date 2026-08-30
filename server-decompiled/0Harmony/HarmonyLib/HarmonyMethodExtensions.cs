using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HarmonyLib
{
	// Token: 0x0200003F RID: 63
	public static class HarmonyMethodExtensions
	{
		// Token: 0x06000115 RID: 277 RVA: 0x00005EFC File Offset: 0x000040FC
		internal static void SetValue(Traverse trv, string name, object val)
		{
			if (val == null)
			{
				return;
			}
			Traverse traverse = trv.Field(name);
			if (name == "methodType" || name == "reversePatchType")
			{
				Type underlyingType = Nullable.GetUnderlyingType(traverse.GetValueType());
				val = Enum.ToObject(underlyingType, (int)val);
			}
			traverse.SetValue(val);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005F54 File Offset: 0x00004154
		public static void CopyTo(this HarmonyMethod from, HarmonyMethod to)
		{
			if (to == null)
			{
				return;
			}
			Traverse fromTrv = Traverse.Create(from);
			Traverse toTrv = Traverse.Create(to);
			HarmonyMethod.HarmonyFields().ForEach(delegate(string f)
			{
				object value = fromTrv.Field(f).GetValue();
				if (value != null)
				{
					HarmonyMethodExtensions.SetValue(toTrv, f, value);
				}
			});
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005F9C File Offset: 0x0000419C
		public static HarmonyMethod Clone(this HarmonyMethod original)
		{
			HarmonyMethod harmonyMethod = new HarmonyMethod();
			original.CopyTo(harmonyMethod);
			return harmonyMethod;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005FB8 File Offset: 0x000041B8
		public static HarmonyMethod Merge(this HarmonyMethod master, HarmonyMethod detail)
		{
			if (detail == null)
			{
				return master;
			}
			HarmonyMethod harmonyMethod = new HarmonyMethod();
			Traverse resultTrv = Traverse.Create(harmonyMethod);
			Traverse masterTrv = Traverse.Create(master);
			Traverse detailTrv = Traverse.Create(detail);
			HarmonyMethod.HarmonyFields().ForEach(delegate(string f)
			{
				object value = masterTrv.Field(f).GetValue();
				object value2 = detailTrv.Field(f).GetValue();
				if (f != "priority")
				{
					HarmonyMethodExtensions.SetValue(resultTrv, f, value2 ?? value);
					return;
				}
				int num = (int)value;
				int num2 = (int)value2;
				int num3 = Math.Max(num, num2);
				if (num == -1 && num2 != -1)
				{
					num3 = num2;
				}
				if (num != -1 && num2 == -1)
				{
					num3 = num;
				}
				HarmonyMethodExtensions.SetValue(resultTrv, f, num3);
			});
			return harmonyMethod;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00006014 File Offset: 0x00004214
		private static HarmonyMethod GetHarmonyMethodInfo(object attribute)
		{
			FieldInfo field = attribute.GetType().GetField("info", AccessTools.all);
			if (field == null)
			{
				return null;
			}
			if (field.FieldType.FullName != PatchTools.harmonyMethodFullName)
			{
				return null;
			}
			object value = field.GetValue(attribute);
			return AccessTools.MakeDeepCopy<HarmonyMethod>(value);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00006064 File Offset: 0x00004264
		public static List<HarmonyMethod> GetFromType(Type type)
		{
			IEnumerable<object> customAttributes = type.GetCustomAttributes(true);
			Func<object, HarmonyMethod> func;
			if ((func = HarmonyMethodExtensions.<>O.<0>__GetHarmonyMethodInfo) == null)
			{
				func = (HarmonyMethodExtensions.<>O.<0>__GetHarmonyMethodInfo = new Func<object, HarmonyMethod>(HarmonyMethodExtensions.GetHarmonyMethodInfo));
			}
			return (from info in customAttributes.Select<object, HarmonyMethod>(func)
				where info != null
				select info).ToList<HarmonyMethod>();
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000060C1 File Offset: 0x000042C1
		public static HarmonyMethod GetMergedFromType(Type type)
		{
			return HarmonyMethod.Merge(HarmonyMethodExtensions.GetFromType(type));
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000060D0 File Offset: 0x000042D0
		public static List<HarmonyMethod> GetFromMethod(MethodBase method)
		{
			IEnumerable<object> customAttributes = method.GetCustomAttributes(true);
			Func<object, HarmonyMethod> func;
			if ((func = HarmonyMethodExtensions.<>O.<0>__GetHarmonyMethodInfo) == null)
			{
				func = (HarmonyMethodExtensions.<>O.<0>__GetHarmonyMethodInfo = new Func<object, HarmonyMethod>(HarmonyMethodExtensions.GetHarmonyMethodInfo));
			}
			return (from info in customAttributes.Select<object, HarmonyMethod>(func)
				where info != null
				select info).ToList<HarmonyMethod>();
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000612D File Offset: 0x0000432D
		public static HarmonyMethod GetMergedFromMethod(MethodBase method)
		{
			return HarmonyMethod.Merge(HarmonyMethodExtensions.GetFromMethod(method));
		}

		// Token: 0x02000092 RID: 146
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400018D RID: 397
			public static Func<object, HarmonyMethod> <0>__GetHarmonyMethodInfo;
		}
	}
}
