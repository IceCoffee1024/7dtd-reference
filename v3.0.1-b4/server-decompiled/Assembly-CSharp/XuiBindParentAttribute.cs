using System;
using JetBrains.Annotations;

// Token: 0x02001101 RID: 4353
[AttributeUsage(AttributeTargets.Field)]
[MeansImplicitUse]
public class XuiBindParentAttribute : Attribute
{
	// Token: 0x060089AF RID: 35247 RVA: 0x00347C24 File Offset: 0x00345E24
	public XuiBindParentAttribute(bool _required = true)
	{
		this.Required = _required;
	}

	// Token: 0x04006618 RID: 26136
	public readonly bool Required;
}
