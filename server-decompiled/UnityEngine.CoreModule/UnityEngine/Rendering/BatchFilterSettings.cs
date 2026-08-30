using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200043F RID: 1087
	public struct BatchFilterSettings
	{
		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x0600245E RID: 9310 RVA: 0x0003D1EC File Offset: 0x0003B3EC
		// (set) Token: 0x0600245F RID: 9311 RVA: 0x0003D1F4 File Offset: 0x0003B3F4
		public MotionVectorGenerationMode motionMode
		{
			get
			{
				return (MotionVectorGenerationMode)this.m_motionMode;
			}
			set
			{
				this.m_motionMode = (byte)value;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06002460 RID: 9312 RVA: 0x0003D1FE File Offset: 0x0003B3FE
		// (set) Token: 0x06002461 RID: 9313 RVA: 0x0003D206 File Offset: 0x0003B406
		public ShadowCastingMode shadowCastingMode
		{
			get
			{
				return (ShadowCastingMode)this.m_shadowMode;
			}
			set
			{
				this.m_shadowMode = (byte)value;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06002462 RID: 9314 RVA: 0x0003D210 File Offset: 0x0003B410
		// (set) Token: 0x06002463 RID: 9315 RVA: 0x0003D21B File Offset: 0x0003B41B
		public bool receiveShadows
		{
			get
			{
				return this.m_receiveShadows > 0;
			}
			set
			{
				this.m_receiveShadows = (value ? 1 : 0);
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002464 RID: 9316 RVA: 0x0003D22B File Offset: 0x0003B42B
		// (set) Token: 0x06002465 RID: 9317 RVA: 0x0003D236 File Offset: 0x0003B436
		public bool staticShadowCaster
		{
			get
			{
				return this.m_staticShadowCaster > 0;
			}
			set
			{
				this.m_staticShadowCaster = (value ? 1 : 0);
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002466 RID: 9318 RVA: 0x0003D246 File Offset: 0x0003B446
		// (set) Token: 0x06002467 RID: 9319 RVA: 0x0003D251 File Offset: 0x0003B451
		public bool allDepthSorted
		{
			get
			{
				return this.m_allDepthSorted > 0;
			}
			set
			{
				this.m_allDepthSorted = (value ? 1 : 0);
			}
		}

		// Token: 0x04000D69 RID: 3433
		public uint renderingLayerMask;

		// Token: 0x04000D6A RID: 3434
		public byte layer;

		// Token: 0x04000D6B RID: 3435
		private byte m_motionMode;

		// Token: 0x04000D6C RID: 3436
		private byte m_shadowMode;

		// Token: 0x04000D6D RID: 3437
		private byte m_receiveShadows;

		// Token: 0x04000D6E RID: 3438
		private byte m_staticShadowCaster;

		// Token: 0x04000D6F RID: 3439
		private byte m_allDepthSorted;
	}
}
