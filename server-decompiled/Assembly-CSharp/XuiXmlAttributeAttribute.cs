using System;
using JetBrains.Annotations;

// Token: 0x02001135 RID: 4405
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
[MeansImplicitUse]
public class XuiXmlAttributeAttribute : Attribute
{
	// Token: 0x06008B12 RID: 35602 RVA: 0x0035096D File Offset: 0x0034EB6D
	public XuiXmlAttributeAttribute(string _attributeName, bool _override = false)
	{
		this.AttributeName = _attributeName;
		this.Override = _override;
	}

	// Token: 0x04006738 RID: 26424
	public readonly string AttributeName;

	// Token: 0x04006739 RID: 26425
	public readonly bool Override;
}
