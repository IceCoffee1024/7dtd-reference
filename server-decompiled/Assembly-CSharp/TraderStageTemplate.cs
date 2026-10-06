using System;

// Token: 0x020008BB RID: 2235
public class TraderStageTemplate
{
	// Token: 0x06004040 RID: 16448 RVA: 0x00193865 File Offset: 0x00191A65
	public bool IsWithin(int traderStage, int quality)
	{
		return (this.Min == -1 || this.Min <= traderStage) && (this.Max == -1 || this.Max >= traderStage) && (this.Quality == -1 || quality == this.Quality);
	}

	// Token: 0x04003454 RID: 13396
	public int Min = -1;

	// Token: 0x04003455 RID: 13397
	public int Max = -1;

	// Token: 0x04003456 RID: 13398
	public int Quality = -1;
}
