using System;
using JetBrains.Annotations;

// Token: 0x02001102 RID: 4354
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
[MeansImplicitUse]
public class XuiBindingNcalcFunctionAttribute : Attribute
{
	// Token: 0x06008A39 RID: 35385 RVA: 0x000021B6 File Offset: 0x000003B6
	public XuiBindingNcalcFunctionAttribute()
	{
	}

	// Token: 0x06008A3A RID: 35386 RVA: 0x0034A72C File Offset: 0x0034892C
	public XuiBindingNcalcFunctionAttribute(object _errorResultValue, int _expectedArgumentCount, string _functionName = null)
	{
		this.FunctionName = _functionName;
		this.ExpectedArgumentCount = _expectedArgumentCount;
		this.ErrorResult = _errorResultValue;
	}

	// Token: 0x040066B2 RID: 26290
	public readonly string FunctionName;

	// Token: 0x040066B3 RID: 26291
	public readonly int ExpectedArgumentCount;

	// Token: 0x040066B4 RID: 26292
	public readonly object ErrorResult;
}
