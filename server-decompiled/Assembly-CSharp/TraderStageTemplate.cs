using System;

// Token: 0x020008B6 RID: 2230
public class TraderStageTemplate
{
	// Token: 0x060040AA RID: 16554 RVA: 0x00193761 File Offset: 0x00191961
	public bool IsWithin(int traderStage, int quality)
	{
		return (this.Min == -1 || this.Min <= traderStage) && (this.Max == -1 || this.Max >= traderStage) && (this.Quality == -1 || quality == this.Quality);
	}

	// Token: 0x04003460 RID: 13408
	public int Min = -1;

	// Token: 0x04003461 RID: 13409
	public int Max = -1;

	// Token: 0x04003462 RID: 13410
	public int Quality = -1;
}
