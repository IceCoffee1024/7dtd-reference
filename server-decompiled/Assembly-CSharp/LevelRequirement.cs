using System;

// Token: 0x02000AEB RID: 2795
public struct LevelRequirement
{
	// Token: 0x06005305 RID: 21253 RVA: 0x001FEB8B File Offset: 0x001FCD8B
	public LevelRequirement(int _level)
	{
		this.Level = _level;
		this.Requirements = null;
	}

	// Token: 0x040040EA RID: 16618
	public readonly int Level;

	// Token: 0x040040EB RID: 16619
	public RequirementGroup Requirements;
}
