using System;
using JetBrains.Annotations;

// Token: 0x02001117 RID: 4375
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
[MeansImplicitUse]
public class XuiBindingNcalcFunctionAttribute : Attribute
{
	// Token: 0x06008A9A RID: 35482 RVA: 0x000021B6 File Offset: 0x000003B6
	public XuiBindingNcalcFunctionAttribute()
	{
	}

	// Token: 0x06008A9B RID: 35483 RVA: 0x0034E9D4 File Offset: 0x0034CBD4
	public XuiBindingNcalcFunctionAttribute(object _errorResultValue, int _expectedArgumentCount, string _functionName = null)
	{
		this.FunctionName = _functionName;
		this.ExpectedArgumentCount = _expectedArgumentCount;
		this.ErrorResult = _errorResultValue;
	}

	// Token: 0x04006701 RID: 26369
	public readonly string FunctionName;

	// Token: 0x04006702 RID: 26370
	public readonly int ExpectedArgumentCount;

	// Token: 0x04006703 RID: 26371
	public readonly object ErrorResult;
}
