using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Unity.Profiling.LowLevel.Unsafe
{
	// Token: 0x0200006E RID: 110
	[StructLayout(LayoutKind.Explicit, Size = 24)]
	public readonly struct ProfilerCategoryDescription
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00003B63 File Offset: 0x00001D63
		public string Name
		{
			get
			{
				return ProfilerUnsafeUtility.Utf8ToString(this.NameUtf8, this.NameUtf8Len);
			}
		}

		// Token: 0x0400017F RID: 383
		[FieldOffset(0)]
		public readonly ushort Id;

		// Token: 0x04000180 RID: 384
		[FieldOffset(2)]
		public readonly ushort Flags;

		// Token: 0x04000181 RID: 385
		[FieldOffset(4)]
		public readonly Color32 Color;

		// Token: 0x04000182 RID: 386
		[FieldOffset(8)]
		private readonly int reserved0;

		// Token: 0x04000183 RID: 387
		[FieldOffset(12)]
		public readonly int NameUtf8Len;

		// Token: 0x04000184 RID: 388
		[FieldOffset(16)]
		public unsafe readonly byte* NameUtf8;
	}
}
