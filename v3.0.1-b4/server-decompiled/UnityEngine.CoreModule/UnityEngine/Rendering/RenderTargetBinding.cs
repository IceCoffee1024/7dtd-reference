using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200040D RID: 1037
	public struct RenderTargetBinding
	{
		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x060021E2 RID: 8674 RVA: 0x00038834 File Offset: 0x00036A34
		// (set) Token: 0x060021E3 RID: 8675 RVA: 0x0003884C File Offset: 0x00036A4C
		public RenderTargetIdentifier[] colorRenderTargets
		{
			get
			{
				return this.m_ColorRenderTargets;
			}
			set
			{
				this.m_ColorRenderTargets = value;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x060021E4 RID: 8676 RVA: 0x00038858 File Offset: 0x00036A58
		// (set) Token: 0x060021E5 RID: 8677 RVA: 0x00038870 File Offset: 0x00036A70
		public RenderTargetIdentifier depthRenderTarget
		{
			get
			{
				return this.m_DepthRenderTarget;
			}
			set
			{
				this.m_DepthRenderTarget = value;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x060021E6 RID: 8678 RVA: 0x0003887C File Offset: 0x00036A7C
		// (set) Token: 0x060021E7 RID: 8679 RVA: 0x00038894 File Offset: 0x00036A94
		public RenderBufferLoadAction[] colorLoadActions
		{
			get
			{
				return this.m_ColorLoadActions;
			}
			set
			{
				this.m_ColorLoadActions = value;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x060021E8 RID: 8680 RVA: 0x000388A0 File Offset: 0x00036AA0
		// (set) Token: 0x060021E9 RID: 8681 RVA: 0x000388B8 File Offset: 0x00036AB8
		public RenderBufferStoreAction[] colorStoreActions
		{
			get
			{
				return this.m_ColorStoreActions;
			}
			set
			{
				this.m_ColorStoreActions = value;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x060021EA RID: 8682 RVA: 0x000388C4 File Offset: 0x00036AC4
		// (set) Token: 0x060021EB RID: 8683 RVA: 0x000388DC File Offset: 0x00036ADC
		public RenderBufferLoadAction depthLoadAction
		{
			get
			{
				return this.m_DepthLoadAction;
			}
			set
			{
				this.m_DepthLoadAction = value;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x060021EC RID: 8684 RVA: 0x000388E8 File Offset: 0x00036AE8
		// (set) Token: 0x060021ED RID: 8685 RVA: 0x00038900 File Offset: 0x00036B00
		public RenderBufferStoreAction depthStoreAction
		{
			get
			{
				return this.m_DepthStoreAction;
			}
			set
			{
				this.m_DepthStoreAction = value;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x060021EE RID: 8686 RVA: 0x0003890C File Offset: 0x00036B0C
		// (set) Token: 0x060021EF RID: 8687 RVA: 0x00038924 File Offset: 0x00036B24
		public RenderTargetFlags flags
		{
			get
			{
				return this.m_Flags;
			}
			set
			{
				this.m_Flags = value;
			}
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x0003892E File Offset: 0x00036B2E
		public RenderTargetBinding(RenderTargetIdentifier[] colorRenderTargets, RenderBufferLoadAction[] colorLoadActions, RenderBufferStoreAction[] colorStoreActions, RenderTargetIdentifier depthRenderTarget, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction)
		{
			this.m_ColorRenderTargets = colorRenderTargets;
			this.m_DepthRenderTarget = depthRenderTarget;
			this.m_ColorLoadActions = colorLoadActions;
			this.m_ColorStoreActions = colorStoreActions;
			this.m_DepthLoadAction = depthLoadAction;
			this.m_DepthStoreAction = depthStoreAction;
			this.m_Flags = RenderTargetFlags.None;
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x00038965 File Offset: 0x00036B65
		public RenderTargetBinding(RenderTargetIdentifier colorRenderTarget, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, RenderTargetIdentifier depthRenderTarget, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction)
		{
			this = new RenderTargetBinding(new RenderTargetIdentifier[] { colorRenderTarget }, new RenderBufferLoadAction[] { colorLoadAction }, new RenderBufferStoreAction[] { colorStoreAction }, depthRenderTarget, depthLoadAction, depthStoreAction);
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x00038998 File Offset: 0x00036B98
		public RenderTargetBinding(RenderTargetSetup setup)
		{
			this.m_ColorRenderTargets = new RenderTargetIdentifier[setup.color.Length];
			for (int i = 0; i < this.m_ColorRenderTargets.Length; i++)
			{
				this.m_ColorRenderTargets[i] = new RenderTargetIdentifier(setup.color[i], setup.mipLevel, setup.cubemapFace, setup.depthSlice);
			}
			this.m_DepthRenderTarget = setup.depth;
			this.m_ColorLoadActions = (RenderBufferLoadAction[])setup.colorLoad.Clone();
			this.m_ColorStoreActions = (RenderBufferStoreAction[])setup.colorStore.Clone();
			this.m_DepthLoadAction = setup.depthLoad;
			this.m_DepthStoreAction = setup.depthStore;
			this.m_Flags = RenderTargetFlags.None;
		}

		// Token: 0x04000C64 RID: 3172
		private RenderTargetIdentifier[] m_ColorRenderTargets;

		// Token: 0x04000C65 RID: 3173
		private RenderTargetIdentifier m_DepthRenderTarget;

		// Token: 0x04000C66 RID: 3174
		private RenderBufferLoadAction[] m_ColorLoadActions;

		// Token: 0x04000C67 RID: 3175
		private RenderBufferStoreAction[] m_ColorStoreActions;

		// Token: 0x04000C68 RID: 3176
		private RenderBufferLoadAction m_DepthLoadAction;

		// Token: 0x04000C69 RID: 3177
		private RenderBufferStoreAction m_DepthStoreAction;

		// Token: 0x04000C6A RID: 3178
		private RenderTargetFlags m_Flags;
	}
}
