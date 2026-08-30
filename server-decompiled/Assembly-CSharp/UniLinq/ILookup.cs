using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017B2 RID: 6066
	public interface ILookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>, IEnumerable
	{
		// Token: 0x170016E8 RID: 5864
		// (get) Token: 0x0600BCB7 RID: 48311
		int Count { get; }

		// Token: 0x170016E9 RID: 5865
		IEnumerable<TElement> this[TKey key] { get; }

		// Token: 0x0600BCB9 RID: 48313
		bool Contains(TKey key);
	}
}
