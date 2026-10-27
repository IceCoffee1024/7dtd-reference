using System;
using SandboxOptions;

// Token: 0x02000E57 RID: 3671
public struct SandboxPresetInfo : IEquatable<SandboxPresetInfo>
{
	// Token: 0x06007030 RID: 28720 RVA: 0x002B7890 File Offset: 0x002B5A90
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

	// Token: 0x06007031 RID: 28721 RVA: 0x002B78FD File Offset: 0x002B5AFD
	public override string ToString()
	{
		return this.FormattedName;
	}

	// Token: 0x06007032 RID: 28722 RVA: 0x002B7905 File Offset: 0x002B5B05
	public bool Equals(SandboxPresetInfo _other)
	{
		return this.InternalName == _other.InternalName;
	}

	// Token: 0x06007033 RID: 28723 RVA: 0x002B7918 File Offset: 0x002B5B18
	public override bool Equals(object _obj)
	{
		if (_obj is SandboxPresetInfo)
		{
			SandboxPresetInfo sandboxPresetInfo = (SandboxPresetInfo)_obj;
			return this.Equals(sandboxPresetInfo);
		}
		return false;
	}

	// Token: 0x06007034 RID: 28724 RVA: 0x002B793D File Offset: 0x002B5B3D
	public override int GetHashCode()
	{
		if (this.InternalName == null)
		{
			return 0;
		}
		return this.InternalName.GetHashCode();
	}

	// Token: 0x04005510 RID: 21776
	public readonly string InternalName;

	// Token: 0x04005511 RID: 21777
	public readonly string FormattedName;

	// Token: 0x04005512 RID: 21778
	public readonly string Description;

	// Token: 0x04005513 RID: 21779
	public readonly string Icon;

	// Token: 0x04005514 RID: 21780
	public readonly int Difficulty;

	// Token: 0x04005515 RID: 21781
	public readonly bool IsUserPreset;

	// Token: 0x04005516 RID: 21782
	public readonly bool IsCustomPreset;

	// Token: 0x04005517 RID: 21783
	public string SandboxCode;
}
