using System;
using SandboxOptions;

// Token: 0x02000E28 RID: 3624
public struct SandboxPresetInfo : IEquatable<SandboxPresetInfo>
{
	// Token: 0x06006F73 RID: 28531 RVA: 0x002AFA50 File Offset: 0x002ADC50
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

	// Token: 0x06006F74 RID: 28532 RVA: 0x002AFABD File Offset: 0x002ADCBD
	public override string ToString()
	{
		return this.FormattedName;
	}

	// Token: 0x06006F75 RID: 28533 RVA: 0x002AFAC5 File Offset: 0x002ADCC5
	public bool Equals(SandboxPresetInfo _other)
	{
		return this.InternalName == _other.InternalName;
	}

	// Token: 0x06006F76 RID: 28534 RVA: 0x002AFAD8 File Offset: 0x002ADCD8
	public override bool Equals(object _obj)
	{
		if (_obj is SandboxPresetInfo)
		{
			SandboxPresetInfo sandboxPresetInfo = (SandboxPresetInfo)_obj;
			return this.Equals(sandboxPresetInfo);
		}
		return false;
	}

	// Token: 0x06006F77 RID: 28535 RVA: 0x002AFAFD File Offset: 0x002ADCFD
	public override int GetHashCode()
	{
		if (this.InternalName == null)
		{
			return 0;
		}
		return this.InternalName.GetHashCode();
	}

	// Token: 0x04005410 RID: 21520
	public readonly string InternalName;

	// Token: 0x04005411 RID: 21521
	public readonly string FormattedName;

	// Token: 0x04005412 RID: 21522
	public readonly string Description;

	// Token: 0x04005413 RID: 21523
	public readonly string Icon;

	// Token: 0x04005414 RID: 21524
	public readonly int Difficulty;

	// Token: 0x04005415 RID: 21525
	public readonly bool IsUserPreset;

	// Token: 0x04005416 RID: 21526
	public readonly bool IsCustomPreset;

	// Token: 0x04005417 RID: 21527
	public string SandboxCode;
}
