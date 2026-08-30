using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpEXR
{
	// Token: 0x020016D6 RID: 5846
	public class OffsetTable : IEnumerable<uint>, IEnumerable
	{
		// Token: 0x17001653 RID: 5715
		// (get) Token: 0x0600B6FA RID: 46842 RVA: 0x0043E382 File Offset: 0x0043C582
		// (set) Token: 0x0600B6FB RID: 46843 RVA: 0x0043E38A File Offset: 0x0043C58A
		public List<uint> Offsets { get; set; }

		// Token: 0x0600B6FC RID: 46844 RVA: 0x0043E393 File Offset: 0x0043C593
		public OffsetTable()
		{
			this.Offsets = new List<uint>();
		}

		// Token: 0x0600B6FD RID: 46845 RVA: 0x0043E3A6 File Offset: 0x0043C5A6
		public OffsetTable(int capacity)
		{
			this.Offsets = new List<uint>(capacity);
		}

		// Token: 0x0600B6FE RID: 46846 RVA: 0x0043E3BC File Offset: 0x0043C5BC
		public void Read(IEXRReader reader, int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.Offsets.Add(reader.ReadUInt32());
				reader.ReadUInt32();
			}
		}

		// Token: 0x0600B6FF RID: 46847 RVA: 0x0043E3ED File Offset: 0x0043C5ED
		public IEnumerator<uint> GetEnumerator()
		{
			return this.Offsets.GetEnumerator();
		}

		// Token: 0x0600B700 RID: 46848 RVA: 0x0043E3FF File Offset: 0x0043C5FF
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
