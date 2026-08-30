using System;

// Token: 0x02000ACE RID: 2766
public struct LevelRequirement
{
	// Token: 0x060052C7 RID: 21191 RVA: 0x001FA3A8 File Offset: 0x001F85A8
	public LevelRequirement(int _level)
	{
		this.Level = _level;
		this.Requirements = null;
	}

	// Token: 0x04004088 RID: 16520
	public readonly int Level;

	// Token: 0x04004089 RID: 16521
	public RequirementGroup Requirements;
}
