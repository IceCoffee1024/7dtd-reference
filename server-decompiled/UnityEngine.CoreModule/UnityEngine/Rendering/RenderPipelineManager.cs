using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x0200046A RID: 1130
	public static class RenderPipelineManager
	{
		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x060025E8 RID: 9704 RVA: 0x00040BD2 File Offset: 0x0003EDD2
		// (set) Token: 0x060025E9 RID: 9705 RVA: 0x00040BD9 File Offset: 0x0003EDD9
		public static RenderPipeline currentPipeline
		{
			get
			{
				return RenderPipelineManager.s_CurrentPipeline;
			}
			private set
			{
				RenderPipelineManager.s_CurrentPipelineType = ((value != null) ? value.GetType().ToString() : "Built-in Pipeline");
				RenderPipelineManager.s_CurrentPipeline = value;
			}
		}

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x060025EA RID: 9706 RVA: 0x00040BFC File Offset: 0x0003EDFC
		// (remove) Token: 0x060025EB RID: 9707 RVA: 0x00040C30 File Offset: 0x0003EE30
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<ScriptableRenderContext, Camera[]> beginFrameRendering;

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x060025EC RID: 9708 RVA: 0x00040C64 File Offset: 0x0003EE64
		// (remove) Token: 0x060025ED RID: 9709 RVA: 0x00040C98 File Offset: 0x0003EE98
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<ScriptableRenderContext, Camera[]> endFrameRendering;

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x060025EE RID: 9710 RVA: 0x00040CCC File Offset: 0x0003EECC
		// (remove) Token: 0x060025EF RID: 9711 RVA: 0x00040D00 File Offset: 0x0003EF00
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<ScriptableRenderContext, List<Camera>> beginContextRendering;

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x060025F0 RID: 9712 RVA: 0x00040D34 File Offset: 0x0003EF34
		// (remove) Token: 0x060025F1 RID: 9713 RVA: 0x00040D68 File Offset: 0x0003EF68
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<ScriptableRenderContext, List<Camera>> endContextRendering;

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x060025F2 RID: 9714 RVA: 0x00040D9C File Offset: 0x0003EF9C
		// (remove) Token: 0x060025F3 RID: 9715 RVA: 0x00040DD0 File Offset: 0x0003EFD0
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<ScriptableRenderContext, Camera> beginCameraRendering;

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x060025F4 RID: 9716 RVA: 0x00040E04 File Offset: 0x0003F004
		// (remove) Token: 0x060025F5 RID: 9717 RVA: 0x00040E38 File Offset: 0x0003F038
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<ScriptableRenderContext, Camera> endCameraRendering;

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x060025F6 RID: 9718 RVA: 0x00040E6C File Offset: 0x0003F06C
		// (remove) Token: 0x060025F7 RID: 9719 RVA: 0x00040EA0 File Offset: 0x0003F0A0
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action activeRenderPipelineTypeChanged;

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x060025F8 RID: 9720 RVA: 0x00040ED4 File Offset: 0x0003F0D4
		// (remove) Token: 0x060025F9 RID: 9721 RVA: 0x00040F08 File Offset: 0x0003F108
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<RenderPipelineAsset, RenderPipelineAsset> activeRenderPipelineAssetChanged;

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x060025FA RID: 9722 RVA: 0x00040F3C File Offset: 0x0003F13C
		// (remove) Token: 0x060025FB RID: 9723 RVA: 0x00040F70 File Offset: 0x0003F170
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action activeRenderPipelineCreated;

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x060025FC RID: 9724 RVA: 0x00040FA4 File Offset: 0x0003F1A4
		// (remove) Token: 0x060025FD RID: 9725 RVA: 0x00040FD8 File Offset: 0x0003F1D8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action activeRenderPipelineDisposed;

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x060025FE RID: 9726 RVA: 0x0004100B File Offset: 0x0003F20B
		public static bool pipelineSwitchCompleted
		{
			get
			{
				return RenderPipelineManager.s_CurrentPipelineAsset == GraphicsSettings.currentRenderPipeline && !RenderPipelineManager.IsPipelineRequireCreation();
			}
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x00041024 File Offset: 0x0003F224
		internal static void BeginContextRendering(ScriptableRenderContext context, List<Camera> cameras)
		{
			Action<ScriptableRenderContext, List<Camera>> action = RenderPipelineManager.beginContextRendering;
			if (action != null)
			{
				action(context, cameras);
			}
			Action<ScriptableRenderContext, Camera[]> action2 = RenderPipelineManager.beginFrameRendering;
			if (action2 != null)
			{
				action2(context, cameras.ToArray());
			}
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x00041052 File Offset: 0x0003F252
		internal static void BeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			Action<ScriptableRenderContext, Camera> action = RenderPipelineManager.beginCameraRendering;
			if (action != null)
			{
				action(context, camera);
			}
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x00041068 File Offset: 0x0003F268
		internal static void EndContextRendering(ScriptableRenderContext context, List<Camera> cameras)
		{
			Action<ScriptableRenderContext, Camera[]> action = RenderPipelineManager.endFrameRendering;
			if (action != null)
			{
				action(context, cameras.ToArray());
			}
			Action<ScriptableRenderContext, List<Camera>> action2 = RenderPipelineManager.endContextRendering;
			if (action2 != null)
			{
				action2(context, cameras);
			}
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x00041096 File Offset: 0x0003F296
		internal static void EndCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			Action<ScriptableRenderContext, Camera> action = RenderPipelineManager.endCameraRendering;
			if (action != null)
			{
				action(context, camera);
			}
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x000410AC File Offset: 0x0003F2AC
		[RequiredByNativeCode]
		internal static void OnActiveRenderPipelineTypeChanged()
		{
			Action action = RenderPipelineManager.activeRenderPipelineTypeChanged;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x000410C0 File Offset: 0x0003F2C0
		[RequiredByNativeCode]
		internal static void OnActiveRenderPipelineAssetChanged(ScriptableObject from, ScriptableObject to)
		{
			Action<RenderPipelineAsset, RenderPipelineAsset> action = RenderPipelineManager.activeRenderPipelineAssetChanged;
			if (action != null)
			{
				action(from as RenderPipelineAsset, to as RenderPipelineAsset);
			}
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x000410E0 File Offset: 0x0003F2E0
		[RequiredByNativeCode]
		internal static void HandleRenderPipelineChange(RenderPipelineAsset pipelineAsset)
		{
			bool flag = RenderPipelineManager.s_CurrentPipelineAsset != pipelineAsset;
			bool flag2 = flag;
			if (flag2)
			{
				RenderPipelineManager.CleanupRenderPipeline();
				RenderPipelineManager.s_CurrentPipelineAsset = pipelineAsset;
			}
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x00041110 File Offset: 0x0003F310
		[RequiredByNativeCode]
		internal static void CleanupRenderPipeline()
		{
			bool flag = RenderPipelineManager.currentPipeline != null && !RenderPipelineManager.currentPipeline.disposed;
			if (flag)
			{
				Action action = RenderPipelineManager.activeRenderPipelineDisposed;
				if (action != null)
				{
					action();
				}
				RenderPipelineManager.currentPipeline.Dispose();
				RenderPipelineManager.s_CurrentPipelineAsset = null;
				RenderPipelineManager.currentPipeline = null;
				SupportedRenderingFeatures.active = new SupportedRenderingFeatures();
			}
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x00041170 File Offset: 0x0003F370
		[RequiredByNativeCode]
		private static string GetCurrentPipelineAssetType()
		{
			return RenderPipelineManager.s_CurrentPipelineType;
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x00041188 File Offset: 0x0003F388
		[RequiredByNativeCode]
		private static void DoRenderLoop_Internal(RenderPipelineAsset pipe, IntPtr loopPtr, Object renderRequest)
		{
			RenderPipelineManager.PrepareRenderPipeline(pipe);
			bool flag = RenderPipelineManager.currentPipeline == null;
			if (!flag)
			{
				ScriptableRenderContext scriptableRenderContext = new ScriptableRenderContext(loopPtr);
				RenderPipelineManager.s_Cameras.Clear();
				scriptableRenderContext.GetCameras(RenderPipelineManager.s_Cameras);
				bool flag2 = renderRequest == null;
				if (flag2)
				{
					RenderPipelineManager.currentPipeline.InternalRender(scriptableRenderContext, RenderPipelineManager.s_Cameras);
				}
				else
				{
					RenderPipelineManager.currentPipeline.InternalProcessRenderRequests<Object>(scriptableRenderContext, RenderPipelineManager.s_Cameras[0], renderRequest);
				}
				RenderPipelineManager.s_Cameras.Clear();
			}
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x0004120C File Offset: 0x0003F40C
		internal static void PrepareRenderPipeline(RenderPipelineAsset pipelineAsset)
		{
			RenderPipelineManager.HandleRenderPipelineChange(pipelineAsset);
			bool flag = RenderPipelineManager.IsPipelineRequireCreation();
			if (flag)
			{
				RenderPipelineManager.currentPipeline = RenderPipelineManager.s_CurrentPipelineAsset.InternalCreatePipeline();
				Action action = RenderPipelineManager.activeRenderPipelineCreated;
				if (action != null)
				{
					action();
				}
			}
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x0004124D File Offset: 0x0003F44D
		private static bool IsPipelineRequireCreation()
		{
			return RenderPipelineManager.s_CurrentPipelineAsset != null && (RenderPipelineManager.currentPipeline == null || RenderPipelineManager.currentPipeline.disposed);
		}

		// Token: 0x04000E69 RID: 3689
		internal static RenderPipelineAsset s_CurrentPipelineAsset;

		// Token: 0x04000E6A RID: 3690
		private static List<Camera> s_Cameras = new List<Camera>();

		// Token: 0x04000E6B RID: 3691
		private static string s_CurrentPipelineType = "Built-in Pipeline";

		// Token: 0x04000E6C RID: 3692
		private const string k_BuiltinPipelineName = "Built-in Pipeline";

		// Token: 0x04000E6D RID: 3693
		private static RenderPipeline s_CurrentPipeline = null;
	}
}
