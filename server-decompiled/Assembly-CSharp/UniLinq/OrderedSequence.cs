using System;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017E4 RID: 6116
	[PublicizedFrom(EAccessModifier.Internal)]
	public class OrderedSequence<TElement, TKey> : OrderedEnumerable<TElement>
	{
		// Token: 0x0600BE2F RID: 48687 RVA: 0x00463B1B File Offset: 0x00461D1B
		[PublicizedFrom(EAccessModifier.Internal)]
		public OrderedSequence(IEnumerable<TElement> source, Func<TElement, TKey> key_selector, IComparer<TKey> comparer, SortDirection direction)
			: base(source)
		{
			this.selector = key_selector;
			this.comparer = comparer ?? Comparer<TKey>.Default;
			this.direction = direction;
		}

		// Token: 0x0600BE30 RID: 48688 RVA: 0x00463B43 File Offset: 0x00461D43
		[PublicizedFrom(EAccessModifier.Internal)]
		public OrderedSequence(OrderedEnumerable<TElement> parent, IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, SortDirection direction)
			: this(source, keySelector, comparer, direction)
		{
			this.parent = parent;
		}

		// Token: 0x0600BE31 RID: 48689 RVA: 0x00463B58 File Offset: 0x00461D58
		public override IEnumerator<TElement> GetEnumerator()
		{
			return base.GetEnumerator();
		}

		// Token: 0x0600BE32 RID: 48690 RVA: 0x00463B60 File Offset: 0x00461D60
		public override SortContext<TElement> CreateContext(SortContext<TElement> current)
		{
			SortContext<TElement> sortContext = new SortSequenceContext<TElement, TKey>(this.selector, this.comparer, this.direction, current);
			if (this.parent != null)
			{
				return this.parent.CreateContext(sortContext);
			}
			return sortContext;
		}

		// Token: 0x0600BE33 RID: 48691 RVA: 0x00463B9C File Offset: 0x00461D9C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IEnumerable<TElement> Sort(IEnumerable<TElement> source)
		{
			return QuickSort<TElement>.Sort(source, this.CreateContext(null));
		}

		// Token: 0x04008E07 RID: 36359
		[PublicizedFrom(EAccessModifier.Private)]
		public OrderedEnumerable<TElement> parent;

		// Token: 0x04008E08 RID: 36360
		[PublicizedFrom(EAccessModifier.Private)]
		public Func<TElement, TKey> selector;

		// Token: 0x04008E09 RID: 36361
		[PublicizedFrom(EAccessModifier.Private)]
		public IComparer<TKey> comparer;

		// Token: 0x04008E0A RID: 36362
		[PublicizedFrom(EAccessModifier.Private)]
		public SortDirection direction;
	}
}
