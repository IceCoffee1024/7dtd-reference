using System;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017E9 RID: 6121
	[PublicizedFrom(EAccessModifier.Internal)]
	public class SortSequenceContext<TElement, TKey> : SortContext<TElement>
	{
		// Token: 0x0600BE43 RID: 48707 RVA: 0x00463DE1 File Offset: 0x00461FE1
		public SortSequenceContext(Func<TElement, TKey> selector, IComparer<TKey> comparer, SortDirection direction, SortContext<TElement> child_context)
			: base(direction, child_context)
		{
			this.selector = selector;
			this.comparer = comparer;
		}

		// Token: 0x0600BE44 RID: 48708 RVA: 0x00463DFC File Offset: 0x00461FFC
		public override void Initialize(TElement[] elements)
		{
			if (this.child_context != null)
			{
				this.child_context.Initialize(elements);
			}
			this.keys = new TKey[elements.Length];
			for (int i = 0; i < this.keys.Length; i++)
			{
				this.keys[i] = this.selector(elements[i]);
			}
		}

		// Token: 0x0600BE45 RID: 48709 RVA: 0x00463E5C File Offset: 0x0046205C
		public override int Compare(int first_index, int second_index)
		{
			int num = this.comparer.Compare(this.keys[first_index], this.keys[second_index]);
			if (num == 0)
			{
				if (this.child_context != null)
				{
					return this.child_context.Compare(first_index, second_index);
				}
				num = ((this.direction == SortDirection.Descending) ? (second_index - first_index) : (first_index - second_index));
			}
			if (this.direction != SortDirection.Descending)
			{
				return num;
			}
			return -num;
		}

		// Token: 0x04008E1C RID: 36380
		[PublicizedFrom(EAccessModifier.Private)]
		public Func<TElement, TKey> selector;

		// Token: 0x04008E1D RID: 36381
		[PublicizedFrom(EAccessModifier.Private)]
		public IComparer<TKey> comparer;

		// Token: 0x04008E1E RID: 36382
		[PublicizedFrom(EAccessModifier.Private)]
		public TKey[] keys;
	}
}
