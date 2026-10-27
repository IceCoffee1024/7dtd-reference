using System;
using JetBrains.Annotations;

// Token: 0x02001145 RID: 4421
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
[MeansImplicitUse]
public class XuiXmlAttributeAttribute : Attribute
{
	// Token: 0x06008B66 RID: 35686 RVA: 0x00352A1D File Offset: 0x00350C1D
	public XuiXmlAttributeAttribute(string _attributeName, bool _override = false)
	{
		this.AttributeName = _attributeName;
		this.Override = _override;
	}

	// Token: 0x04006778 RID: 26488
	public readonly string AttributeName;

	// Token: 0x04006779 RID: 26489
	public readonly bool Override;
}
