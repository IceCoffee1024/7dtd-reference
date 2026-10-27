using System;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x0200181A RID: 6170
	[PublicizedFrom(EAccessModifier.Internal)]
	public class SortSequenceContext<TElement, TKey> : SortContext<TElement>
	{
		// Token: 0x0600BF57 RID: 48983 RVA: 0x0046778D File Offset: 0x0046598D
		public SortSequenceContext(Func<TElement, TKey> selector, IComparer<TKey> comparer, SortDirection direction, SortContext<TElement> child_context)
			: base(direction, child_context)
		{
			this.selector = selector;
			this.comparer = comparer;
		}

		// Token: 0x0600BF58 RID: 48984 RVA: 0x004677A8 File Offset: 0x004659A8
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

		// Token: 0x0600BF59 RID: 48985 RVA: 0x00467808 File Offset: 0x00465A08
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

		// Token: 0x04008F05 RID: 36613
		[PublicizedFrom(EAccessModifier.Private)]
		public Func<TElement, TKey> selector;

		// Token: 0x04008F06 RID: 36614
		[PublicizedFrom(EAccessModifier.Private)]
		public IComparer<TKey> comparer;

		// Token: 0x04008F07 RID: 36615
		[PublicizedFrom(EAccessModifier.Private)]
		public TKey[] keys;
	}
}
