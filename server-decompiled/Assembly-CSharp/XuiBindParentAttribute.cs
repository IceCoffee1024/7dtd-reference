using System;
using JetBrains.Annotations;

// Token: 0x0200111A RID: 4378
[AttributeUsage(AttributeTargets.Field)]
[MeansImplicitUse]
public class XuiBindParentAttribute : Attribute
{
	// Token: 0x06008AA1 RID: 35489 RVA: 0x0034C448 File Offset: 0x0034A648
	public XuiBindParentAttribute(bool _required = true)
	{
		this.Required = _required;
	}

	// Token: 0x040066DF RID: 26335
	public readonly bool Required;
}
