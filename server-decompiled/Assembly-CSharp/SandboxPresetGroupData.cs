using System;

// Token: 0x02000E56 RID: 3670
public struct SandboxPresetGroupData : IEquatable<SandboxPresetGroupData>
{
	// Token: 0x0600702B RID: 28715 RVA: 0x002B7820 File Offset: 0x002B5A20
	public SandboxPresetGroupData(string _internalName, string _formattedName)
	{
		this.InternalName = _internalName;
		this.formattedName = _formattedName;
	}

	// Token: 0x0600702C RID: 28716 RVA: 0x002B7830 File Offset: 0x002B5A30
	public override string ToString()
	{
		return this.formattedName;
	}

	// Token: 0x0600702D RID: 28717 RVA: 0x002B7838 File Offset: 0x002B5A38
	public bool Equals(SandboxPresetGroupData _other)
	{
		return this.InternalName == _other.InternalName;
	}

	// Token: 0x0600702E RID: 28718 RVA: 0x002B784C File Offset: 0x002B5A4C
	public override bool Equals(object _obj)
	{
		if (_obj is SandboxPresetGroupData)
		{
			SandboxPresetGroupData sandboxPresetGroupData = (SandboxPresetGroupData)_obj;
			return this.Equals(sandboxPresetGroupData);
		}
		return false;
	}

	// Token: 0x0600702F RID: 28719 RVA: 0x002B7871 File Offset: 0x002B5A71
	public override int GetHashCode()
	{
		return ((this.InternalName != null) ? this.InternalName.GetHashCode() : 0) * 397;
	}

	// Token: 0x0400550E RID: 21774
	public readonly string InternalName;

	// Token: 0x0400550F RID: 21775
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string formattedName;
}
