using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017E3 RID: 6115
	[PublicizedFrom(EAccessModifier.Internal)]
	public abstract class OrderedEnumerable<TElement> : IOrderedEnumerable<TElement>, IEnumerable<TElement>, IEnumerable
	{
		// Token: 0x0600BE29 RID: 48681 RVA: 0x00463ADA File Offset: 0x00461CDA
		[PublicizedFrom(EAccessModifier.Protected)]
		public OrderedEnumerable(IEnumerable<TElement> source)
		{
			this.source = source;
		}

		// Token: 0x0600BE2A RID: 48682 RVA: 0x00463AE9 File Offset: 0x00461CE9
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600BE2B RID: 48683 RVA: 0x00463AF1 File Offset: 0x00461CF1
		public virtual IEnumerator<TElement> GetEnumerator()
		{
			return this.Sort(this.source).GetEnumerator();
		}

		// Token: 0x0600BE2C RID: 48684
		public abstract SortContext<TElement> CreateContext(SortContext<TElement> current);

		// Token: 0x0600BE2D RID: 48685
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract IEnumerable<TElement> Sort(IEnumerable<TElement> source);

		// Token: 0x0600BE2E RID: 48686 RVA: 0x00463B04 File Offset: 0x00461D04
		public IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> selector, IComparer<TKey> comparer, bool descending)
		{
			return new OrderedSequence<TElement, TKey>(this, this.source, selector, comparer, descending ? SortDirection.Descending : SortDirection.Ascending);
		}

		// Token: 0x04008E06 RID: 36358
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerable<TElement> source;
	}
}
