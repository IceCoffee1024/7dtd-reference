using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017B1 RID: 6065
	public interface IGrouping<TKey, TElement> : IEnumerable<TElement>, IEnumerable
	{
		// Token: 0x170016E7 RID: 5863
		// (get) Token: 0x0600BCB6 RID: 48310
		TKey Key { get; }
	}
}
