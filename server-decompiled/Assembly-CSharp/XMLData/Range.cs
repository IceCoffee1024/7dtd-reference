using System;

namespace XMLData
{
	// Token: 0x02001667 RID: 5735
	public class Range<TValue>
	{
		// Token: 0x0600B36E RID: 45934 RVA: 0x0000640C File Offset: 0x0000460C
		public Range()
		{
		}

		// Token: 0x0600B36F RID: 45935 RVA: 0x0042E62B File Offset: 0x0042C82B
		public Range(bool _hasMin, TValue _min, bool _hasMax, TValue _max)
		{
			this.hasMin = _hasMin;
			this.hasMax = _hasMax;
			this.min = _min;
			this.max = _max;
		}

		// Token: 0x0600B370 RID: 45936 RVA: 0x0042E650 File Offset: 0x0042C850
		public override string ToString()
		{
			return string.Format("{0}-{1}", this.hasMin ? this.min.ToString() : "*", this.hasMax ? this.max.ToString() : "*");
		}

		// Token: 0x040086B4 RID: 34484
		public bool hasMin;

		// Token: 0x040086B5 RID: 34485
		public bool hasMax;

		// Token: 0x040086B6 RID: 34486
		public TValue min;

		// Token: 0x040086B7 RID: 34487
		public TValue max;
	}
}
