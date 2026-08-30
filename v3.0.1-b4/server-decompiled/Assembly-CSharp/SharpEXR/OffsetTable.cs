using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpEXR
{
	// Token: 0x020016B0 RID: 5808
	public class OffsetTable : IEnumerable<uint>, IEnumerable
	{
		// Token: 0x170015E1 RID: 5601
		// (get) Token: 0x0600B577 RID: 46455 RVA: 0x0043860A File Offset: 0x0043680A
		// (set) Token: 0x0600B578 RID: 46456 RVA: 0x00438612 File Offset: 0x00436812
		public List<uint> Offsets { get; set; }

		// Token: 0x0600B579 RID: 46457 RVA: 0x0043861B File Offset: 0x0043681B
		public OffsetTable()
		{
			this.Offsets = new List<uint>();
		}

		// Token: 0x0600B57A RID: 46458 RVA: 0x0043862E File Offset: 0x0043682E
		public OffsetTable(int capacity)
		{
			this.Offsets = new List<uint>(capacity);
		}

		// Token: 0x0600B57B RID: 46459 RVA: 0x00438644 File Offset: 0x00436844
		public void Read(IEXRReader reader, int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.Offsets.Add(reader.ReadUInt32());
				reader.ReadUInt32();
			}
		}

		// Token: 0x0600B57C RID: 46460 RVA: 0x00438675 File Offset: 0x00436875
		public IEnumerator<uint> GetEnumerator()
		{
			return this.Offsets.GetEnumerator();
		}

		// Token: 0x0600B57D RID: 46461 RVA: 0x00438687 File Offset: 0x00436887
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
