using System;
using JetBrains.Annotations;

// Token: 0x02001141 RID: 4417
[AttributeUsage(AttributeTargets.Field)]
[MeansImplicitUse]
public class XuiBindComponentAttribute : Attribute
{
	// Token: 0x06008B5F RID: 35679 RVA: 0x0035297F File Offset: 0x00350B7F
	public XuiBindComponentAttribute(bool _required = true)
	{
		this.Required = _required;
	}

	// Token: 0x06008B60 RID: 35680 RVA: 0x0035298E File Offset: 0x00350B8E
	public XuiBindComponentAttribute(string _xmlElementName, bool _required = true)
	{
		this.XmlElementName = _xmlElementName;
		this.Required = _required;
	}

	// Token: 0x04006772 RID: 26482
	public readonly string XmlElementName;

	// Token: 0x04006773 RID: 26483
	public readonly bool Required;
}
