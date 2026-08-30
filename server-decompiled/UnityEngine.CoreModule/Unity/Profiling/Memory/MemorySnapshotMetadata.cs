using System;

namespace Unity.Profiling.Memory
{
	// Token: 0x02000072 RID: 114
	public class MemorySnapshotMetadata
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00003C91 File Offset: 0x00001E91
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x00003C99 File Offset: 0x00001E99
		public string Description { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00003CA2 File Offset: 0x00001EA2
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x00003CAA File Offset: 0x00001EAA
		internal byte[] Data { get; set; }
	}
}
