using System;

// Token: 0x020014E7 RID: 5351
public readonly struct FloatRange
{
	// Token: 0x0600A77C RID: 42876 RVA: 0x003E68F9 File Offset: 0x003E4AF9
	public FloatRange(float _min, float _max)
	{
		this.min = _min;
		this.max = _max;
	}

	// Token: 0x0600A77D RID: 42877 RVA: 0x003E6909 File Offset: 0x003E4B09
	public bool IsSet()
	{
		return this.min != 0f || this.max != 0f;
	}

	// Token: 0x0600A77E RID: 42878 RVA: 0x003E692A File Offset: 0x003E4B2A
	public float Random(GameRandom _rnd)
	{
		return _rnd.RandomRange(this.min, this.max);
	}

	// Token: 0x0600A77F RID: 42879 RVA: 0x003E6940 File Offset: 0x003E4B40
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

	// Token: 0x04007C7C RID: 31868
	public readonly float min;

	// Token: 0x04007C7D RID: 31869
	public readonly float max;
}
