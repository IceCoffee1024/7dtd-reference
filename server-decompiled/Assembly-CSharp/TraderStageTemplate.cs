using System;

// Token: 0x020008C5 RID: 2245
public class TraderStageTemplate
{
	// Token: 0x06004063 RID: 16483 RVA: 0x0019490D File Offset: 0x00192B0D
	public bool IsWithin(int traderStage, int quality)
	{
		return (this.Min == -1 || this.Min <= traderStage) && (this.Max == -1 || this.Max >= traderStage) && (this.Quality == -1 || quality == this.Quality);
	}

	// Token: 0x04003474 RID: 13428
	public int Min = -1;

	// Token: 0x04003475 RID: 13429
	public int Max = -1;

	// Token: 0x04003476 RID: 13430
	public int Quality = -1;
}
