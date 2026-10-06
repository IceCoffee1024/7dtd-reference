using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017DF RID: 6111
	public interface IOrderedEnumerable<TElement> : IEnumerable<TElement>, IEnumerable
	{
		// Token: 0x0600BE11 RID: 48657
		IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending);
	}
}
