using System;
using JetBrains.Annotations;

// Token: 0x02001100 RID: 4352
[AttributeUsage(AttributeTargets.Field)]
[MeansImplicitUse]
public class XuiBindComponentAttribute : Attribute
{
	// Token: 0x060089AD RID: 35245 RVA: 0x00347BFF File Offset: 0x00345DFF
	public XuiBindComponentAttribute(bool _required = true)
	{
		this.Required = _required;
	}

	// Token: 0x060089AE RID: 35246 RVA: 0x00347C0E File Offset: 0x00345E0E
	public XuiBindComponentAttribute(string _xmlElementName, bool _required = true)
	{
		this.XmlElementName = _xmlElementName;
		this.Required = _required;
	}

	// Token: 0x04006616 RID: 26134
	public readonly string XmlElementName;

	// Token: 0x04006617 RID: 26135
	public readonly bool Required;
}
