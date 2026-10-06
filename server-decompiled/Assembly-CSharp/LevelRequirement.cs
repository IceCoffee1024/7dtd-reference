using System;

// Token: 0x02000AE0 RID: 2784
public struct LevelRequirement
{
	// Token: 0x060052E0 RID: 21216 RVA: 0x001FD608 File Offset: 0x001FB808
	public LevelRequirement(int _level)
	{
		this.Level = _level;
		this.Requirements = null;
	}

	// Token: 0x040040C5 RID: 16581
	public readonly int Level;

	// Token: 0x040040C6 RID: 16582
	public RequirementGroup Requirements;
}
