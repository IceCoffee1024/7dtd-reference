using System;

namespace MusicUtils
{
	// Token: 0x020019EF RID: 6639
	public struct Pair<T1, T2> where T1 : IComparable<T1> where T2 : IComparable<T2>
	{
		// Token: 0x0600C98B RID: 51595 RVA: 0x0049F72E File Offset: 0x0049D92E
		public Pair(T1 _item1, T2 _item2)
		{
			this.item1 = _item1;
			this.item2 = _item2;
		}

		// Token: 0x04009921 RID: 39201
		public T1 item1;

		// Token: 0x04009922 RID: 39202
		public T2 item2;
	}
}
