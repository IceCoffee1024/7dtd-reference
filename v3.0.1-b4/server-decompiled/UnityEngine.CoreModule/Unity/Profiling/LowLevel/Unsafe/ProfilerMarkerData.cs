using System;
using System.Runtime.InteropServices;

namespace Unity.Profiling.LowLevel.Unsafe
{
	// Token: 0x0200006D RID: 109
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	public struct ProfilerMarkerData
	{
		// Token: 0x0400017A RID: 378
		[FieldOffset(0)]
		public byte Type;

		// Token: 0x0400017B RID: 379
		[FieldOffset(1)]
		private readonly byte reserved0;

		// Token: 0x0400017C RID: 380
		[FieldOffset(2)]
		private readonly ushort reserved1;

		// Token: 0x0400017D RID: 381
		[FieldOffset(4)]
		public uint Size;

		// Token: 0x0400017E RID: 382
		[FieldOffset(8)]
		public unsafe void* Ptr;
	}
}
