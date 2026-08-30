using System;

namespace UnityEngine
{
	// Token: 0x02000296 RID: 662
	internal static class UINumericFieldsUtils
	{
		// Token: 0x06001C0F RID: 7183 RVA: 0x0002E6AC File Offset: 0x0002C8AC
		public static bool TryConvertStringToDouble(string str, out double value)
		{
			ExpressionEvaluator.Expression expression;
			return UINumericFieldsUtils.TryConvertStringToDouble(str, out value, out expression);
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x0002E6C8 File Offset: 0x0002C8C8
		public static bool TryConvertStringToDouble(string str, out double value, out ExpressionEvaluator.Expression expr)
		{
			expr = null;
			string text = str.ToLower();
			string text2 = text;
			string text3 = text2;
			if (!(text3 == "inf") && !(text3 == "infinity"))
			{
				if (!(text3 == "-inf") && !(text3 == "-infinity"))
				{
					if (!(text3 == "nan"))
					{
						return ExpressionEvaluator.Evaluate<double>(str, out value, out expr);
					}
					value = double.NaN;
				}
				else
				{
					value = double.NegativeInfinity;
				}
			}
			else
			{
				value = double.PositiveInfinity;
			}
			return true;
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x0002E760 File Offset: 0x0002C960
		public static bool TryConvertStringToDouble(string str, string initialValueAsString, out double value, out ExpressionEvaluator.Expression expression)
		{
			bool flag = UINumericFieldsUtils.TryConvertStringToDouble(str, out value, out expression);
			bool flag2 = !flag && expression != null && !string.IsNullOrEmpty(initialValueAsString);
			if (flag2)
			{
				double num;
				ExpressionEvaluator.Expression expression2;
				bool flag3 = UINumericFieldsUtils.TryConvertStringToDouble(initialValueAsString, out num, out expression2);
				if (flag3)
				{
					value = num;
					flag = expression.Evaluate<double>(ref value, 0, 1);
				}
			}
			return flag;
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x0002E7B8 File Offset: 0x0002C9B8
		public static bool TryConvertStringToFloat(string str, string initialValueAsString, out float value, out ExpressionEvaluator.Expression expression)
		{
			double num;
			bool flag = UINumericFieldsUtils.TryConvertStringToDouble(str, initialValueAsString, out num, out expression);
			value = Mathf.ClampToFloat(num);
			return flag;
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x0002E7E0 File Offset: 0x0002C9E0
		public static bool TryConvertStringToLong(string str, out long value)
		{
			ExpressionEvaluator.Expression expression;
			return ExpressionEvaluator.Evaluate<long>(str, out value, out expression);
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x0002E7FC File Offset: 0x0002C9FC
		public static bool TryConvertStringToLong(string str, out long value, out ExpressionEvaluator.Expression expr)
		{
			return ExpressionEvaluator.Evaluate<long>(str, out value, out expr);
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x0002E818 File Offset: 0x0002CA18
		public static bool TryConvertStringToLong(string str, string initialValueAsString, out long value, out ExpressionEvaluator.Expression expression)
		{
			bool flag = UINumericFieldsUtils.TryConvertStringToLong(str, out value, out expression);
			bool flag2 = !flag && expression != null && !string.IsNullOrEmpty(initialValueAsString);
			if (flag2)
			{
				long num;
				ExpressionEvaluator.Expression expression2;
				bool flag3 = UINumericFieldsUtils.TryConvertStringToLong(initialValueAsString, out num, out expression2);
				if (flag3)
				{
					value = num;
					flag = expression.Evaluate<long>(ref value, 0, 1);
				}
			}
			return flag;
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x0002E870 File Offset: 0x0002CA70
		public static bool TryConvertStringToULong(string str, out ulong value, out ExpressionEvaluator.Expression expr)
		{
			return ExpressionEvaluator.Evaluate<ulong>(str, out value, out expr);
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x0002E88C File Offset: 0x0002CA8C
		public static bool TryConvertStringToULong(string str, string initialValueAsString, out ulong value, out ExpressionEvaluator.Expression expression)
		{
			bool flag = UINumericFieldsUtils.TryConvertStringToULong(str, out value, out expression);
			bool flag2 = !flag && expression != null && !string.IsNullOrEmpty(initialValueAsString);
			if (flag2)
			{
				ulong num;
				ExpressionEvaluator.Expression expression2;
				bool flag3 = UINumericFieldsUtils.TryConvertStringToULong(initialValueAsString, out num, out expression2);
				if (flag3)
				{
					value = num;
					flag = expression.Evaluate<ulong>(ref value, 0, 1);
				}
			}
			return flag;
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x0002E8E4 File Offset: 0x0002CAE4
		public static bool TryConvertStringToInt(string str, string initialValueAsString, out int value, out ExpressionEvaluator.Expression expression)
		{
			long num;
			bool flag = UINumericFieldsUtils.TryConvertStringToLong(str, initialValueAsString, out num, out expression);
			value = Mathf.ClampToInt(num);
			return flag;
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x0002E90C File Offset: 0x0002CB0C
		public static bool TryConvertStringToUInt(string str, string initialValueAsString, out uint value, out ExpressionEvaluator.Expression expression)
		{
			long num;
			bool flag = UINumericFieldsUtils.TryConvertStringToLong(str, initialValueAsString, out num, out expression);
			value = Mathf.ClampToUInt(num);
			return flag;
		}

		// Token: 0x04000970 RID: 2416
		public static readonly string k_AllowedCharactersForFloat = "inftynaeINFTYNAE0123456789.,-*/+%^()cosqrludxvRL=pP#";

		// Token: 0x04000971 RID: 2417
		public static readonly string k_AllowedCharactersForInt = "0123456789-*/+%^()cosintaqrtelfundxvRL,=pPI#";

		// Token: 0x04000972 RID: 2418
		public static readonly string k_DoubleFieldFormatString = "R";

		// Token: 0x04000973 RID: 2419
		public static readonly string k_FloatFieldFormatString = "g7";

		// Token: 0x04000974 RID: 2420
		public static readonly string k_IntFieldFormatString = "#######0";
	}
}
