using System;
using JetBrains.Annotations;

// Token: 0x020010E9 RID: 4329
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
[MeansImplicitUse]
public class XuiBindingNcalcFunctionAttribute : Attribute
{
	// Token: 0x06008947 RID: 35143 RVA: 0x000021B6 File Offset: 0x000003B6
	public XuiBindingNcalcFunctionAttribute()
	{
	}

	// Token: 0x06008948 RID: 35144 RVA: 0x00345F08 File Offset: 0x00344108
	public XuiBindingNcalcFunctionAttribute(object _errorResultValue, int _expectedArgumentCount, string _functionName = null)
	{
		this.FunctionName = _functionName;
		this.ExpectedArgumentCount = _expectedArgumentCount;
		this.ErrorResult = _errorResultValue;
	}

	// Token: 0x040065EB RID: 26091
	public readonly string FunctionName;

	// Token: 0x040065EC RID: 26092
	public readonly int ExpectedArgumentCount;

	// Token: 0x040065ED RID: 26093
	public readonly object ErrorResult;
}
