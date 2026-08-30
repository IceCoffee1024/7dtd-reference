using System;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200014A RID: 330
	[NativeType("Runtime/GfxDevice/GfxDeviceTypes.h")]
	public enum ComputeBufferMode
	{
		// Token: 0x04000423 RID: 1059
		Immutable,
		// Token: 0x04000424 RID: 1060
		Dynamic,
		// Token: 0x04000425 RID: 1061
		[Obsolete("ComputeBufferMode.Circular is deprecated (legacy mode)")]
		Circular,
		// Token: 0x04000426 RID: 1062
		[Obsolete("ComputeBufferMode.StreamOut is deprecated (internal use only)")]
		StreamOut,
		// Token: 0x04000427 RID: 1063
		SubUpdates
	}
}
