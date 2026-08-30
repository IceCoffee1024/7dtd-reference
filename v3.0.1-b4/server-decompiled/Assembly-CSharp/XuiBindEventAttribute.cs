using System;
using JetBrains.Annotations;

// Token: 0x02001102 RID: 4354
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
[MeansImplicitUse]
public class XuiBindEventAttribute : Attribute
{
	// Token: 0x060089B0 RID: 35248 RVA: 0x00347C33 File Offset: 0x00345E33
	public XuiBindEventAttribute(string _targetEvent, string _componentFieldName = null)
	{
		this.ComponentFieldName = _componentFieldName;
		this.TargetEvent = _targetEvent;
	}

	// Token: 0x04006619 RID: 26137
	public readonly string ComponentFieldName;

	// Token: 0x0400661A RID: 26138
	public readonly string TargetEvent;
}
