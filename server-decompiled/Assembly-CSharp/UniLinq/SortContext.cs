using System;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017E7 RID: 6119
	[PublicizedFrom(EAccessModifier.Internal)]
	public abstract class SortContext<TElement> : IComparer<int>
	{
		// Token: 0x0600BE40 RID: 48704 RVA: 0x00463DCB File Offset: 0x00461FCB
		[PublicizedFrom(EAccessModifier.Protected)]
		public SortContext(SortDirection direction, SortContext<TElement> child_context)
		{
			this.direction = direction;
			this.child_context = child_context;
		}

		// Token: 0x0600BE41 RID: 48705
		public abstract void Initialize(TElement[] elements);

		// Token: 0x0600BE42 RID: 48706
		public abstract int Compare(int first_index, int second_index);

		// Token: 0x04008E17 RID: 36375
		[PublicizedFrom(EAccessModifier.Protected)]
		public SortDirection direction;

		// Token: 0x04008E18 RID: 36376
		[PublicizedFrom(EAccessModifier.Protected)]
		public SortContext<TElement> child_context;
	}
}
