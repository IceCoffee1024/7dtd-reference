using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x0200178B RID: 6027
	public interface IGrouping<TKey, TElement> : IEnumerable<TElement>, IEnumerable
	{
		// Token: 0x17001675 RID: 5749
		// (get) Token: 0x0600BB33 RID: 47923
		TKey Key { get; }
	}
}
