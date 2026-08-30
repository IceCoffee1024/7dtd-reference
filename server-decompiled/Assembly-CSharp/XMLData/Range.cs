using System;

namespace XMLData
{
	// Token: 0x02001636 RID: 5686
	public class Range<TValue>
	{
		// Token: 0x0600B284 RID: 45700 RVA: 0x0000640C File Offset: 0x0000460C
		public Range()
		{
		}

		// Token: 0x0600B285 RID: 45701 RVA: 0x0042854B File Offset: 0x0042674B
		public Range(bool _hasMin, TValue _min, bool _hasMax, TValue _max)
		{
			this.hasMin = _hasMin;
			this.hasMax = _hasMax;
			this.min = _min;
			this.max = _max;
		}

		// Token: 0x0600B286 RID: 45702 RVA: 0x00428570 File Offset: 0x00426770
		public override string ToString()
		{
			return string.Format("{0}-{1}", this.hasMin ? this.min.ToString() : "*", this.hasMax ? this.max.ToString() : "*");
		}

		// Token: 0x0400861D RID: 34333
		public bool hasMin;

		// Token: 0x0400861E RID: 34334
		public bool hasMax;

		// Token: 0x0400861F RID: 34335
		public TValue min;

		// Token: 0x04008620 RID: 34336
		public TValue max;
	}
}
