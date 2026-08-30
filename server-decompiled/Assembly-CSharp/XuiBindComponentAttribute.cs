using System;
using JetBrains.Annotations;

// Token: 0x02001119 RID: 4377
[AttributeUsage(AttributeTargets.Field)]
[MeansImplicitUse]
public class XuiBindComponentAttribute : Attribute
{
	// Token: 0x06008A9F RID: 35487 RVA: 0x0034C423 File Offset: 0x0034A623
	public XuiBindComponentAttribute(bool _required = true)
	{
		this.Required = _required;
	}

	// Token: 0x06008AA0 RID: 35488 RVA: 0x0034C432 File Offset: 0x0034A632
	public XuiBindComponentAttribute(string _xmlElementName, bool _required = true)
	{
		this.XmlElementName = _xmlElementName;
		this.Required = _required;
	}

	// Token: 0x040066DD RID: 26333
	public readonly string XmlElementName;

	// Token: 0x040066DE RID: 26334
	public readonly bool Required;
}
