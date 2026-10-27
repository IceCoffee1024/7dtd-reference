using System;
using JetBrains.Annotations;

// Token: 0x02001127 RID: 4391
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
[MeansImplicitUse]
public class XuiBindingNcalcFunctionAttribute : Attribute
{
	// Token: 0x06008AEE RID: 35566 RVA: 0x000021B6 File Offset: 0x000003B6
	public XuiBindingNcalcFunctionAttribute()
	{
	}

	// Token: 0x06008AEF RID: 35567 RVA: 0x00350A84 File Offset: 0x0034EC84
	public XuiBindingNcalcFunctionAttribute(object _errorResultValue, int _expectedArgumentCount, string _functionName = null)
	{
		this.FunctionName = _functionName;
		this.ExpectedArgumentCount = _expectedArgumentCount;
		this.ErrorResult = _errorResultValue;
	}

	// Token: 0x04006741 RID: 26433
	public readonly string FunctionName;

	// Token: 0x04006742 RID: 26434
	public readonly int ExpectedArgumentCount;

	// Token: 0x04006743 RID: 26435
	public readonly object ErrorResult;
}
