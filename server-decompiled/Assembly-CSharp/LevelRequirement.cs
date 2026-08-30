using System;

// Token: 0x02000ABD RID: 2749
public struct LevelRequirement
{
	// Token: 0x06005241 RID: 21057 RVA: 0x001F7910 File Offset: 0x001F5B10
	public LevelRequirement(int _level)
	{
		this.Level = _level;
		this.Requirements = null;
	}

	// Token: 0x04003FF7 RID: 16375
	public readonly int Level;

	// Token: 0x04003FF8 RID: 16376
	public RequirementGroup Requirements;
}
