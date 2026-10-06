using System;

// Token: 0x02000E4B RID: 3659
public struct SandboxPresetGroupData : IEquatable<SandboxPresetGroupData>
{
	// Token: 0x06007006 RID: 28678 RVA: 0x002B62B4 File Offset: 0x002B44B4
	public SandboxPresetGroupData(string _internalName, string _formattedName)
	{
		this.InternalName = _internalName;
		this.formattedName = _formattedName;
	}

	// Token: 0x06007007 RID: 28679 RVA: 0x002B62C4 File Offset: 0x002B44C4
	public override string ToString()
	{
		return this.formattedName;
	}

	// Token: 0x06007008 RID: 28680 RVA: 0x002B62CC File Offset: 0x002B44CC
	public bool Equals(SandboxPresetGroupData _other)
	{
		return this.InternalName == _other.InternalName;
	}

	// Token: 0x06007009 RID: 28681 RVA: 0x002B62E0 File Offset: 0x002B44E0
	public override bool Equals(object _obj)
	{
		if (_obj is SandboxPresetGroupData)
		{
			SandboxPresetGroupData sandboxPresetGroupData = (SandboxPresetGroupData)_obj;
			return this.Equals(sandboxPresetGroupData);
		}
		return false;
	}

	// Token: 0x0600700A RID: 28682 RVA: 0x002B6305 File Offset: 0x002B4505
	public override int GetHashCode()
	{
		return ((this.InternalName != null) ? this.InternalName.GetHashCode() : 0) * 397;
	}

	// Token: 0x040054E9 RID: 21737
	public readonly string InternalName;

	// Token: 0x040054EA RID: 21738
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string formattedName;
}
