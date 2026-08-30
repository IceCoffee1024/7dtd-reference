using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000461 RID: 1121
	public struct RasterState : IEquatable<RasterState>
	{
		// Token: 0x06002597 RID: 9623 RVA: 0x00040195 File Offset: 0x0003E395
		public RasterState(CullMode cullingMode = CullMode.Back, int offsetUnits = 0, float offsetFactor = 0f, bool depthClip = true)
		{
			this.m_CullingMode = cullingMode;
			this.m_OffsetUnits = offsetUnits;
			this.m_OffsetFactor = offsetFactor;
			this.m_DepthClip = Convert.ToByte(depthClip);
			this.m_Conservative = Convert.ToByte(false);
			this.m_Padding1 = 0;
			this.m_Padding2 = 0;
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002598 RID: 9624 RVA: 0x000401D4 File Offset: 0x0003E3D4
		// (set) Token: 0x06002599 RID: 9625 RVA: 0x000401EC File Offset: 0x0003E3EC
		public CullMode cullingMode
		{
			get
			{
				return this.m_CullingMode;
			}
			set
			{
				this.m_CullingMode = value;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x0600259A RID: 9626 RVA: 0x000401F8 File Offset: 0x0003E3F8
		// (set) Token: 0x0600259B RID: 9627 RVA: 0x00040215 File Offset: 0x0003E415
		public bool depthClip
		{
			get
			{
				return Convert.ToBoolean(this.m_DepthClip);
			}
			set
			{
				this.m_DepthClip = Convert.ToByte(value);
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x0600259C RID: 9628 RVA: 0x00040224 File Offset: 0x0003E424
		// (set) Token: 0x0600259D RID: 9629 RVA: 0x00040241 File Offset: 0x0003E441
		public bool conservative
		{
			get
			{
				return Convert.ToBoolean(this.m_Conservative);
			}
			set
			{
				this.m_Conservative = Convert.ToByte(value);
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x0600259E RID: 9630 RVA: 0x00040250 File Offset: 0x0003E450
		// (set) Token: 0x0600259F RID: 9631 RVA: 0x00040268 File Offset: 0x0003E468
		public int offsetUnits
		{
			get
			{
				return this.m_OffsetUnits;
			}
			set
			{
				this.m_OffsetUnits = value;
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x060025A0 RID: 9632 RVA: 0x00040274 File Offset: 0x0003E474
		// (set) Token: 0x060025A1 RID: 9633 RVA: 0x0004028C File Offset: 0x0003E48C
		public float offsetFactor
		{
			get
			{
				return this.m_OffsetFactor;
			}
			set
			{
				this.m_OffsetFactor = value;
			}
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x00040298 File Offset: 0x0003E498
		public bool Equals(RasterState other)
		{
			return this.m_CullingMode == other.m_CullingMode && this.m_OffsetUnits == other.m_OffsetUnits && this.m_OffsetFactor.Equals(other.m_OffsetFactor) && this.m_DepthClip == other.m_DepthClip && this.m_Conservative == other.m_Conservative;
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x000402F8 File Offset: 0x0003E4F8
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is RasterState && this.Equals((RasterState)obj);
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x00040330 File Offset: 0x0003E530
		public override int GetHashCode()
		{
			int num = (int)this.m_CullingMode;
			num = (num * 397) ^ this.m_OffsetUnits;
			num = (num * 397) ^ this.m_OffsetFactor.GetHashCode();
			num = (num * 397) ^ this.m_DepthClip.GetHashCode();
			return (num * 397) ^ this.m_Conservative.GetHashCode();
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x00040398 File Offset: 0x0003E598
		public static bool operator ==(RasterState left, RasterState right)
		{
			return left.Equals(right);
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x000403B4 File Offset: 0x0003E5B4
		public static bool operator !=(RasterState left, RasterState right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000E45 RID: 3653
		public static readonly RasterState defaultValue = new RasterState(CullMode.Back, 0, 0f, true);

		// Token: 0x04000E46 RID: 3654
		private CullMode m_CullingMode;

		// Token: 0x04000E47 RID: 3655
		private int m_OffsetUnits;

		// Token: 0x04000E48 RID: 3656
		private float m_OffsetFactor;

		// Token: 0x04000E49 RID: 3657
		private byte m_DepthClip;

		// Token: 0x04000E4A RID: 3658
		private byte m_Conservative;

		// Token: 0x04000E4B RID: 3659
		private byte m_Padding1;

		// Token: 0x04000E4C RID: 3660
		private byte m_Padding2;
	}
}
