using System;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017BD RID: 6077
	[PublicizedFrom(EAccessModifier.Internal)]
	public class SortSequenceContext<TElement, TKey> : SortContext<TElement>
	{
		// Token: 0x0600BCEC RID: 48364 RVA: 0x0045F8C1 File Offset: 0x0045DAC1
		public SortSequenceContext(Func<TElement, TKey> selector, IComparer<TKey> comparer, SortDirection direction, SortContext<TElement> child_context)
			: base(direction, child_context)
		{
			this.selector = selector;
			this.comparer = comparer;
		}

		// Token: 0x0600BCED RID: 48365 RVA: 0x0045F8DC File Offset: 0x0045DADC
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

		// Token: 0x0600BCEE RID: 48366 RVA: 0x0045F93C File Offset: 0x0045DB3C
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

		// Token: 0x04008D57 RID: 36183
		[PublicizedFrom(EAccessModifier.Private)]
		public Func<TElement, TKey> selector;

		// Token: 0x04008D58 RID: 36184
		[PublicizedFrom(EAccessModifier.Private)]
		public IComparer<TKey> comparer;

		// Token: 0x04008D59 RID: 36185
		[PublicizedFrom(EAccessModifier.Private)]
		public TKey[] keys;
	}
}
