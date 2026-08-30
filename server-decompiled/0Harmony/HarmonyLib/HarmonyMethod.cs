using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HarmonyLib
{
	// Token: 0x0200003E RID: 62
	public class HarmonyMethod
	{
		// Token: 0x06000106 RID: 262 RVA: 0x00005B73 File Offset: 0x00003D73
		public HarmonyMethod()
		{
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005B84 File Offset: 0x00003D84
		private void ImportMethod(MethodInfo theMethod)
		{
			if (theMethod == null)
			{
				throw new ArgumentNullException("theMethod", "Harmony method is null (did you target a wrong or missing method?)");
			}
			if (!theMethod.IsStatic)
			{
				throw new ArgumentException("Harmony method must be static", "theMethod");
			}
			this.method = theMethod;
			List<HarmonyMethod> fromMethod = HarmonyMethodExtensions.GetFromMethod(this.method);
			if (fromMethod != null)
			{
				HarmonyMethod.Merge(fromMethod).CopyTo(this);
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005BDE File Offset: 0x00003DDE
		public HarmonyMethod(MethodInfo method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			this.ImportMethod(method);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005C02 File Offset: 0x00003E02
		public HarmonyMethod(Delegate @delegate)
			: this(@delegate.Method)
		{
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005C10 File Offset: 0x00003E10
		public HarmonyMethod(MethodInfo method, int priority = -1, string[] before = null, string[] after = null, bool? debug = null)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			this.ImportMethod(method);
			this.priority = priority;
			this.before = before;
			this.after = after;
			this.debug = debug;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005C5D File Offset: 0x00003E5D
		public HarmonyMethod(Delegate @delegate, int priority = -1, string[] before = null, string[] after = null, bool? debug = null)
			: this(@delegate.Method, priority, before, after, debug)
		{
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005C74 File Offset: 0x00003E74
		public HarmonyMethod(Type methodType, string methodName, Type[] argumentTypes = null)
		{
			MethodInfo methodInfo = AccessTools.Method(methodType, methodName, argumentTypes, null);
			if (methodInfo == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot not find method for type ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(methodType);
				defaultInterpolatedStringHandler.AppendLiteral(" and name ");
				defaultInterpolatedStringHandler.AppendFormatted(methodName);
				defaultInterpolatedStringHandler.AppendLiteral(" and parameters ");
				defaultInterpolatedStringHandler.AppendFormatted((argumentTypes != null) ? argumentTypes.Description() : null);
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.ImportMethod(methodInfo);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005D00 File Offset: 0x00003F00
		public static List<string> HarmonyFields()
		{
			return (from s in AccessTools.GetFieldNames(typeof(HarmonyMethod))
				where s != "method"
				select s).ToList<string>();
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005D3A File Offset: 0x00003F3A
		public static HarmonyMethod Merge(List<HarmonyMethod> attributes)
		{
			if (attributes == null || attributes.Count == 0)
			{
				return new HarmonyMethod();
			}
			return HarmonyMethod.Merge(attributes);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005D54 File Offset: 0x00003F54
		internal static HarmonyMethod Merge(IEnumerable<HarmonyMethod> attributes)
		{
			HarmonyMethod harmonyMethod = new HarmonyMethod();
			if (attributes == null)
			{
				return harmonyMethod;
			}
			Traverse resultTrv = Traverse.Create(harmonyMethod);
			attributes.Do<HarmonyMethod>(delegate(HarmonyMethod attribute)
			{
				Traverse trv = Traverse.Create(attribute);
				HarmonyMethod.HarmonyFields().ForEach(delegate(string f)
				{
					object value = trv.Field(f).GetValue();
					if (value != null && (f != "priority" || (int)value != -1))
					{
						HarmonyMethodExtensions.SetValue(resultTrv, f, value);
					}
				});
			});
			return harmonyMethod;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005D94 File Offset: 0x00003F94
		public override string ToString()
		{
			string result = "";
			Traverse trv = Traverse.Create(this);
			HarmonyMethod.HarmonyFields().ForEach(delegate(string f)
			{
				if (result.Length > 0)
				{
					result += ", ";
				}
				string result2 = result;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted(f);
				defaultInterpolatedStringHandler.AppendLiteral("=");
				defaultInterpolatedStringHandler.AppendFormatted<object>(trv.Field(f).GetValue());
				result = result2 + defaultInterpolatedStringHandler.ToStringAndClear();
			});
			return "HarmonyMethod[" + result + "]";
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005DEC File Offset: 0x00003FEC
		internal string Description()
		{
			string text = ((this.declaringType != null) ? this.declaringType.FullDescription() : "undefined");
			string text2 = this.methodName ?? "undefined";
			string text3 = ((this.methodType != null) ? this.methodType.Value.ToString() : "undefined");
			string text4 = ((this.argumentTypes != null) ? this.argumentTypes.Description() : "undefined");
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 4);
			defaultInterpolatedStringHandler.AppendLiteral("(class=");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral(", methodname=");
			defaultInterpolatedStringHandler.AppendFormatted(text2);
			defaultInterpolatedStringHandler.AppendLiteral(", type=");
			defaultInterpolatedStringHandler.AppendFormatted(text3);
			defaultInterpolatedStringHandler.AppendLiteral(", args=");
			defaultInterpolatedStringHandler.AppendFormatted(text4);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005EDB File Offset: 0x000040DB
		internal Type[] GetArgumentList()
		{
			return this.argumentTypes ?? Type.EmptyTypes;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005EEC File Offset: 0x000040EC
		public static implicit operator HarmonyMethod(MethodInfo method)
		{
			return new HarmonyMethod(method);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005EF4 File Offset: 0x000040F4
		public static implicit operator HarmonyMethod(Delegate @delegate)
		{
			return new HarmonyMethod(@delegate);
		}

		// Token: 0x04000064 RID: 100
		public MethodInfo method;

		// Token: 0x04000065 RID: 101
		public string category;

		// Token: 0x04000066 RID: 102
		public Type declaringType;

		// Token: 0x04000067 RID: 103
		public string methodName;

		// Token: 0x04000068 RID: 104
		public MethodType? methodType;

		// Token: 0x04000069 RID: 105
		public Type[] argumentTypes;

		// Token: 0x0400006A RID: 106
		public int priority = -1;

		// Token: 0x0400006B RID: 107
		public string[] before;

		// Token: 0x0400006C RID: 108
		public string[] after;

		// Token: 0x0400006D RID: 109
		public HarmonyReversePatchType? reversePatchType;

		// Token: 0x0400006E RID: 110
		public bool? debug;

		// Token: 0x0400006F RID: 111
		public string debugEmitPath;

		// Token: 0x04000070 RID: 112
		public bool nonVirtualDelegate;

		// Token: 0x04000071 RID: 113
		public bool? wrapTryCatch;

		// Token: 0x04000072 RID: 114
		public bool? optional;
	}
}
