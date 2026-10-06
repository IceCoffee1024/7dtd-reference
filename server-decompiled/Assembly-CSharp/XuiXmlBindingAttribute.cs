using System;
using JetBrains.Annotations;

// Token: 0x02001138 RID: 4408
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
[MeansImplicitUse]
public class XuiXmlBindingAttribute : Attribute
{
	// Token: 0x06008B15 RID: 35605 RVA: 0x00350983 File Offset: 0x0034EB83
	public XuiXmlBindingAttribute(string _bindingName)
	{
		this.BindingName = _bindingName;
	}

	// Token: 0x0400673A RID: 26426
	public readonly string BindingName;
}
