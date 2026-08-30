using System;

// Token: 0x020010C2 RID: 4290
public class DisplayInfoEntry
{
	// Token: 0x17000FB8 RID: 4024
	// (get) Token: 0x06008888 RID: 34952 RVA: 0x00341ED3 File Offset: 0x003400D3
	// (set) Token: 0x06008889 RID: 34953 RVA: 0x00341EDB File Offset: 0x003400DB
	public FastTags<TagGroup.Global> Tags
	{
		get
		{
			return this.tags;
		}
		set
		{
			this.tags = value;
			this.TagsSet = true;
		}
	}

	// Token: 0x04006566 RID: 25958
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tags = FastTags<TagGroup.Global>.none;

	// Token: 0x04006567 RID: 25959
	public bool TagsSet;

	// Token: 0x04006568 RID: 25960
	public PassiveEffects StatType;

	// Token: 0x04006569 RID: 25961
	public string CustomName = "";

	// Token: 0x0400656A RID: 25962
	public string TitleOverride;

	// Token: 0x0400656B RID: 25963
	public DisplayInfoEntry.DisplayTypes DisplayType;

	// Token: 0x0400656C RID: 25964
	public bool ShowInverted;

	// Token: 0x0400656D RID: 25965
	public bool NegativePreferred;

	// Token: 0x0400656E RID: 25966
	public bool DisplayLeadingPlus;

	// Token: 0x020010C3 RID: 4291
	public enum DisplayTypes
	{
		// Token: 0x04006570 RID: 25968
		Integer,
		// Token: 0x04006571 RID: 25969
		Decimal1,
		// Token: 0x04006572 RID: 25970
		Decimal2,
		// Token: 0x04006573 RID: 25971
		Bool,
		// Token: 0x04006574 RID: 25972
		Percent,
		// Token: 0x04006575 RID: 25973
		Time
	}
}
