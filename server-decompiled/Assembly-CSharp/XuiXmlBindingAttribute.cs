using System;
using JetBrains.Annotations;

// Token: 0x02001148 RID: 4424
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
[MeansImplicitUse]
public class XuiXmlBindingAttribute : Attribute
{
	// Token: 0x06008B69 RID: 35689 RVA: 0x00352A33 File Offset: 0x00350C33
	public XuiXmlBindingAttribute(string _bindingName)
	{
		this.BindingName = _bindingName;
	}

	// Token: 0x0400677A RID: 26490
	public readonly string BindingName;
}
