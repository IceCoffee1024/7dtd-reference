using System;
using JetBrains.Annotations;

// Token: 0x0200111B RID: 4379
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
[MeansImplicitUse]
public class XuiBindEventAttribute : Attribute
{
	// Token: 0x06008AA2 RID: 35490 RVA: 0x0034C457 File Offset: 0x0034A657
	public XuiBindEventAttribute(string _targetEvent, string _componentFieldName = null)
	{
		this.ComponentFieldName = _componentFieldName;
		this.TargetEvent = _targetEvent;
	}

	// Token: 0x040066E0 RID: 26336
	public readonly string ComponentFieldName;

	// Token: 0x040066E1 RID: 26337
	public readonly string TargetEvent;
}
