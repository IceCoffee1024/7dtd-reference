using System;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.PlayerLoop
{
	// Token: 0x0200033A RID: 826
	[MovedFrom("UnityEngine.Experimental.PlayerLoop")]
	[RequiredByNativeCode]
	public struct EarlyUpdate
	{
		// Token: 0x0200033B RID: 827
		[RequiredByNativeCode]
		public struct PollPlayerConnection
		{
		}

		// Token: 0x0200033C RID: 828
		[Obsolete("ProfilerStartFrame player loop component has been moved to the Initialization category. (UnityUpgradable) -> UnityEngine.PlayerLoop.Initialization/ProfilerStartFrame", true)]
		public struct ProfilerStartFrame
		{
		}

		// Token: 0x0200033D RID: 829
		[RequiredByNativeCode]
		public struct PollHtcsPlayerConnection
		{
		}

		// Token: 0x0200033E RID: 830
		[RequiredByNativeCode]
		public struct GpuTimestamp
		{
		}

		// Token: 0x0200033F RID: 831
		[RequiredByNativeCode]
		public struct AnalyticsCoreStatsUpdate
		{
		}

		// Token: 0x02000340 RID: 832
		[RequiredByNativeCode]
		public struct UnityWebRequestUpdate
		{
		}

		// Token: 0x02000341 RID: 833
		[RequiredByNativeCode]
		public struct UpdateStreamingManager
		{
		}

		// Token: 0x02000342 RID: 834
		[RequiredByNativeCode]
		public struct ExecuteMainThreadJobs
		{
		}

		// Token: 0x02000343 RID: 835
		[RequiredByNativeCode]
		public struct ProcessMouseInWindow
		{
		}

		// Token: 0x02000344 RID: 836
		[RequiredByNativeCode]
		public struct ClearIntermediateRenderers
		{
		}

		// Token: 0x02000345 RID: 837
		[RequiredByNativeCode]
		public struct ClearLines
		{
		}

		// Token: 0x02000346 RID: 838
		[RequiredByNativeCode]
		public struct PresentBeforeUpdate
		{
		}

		// Token: 0x02000347 RID: 839
		[RequiredByNativeCode]
		public struct ResetFrameStatsAfterPresent
		{
		}

		// Token: 0x02000348 RID: 840
		[RequiredByNativeCode]
		public struct UpdateAsyncReadbackManager
		{
		}

		// Token: 0x02000349 RID: 841
		[RequiredByNativeCode]
		public struct UpdateTextureStreamingManager
		{
		}

		// Token: 0x0200034A RID: 842
		[RequiredByNativeCode]
		public struct UpdatePreloading
		{
		}

		// Token: 0x0200034B RID: 843
		[RequiredByNativeCode]
		public struct UpdateContentLoading
		{
		}

		// Token: 0x0200034C RID: 844
		[RequiredByNativeCode]
		public struct UpdateAsyncInstantiate
		{
		}

		// Token: 0x0200034D RID: 845
		[RequiredByNativeCode]
		public struct RendererNotifyInvisible
		{
		}

		// Token: 0x0200034E RID: 846
		[RequiredByNativeCode]
		public struct PlayerCleanupCachedData
		{
		}

		// Token: 0x0200034F RID: 847
		[RequiredByNativeCode]
		public struct UpdateMainGameViewRect
		{
		}

		// Token: 0x02000350 RID: 848
		[RequiredByNativeCode]
		public struct UpdateCanvasRectTransform
		{
		}

		// Token: 0x02000351 RID: 849
		[RequiredByNativeCode]
		public struct UpdateInputManager
		{
		}

		// Token: 0x02000352 RID: 850
		[RequiredByNativeCode]
		public struct ProcessRemoteInput
		{
		}

		// Token: 0x02000353 RID: 851
		[RequiredByNativeCode]
		public struct XRUpdate
		{
		}

		// Token: 0x02000354 RID: 852
		[RequiredByNativeCode]
		public struct ScriptRunDelayedStartupFrame
		{
		}

		// Token: 0x02000355 RID: 853
		[RequiredByNativeCode]
		public struct UpdateKinect
		{
		}

		// Token: 0x02000356 RID: 854
		[RequiredByNativeCode]
		public struct DeliverIosPlatformEvents
		{
		}

		// Token: 0x02000357 RID: 855
		[RequiredByNativeCode]
		public struct DispatchEventQueueEvents
		{
		}

		// Token: 0x02000358 RID: 856
		[RequiredByNativeCode]
		public struct Physics2DEarlyUpdate
		{
		}

		// Token: 0x02000359 RID: 857
		[RequiredByNativeCode]
		public struct PhysicsResetInterpolatedTransformPosition
		{
		}

		// Token: 0x0200035A RID: 858
		[RequiredByNativeCode]
		public struct SpriteAtlasManagerUpdate
		{
		}

		// Token: 0x0200035B RID: 859
		[Obsolete("TangoUpdate has been deprecated. Use ARCoreUpdate instead (UnityUpgradable) -> UnityEngine.PlayerLoop.EarlyUpdate/ARCoreUpdate", false)]
		[RequiredByNativeCode]
		public struct TangoUpdate
		{
		}

		// Token: 0x0200035C RID: 860
		[RequiredByNativeCode]
		public struct ARCoreUpdate
		{
		}

		// Token: 0x0200035D RID: 861
		[RequiredByNativeCode]
		public struct PerformanceAnalyticsUpdate
		{
		}
	}
}
