using System;

// Token: 0x020014E8 RID: 5352
public readonly struct IntRange
{
	// Token: 0x0600A780 RID: 42880 RVA: 0x003E698C File Offset: 0x003E4B8C
	public IntRange(int _min, int _max)
	{
		this.min = _min;
		this.max = _max;
	}

	// Token: 0x0600A781 RID: 42881 RVA: 0x003E699C File Offset: 0x003E4B9C
	public bool IsSet()
	{
		return this.min != 0 || this.max != 0;
	}

	// Token: 0x0600A782 RID: 42882 RVA: 0x003E69B1 File Offset: 0x003E4BB1
	public int Random(GameRandom _rnd)
	{
		return _rnd.RandomRange(this.min, this.max);
	}

	// Token: 0x0600A783 RID: 42883 RVA: 0x003E69C5 File Offset: 0x003E4BC5
	public int RandomInclusive(GameRandom _rnd)
	{
		return _rnd.RandomRange(this.min, this.max + 1);
	}

	// Token: 0x0600A784 RID: 42884 RVA: 0x003E69DC File Offset: 0x003E4BDC
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

	// Token: 0x04007C7E RID: 31870
	public readonly int min;

	// Token: 0x04007C7F RID: 31871
	public readonly int max;
}
