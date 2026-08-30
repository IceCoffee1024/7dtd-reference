using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x02001791 RID: 6033
	[PublicizedFrom(EAccessModifier.Internal)]
	public abstract class OrderedEnumerable<TElement> : IOrderedEnumerable<TElement>, IEnumerable<TElement>, IEnumerable
	{
		// Token: 0x0600BB4F RID: 47951 RVA: 0x00459842 File Offset: 0x00457A42
		[PublicizedFrom(EAccessModifier.Protected)]
		public OrderedEnumerable(IEnumerable<TElement> source)
		{
			this.source = source;
		}

		// Token: 0x0600BB50 RID: 47952 RVA: 0x00459851 File Offset: 0x00457A51
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600BB51 RID: 47953 RVA: 0x00459859 File Offset: 0x00457A59
		public virtual IEnumerator<TElement> GetEnumerator()
		{
			return this.Sort(this.source).GetEnumerator();
		}

		// Token: 0x0600BB52 RID: 47954
		public abstract SortContext<TElement> CreateContext(SortContext<TElement> current);

		// Token: 0x0600BB53 RID: 47955
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract IEnumerable<TElement> Sort(IEnumerable<TElement> source);

		// Token: 0x0600BB54 RID: 47956 RVA: 0x0045986C File Offset: 0x00457A6C
		public IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> selector, IComparer<TKey> comparer, bool descending)
		{
			return new OrderedSequence<TElement, TKey>(this, this.source, selector, comparer, descending ? SortDirection.Descending : SortDirection.Ascending);
		}

		// Token: 0x04008C25 RID: 35877
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerable<TElement> source;
	}
}
