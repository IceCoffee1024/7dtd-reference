using System;
using JetBrains.Annotations;

// Token: 0x02001143 RID: 4419
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
[MeansImplicitUse]
public class XuiBindEventAttribute : Attribute
{
	// Token: 0x06008B62 RID: 35682 RVA: 0x003529B3 File Offset: 0x00350BB3
	public XuiBindEventAttribute(string _targetEvent, string _componentFieldName = null)
	{
		this.ComponentFieldName = _componentFieldName;
		this.TargetEvent = _targetEvent;
	}

	// Token: 0x04006775 RID: 26485
	public readonly string ComponentFieldName;

	// Token: 0x04006776 RID: 26486
	public readonly string TargetEvent;
}
