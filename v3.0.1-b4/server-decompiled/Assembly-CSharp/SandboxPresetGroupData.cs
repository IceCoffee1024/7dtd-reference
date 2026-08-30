using System;

// Token: 0x02000E27 RID: 3623
public struct SandboxPresetGroupData : IEquatable<SandboxPresetGroupData>
{
	// Token: 0x06006F6E RID: 28526 RVA: 0x002AF9E0 File Offset: 0x002ADBE0
	public SandboxPresetGroupData(string _internalName, string _formattedName)
	{
		this.InternalName = _internalName;
		this.formattedName = _formattedName;
	}

	// Token: 0x06006F6F RID: 28527 RVA: 0x002AF9F0 File Offset: 0x002ADBF0
	public override string ToString()
	{
		return this.formattedName;
	}

	// Token: 0x06006F70 RID: 28528 RVA: 0x002AF9F8 File Offset: 0x002ADBF8
	public bool Equals(SandboxPresetGroupData _other)
	{
		return this.InternalName == _other.InternalName;
	}

	// Token: 0x06006F71 RID: 28529 RVA: 0x002AFA0C File Offset: 0x002ADC0C
	public override bool Equals(object _obj)
	{
		if (_obj is SandboxPresetGroupData)
		{
			SandboxPresetGroupData sandboxPresetGroupData = (SandboxPresetGroupData)_obj;
			return this.Equals(sandboxPresetGroupData);
		}
		return false;
	}

	// Token: 0x06006F72 RID: 28530 RVA: 0x002AFA31 File Offset: 0x002ADC31
	public override int GetHashCode()
	{
		return ((this.InternalName != null) ? this.InternalName.GetHashCode() : 0) * 397;
	}

	// Token: 0x0400540E RID: 21518
	public readonly string InternalName;

	// Token: 0x0400540F RID: 21519
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string formattedName;
}
