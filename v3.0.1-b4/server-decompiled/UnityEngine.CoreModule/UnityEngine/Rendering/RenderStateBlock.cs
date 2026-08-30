using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200046C RID: 1132
	public struct RenderStateBlock : IEquatable<RenderStateBlock>
	{
		// Token: 0x0600261A RID: 9754 RVA: 0x0004154F File Offset: 0x0003F74F
		public RenderStateBlock(RenderStateMask mask)
		{
			this.m_BlendState = BlendState.defaultValue;
			this.m_RasterState = RasterState.defaultValue;
			this.m_DepthState = DepthState.defaultValue;
			this.m_StencilState = StencilState.defaultValue;
			this.m_StencilReference = 0;
			this.m_Mask = mask;
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x0600261B RID: 9755 RVA: 0x0004158C File Offset: 0x0003F78C
		// (set) Token: 0x0600261C RID: 9756 RVA: 0x000415A4 File Offset: 0x0003F7A4
		public BlendState blendState
		{
			get
			{
				return this.m_BlendState;
			}
			set
			{
				this.m_BlendState = value;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x0600261D RID: 9757 RVA: 0x000415B0 File Offset: 0x0003F7B0
		// (set) Token: 0x0600261E RID: 9758 RVA: 0x000415C8 File Offset: 0x0003F7C8
		public RasterState rasterState
		{
			get
			{
				return this.m_RasterState;
			}
			set
			{
				this.m_RasterState = value;
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x0600261F RID: 9759 RVA: 0x000415D4 File Offset: 0x0003F7D4
		// (set) Token: 0x06002620 RID: 9760 RVA: 0x000415EC File Offset: 0x0003F7EC
		public DepthState depthState
		{
			get
			{
				return this.m_DepthState;
			}
			set
			{
				this.m_DepthState = value;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06002621 RID: 9761 RVA: 0x000415F8 File Offset: 0x0003F7F8
		// (set) Token: 0x06002622 RID: 9762 RVA: 0x00041610 File Offset: 0x0003F810
		public StencilState stencilState
		{
			get
			{
				return this.m_StencilState;
			}
			set
			{
				this.m_StencilState = value;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06002623 RID: 9763 RVA: 0x0004161C File Offset: 0x0003F81C
		// (set) Token: 0x06002624 RID: 9764 RVA: 0x00041634 File Offset: 0x0003F834
		public int stencilReference
		{
			get
			{
				return this.m_StencilReference;
			}
			set
			{
				this.m_StencilReference = value;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06002625 RID: 9765 RVA: 0x00041640 File Offset: 0x0003F840
		// (set) Token: 0x06002626 RID: 9766 RVA: 0x00041658 File Offset: 0x0003F858
		public RenderStateMask mask
		{
			get
			{
				return this.m_Mask;
			}
			set
			{
				this.m_Mask = value;
			}
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x00041664 File Offset: 0x0003F864
		public bool Equals(RenderStateBlock other)
		{
			return this.m_BlendState.Equals(other.m_BlendState) && this.m_RasterState.Equals(other.m_RasterState) && this.m_DepthState.Equals(other.m_DepthState) && this.m_StencilState.Equals(other.m_StencilState) && this.m_StencilReference == other.m_StencilReference && this.m_Mask == other.m_Mask;
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x000416E4 File Offset: 0x0003F8E4
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is RenderStateBlock && this.Equals((RenderStateBlock)obj);
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x0004171C File Offset: 0x0003F91C
		public override int GetHashCode()
		{
			int num = this.m_BlendState.GetHashCode();
			num = (num * 397) ^ this.m_RasterState.GetHashCode();
			num = (num * 397) ^ this.m_DepthState.GetHashCode();
			num = (num * 397) ^ this.m_StencilState.GetHashCode();
			num = (num * 397) ^ this.m_StencilReference;
			return (num * 397) ^ (int)this.m_Mask;
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x000417B0 File Offset: 0x0003F9B0
		public static bool operator ==(RenderStateBlock left, RenderStateBlock right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x000417CC File Offset: 0x0003F9CC
		public static bool operator !=(RenderStateBlock left, RenderStateBlock right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000E7E RID: 3710
		private BlendState m_BlendState;

		// Token: 0x04000E7F RID: 3711
		private RasterState m_RasterState;

		// Token: 0x04000E80 RID: 3712
		private DepthState m_DepthState;

		// Token: 0x04000E81 RID: 3713
		private StencilState m_StencilState;

		// Token: 0x04000E82 RID: 3714
		private int m_StencilReference;

		// Token: 0x04000E83 RID: 3715
		private RenderStateMask m_Mask;
	}
}
