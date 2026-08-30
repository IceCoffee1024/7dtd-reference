using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016C0 RID: 5824
	public struct Rational
	{
		// Token: 0x0600B5A5 RID: 46501 RVA: 0x004392F2 File Offset: 0x004374F2
		public Rational(int numerator, uint denominator)
		{
			this.Numerator = numerator;
			this.Denominator = denominator;
		}

		// Token: 0x0600B5A6 RID: 46502 RVA: 0x00439302 File Offset: 0x00437502
		public override string ToString()
		{
			return string.Format("{0}/{1}", this.Numerator, this.Denominator);
		}

		// Token: 0x170015E7 RID: 5607
		// (get) Token: 0x0600B5A7 RID: 46503 RVA: 0x00439324 File Offset: 0x00437524
		public double Value
		{
			get
			{
				return (double)this.Numerator / this.Denominator;
			}
		}

		// Token: 0x040087C1 RID: 34753
		public readonly int Numerator;

		// Token: 0x040087C2 RID: 34754
		public readonly uint Denominator;
	}
}
