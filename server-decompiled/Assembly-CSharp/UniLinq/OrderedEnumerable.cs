using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x02001814 RID: 6164
	[PublicizedFrom(EAccessModifier.Internal)]
	public abstract class OrderedEnumerable<TElement> : IOrderedEnumerable<TElement>, IEnumerable<TElement>, IEnumerable
	{
		// Token: 0x0600BF3D RID: 48957 RVA: 0x00467486 File Offset: 0x00465686
		[PublicizedFrom(EAccessModifier.Protected)]
		public OrderedEnumerable(IEnumerable<TElement> source)
		{
			this.source = source;
		}

		// Token: 0x0600BF3E RID: 48958 RVA: 0x00467495 File Offset: 0x00465695
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600BF3F RID: 48959 RVA: 0x0046749D File Offset: 0x0046569D
		public virtual IEnumerator<TElement> GetEnumerator()
		{
			return this.Sort(this.source).GetEnumerator();
		}

		// Token: 0x0600BF40 RID: 48960
		public abstract SortContext<TElement> CreateContext(SortContext<TElement> current);

		// Token: 0x0600BF41 RID: 48961
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract IEnumerable<TElement> Sort(IEnumerable<TElement> source);

		// Token: 0x0600BF42 RID: 48962 RVA: 0x004674B0 File Offset: 0x004656B0
		public IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> selector, IComparer<TKey> comparer, bool descending)
		{
			return new OrderedSequence<TElement, TKey>(this, this.source, selector, comparer, descending ? SortDirection.Descending : SortDirection.Ascending);
		}

		// Token: 0x04008EEF RID: 36591
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerable<TElement> source;
	}
}
