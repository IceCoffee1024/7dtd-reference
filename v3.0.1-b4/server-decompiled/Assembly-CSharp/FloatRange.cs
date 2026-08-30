using System;

// Token: 0x02001498 RID: 5272
public readonly struct FloatRange
{
	// Token: 0x0600A574 RID: 42356 RVA: 0x003DA361 File Offset: 0x003D8561
	public FloatRange(float _min, float _max)
	{
		this.min = _min;
		this.max = _max;
	}

	// Token: 0x0600A575 RID: 42357 RVA: 0x003DA371 File Offset: 0x003D8571
	public bool IsSet()
	{
		return this.min != 0f || this.max != 0f;
	}

	// Token: 0x0600A576 RID: 42358 RVA: 0x003DA392 File Offset: 0x003D8592
	public float Random(GameRandom _rnd)
	{
		return _rnd.RandomRange(this.min, this.max);
	}

	// Token: 0x0600A577 RID: 42359 RVA: 0x003DA3A8 File Offset: 0x003D85A8
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

	// Token: 0x04007ADF RID: 31455
	public readonly float min;

	// Token: 0x04007AE0 RID: 31456
	public readonly float max;
}
