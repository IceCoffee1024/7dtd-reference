using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x0200180E RID: 6158
	public interface IGrouping<TKey, TElement> : IEnumerable<TElement>, IEnumerable
	{
		// Token: 0x170017D1 RID: 6097
		// (get) Token: 0x0600BF21 RID: 48929
		TKey Key { get; }
	}
}
