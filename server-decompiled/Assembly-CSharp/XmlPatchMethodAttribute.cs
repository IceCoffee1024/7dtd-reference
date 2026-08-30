using System;
using JetBrains.Annotations;

// Token: 0x02001506 RID: 5382
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
[MeansImplicitUse]
public class XmlPatchMethodAttribute : Attribute
{
	// Token: 0x0600A95F RID: 43359 RVA: 0x003EF0B6 File Offset: 0x003ED2B6
	public XmlPatchMethodAttribute(string _patchName)
	{
		this.PatchName = _patchName;
	}

	// Token: 0x0600A960 RID: 43360 RVA: 0x003EF0CC File Offset: 0x003ED2CC
	public XmlPatchMethodAttribute(string _patchName, bool _requiresXpath)
	{
		this.PatchName = _patchName;
		this.RequiresXpath = _requiresXpath;
	}

	// Token: 0x04007D32 RID: 32050
	public readonly string PatchName;

	// Token: 0x04007D33 RID: 32051
	public readonly bool RequiresXpath = true;
}
