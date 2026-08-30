using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib.Tools;

namespace HarmonyLib
{
	// Token: 0x0200004E RID: 78
	public static class GeneralExtensions
	{
		// Token: 0x0600035D RID: 861 RVA: 0x0000D660 File Offset: 0x0000B860
		public static string Join<T>(this IEnumerable<T> enumeration, Func<T, string> converter = null, string delimiter = ", ")
		{
			if (converter == null)
			{
				converter = (T t) => t.ToString();
			}
			return enumeration.Aggregate("", (string prev, T curr) => prev + ((prev.Length > 0) ? delimiter : "") + converter(curr));
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000D6C8 File Offset: 0x0000B8C8
		public static string Description(this Type[] parameters)
		{
			if (parameters == null)
			{
				return "NULL";
			}
			return "(" + parameters.Join<Type>((Type p) => p.FullDescription(), ", ") + ")";
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000D717 File Offset: 0x0000B917
		public static string FullDescription(this Type type)
		{
			if (type != null)
			{
				return TypeNameHelper.GetTypeDisplayName(type);
			}
			return "null";
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000D728 File Offset: 0x0000B928
		public static string FullDescription(this MethodBase member)
		{
			if (member == null)
			{
				return "null";
			}
			Type returnedType = AccessTools.GetReturnedType(member);
			StringBuilder stringBuilder = new StringBuilder();
			if (member.IsStatic)
			{
				stringBuilder.Append("static ");
			}
			if (member.IsAbstract)
			{
				stringBuilder.Append("abstract ");
			}
			if (member.IsVirtual)
			{
				stringBuilder.Append("virtual ");
			}
			stringBuilder.Append(returnedType.FullDescription() + " ");
			if (member.DeclaringType != null)
			{
				stringBuilder.Append(member.DeclaringType.FullDescription() + "::");
			}
			string text = member.GetParameters().Join<ParameterInfo>((ParameterInfo p) => p.ParameterType.FullDescription() + " " + p.Name, ", ");
			stringBuilder.Append(member.Name + "(" + text + ")");
			return stringBuilder.ToString();
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000D815 File Offset: 0x0000BA15
		public static Type[] Types(this ParameterInfo[] pinfo)
		{
			return pinfo.Select<ParameterInfo, Type>((ParameterInfo pi) => pi.ParameterType).ToArray<Type>();
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000D844 File Offset: 0x0000BA44
		public static T GetValueSafe<S, T>(this Dictionary<S, T> dictionary, S key)
		{
			T t;
			if (dictionary.TryGetValue(key, out t))
			{
				return t;
			}
			return default(T);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000D868 File Offset: 0x0000BA68
		public static T GetTypedValue<T>(this Dictionary<string, object> dictionary, string key)
		{
			object obj;
			if (dictionary.TryGetValue(key, out obj) && obj is T)
			{
				return (T)((object)obj);
			}
			return default(T);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000D898 File Offset: 0x0000BA98
		public static string ToLiteral(this string input, string quoteChar = "\"")
		{
			StringBuilder stringBuilder = new StringBuilder(input.Length + 2);
			stringBuilder.Append(quoteChar);
			int i = 0;
			while (i < input.Length)
			{
				char c = input[i];
				if (c <= '"')
				{
					switch (c)
					{
					case '\0':
						stringBuilder.Append("\\0");
						break;
					case '\u0001':
					case '\u0002':
					case '\u0003':
					case '\u0004':
					case '\u0005':
					case '\u0006':
						goto IL_012C;
					case '\a':
						stringBuilder.Append("\\a");
						break;
					case '\b':
						stringBuilder.Append("\\b");
						break;
					case '\t':
						stringBuilder.Append("\\t");
						break;
					case '\n':
						stringBuilder.Append("\\n");
						break;
					case '\v':
						stringBuilder.Append("\\v");
						break;
					case '\f':
						stringBuilder.Append("\\f");
						break;
					case '\r':
						stringBuilder.Append("\\r");
						break;
					default:
						if (c != '"')
						{
							goto IL_012C;
						}
						stringBuilder.Append("\\\"");
						break;
					}
				}
				else if (c != '\'')
				{
					if (c != '\\')
					{
						goto IL_012C;
					}
					stringBuilder.Append("\\\\");
				}
				else
				{
					stringBuilder.Append("\\'");
				}
				IL_0162:
				i++;
				continue;
				IL_012C:
				if (c >= ' ' && c <= '~')
				{
					stringBuilder.Append(c);
					goto IL_0162;
				}
				stringBuilder.Append("\\u");
				StringBuilder stringBuilder2 = stringBuilder;
				int num = (int)c;
				stringBuilder2.Append(num.ToString("x4"));
				goto IL_0162;
			}
			stringBuilder.Append(quoteChar);
			return stringBuilder.ToString();
		}
	}
}
