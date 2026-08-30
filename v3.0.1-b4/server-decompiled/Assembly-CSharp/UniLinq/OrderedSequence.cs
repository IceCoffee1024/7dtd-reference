using System;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x02001792 RID: 6034
	[PublicizedFrom(EAccessModifier.Internal)]
	public class OrderedSequence<TElement, TKey> : OrderedEnumerable<TElement>
	{
		// Token: 0x0600BB55 RID: 47957 RVA: 0x00459883 File Offset: 0x00457A83
		[PublicizedFrom(EAccessModifier.Internal)]
		public OrderedSequence(IEnumerable<TElement> source, Func<TElement, TKey> key_selector, IComparer<TKey> comparer, SortDirection direction)
			: base(source)
		{
			this.selector = key_selector;
			this.comparer = comparer ?? Comparer<TKey>.Default;
			this.direction = direction;
		}

		// Token: 0x0600BB56 RID: 47958 RVA: 0x004598AB File Offset: 0x00457AAB
		[PublicizedFrom(EAccessModifier.Internal)]
		public OrderedSequence(OrderedEnumerable<TElement> parent, IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, SortDirection direction)
			: this(source, keySelector, comparer, direction)
		{
			this.parent = parent;
		}

		// Token: 0x0600BB57 RID: 47959 RVA: 0x004598C0 File Offset: 0x00457AC0
		public override IEnumerator<TElement> GetEnumerator()
		{
			return base.GetEnumerator();
		}

		// Token: 0x0600BB58 RID: 47960 RVA: 0x004598C8 File Offset: 0x00457AC8
		public override SortContext<TElement> CreateContext(SortContext<TElement> current)
		{
			SortContext<TElement> sortContext = new SortSequenceContext<TElement, TKey>(this.selector, this.comparer, this.direction, current);
			if (this.parent != null)
			{
				return this.parent.CreateContext(sortContext);
			}
			return sortContext;
		}

		// Token: 0x0600BB59 RID: 47961 RVA: 0x00459904 File Offset: 0x00457B04
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IEnumerable<TElement> Sort(IEnumerable<TElement> source)
		{
			return QuickSort<TElement>.Sort(source, this.CreateContext(null));
		}

		// Token: 0x04008C26 RID: 35878
		[PublicizedFrom(EAccessModifier.Private)]
		public OrderedEnumerable<TElement> parent;

		// Token: 0x04008C27 RID: 35879
		[PublicizedFrom(EAccessModifier.Private)]
		public Func<TElement, TKey> selector;

		// Token: 0x04008C28 RID: 35880
		[PublicizedFrom(EAccessModifier.Private)]
		public IComparer<TKey> comparer;

		// Token: 0x04008C29 RID: 35881
		[PublicizedFrom(EAccessModifier.Private)]
		public SortDirection direction;
	}
}
