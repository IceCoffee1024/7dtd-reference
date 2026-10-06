using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017DD RID: 6109
	public interface IGrouping<TKey, TElement> : IEnumerable<TElement>, IEnumerable
	{
		// Token: 0x17001789 RID: 6025
		// (get) Token: 0x0600BE0D RID: 48653
		TKey Key { get; }
	}
}
