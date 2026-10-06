using System;
using SandboxOptions;

// Token: 0x02000E4C RID: 3660
public struct SandboxPresetInfo : IEquatable<SandboxPresetInfo>
{
	// Token: 0x0600700B RID: 28683 RVA: 0x002B6324 File Offset: 0x002B4524
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

	// Token: 0x0600700C RID: 28684 RVA: 0x002B6391 File Offset: 0x002B4591
	public override string ToString()
	{
		return this.FormattedName;
	}

	// Token: 0x0600700D RID: 28685 RVA: 0x002B6399 File Offset: 0x002B4599
	public bool Equals(SandboxPresetInfo _other)
	{
		return this.InternalName == _other.InternalName;
	}

	// Token: 0x0600700E RID: 28686 RVA: 0x002B63AC File Offset: 0x002B45AC
	public override bool Equals(object _obj)
	{
		if (_obj is SandboxPresetInfo)
		{
			SandboxPresetInfo sandboxPresetInfo = (SandboxPresetInfo)_obj;
			return this.Equals(sandboxPresetInfo);
		}
		return false;
	}

	// Token: 0x0600700F RID: 28687 RVA: 0x002B63D1 File Offset: 0x002B45D1
	public override int GetHashCode()
	{
		if (this.InternalName == null)
		{
			return 0;
		}
		return this.InternalName.GetHashCode();
	}

	// Token: 0x040054EB RID: 21739
	public readonly string InternalName;

	// Token: 0x040054EC RID: 21740
	public readonly string FormattedName;

	// Token: 0x040054ED RID: 21741
	public readonly string Description;

	// Token: 0x040054EE RID: 21742
	public readonly string Icon;

	// Token: 0x040054EF RID: 21743
	public readonly int Difficulty;

	// Token: 0x040054F0 RID: 21744
	public readonly bool IsUserPreset;

	// Token: 0x040054F1 RID: 21745
	public readonly bool IsCustomPreset;

	// Token: 0x040054F2 RID: 21746
	public string SandboxCode;
}
