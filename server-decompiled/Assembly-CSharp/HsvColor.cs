using System;

// Token: 0x0200141D RID: 5149
public struct HsvColor
{
	// Token: 0x170012F6 RID: 4854
	// (get) Token: 0x0600A1CE RID: 41422 RVA: 0x003CB800 File Offset: 0x003C9A00
	// (set) Token: 0x0600A1CF RID: 41423 RVA: 0x003CB80F File Offset: 0x003C9A0F
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

	// Token: 0x170012F7 RID: 4855
	// (get) Token: 0x0600A1D0 RID: 41424 RVA: 0x003CB823 File Offset: 0x003C9A23
	// (set) Token: 0x0600A1D1 RID: 41425 RVA: 0x003CB82C File Offset: 0x003C9A2C
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

	// Token: 0x170012F8 RID: 4856
	// (get) Token: 0x0600A1D2 RID: 41426 RVA: 0x003CB836 File Offset: 0x003C9A36
	// (set) Token: 0x0600A1D3 RID: 41427 RVA: 0x003CB83F File Offset: 0x003C9A3F
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

	// Token: 0x0600A1D4 RID: 41428 RVA: 0x003CB849 File Offset: 0x003C9A49
	public HsvColor(double h, double s, double v)
	{
		this.H = h;
		this.S = s;
		this.V = v;
	}

	// Token: 0x0600A1D5 RID: 41429 RVA: 0x003CB860 File Offset: 0x003C9A60
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

	// Token: 0x040079AC RID: 31148
	public double H;

	// Token: 0x040079AD RID: 31149
	public double S;

	// Token: 0x040079AE RID: 31150
	public double V;
}
