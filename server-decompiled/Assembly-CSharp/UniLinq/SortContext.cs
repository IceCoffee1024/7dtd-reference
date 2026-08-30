using System;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x02001795 RID: 6037
	[PublicizedFrom(EAccessModifier.Internal)]
	public abstract class SortContext<TElement> : IComparer<int>
	{
		// Token: 0x0600BB66 RID: 47974 RVA: 0x00459B33 File Offset: 0x00457D33
		[PublicizedFrom(EAccessModifier.Protected)]
		public SortContext(SortDirection direction, SortContext<TElement> child_context)
		{
			this.direction = direction;
			this.child_context = child_context;
		}

		// Token: 0x0600BB67 RID: 47975
		public abstract void Initialize(TElement[] elements);

		// Token: 0x0600BB68 RID: 47976
		public abstract int Compare(int first_index, int second_index);

		// Token: 0x04008C36 RID: 35894
		[PublicizedFrom(EAccessModifier.Protected)]
		public SortDirection direction;

		// Token: 0x04008C37 RID: 35895
		[PublicizedFrom(EAccessModifier.Protected)]
		public SortContext<TElement> child_context;
	}
}
