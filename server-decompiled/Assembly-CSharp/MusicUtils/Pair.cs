using System;

namespace MusicUtils
{
	// Token: 0x02001A42 RID: 6722
	public struct Pair<T1, T2> where T1 : IComparable<T1> where T2 : IComparable<T2>
	{
		// Token: 0x0600CC6C RID: 52332 RVA: 0x004A9FEE File Offset: 0x004A81EE
		public Pair(T1 _item1, T2 _item2)
		{
			this.item1 = _item1;
			this.item2 = _item2;
		}

		// Token: 0x04009B18 RID: 39704
		public T1 item1;

		// Token: 0x04009B19 RID: 39705
		public T2 item2;
	}
}
