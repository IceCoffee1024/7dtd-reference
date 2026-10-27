using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x02001731 RID: 5937
	public struct Rational
	{
		// Token: 0x0600B953 RID: 47443 RVA: 0x0044602E File Offset: 0x0044422E
		public Rational(int numerator, uint denominator)
		{
			this.Numerator = numerator;
			this.Denominator = denominator;
		}

		// Token: 0x0600B954 RID: 47444 RVA: 0x0044603E File Offset: 0x0044423E
		public override string ToString()
		{
			return string.Format("{0}/{1}", this.Numerator, this.Denominator);
		}

		// Token: 0x17001742 RID: 5954
		// (get) Token: 0x0600B955 RID: 47445 RVA: 0x00446060 File Offset: 0x00444260
		public double Value
		{
			get
			{
				return (double)this.Numerator / this.Denominator;
			}
		}

		// Token: 0x04008A55 RID: 35413
		public readonly int Numerator;

		// Token: 0x04008A56 RID: 35414
		public readonly uint Denominator;
	}
}
