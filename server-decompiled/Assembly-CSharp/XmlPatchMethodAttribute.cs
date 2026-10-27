using System;
using JetBrains.Annotations;

// Token: 0x0200153A RID: 5434
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
[MeansImplicitUse]
public class XmlPatchMethodAttribute : Attribute
{
	// Token: 0x0600AA6A RID: 43626 RVA: 0x003F630E File Offset: 0x003F450E
	public XmlPatchMethodAttribute(string _patchName)
	{
		this.PatchName = _patchName;
	}

	// Token: 0x0600AA6B RID: 43627 RVA: 0x003F6324 File Offset: 0x003F4524
	public XmlPatchMethodAttribute(string _patchName, bool _requiresXpath)
	{
		this.PatchName = _patchName;
		this.RequiresXpath = _requiresXpath;
	}

	// Token: 0x04007DF6 RID: 32246
	public readonly string PatchName;

	// Token: 0x04007DF7 RID: 32247
	public readonly bool RequiresXpath = true;
}
