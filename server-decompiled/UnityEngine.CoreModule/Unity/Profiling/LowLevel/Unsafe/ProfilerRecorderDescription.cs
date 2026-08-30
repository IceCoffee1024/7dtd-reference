using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace Unity.Profiling.LowLevel.Unsafe
{
	// Token: 0x0200006B RID: 107
	[UsedByNativeCode]
	[StructLayout(LayoutKind.Explicit, Size = 24)]
	public readonly struct ProfilerRecorderDescription
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600019A RID: 410 RVA: 0x000039E5 File Offset: 0x00001BE5
		public ProfilerCategory Category
		{
			get
			{
				return this.category;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600019B RID: 411 RVA: 0x000039ED File Offset: 0x00001BED
		public MarkerFlags Flags
		{
			get
			{
				return this.flags;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600019C RID: 412 RVA: 0x000039F5 File Offset: 0x00001BF5
		public ProfilerMarkerDataType DataType
		{
			get
			{
				return this.dataType;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600019D RID: 413 RVA: 0x000039FD File Offset: 0x00001BFD
		public ProfilerMarkerDataUnit UnitType
		{
			get
			{
				return this.unitType;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00003A05 File Offset: 0x00001C05
		public int NameUtf8Len
		{
			get
			{
				return this.nameUtf8Len;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00003A0D File Offset: 0x00001C0D
		public unsafe byte* NameUtf8
		{
			get
			{
				return this.nameUtf8;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00003A15 File Offset: 0x00001C15
		public string Name
		{
			get
			{
				return ProfilerUnsafeUtility.Utf8ToString(this.nameUtf8, this.nameUtf8Len);
			}
		}

		// Token: 0x04000171 RID: 369
		[FieldOffset(0)]
		private readonly ProfilerCategory category;

		// Token: 0x04000172 RID: 370
		[FieldOffset(2)]
		private readonly MarkerFlags flags;

		// Token: 0x04000173 RID: 371
		[FieldOffset(4)]
		private readonly ProfilerMarkerDataType dataType;

		// Token: 0x04000174 RID: 372
		[FieldOffset(5)]
		private readonly ProfilerMarkerDataUnit unitType;

		// Token: 0x04000175 RID: 373
		[FieldOffset(8)]
		private readonly int reserved0;

		// Token: 0x04000176 RID: 374
		[FieldOffset(12)]
		private readonly int nameUtf8Len;

		// Token: 0x04000177 RID: 375
		[FieldOffset(16)]
		private unsafe readonly byte* nameUtf8;
	}
}
