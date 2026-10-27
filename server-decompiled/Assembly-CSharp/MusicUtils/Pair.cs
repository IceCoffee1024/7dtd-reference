using System;

namespace MusicUtils
{
	// Token: 0x02001A73 RID: 6771
	public struct Pair<T1, T2> where T1 : IComparable<T1> where T2 : IComparable<T2>
	{
		// Token: 0x0600CD80 RID: 52608 RVA: 0x004ADAEA File Offset: 0x004ABCEA
		public Pair(T1 _item1, T2 _item2)
		{
			this.item1 = _item1;
			this.item2 = _item2;
		}

		// Token: 0x04009C01 RID: 39937
		public T1 item1;

		// Token: 0x04009C02 RID: 39938
		public T2 item2;
	}
}
