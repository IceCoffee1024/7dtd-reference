using System;
using JetBrains.Annotations;

// Token: 0x02001104 RID: 4356
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
[MeansImplicitUse]
public class XuiXmlAttributeAttribute : Attribute
{
	// Token: 0x060089B4 RID: 35252 RVA: 0x00347C9D File Offset: 0x00345E9D
	public XuiXmlAttributeAttribute(string _attributeName, bool _override = false)
	{
		this.AttributeName = _attributeName;
		this.Override = _override;
	}

	// Token: 0x0400661C RID: 26140
	public readonly string AttributeName;

	// Token: 0x0400661D RID: 26141
	public readonly bool Override;
}
