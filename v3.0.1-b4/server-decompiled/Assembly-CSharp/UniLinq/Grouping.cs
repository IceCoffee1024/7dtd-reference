using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x0200178A RID: 6026
	[PublicizedFrom(EAccessModifier.Internal)]
	public class Grouping<K, T> : IGrouping<K, T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x0600BB2E RID: 47918 RVA: 0x0045937F File Offset: 0x0045757F
		public Grouping(K key, IEnumerable<T> group)
		{
			this.group = group;
			this.key = key;
		}

		// Token: 0x17001674 RID: 5748
		// (get) Token: 0x0600BB2F RID: 47919 RVA: 0x00459395 File Offset: 0x00457595
		// (set) Token: 0x0600BB30 RID: 47920 RVA: 0x0045939D File Offset: 0x0045759D
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

		// Token: 0x0600BB31 RID: 47921 RVA: 0x004593A6 File Offset: 0x004575A6
		public IEnumerator<T> GetEnumerator()
		{
			return this.group.GetEnumerator();
		}

		// Token: 0x0600BB32 RID: 47922 RVA: 0x004593A6 File Offset: 0x004575A6
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.group.GetEnumerator();
		}

		// Token: 0x04008C16 RID: 35862
		[PublicizedFrom(EAccessModifier.Private)]
		public K key;

		// Token: 0x04008C17 RID: 35863
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerable<T> group;
	}
}
