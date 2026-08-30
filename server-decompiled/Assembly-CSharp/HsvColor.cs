using System;

// Token: 0x02001402 RID: 5122
public struct HsvColor
{
	// Token: 0x170012BD RID: 4797
	// (get) Token: 0x0600A0D1 RID: 41169 RVA: 0x003C64BC File Offset: 0x003C46BC
	// (set) Token: 0x0600A0D2 RID: 41170 RVA: 0x003C64CB File Offset: 0x003C46CB
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

	// Token: 0x170012BE RID: 4798
	// (get) Token: 0x0600A0D3 RID: 41171 RVA: 0x003C64DF File Offset: 0x003C46DF
	// (set) Token: 0x0600A0D4 RID: 41172 RVA: 0x003C64E8 File Offset: 0x003C46E8
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

	// Token: 0x170012BF RID: 4799
	// (get) Token: 0x0600A0D5 RID: 41173 RVA: 0x003C64F2 File Offset: 0x003C46F2
	// (set) Token: 0x0600A0D6 RID: 41174 RVA: 0x003C64FB File Offset: 0x003C46FB
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

	// Token: 0x0600A0D7 RID: 41175 RVA: 0x003C6505 File Offset: 0x003C4705
	public HsvColor(double h, double s, double v)
	{
		this.H = h;
		this.S = s;
		this.V = v;
	}

	// Token: 0x0600A0D8 RID: 41176 RVA: 0x003C651C File Offset: 0x003C471C
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

	// Token: 0x040078D3 RID: 30931
	public double H;

	// Token: 0x040078D4 RID: 30932
	public double S;

	// Token: 0x040078D5 RID: 30933
	public double V;
}
