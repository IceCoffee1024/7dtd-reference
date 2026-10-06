using System;

// Token: 0x020014D6 RID: 5334
public readonly struct IntRange
{
	// Token: 0x0600A728 RID: 42792 RVA: 0x003E4588 File Offset: 0x003E2788
	public IntRange(int _min, int _max)
	{
		this.min = _min;
		this.max = _max;
	}

	// Token: 0x0600A729 RID: 42793 RVA: 0x003E4598 File Offset: 0x003E2798
	public bool IsSet()
	{
		return this.min != 0 || this.max != 0;
	}

	// Token: 0x0600A72A RID: 42794 RVA: 0x003E45AD File Offset: 0x003E27AD
	public int Random(GameRandom _rnd)
	{
		return _rnd.RandomRange(this.min, this.max);
	}

	// Token: 0x0600A72B RID: 42795 RVA: 0x003E45C1 File Offset: 0x003E27C1
	public int RandomInclusive(GameRandom _rnd)
	{
		return _rnd.RandomRange(this.min, this.max + 1);
	}

	// Token: 0x0600A72C RID: 42796 RVA: 0x003E45D8 File Offset: 0x003E27D8
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

	// Token: 0x04007C3D RID: 31805
	public readonly int min;

	// Token: 0x04007C3E RID: 31806
	public readonly int max;
}
