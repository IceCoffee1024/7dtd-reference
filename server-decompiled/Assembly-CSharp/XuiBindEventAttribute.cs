using System;
using JetBrains.Annotations;

// Token: 0x02001133 RID: 4403
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
[MeansImplicitUse]
public class XuiBindEventAttribute : Attribute
{
	// Token: 0x06008B0E RID: 35598 RVA: 0x00350903 File Offset: 0x0034EB03
	public XuiBindEventAttribute(string _targetEvent, string _componentFieldName = null)
	{
		this.ComponentFieldName = _componentFieldName;
		this.TargetEvent = _targetEvent;
	}

	// Token: 0x04006735 RID: 26421
	public readonly string ComponentFieldName;

	// Token: 0x04006736 RID: 26422
	public readonly string TargetEvent;
}
