using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016E6 RID: 5862
	public struct Rational
	{
		// Token: 0x0600B728 RID: 46888 RVA: 0x0043F06A File Offset: 0x0043D26A
		public Rational(int numerator, uint denominator)
		{
			this.Numerator = numerator;
			this.Denominator = denominator;
		}

		// Token: 0x0600B729 RID: 46889 RVA: 0x0043F07A File Offset: 0x0043D27A
		public override string ToString()
		{
			return string.Format("{0}/{1}", this.Numerator, this.Denominator);
		}

		// Token: 0x17001659 RID: 5721
		// (get) Token: 0x0600B72A RID: 46890 RVA: 0x0043F09C File Offset: 0x0043D29C
		public double Value
		{
			get
			{
				return (double)this.Numerator / this.Denominator;
			}
		}

		// Token: 0x040088DD RID: 35037
		public readonly int Numerator;

		// Token: 0x040088DE RID: 35038
		public readonly uint Denominator;
	}
}
