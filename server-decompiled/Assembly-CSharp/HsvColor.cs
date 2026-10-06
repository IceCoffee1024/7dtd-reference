using System;

// Token: 0x0200143D RID: 5181
public struct HsvColor
{
	// Token: 0x1700133B RID: 4923
	// (get) Token: 0x0600A279 RID: 41593 RVA: 0x003D039C File Offset: 0x003CE59C
	// (set) Token: 0x0600A27A RID: 41594 RVA: 0x003D03AB File Offset: 0x003CE5AB
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

	// Token: 0x1700133C RID: 4924
	// (get) Token: 0x0600A27B RID: 41595 RVA: 0x003D03BF File Offset: 0x003CE5BF
	// (set) Token: 0x0600A27C RID: 41596 RVA: 0x003D03C8 File Offset: 0x003CE5C8
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

	// Token: 0x1700133D RID: 4925
	// (get) Token: 0x0600A27D RID: 41597 RVA: 0x003D03D2 File Offset: 0x003CE5D2
	// (set) Token: 0x0600A27E RID: 41598 RVA: 0x003D03DB File Offset: 0x003CE5DB
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

	// Token: 0x0600A27F RID: 41599 RVA: 0x003D03E5 File Offset: 0x003CE5E5
	public HsvColor(double h, double s, double v)
	{
		this.H = h;
		this.S = s;
		this.V = v;
	}

	// Token: 0x0600A280 RID: 41600 RVA: 0x003D03FC File Offset: 0x003CE5FC
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

	// Token: 0x04007A27 RID: 31271
	public double H;

	// Token: 0x04007A28 RID: 31272
	public double S;

	// Token: 0x04007A29 RID: 31273
	public double V;
}
