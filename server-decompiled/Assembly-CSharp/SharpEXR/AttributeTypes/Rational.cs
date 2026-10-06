using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x02001712 RID: 5906
	public struct Rational
	{
		// Token: 0x0600B87F RID: 47231 RVA: 0x0044358A File Offset: 0x0044178A
		public Rational(int numerator, uint denominator)
		{
			this.Numerator = numerator;
			this.Denominator = denominator;
		}

		// Token: 0x0600B880 RID: 47232 RVA: 0x0044359A File Offset: 0x0044179A
		public override string ToString()
		{
			return string.Format("{0}/{1}", this.Numerator, this.Denominator);
		}

		// Token: 0x170016FB RID: 5883
		// (get) Token: 0x0600B881 RID: 47233 RVA: 0x004435BC File Offset: 0x004417BC
		public double Value
		{
			get
			{
				return (double)this.Numerator / this.Denominator;
			}
		}

		// Token: 0x040089A2 RID: 35234
		public readonly int Numerator;

		// Token: 0x040089A3 RID: 35235
		public readonly uint Denominator;
	}
}
