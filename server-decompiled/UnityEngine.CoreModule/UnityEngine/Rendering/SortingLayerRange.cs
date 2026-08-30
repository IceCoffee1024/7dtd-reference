using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000478 RID: 1144
	public struct SortingLayerRange : IEquatable<SortingLayerRange>
	{
		// Token: 0x060026DD RID: 9949 RVA: 0x00042B8E File Offset: 0x00040D8E
		public SortingLayerRange(short lowerBound, short upperBound)
		{
			this.m_LowerBound = lowerBound;
			this.m_UpperBound = upperBound;
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x060026DE RID: 9950 RVA: 0x00042BA0 File Offset: 0x00040DA0
		// (set) Token: 0x060026DF RID: 9951 RVA: 0x00042BB8 File Offset: 0x00040DB8
		public short lowerBound
		{
			get
			{
				return this.m_LowerBound;
			}
			set
			{
				this.m_LowerBound = value;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x060026E0 RID: 9952 RVA: 0x00042BC4 File Offset: 0x00040DC4
		// (set) Token: 0x060026E1 RID: 9953 RVA: 0x00042BDC File Offset: 0x00040DDC
		public short upperBound
		{
			get
			{
				return this.m_UpperBound;
			}
			set
			{
				this.m_UpperBound = value;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060026E2 RID: 9954 RVA: 0x00042BE8 File Offset: 0x00040DE8
		public static SortingLayerRange all
		{
			get
			{
				return new SortingLayerRange
				{
					m_LowerBound = short.MinValue,
					m_UpperBound = short.MaxValue
				};
			}
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x00042C18 File Offset: 0x00040E18
		public bool Equals(SortingLayerRange other)
		{
			return this.m_LowerBound == other.m_LowerBound && this.m_UpperBound == other.m_UpperBound;
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x00042C4C File Offset: 0x00040E4C
		public override bool Equals(object obj)
		{
			bool flag = !(obj is SortingLayerRange);
			return !flag && this.Equals((SortingLayerRange)obj);
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x00042C80 File Offset: 0x00040E80
		public static bool operator !=(SortingLayerRange lhs, SortingLayerRange rhs)
		{
			return !lhs.Equals(rhs);
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x00042CA0 File Offset: 0x00040EA0
		public static bool operator ==(SortingLayerRange lhs, SortingLayerRange rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x00042CBC File Offset: 0x00040EBC
		public override int GetHashCode()
		{
			return ((int)this.m_UpperBound << 16) | ((int)this.m_LowerBound & 65535);
		}

		// Token: 0x04000EB7 RID: 3767
		private short m_LowerBound;

		// Token: 0x04000EB8 RID: 3768
		private short m_UpperBound;
	}
}
