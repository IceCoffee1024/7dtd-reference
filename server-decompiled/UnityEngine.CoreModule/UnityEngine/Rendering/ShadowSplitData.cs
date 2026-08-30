using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000475 RID: 1141
	[UsedByNativeCode]
	public struct ShadowSplitData : IEquatable<ShadowSplitData>
	{
		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x060026CB RID: 9931 RVA: 0x00042830 File Offset: 0x00040A30
		// (set) Token: 0x060026CC RID: 9932 RVA: 0x00042848 File Offset: 0x00040A48
		public int cullingPlaneCount
		{
			get
			{
				return this.m_CullingPlaneCount;
			}
			set
			{
				bool flag = value < 0 || value > 10;
				if (flag)
				{
					throw new ArgumentException(string.Format("Value should range from {0} to ShadowSplitData.maximumCullingPlaneCount ({1}), but was {2}.", 0, 10, value));
				}
				this.m_CullingPlaneCount = value;
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x060026CD RID: 9933 RVA: 0x00042890 File Offset: 0x00040A90
		// (set) Token: 0x060026CE RID: 9934 RVA: 0x000428A8 File Offset: 0x00040AA8
		public Vector4 cullingSphere
		{
			get
			{
				return this.m_CullingSphere;
			}
			set
			{
				this.m_CullingSphere = value;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x060026CF RID: 9935 RVA: 0x000428B4 File Offset: 0x00040AB4
		// (set) Token: 0x060026D0 RID: 9936 RVA: 0x000428CC File Offset: 0x00040ACC
		public Matrix4x4 cullingMatrix
		{
			get
			{
				return this.m_CullingMatrix;
			}
			set
			{
				this.m_CullingMatrix = value;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x060026D1 RID: 9937 RVA: 0x000428D8 File Offset: 0x00040AD8
		// (set) Token: 0x060026D2 RID: 9938 RVA: 0x000428F0 File Offset: 0x00040AF0
		public float cullingNearPlane
		{
			get
			{
				return this.m_CullingNearPlane;
			}
			set
			{
				this.m_CullingNearPlane = value;
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x060026D3 RID: 9939 RVA: 0x000428FC File Offset: 0x00040AFC
		// (set) Token: 0x060026D4 RID: 9940 RVA: 0x00042914 File Offset: 0x00040B14
		public float shadowCascadeBlendCullingFactor
		{
			get
			{
				return this.m_ShadowCascadeBlendCullingFactor;
			}
			set
			{
				bool flag = value < 0f || value > 1f;
				if (flag)
				{
					throw new ArgumentException(string.Format("Value should range from {0} to {1}, but was {2}.", 0, 1, value));
				}
				this.m_ShadowCascadeBlendCullingFactor = value;
			}
		}

		// Token: 0x060026D5 RID: 9941 RVA: 0x00042964 File Offset: 0x00040B64
		public unsafe Plane GetCullingPlane(int index)
		{
			bool flag = index < 0 || index >= this.cullingPlaneCount;
			if (flag)
			{
				throw new ArgumentException("index", string.Format("Index should be at least {0} and less than cullingPlaneCount ({1}), but was {2}.", 0, this.cullingPlaneCount, index));
			}
			fixed (byte* ptr = &this.m_CullingPlanes.FixedElementField)
			{
				byte* ptr2 = ptr;
				Plane* ptr3 = (Plane*)ptr2;
				return ptr3[index];
			}
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x000429E0 File Offset: 0x00040BE0
		public unsafe void SetCullingPlane(int index, Plane plane)
		{
			bool flag = index < 0 || index >= this.cullingPlaneCount;
			if (flag)
			{
				throw new ArgumentException("index", string.Format("Index should be at least {0} and less than cullingPlaneCount ({1}), but was {2}.", 0, this.cullingPlaneCount, index));
			}
			fixed (byte* ptr = &this.m_CullingPlanes.FixedElementField)
			{
				byte* ptr2 = ptr;
				Plane* ptr3 = (Plane*)ptr2;
				ptr3[index] = plane;
			}
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x00042A58 File Offset: 0x00040C58
		public bool Equals(ShadowSplitData other)
		{
			bool flag = this.m_CullingPlaneCount != other.m_CullingPlaneCount;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				for (int i = 0; i < this.cullingPlaneCount; i++)
				{
					bool flag3 = !this.GetCullingPlane(i).Equals(other.GetCullingPlane(i));
					if (flag3)
					{
						return false;
					}
				}
				flag2 = this.m_CullingSphere.Equals(other.m_CullingSphere);
			}
			return flag2;
		}

		// Token: 0x060026D8 RID: 9944 RVA: 0x00042AE0 File Offset: 0x00040CE0
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is ShadowSplitData && this.Equals((ShadowSplitData)obj);
		}

		// Token: 0x060026D9 RID: 9945 RVA: 0x00042B18 File Offset: 0x00040D18
		public override int GetHashCode()
		{
			return (this.m_CullingPlaneCount * 397) ^ this.m_CullingSphere.GetHashCode();
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x00042B4C File Offset: 0x00040D4C
		public static bool operator ==(ShadowSplitData left, ShadowSplitData right)
		{
			return left.Equals(right);
		}

		// Token: 0x060026DB RID: 9947 RVA: 0x00042B68 File Offset: 0x00040D68
		public static bool operator !=(ShadowSplitData left, ShadowSplitData right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000EA3 RID: 3747
		private const int k_MaximumCullingPlaneCount = 10;

		// Token: 0x04000EA4 RID: 3748
		public static readonly int maximumCullingPlaneCount = 10;

		// Token: 0x04000EA5 RID: 3749
		private int m_CullingPlaneCount;

		// Token: 0x04000EA6 RID: 3750
		[FixedBuffer(typeof(byte), 160)]
		internal ShadowSplitData.<m_CullingPlanes>e__FixedBuffer m_CullingPlanes;

		// Token: 0x04000EA7 RID: 3751
		private Vector4 m_CullingSphere;

		// Token: 0x04000EA8 RID: 3752
		private float m_ShadowCascadeBlendCullingFactor;

		// Token: 0x04000EA9 RID: 3753
		private float m_CullingNearPlane;

		// Token: 0x04000EAA RID: 3754
		private Matrix4x4 m_CullingMatrix;

		// Token: 0x02000476 RID: 1142
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 160)]
		public struct <m_CullingPlanes>e__FixedBuffer
		{
			// Token: 0x04000EAB RID: 3755
			public byte FixedElementField;
		}
	}
}
