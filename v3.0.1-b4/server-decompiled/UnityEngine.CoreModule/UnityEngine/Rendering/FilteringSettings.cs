using System;
using UnityEngine.Internal;

namespace UnityEngine.Rendering
{
	// Token: 0x0200045B RID: 1115
	public struct FilteringSettings : IEquatable<FilteringSettings>
	{
		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06002569 RID: 9577 RVA: 0x0003FC5E File Offset: 0x0003DE5E
		public static FilteringSettings defaultValue
		{
			get
			{
				return new FilteringSettings(new RenderQueueRange?(RenderQueueRange.all), -1, uint.MaxValue, 0);
			}
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x0003FC74 File Offset: 0x0003DE74
		public FilteringSettings([DefaultValue("RenderQueueRange.all")] RenderQueueRange? renderQueueRange = null, int layerMask = -1, uint renderingLayerMask = 4294967295U, int excludeMotionVectorObjects = 0)
		{
			this = default(FilteringSettings);
			this.m_RenderQueueRange = renderQueueRange ?? RenderQueueRange.all;
			this.m_LayerMask = layerMask;
			this.m_RenderingLayerMask = renderingLayerMask;
			this.m_ExcludeMotionVectorObjects = excludeMotionVectorObjects;
			this.m_SortingLayerRange = SortingLayerRange.all;
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x0600256B RID: 9579 RVA: 0x0003FCCC File Offset: 0x0003DECC
		// (set) Token: 0x0600256C RID: 9580 RVA: 0x0003FCE4 File Offset: 0x0003DEE4
		public RenderQueueRange renderQueueRange
		{
			get
			{
				return this.m_RenderQueueRange;
			}
			set
			{
				this.m_RenderQueueRange = value;
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x0600256D RID: 9581 RVA: 0x0003FCF0 File Offset: 0x0003DEF0
		// (set) Token: 0x0600256E RID: 9582 RVA: 0x0003FD08 File Offset: 0x0003DF08
		public int layerMask
		{
			get
			{
				return this.m_LayerMask;
			}
			set
			{
				this.m_LayerMask = value;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x0600256F RID: 9583 RVA: 0x0003FD14 File Offset: 0x0003DF14
		// (set) Token: 0x06002570 RID: 9584 RVA: 0x0003FD2C File Offset: 0x0003DF2C
		public uint renderingLayerMask
		{
			get
			{
				return this.m_RenderingLayerMask;
			}
			set
			{
				this.m_RenderingLayerMask = value;
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06002571 RID: 9585 RVA: 0x0003FD38 File Offset: 0x0003DF38
		// (set) Token: 0x06002572 RID: 9586 RVA: 0x0003FD53 File Offset: 0x0003DF53
		public bool excludeMotionVectorObjects
		{
			get
			{
				return this.m_ExcludeMotionVectorObjects != 0;
			}
			set
			{
				this.m_ExcludeMotionVectorObjects = (value ? 1 : 0);
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06002573 RID: 9587 RVA: 0x0003FD64 File Offset: 0x0003DF64
		// (set) Token: 0x06002574 RID: 9588 RVA: 0x0003FD7C File Offset: 0x0003DF7C
		public SortingLayerRange sortingLayerRange
		{
			get
			{
				return this.m_SortingLayerRange;
			}
			set
			{
				this.m_SortingLayerRange = value;
			}
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x0003FD88 File Offset: 0x0003DF88
		public bool Equals(FilteringSettings other)
		{
			return this.m_RenderQueueRange.Equals(other.m_RenderQueueRange) && this.m_LayerMask == other.m_LayerMask && this.m_RenderingLayerMask == other.m_RenderingLayerMask && this.m_ExcludeMotionVectorObjects == other.m_ExcludeMotionVectorObjects;
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x0003FDDC File Offset: 0x0003DFDC
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is FilteringSettings && this.Equals((FilteringSettings)obj);
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x0003FE14 File Offset: 0x0003E014
		public override int GetHashCode()
		{
			int num = this.m_RenderQueueRange.GetHashCode();
			num = (num * 397) ^ this.m_LayerMask;
			num = (num * 397) ^ (int)this.m_RenderingLayerMask;
			return (num * 397) ^ this.m_ExcludeMotionVectorObjects;
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x0003FE68 File Offset: 0x0003E068
		public static bool operator ==(FilteringSettings left, FilteringSettings right)
		{
			return left.Equals(right);
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x0003FE84 File Offset: 0x0003E084
		public static bool operator !=(FilteringSettings left, FilteringSettings right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000E25 RID: 3621
		private RenderQueueRange m_RenderQueueRange;

		// Token: 0x04000E26 RID: 3622
		private int m_LayerMask;

		// Token: 0x04000E27 RID: 3623
		private uint m_RenderingLayerMask;

		// Token: 0x04000E28 RID: 3624
		private int m_ExcludeMotionVectorObjects;

		// Token: 0x04000E29 RID: 3625
		private SortingLayerRange m_SortingLayerRange;
	}
}
