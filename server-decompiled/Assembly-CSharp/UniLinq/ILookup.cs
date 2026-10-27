using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x0200180F RID: 6159
	public interface ILookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>, IEnumerable
	{
		// Token: 0x170017D2 RID: 6098
		// (get) Token: 0x0600BF22 RID: 48930
		int Count { get; }

		// Token: 0x170017D3 RID: 6099
		IEnumerable<TElement> this[TKey key] { get; }

		// Token: 0x0600BF24 RID: 48932
		bool Contains(TKey key);
	}
}
