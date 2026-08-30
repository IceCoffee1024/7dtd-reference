using System;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017BB RID: 6075
	[PublicizedFrom(EAccessModifier.Internal)]
	public abstract class SortContext<TElement> : IComparer<int>
	{
		// Token: 0x0600BCE9 RID: 48361 RVA: 0x0045F8AB File Offset: 0x0045DAAB
		[PublicizedFrom(EAccessModifier.Protected)]
		public SortContext(SortDirection direction, SortContext<TElement> child_context)
		{
			this.direction = direction;
			this.child_context = child_context;
		}

		// Token: 0x0600BCEA RID: 48362
		public abstract void Initialize(TElement[] elements);

		// Token: 0x0600BCEB RID: 48363
		public abstract int Compare(int first_index, int second_index);

		// Token: 0x04008D52 RID: 36178
		[PublicizedFrom(EAccessModifier.Protected)]
		public SortDirection direction;

		// Token: 0x04008D53 RID: 36179
		[PublicizedFrom(EAccessModifier.Protected)]
		public SortContext<TElement> child_context;
	}
}
