using System;

namespace UnityEngine
{
	// Token: 0x02000253 RID: 595
	public struct RangeInt
	{
		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001963 RID: 6499 RVA: 0x0002A6A4 File Offset: 0x000288A4
		public int end
		{
			get
			{
				return this.start + this.length;
			}
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x0002A6C3 File Offset: 0x000288C3
		public RangeInt(int start, int length)
		{
			this.start = start;
			this.length = length;
		}

		// Token: 0x040008D7 RID: 2263
		public int start;

		// Token: 0x040008D8 RID: 2264
		public int length;
	}
}
