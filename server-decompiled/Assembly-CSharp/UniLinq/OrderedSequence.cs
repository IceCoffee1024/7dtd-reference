using System;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x02001815 RID: 6165
	[PublicizedFrom(EAccessModifier.Internal)]
	public class OrderedSequence<TElement, TKey> : OrderedEnumerable<TElement>
	{
		// Token: 0x0600BF43 RID: 48963 RVA: 0x004674C7 File Offset: 0x004656C7
		[PublicizedFrom(EAccessModifier.Internal)]
		public OrderedSequence(IEnumerable<TElement> source, Func<TElement, TKey> key_selector, IComparer<TKey> comparer, SortDirection direction)
			: base(source)
		{
			this.selector = key_selector;
			this.comparer = comparer ?? Comparer<TKey>.Default;
			this.direction = direction;
		}

		// Token: 0x0600BF44 RID: 48964 RVA: 0x004674EF File Offset: 0x004656EF
		[PublicizedFrom(EAccessModifier.Internal)]
		public OrderedSequence(OrderedEnumerable<TElement> parent, IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, SortDirection direction)
			: this(source, keySelector, comparer, direction)
		{
			this.parent = parent;
		}

		// Token: 0x0600BF45 RID: 48965 RVA: 0x00467504 File Offset: 0x00465704
		public override IEnumerator<TElement> GetEnumerator()
		{
			return base.GetEnumerator();
		}

		// Token: 0x0600BF46 RID: 48966 RVA: 0x0046750C File Offset: 0x0046570C
		public override SortContext<TElement> CreateContext(SortContext<TElement> current)
		{
			SortContext<TElement> sortContext = new SortSequenceContext<TElement, TKey>(this.selector, this.comparer, this.direction, current);
			if (this.parent != null)
			{
				return this.parent.CreateContext(sortContext);
			}
			return sortContext;
		}

		// Token: 0x0600BF47 RID: 48967 RVA: 0x00467548 File Offset: 0x00465748
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IEnumerable<TElement> Sort(IEnumerable<TElement> source)
		{
			return QuickSort<TElement>.Sort(source, this.CreateContext(null));
		}

		// Token: 0x04008EF0 RID: 36592
		[PublicizedFrom(EAccessModifier.Private)]
		public OrderedEnumerable<TElement> parent;

		// Token: 0x04008EF1 RID: 36593
		[PublicizedFrom(EAccessModifier.Private)]
		public Func<TElement, TKey> selector;

		// Token: 0x04008EF2 RID: 36594
		[PublicizedFrom(EAccessModifier.Private)]
		public IComparer<TKey> comparer;

		// Token: 0x04008EF3 RID: 36595
		[PublicizedFrom(EAccessModifier.Private)]
		public SortDirection direction;
	}
}
