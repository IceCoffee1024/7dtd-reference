using System;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.PlayerLoop
{
	// Token: 0x0200038B RID: 907
	[MovedFrom("UnityEngine.Experimental.PlayerLoop")]
	[RequiredByNativeCode]
	public struct PostLateUpdate
	{
		// Token: 0x0200038C RID: 908
		[RequiredByNativeCode]
		public struct PlayerSendFrameStarted
		{
		}

		// Token: 0x0200038D RID: 909
		[RequiredByNativeCode]
		public struct UpdateRectTransform
		{
		}

		// Token: 0x0200038E RID: 910
		[RequiredByNativeCode]
		public struct UpdateCanvasRectTransform
		{
		}

		// Token: 0x0200038F RID: 911
		[RequiredByNativeCode]
		public struct PlayerUpdateCanvases
		{
		}

		// Token: 0x02000390 RID: 912
		[RequiredByNativeCode]
		public struct UpdateAudio
		{
		}

		// Token: 0x02000391 RID: 913
		[RequiredByNativeCode]
		public struct UpdateVideo
		{
		}

		// Token: 0x02000392 RID: 914
		[RequiredByNativeCode]
		public struct DirectorLateUpdate
		{
		}

		// Token: 0x02000393 RID: 915
		[RequiredByNativeCode]
		public struct ScriptRunDelayedDynamicFrameRate
		{
		}

		// Token: 0x02000394 RID: 916
		[RequiredByNativeCode]
		public struct VFXUpdate
		{
		}

		// Token: 0x02000395 RID: 917
		[RequiredByNativeCode]
		public struct ParticleSystemEndUpdateAll
		{
		}

		// Token: 0x02000396 RID: 918
		[RequiredByNativeCode]
		public struct EndGraphicsJobsAfterScriptLateUpdate
		{
		}

		// Token: 0x02000397 RID: 919
		[RequiredByNativeCode]
		public struct UpdateSubstance
		{
		}

		// Token: 0x02000398 RID: 920
		[RequiredByNativeCode]
		public struct UpdateCustomRenderTextures
		{
		}

		// Token: 0x02000399 RID: 921
		[RequiredByNativeCode]
		public struct XRPostLateUpdate
		{
		}

		// Token: 0x0200039A RID: 922
		[RequiredByNativeCode]
		public struct UpdateAllRenderers
		{
		}

		// Token: 0x0200039B RID: 923
		[RequiredByNativeCode]
		public struct UpdateLightProbeProxyVolumes
		{
		}

		// Token: 0x0200039C RID: 924
		[RequiredByNativeCode]
		public struct EnlightenRuntimeUpdate
		{
		}

		// Token: 0x0200039D RID: 925
		[RequiredByNativeCode]
		public struct UpdateAllSkinnedMeshes
		{
		}

		// Token: 0x0200039E RID: 926
		[RequiredByNativeCode]
		public struct ProcessWebSendMessages
		{
		}

		// Token: 0x0200039F RID: 927
		[RequiredByNativeCode]
		public struct SortingGroupsUpdate
		{
		}

		// Token: 0x020003A0 RID: 928
		[RequiredByNativeCode]
		public struct UpdateVideoTextures
		{
		}

		// Token: 0x020003A1 RID: 929
		[RequiredByNativeCode]
		public struct DirectorRenderImage
		{
		}

		// Token: 0x020003A2 RID: 930
		[RequiredByNativeCode]
		public struct PlayerEmitCanvasGeometry
		{
		}

		// Token: 0x020003A3 RID: 931
		[RequiredByNativeCode]
		internal struct PlayerRenderUIEBatchModeOffscreen
		{
		}

		// Token: 0x020003A4 RID: 932
		[RequiredByNativeCode]
		public struct FinishFrameRendering
		{
		}

		// Token: 0x020003A5 RID: 933
		[RequiredByNativeCode]
		public struct BatchModeUpdate
		{
		}

		// Token: 0x020003A6 RID: 934
		[RequiredByNativeCode]
		public struct PlayerSendFrameComplete
		{
		}

		// Token: 0x020003A7 RID: 935
		[RequiredByNativeCode]
		public struct UpdateCaptureScreenshot
		{
		}

		// Token: 0x020003A8 RID: 936
		[RequiredByNativeCode]
		public struct PresentAfterDraw
		{
		}

		// Token: 0x020003A9 RID: 937
		[RequiredByNativeCode]
		public struct ClearImmediateRenderers
		{
		}

		// Token: 0x020003AA RID: 938
		[RequiredByNativeCode]
		public struct XRPostPresent
		{
		}

		// Token: 0x020003AB RID: 939
		[RequiredByNativeCode]
		public struct UpdateResolution
		{
		}

		// Token: 0x020003AC RID: 940
		[RequiredByNativeCode]
		public struct InputEndFrame
		{
		}

		// Token: 0x020003AD RID: 941
		[RequiredByNativeCode]
		public struct GUIClearEvents
		{
		}

		// Token: 0x020003AE RID: 942
		[RequiredByNativeCode]
		public struct ShaderHandleErrors
		{
		}

		// Token: 0x020003AF RID: 943
		[RequiredByNativeCode]
		public struct ResetInputAxis
		{
		}

		// Token: 0x020003B0 RID: 944
		[RequiredByNativeCode]
		public struct ThreadedLoadingDebug
		{
		}

		// Token: 0x020003B1 RID: 945
		[RequiredByNativeCode]
		public struct ProfilerSynchronizeStats
		{
		}

		// Token: 0x020003B2 RID: 946
		[RequiredByNativeCode]
		public struct MemoryFrameMaintenance
		{
		}

		// Token: 0x020003B3 RID: 947
		[RequiredByNativeCode]
		public struct ExecuteGameCenterCallbacks
		{
		}

		// Token: 0x020003B4 RID: 948
		[RequiredByNativeCode]
		public struct XRPreEndFrame
		{
		}

		// Token: 0x020003B5 RID: 949
		[RequiredByNativeCode]
		public struct ProfilerEndFrame
		{
		}

		// Token: 0x020003B6 RID: 950
		[RequiredByNativeCode]
		public struct GraphicsWarmupPreloadedShaders
		{
		}

		// Token: 0x020003B7 RID: 951
		[RequiredByNativeCode]
		public struct PlayerSendFramePostPresent
		{
		}

		// Token: 0x020003B8 RID: 952
		[RequiredByNativeCode]
		public struct PhysicsSkinnedClothBeginUpdate
		{
		}

		// Token: 0x020003B9 RID: 953
		[RequiredByNativeCode]
		public struct PhysicsSkinnedClothFinishUpdate
		{
		}

		// Token: 0x020003BA RID: 954
		[RequiredByNativeCode]
		public struct TriggerEndOfFrameCallbacks
		{
		}

		// Token: 0x020003BB RID: 955
		[RequiredByNativeCode]
		public struct ObjectDispatcherPostLateUpdate
		{
		}
	}
}
