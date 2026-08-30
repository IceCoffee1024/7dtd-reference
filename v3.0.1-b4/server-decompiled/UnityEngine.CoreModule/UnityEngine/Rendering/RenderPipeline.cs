using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x02000466 RID: 1126
	public abstract class RenderPipeline
	{
		// Token: 0x060025B8 RID: 9656
		protected abstract void Render(ScriptableRenderContext context, Camera[] cameras);

		// Token: 0x060025B9 RID: 9657 RVA: 0x00002669 File Offset: 0x00000869
		protected virtual void ProcessRenderRequests<RequestData>(ScriptableRenderContext context, Camera camera, RequestData renderRequest)
		{
		}

		// Token: 0x060025BA RID: 9658 RVA: 0x00040954 File Offset: 0x0003EB54
		protected internal virtual bool IsRenderRequestSupported<RequestData>(Camera camera, RequestData data)
		{
			return false;
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x00040967 File Offset: 0x0003EB67
		protected static void BeginFrameRendering(ScriptableRenderContext context, Camera[] cameras)
		{
			RenderPipelineManager.BeginContextRendering(context, new List<Camera>(cameras));
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x00040977 File Offset: 0x0003EB77
		protected static void BeginContextRendering(ScriptableRenderContext context, List<Camera> cameras)
		{
			RenderPipelineManager.BeginContextRendering(context, cameras);
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x00040982 File Offset: 0x0003EB82
		protected static void BeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			RenderPipelineManager.BeginCameraRendering(context, camera);
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x0004098D File Offset: 0x0003EB8D
		protected static void EndContextRendering(ScriptableRenderContext context, List<Camera> cameras)
		{
			RenderPipelineManager.EndContextRendering(context, cameras);
		}

		// Token: 0x060025BF RID: 9663 RVA: 0x00040998 File Offset: 0x0003EB98
		protected static void EndFrameRendering(ScriptableRenderContext context, Camera[] cameras)
		{
			RenderPipelineManager.EndContextRendering(context, new List<Camera>(cameras));
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x000409A8 File Offset: 0x0003EBA8
		protected static void EndCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			RenderPipelineManager.EndCameraRendering(context, camera);
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x000409B3 File Offset: 0x0003EBB3
		protected virtual void Render(ScriptableRenderContext context, List<Camera> cameras)
		{
			this.Render(context, cameras.ToArray());
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x000409C4 File Offset: 0x0003EBC4
		internal void InternalRender(ScriptableRenderContext context, List<Camera> cameras)
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(string.Format("{0} has been disposed. Do not call Render on disposed a RenderPipeline.", this));
			}
			this.Render(context, cameras);
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x000409F8 File Offset: 0x0003EBF8
		internal void InternalProcessRenderRequests<RequestData>(ScriptableRenderContext context, Camera camera, RequestData renderRequest)
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(string.Format("{0} has been disposed. Do not call Render on disposed a RenderPipeline.", this));
			}
			this.ProcessRenderRequests<RequestData>(context, camera, renderRequest);
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x00040A2C File Offset: 0x0003EC2C
		public static bool SupportsRenderRequest<RequestData>(Camera camera, RequestData data)
		{
			bool flag = false;
			bool flag2 = GraphicsSettings.currentRenderPipeline != null;
			if (flag2)
			{
				bool flag3 = RenderPipelineManager.currentPipeline == null;
				if (flag3)
				{
					RenderPipelineManager.PrepareRenderPipeline(GraphicsSettings.currentRenderPipeline);
				}
				flag = RenderPipelineManager.currentPipeline.IsRenderRequestSupported<RequestData>(camera, data);
			}
			return flag;
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x00040A78 File Offset: 0x0003EC78
		public static void SubmitRenderRequest<RequestData>(Camera camera, RequestData data)
		{
			camera.SubmitRenderRequest<RequestData>(data);
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x060025C6 RID: 9670 RVA: 0x00040A83 File Offset: 0x0003EC83
		// (set) Token: 0x060025C7 RID: 9671 RVA: 0x00040A8B File Offset: 0x0003EC8B
		public bool disposed { get; private set; }

		// Token: 0x060025C8 RID: 9672 RVA: 0x00040A94 File Offset: 0x0003EC94
		internal void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
			this.disposed = true;
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x00002669 File Offset: 0x00000869
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x060025CA RID: 9674 RVA: 0x00040AB0 File Offset: 0x0003ECB0
		public virtual RenderPipelineGlobalSettings defaultSettings
		{
			get
			{
				return null;
			}
		}

		// Token: 0x02000467 RID: 1127
		public class StandardRequest
		{
			// Token: 0x04000E65 RID: 3685
			public RenderTexture destination = null;

			// Token: 0x04000E66 RID: 3686
			public int mipLevel = 0;

			// Token: 0x04000E67 RID: 3687
			public CubemapFace face = CubemapFace.Unknown;

			// Token: 0x04000E68 RID: 3688
			public int slice = 0;
		}
	}
}
