using System;

// Token: 0x020014B3 RID: 5299
public readonly struct FloatRange
{
	// Token: 0x0600A671 RID: 42609 RVA: 0x003DF6A1 File Offset: 0x003DD8A1
	public FloatRange(float _min, float _max)
	{
		this.min = _min;
		this.max = _max;
	}

	// Token: 0x0600A672 RID: 42610 RVA: 0x003DF6B1 File Offset: 0x003DD8B1
	public bool IsSet()
	{
		return this.min != 0f || this.max != 0f;
	}

	// Token: 0x0600A673 RID: 42611 RVA: 0x003DF6D2 File Offset: 0x003DD8D2
	public float Random(GameRandom _rnd)
	{
		return _rnd.RandomRange(this.min, this.max);
	}

	// Token: 0x0600A674 RID: 42612 RVA: 0x003DF6E8 File Offset: 0x003DD8E8
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"(",
			this.min.ToCultureInvariantString(),
			"-",
			this.max.ToCultureInvariantString(),
			")"
		});
	}

	// Token: 0x04007BB8 RID: 31672
	public readonly float min;

	// Token: 0x04007BB9 RID: 31673
	public readonly float max;
}
