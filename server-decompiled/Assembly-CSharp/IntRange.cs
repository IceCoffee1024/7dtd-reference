using System;

// Token: 0x020014B4 RID: 5300
public readonly struct IntRange
{
	// Token: 0x0600A675 RID: 42613 RVA: 0x003DF734 File Offset: 0x003DD934
	public IntRange(int _min, int _max)
	{
		this.min = _min;
		this.max = _max;
	}

	// Token: 0x0600A676 RID: 42614 RVA: 0x003DF744 File Offset: 0x003DD944
	public bool IsSet()
	{
		return this.min != 0 || this.max != 0;
	}

	// Token: 0x0600A677 RID: 42615 RVA: 0x003DF759 File Offset: 0x003DD959
	public int Random(GameRandom _rnd)
	{
		return _rnd.RandomRange(this.min, this.max);
	}

	// Token: 0x0600A678 RID: 42616 RVA: 0x003DF76D File Offset: 0x003DD96D
	public int RandomInclusive(GameRandom _rnd)
	{
		return _rnd.RandomRange(this.min, this.max + 1);
	}

	// Token: 0x0600A679 RID: 42617 RVA: 0x003DF784 File Offset: 0x003DD984
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"(",
			this.min.ToString(),
			"-",
			this.max.ToString(),
			")"
		});
	}

	// Token: 0x04007BBA RID: 31674
	public readonly int min;

	// Token: 0x04007BBB RID: 31675
	public readonly int max;
}
