using System;

// Token: 0x0200144F RID: 5199
public struct HsvColor
{
	// Token: 0x17001347 RID: 4935
	// (get) Token: 0x0600A2D1 RID: 41681 RVA: 0x003D27A0 File Offset: 0x003D09A0
	// (set) Token: 0x0600A2D2 RID: 41682 RVA: 0x003D27AF File Offset: 0x003D09AF
	public float normalizedH
	{
		get
		{
			return (float)this.H / 360f;
		}
		set
		{
			this.H = (double)value * 360.0;
		}
	}

	// Token: 0x17001348 RID: 4936
	// (get) Token: 0x0600A2D3 RID: 41683 RVA: 0x003D27C3 File Offset: 0x003D09C3
	// (set) Token: 0x0600A2D4 RID: 41684 RVA: 0x003D27CC File Offset: 0x003D09CC
	public float normalizedS
	{
		get
		{
			return (float)this.S;
		}
		set
		{
			this.S = (double)value;
		}
	}

	// Token: 0x17001349 RID: 4937
	// (get) Token: 0x0600A2D5 RID: 41685 RVA: 0x003D27D6 File Offset: 0x003D09D6
	// (set) Token: 0x0600A2D6 RID: 41686 RVA: 0x003D27DF File Offset: 0x003D09DF
	public float normalizedV
	{
		get
		{
			return (float)this.V;
		}
		set
		{
			this.V = (double)value;
		}
	}

	// Token: 0x0600A2D7 RID: 41687 RVA: 0x003D27E9 File Offset: 0x003D09E9
	public HsvColor(double h, double s, double v)
	{
		this.H = h;
		this.S = s;
		this.V = v;
	}

	// Token: 0x0600A2D8 RID: 41688 RVA: 0x003D2800 File Offset: 0x003D0A00
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"{",
			this.H.ToCultureInvariantString("f2"),
			",",
			this.S.ToCultureInvariantString("f2"),
			",",
			this.V.ToCultureInvariantString("f2"),
			"}"
		});
	}

	// Token: 0x04007A68 RID: 31336
	public double H;

	// Token: 0x04007A69 RID: 31337
	public double S;

	// Token: 0x04007A6A RID: 31338
	public double V;
}
