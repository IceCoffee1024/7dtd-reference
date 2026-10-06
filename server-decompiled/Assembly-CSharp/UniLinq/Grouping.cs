using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017DC RID: 6108
	[PublicizedFrom(EAccessModifier.Internal)]
	public class Grouping<K, T> : IGrouping<K, T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x0600BE08 RID: 48648 RVA: 0x00463617 File Offset: 0x00461817
		public Grouping(K key, IEnumerable<T> group)
		{
			this.group = group;
			this.key = key;
		}

		// Token: 0x17001788 RID: 6024
		// (get) Token: 0x0600BE09 RID: 48649 RVA: 0x0046362D File Offset: 0x0046182D
		// (set) Token: 0x0600BE0A RID: 48650 RVA: 0x00463635 File Offset: 0x00461835
		public K Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
			}
		}

		// Token: 0x0600BE0B RID: 48651 RVA: 0x0046363E File Offset: 0x0046183E
		public IEnumerator<T> GetEnumerator()
		{
			return this.group.GetEnumerator();
		}

		// Token: 0x0600BE0C RID: 48652 RVA: 0x0046363E File Offset: 0x0046183E
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.group.GetEnumerator();
		}

		// Token: 0x04008DF7 RID: 36343
		[PublicizedFrom(EAccessModifier.Private)]
		public K key;

		// Token: 0x04008DF8 RID: 36344
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerable<T> group;
	}
}
