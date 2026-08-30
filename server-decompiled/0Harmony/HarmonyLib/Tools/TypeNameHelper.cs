using System;
using System.Collections.Generic;
using System.Text;

namespace HarmonyLib.Tools
{
	// Token: 0x02000059 RID: 89
	internal static class TypeNameHelper
	{
		// Token: 0x060003DB RID: 987 RVA: 0x0000FA14 File Offset: 0x0000DC14
		public static string GetTypeDisplayName(Type type)
		{
			StringBuilder stringBuilder = new StringBuilder();
			TypeNameHelper.ProcessType(stringBuilder, type);
			return stringBuilder.ToString();
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000FA34 File Offset: 0x0000DC34
		private static void ProcessType(StringBuilder builder, Type type)
		{
			if (type.IsGenericType)
			{
				Type underlyingType = Nullable.GetUnderlyingType(type);
				if (underlyingType != null)
				{
					TypeNameHelper.ProcessType(builder, underlyingType);
					builder.Append('?');
					return;
				}
				Type[] genericArguments = type.GetGenericArguments();
				TypeNameHelper.ProcessGenericType(builder, type, genericArguments, genericArguments.Length);
				return;
			}
			else
			{
				if (type.IsArray)
				{
					TypeNameHelper.ProcessArrayType(builder, type);
					return;
				}
				string text;
				if (TypeNameHelper.BuiltInTypeNames.TryGetValue(type, out text))
				{
					builder.Append(text);
					return;
				}
				if (type.Namespace == "System")
				{
					builder.Append(type.Name);
					return;
				}
				if (type.Assembly.ManifestModule.Name == "FSharp.Core.dll" && TypeNameHelper.FSharpTypeNames.TryGetValue(type.Name, out text))
				{
					builder.Append(text);
					return;
				}
				if (type.IsGenericParameter)
				{
					builder.Append(type.Name);
					return;
				}
				builder.Append(type.FullName ?? type.Name);
				return;
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000FB2C File Offset: 0x0000DD2C
		private static void ProcessArrayType(StringBuilder builder, Type type)
		{
			Type type2 = type;
			while (type2.IsArray)
			{
				type2 = type2.GetElementType();
			}
			TypeNameHelper.ProcessType(builder, type2);
			while (type.IsArray)
			{
				builder.Append('[');
				builder.Append(',', type.GetArrayRank() - 1);
				builder.Append(']');
				type = type.GetElementType();
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000FB88 File Offset: 0x0000DD88
		private static void ProcessGenericType(StringBuilder builder, Type type, Type[] genericArguments, int length)
		{
			int num = 0;
			if (type.IsNested)
			{
				num = type.DeclaringType.GetGenericArguments().Length;
			}
			if (type.IsNested)
			{
				TypeNameHelper.ProcessGenericType(builder, type.DeclaringType, genericArguments, num);
				builder.Append('+');
			}
			else if (!string.IsNullOrEmpty(type.Namespace))
			{
				builder.Append(type.Namespace);
				builder.Append('.');
			}
			int num2 = type.Name.IndexOf('`');
			if (num2 <= 0)
			{
				builder.Append(type.Name);
				return;
			}
			string text;
			if (type.Assembly.ManifestModule.Name == "FSharp.Core.dll" && TypeNameHelper.FSharpTypeNames.TryGetValue(type.Name, out text))
			{
				builder.Append(text);
			}
			else
			{
				builder.Append(type.Name, 0, num2);
			}
			builder.Append('<');
			for (int i = num; i < length; i++)
			{
				TypeNameHelper.ProcessType(builder, genericArguments[i]);
				if (i + 1 != length)
				{
					builder.Append(',');
					if (!genericArguments[i + 1].IsGenericParameter)
					{
						builder.Append(' ');
					}
				}
			}
			builder.Append('>');
		}

		// Token: 0x040000D6 RID: 214
		private static readonly Dictionary<Type, string> BuiltInTypeNames = new Dictionary<Type, string>
		{
			{
				typeof(void),
				"void"
			},
			{
				typeof(bool),
				"bool"
			},
			{
				typeof(byte),
				"byte"
			},
			{
				typeof(char),
				"char"
			},
			{
				typeof(decimal),
				"decimal"
			},
			{
				typeof(double),
				"double"
			},
			{
				typeof(float),
				"float"
			},
			{
				typeof(int),
				"int"
			},
			{
				typeof(long),
				"long"
			},
			{
				typeof(object),
				"object"
			},
			{
				typeof(sbyte),
				"sbyte"
			},
			{
				typeof(short),
				"short"
			},
			{
				typeof(string),
				"string"
			},
			{
				typeof(uint),
				"uint"
			},
			{
				typeof(ulong),
				"ulong"
			},
			{
				typeof(ushort),
				"ushort"
			}
		};

		// Token: 0x040000D7 RID: 215
		private static readonly Dictionary<string, string> FSharpTypeNames = new Dictionary<string, string>
		{
			{ "Unit", "void" },
			{ "FSharpOption", "Option" },
			{ "FSharpAsync", "Async" },
			{ "FSharpOption`1", "Option" },
			{ "FSharpAsync`1", "Async" }
		};
	}
}
