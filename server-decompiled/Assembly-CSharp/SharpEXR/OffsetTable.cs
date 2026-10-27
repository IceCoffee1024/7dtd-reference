using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpEXR
{
	// Token: 0x02001721 RID: 5921
	public class OffsetTable : IEnumerable<uint>, IEnumerable
	{
		// Token: 0x1700173C RID: 5948
		// (get) Token: 0x0600B925 RID: 47397 RVA: 0x00445346 File Offset: 0x00443546
		// (set) Token: 0x0600B926 RID: 47398 RVA: 0x0044534E File Offset: 0x0044354E
		public List<uint> Offsets { get; set; }

		// Token: 0x0600B927 RID: 47399 RVA: 0x00445357 File Offset: 0x00443557
		public OffsetTable()
		{
			this.Offsets = new List<uint>();
		}

		// Token: 0x0600B928 RID: 47400 RVA: 0x0044536A File Offset: 0x0044356A
		public OffsetTable(int capacity)
		{
			this.Offsets = new List<uint>(capacity);
		}

		// Token: 0x0600B929 RID: 47401 RVA: 0x00445380 File Offset: 0x00443580
		public void Read(IEXRReader reader, int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.Offsets.Add(reader.ReadUInt32());
				reader.ReadUInt32();
			}
		}

		// Token: 0x0600B92A RID: 47402 RVA: 0x004453B1 File Offset: 0x004435B1
		public IEnumerator<uint> GetEnumerator()
		{
			return this.Offsets.GetEnumerator();
		}

		// Token: 0x0600B92B RID: 47403 RVA: 0x004453C3 File Offset: 0x004435C3
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
