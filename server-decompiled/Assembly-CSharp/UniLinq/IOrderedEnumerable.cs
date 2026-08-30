using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x0200178D RID: 6029
	public interface IOrderedEnumerable<TElement> : IEnumerable<TElement>, IEnumerable
	{
		// Token: 0x0600BB37 RID: 47927
		IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending);
	}
}
