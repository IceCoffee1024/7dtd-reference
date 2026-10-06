using System;

// Token: 0x020014D5 RID: 5333
public readonly struct FloatRange
{
	// Token: 0x0600A724 RID: 42788 RVA: 0x003E44F5 File Offset: 0x003E26F5
	public FloatRange(float _min, float _max)
	{
		this.min = _min;
		this.max = _max;
	}

	// Token: 0x0600A725 RID: 42789 RVA: 0x003E4505 File Offset: 0x003E2705
	public bool IsSet()
	{
		return this.min != 0f || this.max != 0f;
	}

	// Token: 0x0600A726 RID: 42790 RVA: 0x003E4526 File Offset: 0x003E2726
	public float Random(GameRandom _rnd)
	{
		return _rnd.RandomRange(this.min, this.max);
	}

	// Token: 0x0600A727 RID: 42791 RVA: 0x003E453C File Offset: 0x003E273C
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

	// Token: 0x04007C3B RID: 31803
	public readonly float min;

	// Token: 0x04007C3C RID: 31804
	public readonly float max;
}
