using System;

namespace XMLData
{
	// Token: 0x02001655 RID: 5717
	public class Range<TValue>
	{
		// Token: 0x0600B316 RID: 45846 RVA: 0x0000640C File Offset: 0x0000460C
		public Range()
		{
		}

		// Token: 0x0600B317 RID: 45847 RVA: 0x0042C227 File Offset: 0x0042A427
		public Range(bool _hasMin, TValue _min, bool _hasMax, TValue _max)
		{
			this.hasMin = _hasMin;
			this.hasMax = _hasMax;
			this.min = _min;
			this.max = _max;
		}

		// Token: 0x0600B318 RID: 45848 RVA: 0x0042C24C File Offset: 0x0042A44C
		public override string ToString()
		{
			return string.Format("{0}-{1}", this.hasMin ? this.min.ToString() : "*", this.hasMax ? this.max.ToString() : "*");
		}

		// Token: 0x04008673 RID: 34419
		public bool hasMin;

		// Token: 0x04008674 RID: 34420
		public bool hasMax;

		// Token: 0x04008675 RID: 34421
		public TValue min;

		// Token: 0x04008676 RID: 34422
		public TValue max;
	}
}
