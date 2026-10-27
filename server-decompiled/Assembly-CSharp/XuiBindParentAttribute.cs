using System;
using JetBrains.Annotations;

// Token: 0x02001142 RID: 4418
[AttributeUsage(AttributeTargets.Field)]
[MeansImplicitUse]
public class XuiBindParentAttribute : Attribute
{
	// Token: 0x06008B61 RID: 35681 RVA: 0x003529A4 File Offset: 0x00350BA4
	public XuiBindParentAttribute(bool _required = true)
	{
		this.Required = _required;
	}

	// Token: 0x04006774 RID: 26484
	public readonly bool Required;
}
