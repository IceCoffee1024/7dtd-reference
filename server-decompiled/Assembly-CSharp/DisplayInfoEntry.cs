using System;

// Token: 0x020010FC RID: 4348
public class DisplayInfoEntry
{
	// Token: 0x17001031 RID: 4145
	// (get) Token: 0x06008A1C RID: 35356 RVA: 0x0034C558 File Offset: 0x0034A758
	// (set) Token: 0x06008A1D RID: 35357 RVA: 0x0034C560 File Offset: 0x0034A760
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

	// Token: 0x040066AF RID: 26287
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tags = FastTags<TagGroup.Global>.none;

	// Token: 0x040066B0 RID: 26288
	public bool TagsSet;

	// Token: 0x040066B1 RID: 26289
	public PassiveEffects StatType;

	// Token: 0x040066B2 RID: 26290
	public string CustomName = "";

	// Token: 0x040066B3 RID: 26291
	public string TitleOverride;

	// Token: 0x040066B4 RID: 26292
	public DisplayInfoEntry.DisplayTypes DisplayType;

	// Token: 0x040066B5 RID: 26293
	public bool ShowInverted;

	// Token: 0x040066B6 RID: 26294
	public bool NegativePreferred;

	// Token: 0x040066B7 RID: 26295
	public bool DisplayLeadingPlus;

	// Token: 0x020010FD RID: 4349
	public enum DisplayTypes
	{
		// Token: 0x040066B9 RID: 26297
		Integer,
		// Token: 0x040066BA RID: 26298
		Decimal1,
		// Token: 0x040066BB RID: 26299
		Decimal2,
		// Token: 0x040066BC RID: 26300
		Bool,
		// Token: 0x040066BD RID: 26301
		Percent,
		// Token: 0x040066BE RID: 26302
		Time
	}
}
