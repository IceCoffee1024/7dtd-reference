using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x0200009C RID: 156
	[NativeContainer]
	internal struct NativeArrayDispose
	{
		// Token: 0x0600030F RID: 783 RVA: 0x00005D5C File Offset: 0x00003F5C
		public void Dispose()
		{
			UnsafeUtility.FreeTracked(this.m_Buffer, this.m_AllocatorLabel);
		}

		// Token: 0x04000237 RID: 567
		[NativeDisableUnsafePtrRestriction]
		internal unsafe void* m_Buffer;

		// Token: 0x04000238 RID: 568
		internal Allocator m_AllocatorLabel;
	}
}
