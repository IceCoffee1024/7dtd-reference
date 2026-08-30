using System;

namespace XMLData
{
	// Token: 0x0200161B RID: 5659
	public class Range<TValue>
	{
		// Token: 0x0600B187 RID: 45447 RVA: 0x0000640C File Offset: 0x0000460C
		public Range()
		{
		}

		// Token: 0x0600B188 RID: 45448 RVA: 0x0042320B File Offset: 0x0042140B
		public Range(bool _hasMin, TValue _min, bool _hasMax, TValue _max)
		{
			this.hasMin = _hasMin;
			this.hasMax = _hasMax;
			this.min = _min;
			this.max = _max;
		}

		// Token: 0x0600B189 RID: 45449 RVA: 0x00423230 File Offset: 0x00421430
		public override string ToString()
		{
			return string.Format("{0}-{1}", this.hasMin ? this.min.ToString() : "*", this.hasMax ? this.max.ToString() : "*");
		}

		// Token: 0x04008544 RID: 34116
		public bool hasMin;

		// Token: 0x04008545 RID: 34117
		public bool hasMax;

		// Token: 0x04008546 RID: 34118
		public TValue min;

		// Token: 0x04008547 RID: 34119
		public TValue max;
	}
}
