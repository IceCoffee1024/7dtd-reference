using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017B7 RID: 6071
	[PublicizedFrom(EAccessModifier.Internal)]
	public abstract class OrderedEnumerable<TElement> : IOrderedEnumerable<TElement>, IEnumerable<TElement>, IEnumerable
	{
		// Token: 0x0600BCD2 RID: 48338 RVA: 0x0045F5BA File Offset: 0x0045D7BA
		[PublicizedFrom(EAccessModifier.Protected)]
		public OrderedEnumerable(IEnumerable<TElement> source)
		{
			this.source = source;
		}

		// Token: 0x0600BCD3 RID: 48339 RVA: 0x0045F5C9 File Offset: 0x0045D7C9
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600BCD4 RID: 48340 RVA: 0x0045F5D1 File Offset: 0x0045D7D1
		public virtual IEnumerator<TElement> GetEnumerator()
		{
			return this.Sort(this.source).GetEnumerator();
		}

		// Token: 0x0600BCD5 RID: 48341
		public abstract SortContext<TElement> CreateContext(SortContext<TElement> current);

		// Token: 0x0600BCD6 RID: 48342
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract IEnumerable<TElement> Sort(IEnumerable<TElement> source);

		// Token: 0x0600BCD7 RID: 48343 RVA: 0x0045F5E4 File Offset: 0x0045D7E4
		public IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> selector, IComparer<TKey> comparer, bool descending)
		{
			return new OrderedSequence<TElement, TKey>(this, this.source, selector, comparer, descending ? SortDirection.Descending : SortDirection.Ascending);
		}

		// Token: 0x04008D41 RID: 36161
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerable<TElement> source;
	}
}
