using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x02001810 RID: 6160
	public interface IOrderedEnumerable<TElement> : IEnumerable<TElement>, IEnumerable
	{
		// Token: 0x0600BF25 RID: 48933
		IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending);
	}
}
