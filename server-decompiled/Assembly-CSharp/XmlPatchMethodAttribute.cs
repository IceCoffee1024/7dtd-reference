using System;
using JetBrains.Annotations;

// Token: 0x020014EB RID: 5355
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
[MeansImplicitUse]
public class XmlPatchMethodAttribute : Attribute
{
	// Token: 0x0600A862 RID: 43106 RVA: 0x003E9D76 File Offset: 0x003E7F76
	public XmlPatchMethodAttribute(string _patchName)
	{
		this.PatchName = _patchName;
	}

	// Token: 0x0600A863 RID: 43107 RVA: 0x003E9D8C File Offset: 0x003E7F8C
	public XmlPatchMethodAttribute(string _patchName, bool _requiresXpath)
	{
		this.PatchName = _patchName;
		this.RequiresXpath = _requiresXpath;
	}

	// Token: 0x04007C59 RID: 31833
	public readonly string PatchName;

	// Token: 0x04007C5A RID: 31834
	public readonly bool RequiresXpath = true;
}
