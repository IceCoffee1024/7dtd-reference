using System;

namespace MusicUtils
{
	// Token: 0x02001A16 RID: 6678
	public struct Pair<T1, T2> where T1 : IComparable<T1> where T2 : IComparable<T2>
	{
		// Token: 0x0600CB14 RID: 51988 RVA: 0x004A5B22 File Offset: 0x004A3D22
		public Pair(T1 _item1, T2 _item2)
		{
			this.item1 = _item1;
			this.item2 = _item2;
		}

		// Token: 0x04009A52 RID: 39506
		public T1 item1;

		// Token: 0x04009A53 RID: 39507
		public T2 item2;
	}
}
