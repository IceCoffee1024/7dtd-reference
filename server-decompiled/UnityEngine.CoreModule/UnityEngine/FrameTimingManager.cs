using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200014F RID: 335
	[StaticAccessor("GetUncheckedRealGfxDevice().GetFrameTimingManager()", StaticAccessorType.Dot)]
	public static class FrameTimingManager
	{
		// Token: 0x06000A79 RID: 2681
		[StaticAccessor("FrameTimingManager", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsFeatureEnabled();

		// Token: 0x06000A7A RID: 2682
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CaptureFrameTimings();

		// Token: 0x06000A7B RID: 2683
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint GetLatestTimings(uint numFrames, [Unmarshalled] FrameTiming[] timings);

		// Token: 0x06000A7C RID: 2684
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetVSyncsPerSecond();

		// Token: 0x06000A7D RID: 2685
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong GetGpuTimerFrequency();

		// Token: 0x06000A7E RID: 2686
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong GetCpuTimerFrequency();
	}
}
