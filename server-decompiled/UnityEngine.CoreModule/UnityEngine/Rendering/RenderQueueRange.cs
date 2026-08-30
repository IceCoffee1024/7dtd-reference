using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200046B RID: 1131
	public struct RenderQueueRange : IEquatable<RenderQueueRange>
	{
		// Token: 0x0600260C RID: 9740 RVA: 0x00041290 File Offset: 0x0003F490
		public RenderQueueRange(int lowerBound, int upperBound)
		{
			bool flag = lowerBound < 0 || lowerBound > 5000;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("lowerBound", lowerBound, string.Format("The lower bound must be at least {0} and at most {1}.", 0, 5000));
			}
			bool flag2 = upperBound < 0 || upperBound > 5000;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("upperBound", upperBound, string.Format("The upper bound must be at least {0} and at most {1}.", 0, 5000));
			}
			this.m_LowerBound = lowerBound;
			this.m_UpperBound = upperBound;
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x0600260D RID: 9741 RVA: 0x00041328 File Offset: 0x0003F528
		public static RenderQueueRange all
		{
			get
			{
				return new RenderQueueRange
				{
					m_LowerBound = 0,
					m_UpperBound = 5000
				};
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x0600260E RID: 9742 RVA: 0x00041354 File Offset: 0x0003F554
		public static RenderQueueRange opaque
		{
			get
			{
				return new RenderQueueRange
				{
					m_LowerBound = 0,
					m_UpperBound = 2500
				};
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x0600260F RID: 9743 RVA: 0x00041380 File Offset: 0x0003F580
		public static RenderQueueRange transparent
		{
			get
			{
				return new RenderQueueRange
				{
					m_LowerBound = 2501,
					m_UpperBound = 5000
				};
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06002610 RID: 9744 RVA: 0x000413B0 File Offset: 0x0003F5B0
		// (set) Token: 0x06002611 RID: 9745 RVA: 0x000413C8 File Offset: 0x0003F5C8
		public int lowerBound
		{
			get
			{
				return this.m_LowerBound;
			}
			set
			{
				bool flag = value < 0 || value > 5000;
				if (flag)
				{
					throw new ArgumentOutOfRangeException(string.Format("The lower bound must be at least {0} and at most {1}.", 0, 5000));
				}
				this.m_LowerBound = value;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06002612 RID: 9746 RVA: 0x00041410 File Offset: 0x0003F610
		// (set) Token: 0x06002613 RID: 9747 RVA: 0x00041428 File Offset: 0x0003F628
		public int upperBound
		{
			get
			{
				return this.m_UpperBound;
			}
			set
			{
				bool flag = value < 0 || value > 5000;
				if (flag)
				{
					throw new ArgumentOutOfRangeException(string.Format("The upper bound must be at least {0} and at most {1}.", 0, 5000));
				}
				this.m_UpperBound = value;
			}
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x00041470 File Offset: 0x0003F670
		public bool Equals(RenderQueueRange other)
		{
			return this.m_LowerBound == other.m_LowerBound && this.m_UpperBound == other.m_UpperBound;
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x000414A4 File Offset: 0x0003F6A4
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is RenderQueueRange && this.Equals((RenderQueueRange)obj);
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x000414DC File Offset: 0x0003F6DC
		public override int GetHashCode()
		{
			return (this.m_LowerBound * 397) ^ this.m_UpperBound;
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x00041504 File Offset: 0x0003F704
		public static bool operator ==(RenderQueueRange left, RenderQueueRange right)
		{
			return left.Equals(right);
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x00041520 File Offset: 0x0003F720
		public static bool operator !=(RenderQueueRange left, RenderQueueRange right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000E78 RID: 3704
		private int m_LowerBound;

		// Token: 0x04000E79 RID: 3705
		private int m_UpperBound;

		// Token: 0x04000E7A RID: 3706
		private const int k_MinimumBound = 0;

		// Token: 0x04000E7B RID: 3707
		public static readonly int minimumBound = 0;

		// Token: 0x04000E7C RID: 3708
		private const int k_MaximumBound = 5000;

		// Token: 0x04000E7D RID: 3709
		public static readonly int maximumBound = 5000;
	}
}
