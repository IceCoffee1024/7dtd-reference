using System;

// Token: 0x02000E3C RID: 3644
public struct SandboxPresetGroupData : IEquatable<SandboxPresetGroupData>
{
	// Token: 0x0600700E RID: 28686 RVA: 0x002B2F04 File Offset: 0x002B1104
	public SandboxPresetGroupData(string _internalName, string _formattedName)
	{
		this.InternalName = _internalName;
		this.formattedName = _formattedName;
	}

	// Token: 0x0600700F RID: 28687 RVA: 0x002B2F14 File Offset: 0x002B1114
	public override string ToString()
	{
		return this.formattedName;
	}

	// Token: 0x06007010 RID: 28688 RVA: 0x002B2F1C File Offset: 0x002B111C
	public bool Equals(SandboxPresetGroupData _other)
	{
		return this.InternalName == _other.InternalName;
	}

	// Token: 0x06007011 RID: 28689 RVA: 0x002B2F30 File Offset: 0x002B1130
	public override bool Equals(object _obj)
	{
		if (_obj is SandboxPresetGroupData)
		{
			SandboxPresetGroupData sandboxPresetGroupData = (SandboxPresetGroupData)_obj;
			return this.Equals(sandboxPresetGroupData);
		}
		return false;
	}

	// Token: 0x06007012 RID: 28690 RVA: 0x002B2F55 File Offset: 0x002B1155
	public override int GetHashCode()
	{
		return ((this.InternalName != null) ? this.InternalName.GetHashCode() : 0) * 397;
	}

	// Token: 0x040054B2 RID: 21682
	public readonly string InternalName;

	// Token: 0x040054B3 RID: 21683
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string formattedName;
}
