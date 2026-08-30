using System;
using SandboxOptions;

// Token: 0x02000E3D RID: 3645
public struct SandboxPresetInfo : IEquatable<SandboxPresetInfo>
{
	// Token: 0x06007013 RID: 28691 RVA: 0x002B2F74 File Offset: 0x002B1174
	public SandboxPresetInfo(SandboxOptionPreset _preset)
	{
		this.InternalName = _preset.Name;
		this.FormattedName = _preset.DisplayName;
		this.Description = _preset.DisplayDescription;
		this.Icon = _preset.Icon;
		this.Difficulty = (int)_preset.DifficultyRating;
		this.IsUserPreset = _preset.IsUserPreset;
		this.SandboxCode = _preset.SandboxCode;
		this.IsCustomPreset = _preset.IsCustomPreset;
	}

	// Token: 0x06007014 RID: 28692 RVA: 0x002B2FE1 File Offset: 0x002B11E1
	public override string ToString()
	{
		return this.FormattedName;
	}

	// Token: 0x06007015 RID: 28693 RVA: 0x002B2FE9 File Offset: 0x002B11E9
	public bool Equals(SandboxPresetInfo _other)
	{
		return this.InternalName == _other.InternalName;
	}

	// Token: 0x06007016 RID: 28694 RVA: 0x002B2FFC File Offset: 0x002B11FC
	public override bool Equals(object _obj)
	{
		if (_obj is SandboxPresetInfo)
		{
			SandboxPresetInfo sandboxPresetInfo = (SandboxPresetInfo)_obj;
			return this.Equals(sandboxPresetInfo);
		}
		return false;
	}

	// Token: 0x06007017 RID: 28695 RVA: 0x002B3021 File Offset: 0x002B1221
	public override int GetHashCode()
	{
		if (this.InternalName == null)
		{
			return 0;
		}
		return this.InternalName.GetHashCode();
	}

	// Token: 0x040054B4 RID: 21684
	public readonly string InternalName;

	// Token: 0x040054B5 RID: 21685
	public readonly string FormattedName;

	// Token: 0x040054B6 RID: 21686
	public readonly string Description;

	// Token: 0x040054B7 RID: 21687
	public readonly string Icon;

	// Token: 0x040054B8 RID: 21688
	public readonly int Difficulty;

	// Token: 0x040054B9 RID: 21689
	public readonly bool IsUserPreset;

	// Token: 0x040054BA RID: 21690
	public readonly bool IsCustomPreset;

	// Token: 0x040054BB RID: 21691
	public string SandboxCode;
}
