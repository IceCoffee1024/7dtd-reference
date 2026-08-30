using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000299 RID: 665
	[StaticAccessor("FrameDebugger", StaticAccessorType.DoubleColon)]
	[NativeHeader("Runtime/Profiler/PerformanceTools/FrameDebugger.h")]
	public static class FrameDebugger
	{
		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001C2D RID: 7213 RVA: 0x0002ED36 File Offset: 0x0002CF36
		public static bool enabled
		{
			get
			{
				return FrameDebugger.IsLocalEnabled() || FrameDebugger.IsRemoteEnabled();
			}
		}

		// Token: 0x06001C2E RID: 7214
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsLocalEnabled();

		// Token: 0x06001C2F RID: 7215
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsRemoteEnabled();
	}
}
