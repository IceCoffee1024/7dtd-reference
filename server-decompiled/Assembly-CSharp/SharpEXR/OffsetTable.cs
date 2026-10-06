using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpEXR
{
	// Token: 0x02001702 RID: 5890
	public class OffsetTable : IEnumerable<uint>, IEnumerable
	{
		// Token: 0x170016F5 RID: 5877
		// (get) Token: 0x0600B851 RID: 47185 RVA: 0x004428A2 File Offset: 0x00440AA2
		// (set) Token: 0x0600B852 RID: 47186 RVA: 0x004428AA File Offset: 0x00440AAA
		public List<uint> Offsets { get; set; }

		// Token: 0x0600B853 RID: 47187 RVA: 0x004428B3 File Offset: 0x00440AB3
		public OffsetTable()
		{
			this.Offsets = new List<uint>();
		}

		// Token: 0x0600B854 RID: 47188 RVA: 0x004428C6 File Offset: 0x00440AC6
		public OffsetTable(int capacity)
		{
			this.Offsets = new List<uint>(capacity);
		}

		// Token: 0x0600B855 RID: 47189 RVA: 0x004428DC File Offset: 0x00440ADC
		public void Read(IEXRReader reader, int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.Offsets.Add(reader.ReadUInt32());
				reader.ReadUInt32();
			}
		}

		// Token: 0x0600B856 RID: 47190 RVA: 0x0044290D File Offset: 0x00440B0D
		public IEnumerator<uint> GetEnumerator()
		{
			return this.Offsets.GetEnumerator();
		}

		// Token: 0x0600B857 RID: 47191 RVA: 0x0044291F File Offset: 0x00440B1F
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
