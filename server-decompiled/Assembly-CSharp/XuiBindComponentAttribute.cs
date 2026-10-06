using System;
using JetBrains.Annotations;

// Token: 0x02001131 RID: 4401
[AttributeUsage(AttributeTargets.Field)]
[MeansImplicitUse]
public class XuiBindComponentAttribute : Attribute
{
	// Token: 0x06008B0B RID: 35595 RVA: 0x003508CF File Offset: 0x0034EACF
	public XuiBindComponentAttribute(bool _required = true)
	{
		this.Required = _required;
	}

	// Token: 0x06008B0C RID: 35596 RVA: 0x003508DE File Offset: 0x0034EADE
	public XuiBindComponentAttribute(string _xmlElementName, bool _required = true)
	{
		this.XmlElementName = _xmlElementName;
		this.Required = _required;
	}

	// Token: 0x04006732 RID: 26418
	public readonly string XmlElementName;

	// Token: 0x04006733 RID: 26419
	public readonly bool Required;
}
