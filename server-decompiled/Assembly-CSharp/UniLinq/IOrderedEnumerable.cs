using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017B3 RID: 6067
	public interface IOrderedEnumerable<TElement> : IEnumerable<TElement>, IEnumerable
	{
		// Token: 0x0600BCBA RID: 48314
		IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending);
	}
}
