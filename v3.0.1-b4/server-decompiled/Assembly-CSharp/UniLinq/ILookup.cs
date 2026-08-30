using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x0200178C RID: 6028
	public interface ILookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>, IEnumerable
	{
		// Token: 0x17001676 RID: 5750
		// (get) Token: 0x0600BB34 RID: 47924
		int Count { get; }

		// Token: 0x17001677 RID: 5751
		IEnumerable<TElement> this[TKey key] { get; }

		// Token: 0x0600BB36 RID: 47926
		bool Contains(TKey key);
	}
}
