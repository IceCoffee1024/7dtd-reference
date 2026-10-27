using System;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x02001818 RID: 6168
	[PublicizedFrom(EAccessModifier.Internal)]
	public abstract class SortContext<TElement> : IComparer<int>
	{
		// Token: 0x0600BF54 RID: 48980 RVA: 0x00467777 File Offset: 0x00465977
		[PublicizedFrom(EAccessModifier.Protected)]
		public SortContext(SortDirection direction, SortContext<TElement> child_context)
		{
			this.direction = direction;
			this.child_context = child_context;
		}

		// Token: 0x0600BF55 RID: 48981
		public abstract void Initialize(TElement[] elements);

		// Token: 0x0600BF56 RID: 48982
		public abstract int Compare(int first_index, int second_index);

		// Token: 0x04008F00 RID: 36608
		[PublicizedFrom(EAccessModifier.Protected)]
		public SortDirection direction;

		// Token: 0x04008F01 RID: 36609
		[PublicizedFrom(EAccessModifier.Protected)]
		public SortContext<TElement> child_context;
	}
}
