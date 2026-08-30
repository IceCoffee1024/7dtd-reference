using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200047A RID: 1146
	public struct SortingSettings : IEquatable<SortingSettings>
	{
		// Token: 0x060026E8 RID: 9960 RVA: 0x00042CE4 File Offset: 0x00040EE4
		public SortingSettings(Camera camera)
		{
			ScriptableRenderContext.InitializeSortSettings(camera, out this);
			this.m_Criteria = this.criteria;
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x060026E9 RID: 9961 RVA: 0x00042CFC File Offset: 0x00040EFC
		// (set) Token: 0x060026EA RID: 9962 RVA: 0x00042D14 File Offset: 0x00040F14
		public Matrix4x4 worldToCameraMatrix
		{
			get
			{
				return this.m_WorldToCameraMatrix;
			}
			set
			{
				this.m_WorldToCameraMatrix = value;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x060026EB RID: 9963 RVA: 0x00042D20 File Offset: 0x00040F20
		// (set) Token: 0x060026EC RID: 9964 RVA: 0x00042D38 File Offset: 0x00040F38
		public Vector3 cameraPosition
		{
			get
			{
				return this.m_CameraPosition;
			}
			set
			{
				this.m_CameraPosition = value;
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x060026ED RID: 9965 RVA: 0x00042D44 File Offset: 0x00040F44
		// (set) Token: 0x060026EE RID: 9966 RVA: 0x00042D5C File Offset: 0x00040F5C
		public Vector3 customAxis
		{
			get
			{
				return this.m_CustomAxis;
			}
			set
			{
				this.m_CustomAxis = value;
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x060026EF RID: 9967 RVA: 0x00042D68 File Offset: 0x00040F68
		// (set) Token: 0x060026F0 RID: 9968 RVA: 0x00042D80 File Offset: 0x00040F80
		public SortingCriteria criteria
		{
			get
			{
				return this.m_Criteria;
			}
			set
			{
				this.m_Criteria = value;
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x060026F1 RID: 9969 RVA: 0x00042D8C File Offset: 0x00040F8C
		// (set) Token: 0x060026F2 RID: 9970 RVA: 0x00042DA4 File Offset: 0x00040FA4
		public DistanceMetric distanceMetric
		{
			get
			{
				return this.m_DistanceMetric;
			}
			set
			{
				this.m_DistanceMetric = value;
			}
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x00042DB0 File Offset: 0x00040FB0
		public bool Equals(SortingSettings other)
		{
			return this.m_WorldToCameraMatrix.Equals(other.m_WorldToCameraMatrix) && this.m_CameraPosition.Equals(other.m_CameraPosition) && this.m_CustomAxis.Equals(other.m_CustomAxis) && this.m_Criteria == other.m_Criteria && this.m_DistanceMetric == other.m_DistanceMetric;
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x00042E1C File Offset: 0x0004101C
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is SortingSettings && this.Equals((SortingSettings)obj);
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x00042E54 File Offset: 0x00041054
		public override int GetHashCode()
		{
			int num = this.m_WorldToCameraMatrix.GetHashCode();
			num = (num * 397) ^ this.m_CameraPosition.GetHashCode();
			num = (num * 397) ^ this.m_CustomAxis.GetHashCode();
			num = (num * 397) ^ (int)this.m_Criteria;
			return (num * 397) ^ (int)this.m_DistanceMetric;
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x00042ECC File Offset: 0x000410CC
		public static bool operator ==(SortingSettings left, SortingSettings right)
		{
			return left.Equals(right);
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x00042EE8 File Offset: 0x000410E8
		public static bool operator !=(SortingSettings left, SortingSettings right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000EBD RID: 3773
		private Matrix4x4 m_WorldToCameraMatrix;

		// Token: 0x04000EBE RID: 3774
		private Vector3 m_CameraPosition;

		// Token: 0x04000EBF RID: 3775
		private Vector3 m_CustomAxis;

		// Token: 0x04000EC0 RID: 3776
		private SortingCriteria m_Criteria;

		// Token: 0x04000EC1 RID: 3777
		private DistanceMetric m_DistanceMetric;
	}
}
