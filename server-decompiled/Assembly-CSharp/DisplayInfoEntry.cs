using System;

// Token: 0x020010EC RID: 4332
public class DisplayInfoEntry
{
	// Token: 0x17001026 RID: 4134
	// (get) Token: 0x060089C8 RID: 35272 RVA: 0x0034A4A7 File Offset: 0x003486A7
	// (set) Token: 0x060089C9 RID: 35273 RVA: 0x0034A4AF File Offset: 0x003486AF
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

	// Token: 0x0400666F RID: 26223
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tags = FastTags<TagGroup.Global>.none;

	// Token: 0x04006670 RID: 26224
	public bool TagsSet;

	// Token: 0x04006671 RID: 26225
	public PassiveEffects StatType;

	// Token: 0x04006672 RID: 26226
	public string CustomName = "";

	// Token: 0x04006673 RID: 26227
	public string TitleOverride;

	// Token: 0x04006674 RID: 26228
	public DisplayInfoEntry.DisplayTypes DisplayType;

	// Token: 0x04006675 RID: 26229
	public bool ShowInverted;

	// Token: 0x04006676 RID: 26230
	public bool NegativePreferred;

	// Token: 0x04006677 RID: 26231
	public bool DisplayLeadingPlus;

	// Token: 0x020010ED RID: 4333
	public enum DisplayTypes
	{
		// Token: 0x04006679 RID: 26233
		Integer,
		// Token: 0x0400667A RID: 26234
		Decimal1,
		// Token: 0x0400667B RID: 26235
		Decimal2,
		// Token: 0x0400667C RID: 26236
		Bool,
		// Token: 0x0400667D RID: 26237
		Percent,
		// Token: 0x0400667E RID: 26238
		Time
	}
}
