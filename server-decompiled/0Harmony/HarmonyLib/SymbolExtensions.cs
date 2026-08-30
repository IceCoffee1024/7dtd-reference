using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HarmonyLib
{
	// Token: 0x02000054 RID: 84
	public static class SymbolExtensions
	{
		// Token: 0x0600039D RID: 925 RVA: 0x0000EED7 File Offset: 0x0000D0D7
		public static MethodInfo GetMethodInfo(Expression<Action> expression)
		{
			return SymbolExtensions.GetMethodInfo(expression);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000EEDF File Offset: 0x0000D0DF
		public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
		{
			return SymbolExtensions.GetMethodInfo(expression);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000EEE7 File Offset: 0x0000D0E7
		public static MethodInfo GetMethodInfo<T, TResult>(Expression<Func<T, TResult>> expression)
		{
			return SymbolExtensions.GetMethodInfo(expression);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000EEF0 File Offset: 0x0000D0F0
		public static MethodInfo GetMethodInfo(LambdaExpression expression)
		{
			MethodCallExpression methodCallExpression = expression.Body as MethodCallExpression;
			if (methodCallExpression == null)
			{
				UnaryExpression unaryExpression = expression.Body as UnaryExpression;
				if (unaryExpression != null)
				{
					MethodCallExpression methodCallExpression2 = unaryExpression.Operand as MethodCallExpression;
					if (methodCallExpression2 != null)
					{
						ConstantExpression constantExpression = methodCallExpression2.Object as ConstantExpression;
						if (constantExpression != null)
						{
							MethodInfo methodInfo = constantExpression.Value as MethodInfo;
							if (methodInfo != null)
							{
								return methodInfo;
							}
						}
					}
				}
				throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");
			}
			MethodInfo method = methodCallExpression.Method;
			if (method == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot find method for expression ");
				defaultInterpolatedStringHandler.AppendFormatted<LambdaExpression>(expression);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return method;
		}
	}
}
