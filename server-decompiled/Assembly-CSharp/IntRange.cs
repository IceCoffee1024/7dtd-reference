using System;

// Token: 0x02001499 RID: 5273
public readonly struct IntRange
{
	// Token: 0x0600A578 RID: 42360 RVA: 0x003DA3F4 File Offset: 0x003D85F4
	public IntRange(int _min, int _max)
	{
		this.min = _min;
		this.max = _max;
	}

	// Token: 0x0600A579 RID: 42361 RVA: 0x003DA404 File Offset: 0x003D8604
	public bool IsSet()
	{
		return this.min != 0 || this.max != 0;
	}

	// Token: 0x0600A57A RID: 42362 RVA: 0x003DA419 File Offset: 0x003D8619
	public int Random(GameRandom _rnd)
	{
		return _rnd.RandomRange(this.min, this.max);
	}

	// Token: 0x0600A57B RID: 42363 RVA: 0x003DA42D File Offset: 0x003D862D
	public int RandomInclusive(GameRandom _rnd)
	{
		return _rnd.RandomRange(this.min, this.max + 1);
	}

	// Token: 0x0600A57C RID: 42364 RVA: 0x003DA444 File Offset: 0x003D8644
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

	// Token: 0x04007AE1 RID: 31457
	public readonly int min;

	// Token: 0x04007AE2 RID: 31458
	public readonly int max;
}
