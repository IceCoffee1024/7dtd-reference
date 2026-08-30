using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017B0 RID: 6064
	[PublicizedFrom(EAccessModifier.Internal)]
	public class Grouping<K, T> : IGrouping<K, T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x0600BCB1 RID: 48305 RVA: 0x0045F0F7 File Offset: 0x0045D2F7
		public Grouping(K key, IEnumerable<T> group)
		{
			this.group = group;
			this.key = key;
		}

		// Token: 0x170016E6 RID: 5862
		// (get) Token: 0x0600BCB2 RID: 48306 RVA: 0x0045F10D File Offset: 0x0045D30D
		// (set) Token: 0x0600BCB3 RID: 48307 RVA: 0x0045F115 File Offset: 0x0045D315
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

		// Token: 0x0600BCB4 RID: 48308 RVA: 0x0045F11E File Offset: 0x0045D31E
		public IEnumerator<T> GetEnumerator()
		{
			return this.group.GetEnumerator();
		}

		// Token: 0x0600BCB5 RID: 48309 RVA: 0x0045F11E File Offset: 0x0045D31E
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.group.GetEnumerator();
		}

		// Token: 0x04008D32 RID: 36146
		[PublicizedFrom(EAccessModifier.Private)]
		public K key;

		// Token: 0x04008D33 RID: 36147
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerable<T> group;
	}
}
