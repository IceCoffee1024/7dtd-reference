using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017DE RID: 6110
	public interface ILookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>, IEnumerable
	{
		// Token: 0x1700178A RID: 6026
		// (get) Token: 0x0600BE0E RID: 48654
		int Count { get; }

		// Token: 0x1700178B RID: 6027
		IEnumerable<TElement> this[TKey key] { get; }

		// Token: 0x0600BE10 RID: 48656
		bool Contains(TKey key);
	}
}
