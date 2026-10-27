using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x0200180D RID: 6157
	[PublicizedFrom(EAccessModifier.Internal)]
	public class Grouping<K, T> : IGrouping<K, T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x0600BF1C RID: 48924 RVA: 0x00466FC3 File Offset: 0x004651C3
		public Grouping(K key, IEnumerable<T> group)
		{
			this.group = group;
			this.key = key;
		}

		// Token: 0x170017D0 RID: 6096
		// (get) Token: 0x0600BF1D RID: 48925 RVA: 0x00466FD9 File Offset: 0x004651D9
		// (set) Token: 0x0600BF1E RID: 48926 RVA: 0x00466FE1 File Offset: 0x004651E1
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

		// Token: 0x0600BF1F RID: 48927 RVA: 0x00466FEA File Offset: 0x004651EA
		public IEnumerator<T> GetEnumerator()
		{
			return this.group.GetEnumerator();
		}

		// Token: 0x0600BF20 RID: 48928 RVA: 0x00466FEA File Offset: 0x004651EA
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.group.GetEnumerator();
		}

		// Token: 0x04008EE0 RID: 36576
		[PublicizedFrom(EAccessModifier.Private)]
		public K key;

		// Token: 0x04008EE1 RID: 36577
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerable<T> group;
	}
}
