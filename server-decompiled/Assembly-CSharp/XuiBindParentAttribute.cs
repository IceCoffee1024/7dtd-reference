using System;
using JetBrains.Annotations;

// Token: 0x02001132 RID: 4402
[AttributeUsage(AttributeTargets.Field)]
[MeansImplicitUse]
public class XuiBindParentAttribute : Attribute
{
	// Token: 0x06008B0D RID: 35597 RVA: 0x003508F4 File Offset: 0x0034EAF4
	public XuiBindParentAttribute(bool _required = true)
	{
		this.Required = _required;
	}

	// Token: 0x04006734 RID: 26420
	public readonly bool Required;
}
