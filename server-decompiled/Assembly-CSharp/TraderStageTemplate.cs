using System;

// Token: 0x020008AA RID: 2218
public class TraderStageTemplate
{
	// Token: 0x06004045 RID: 16453 RVA: 0x00191689 File Offset: 0x0018F889
	public bool IsWithin(int traderStage, int quality)
	{
		return (this.Min == -1 || this.Min <= traderStage) && (this.Max == -1 || this.Max >= traderStage) && (this.Quality == -1 || quality == this.Quality);
	}

	// Token: 0x040033DF RID: 13279
	public int Min = -1;

	// Token: 0x040033E0 RID: 13280
	public int Max = -1;

	// Token: 0x040033E1 RID: 13281
	public int Quality = -1;
}
