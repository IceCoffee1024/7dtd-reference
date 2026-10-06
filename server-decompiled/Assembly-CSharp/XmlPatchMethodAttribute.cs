using System;
using JetBrains.Annotations;

// Token: 0x02001528 RID: 5416
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
[MeansImplicitUse]
public class XmlPatchMethodAttribute : Attribute
{
	// Token: 0x0600AA12 RID: 43538 RVA: 0x003F3F0A File Offset: 0x003F210A
	public XmlPatchMethodAttribute(string _patchName)
	{
		this.PatchName = _patchName;
	}

	// Token: 0x0600AA13 RID: 43539 RVA: 0x003F3F20 File Offset: 0x003F2120
	public XmlPatchMethodAttribute(string _patchName, bool _requiresXpath)
	{
		this.PatchName = _patchName;
		this.RequiresXpath = _requiresXpath;
	}

	// Token: 0x04007DB5 RID: 32181
	public readonly string PatchName;

	// Token: 0x04007DB6 RID: 32182
	public readonly bool RequiresXpath = true;
}
