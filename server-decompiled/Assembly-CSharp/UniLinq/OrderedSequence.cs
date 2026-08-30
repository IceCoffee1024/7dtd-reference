using System;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017B8 RID: 6072
	[PublicizedFrom(EAccessModifier.Internal)]
	public class OrderedSequence<TElement, TKey> : OrderedEnumerable<TElement>
	{
		// Token: 0x0600BCD8 RID: 48344 RVA: 0x0045F5FB File Offset: 0x0045D7FB
		[PublicizedFrom(EAccessModifier.Internal)]
		public OrderedSequence(IEnumerable<TElement> source, Func<TElement, TKey> key_selector, IComparer<TKey> comparer, SortDirection direction)
			: base(source)
		{
			this.selector = key_selector;
			this.comparer = comparer ?? Comparer<TKey>.Default;
			this.direction = direction;
		}

		// Token: 0x0600BCD9 RID: 48345 RVA: 0x0045F623 File Offset: 0x0045D823
		[PublicizedFrom(EAccessModifier.Internal)]
		public OrderedSequence(OrderedEnumerable<TElement> parent, IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, SortDirection direction)
			: this(source, keySelector, comparer, direction)
		{
			this.parent = parent;
		}

		// Token: 0x0600BCDA RID: 48346 RVA: 0x0045F638 File Offset: 0x0045D838
		public override IEnumerator<TElement> GetEnumerator()
		{
			return base.GetEnumerator();
		}

		// Token: 0x0600BCDB RID: 48347 RVA: 0x0045F640 File Offset: 0x0045D840
		public override SortContext<TElement> CreateContext(SortContext<TElement> current)
		{
			SortContext<TElement> sortContext = new SortSequenceContext<TElement, TKey>(this.selector, this.comparer, this.direction, current);
			if (this.parent != null)
			{
				return this.parent.CreateContext(sortContext);
			}
			return sortContext;
		}

		// Token: 0x0600BCDC RID: 48348 RVA: 0x0045F67C File Offset: 0x0045D87C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IEnumerable<TElement> Sort(IEnumerable<TElement> source)
		{
			return QuickSort<TElement>.Sort(source, this.CreateContext(null));
		}

		// Token: 0x04008D42 RID: 36162
		[PublicizedFrom(EAccessModifier.Private)]
		public OrderedEnumerable<TElement> parent;

		// Token: 0x04008D43 RID: 36163
		[PublicizedFrom(EAccessModifier.Private)]
		public Func<TElement, TKey> selector;

		// Token: 0x04008D44 RID: 36164
		[PublicizedFrom(EAccessModifier.Private)]
		public IComparer<TKey> comparer;

		// Token: 0x04008D45 RID: 36165
		[PublicizedFrom(EAccessModifier.Private)]
		public SortDirection direction;
	}
}
