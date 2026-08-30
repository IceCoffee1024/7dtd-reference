using System;
using JetBrains.Annotations;

// Token: 0x02001107 RID: 4359
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
[MeansImplicitUse]
public class XuiXmlBindingAttribute : Attribute
{
	// Token: 0x060089B7 RID: 35255 RVA: 0x00347CB3 File Offset: 0x00345EB3
	public XuiXmlBindingAttribute(string _bindingName)
	{
		this.BindingName = _bindingName;
	}

	// Token: 0x0400661E RID: 26142
	public readonly string BindingName;
}
