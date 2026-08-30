using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000481 RID: 1153
	[UsedByNativeCode]
	public struct VisibleReflectionProbe : IEquatable<VisibleReflectionProbe>
	{
		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002782 RID: 10114 RVA: 0x00043D9D File Offset: 0x00041F9D
		public Texture texture
		{
			get
			{
				return (Texture)Object.FindObjectFromInstanceID(this.m_TextureId);
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002783 RID: 10115 RVA: 0x00043DAF File Offset: 0x00041FAF
		public ReflectionProbe reflectionProbe
		{
			get
			{
				return (ReflectionProbe)Object.FindObjectFromInstanceID(this.m_InstanceId);
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002784 RID: 10116 RVA: 0x00043DC4 File Offset: 0x00041FC4
		// (set) Token: 0x06002785 RID: 10117 RVA: 0x00043DDC File Offset: 0x00041FDC
		public Bounds bounds
		{
			get
			{
				return this.m_Bounds;
			}
			set
			{
				this.m_Bounds = value;
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002786 RID: 10118 RVA: 0x00043DE8 File Offset: 0x00041FE8
		// (set) Token: 0x06002787 RID: 10119 RVA: 0x00043E00 File Offset: 0x00042000
		public Matrix4x4 localToWorldMatrix
		{
			get
			{
				return this.m_LocalToWorldMatrix;
			}
			set
			{
				this.m_LocalToWorldMatrix = value;
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002788 RID: 10120 RVA: 0x00043E0C File Offset: 0x0004200C
		// (set) Token: 0x06002789 RID: 10121 RVA: 0x00043E24 File Offset: 0x00042024
		public Vector4 hdrData
		{
			get
			{
				return this.m_HdrData;
			}
			set
			{
				this.m_HdrData = value;
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x0600278A RID: 10122 RVA: 0x00043E30 File Offset: 0x00042030
		// (set) Token: 0x0600278B RID: 10123 RVA: 0x00043E48 File Offset: 0x00042048
		public Vector3 center
		{
			get
			{
				return this.m_Center;
			}
			set
			{
				this.m_Center = value;
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x0600278C RID: 10124 RVA: 0x00043E54 File Offset: 0x00042054
		// (set) Token: 0x0600278D RID: 10125 RVA: 0x00043E6C File Offset: 0x0004206C
		public float blendDistance
		{
			get
			{
				return this.m_BlendDistance;
			}
			set
			{
				this.m_BlendDistance = value;
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x0600278E RID: 10126 RVA: 0x00043E78 File Offset: 0x00042078
		// (set) Token: 0x0600278F RID: 10127 RVA: 0x00043E90 File Offset: 0x00042090
		public int importance
		{
			get
			{
				return this.m_Importance;
			}
			set
			{
				this.m_Importance = value;
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06002790 RID: 10128 RVA: 0x00043E9C File Offset: 0x0004209C
		// (set) Token: 0x06002791 RID: 10129 RVA: 0x00043EB9 File Offset: 0x000420B9
		public bool isBoxProjection
		{
			get
			{
				return Convert.ToBoolean(this.m_BoxProjection);
			}
			set
			{
				this.m_BoxProjection = Convert.ToInt32(value);
			}
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x00043EC8 File Offset: 0x000420C8
		public bool Equals(VisibleReflectionProbe other)
		{
			return this.m_Bounds.Equals(other.m_Bounds) && this.m_LocalToWorldMatrix.Equals(other.m_LocalToWorldMatrix) && this.m_HdrData.Equals(other.m_HdrData) && this.m_Center.Equals(other.m_Center) && this.m_BlendDistance.Equals(other.m_BlendDistance) && this.m_Importance == other.m_Importance && this.m_BoxProjection == other.m_BoxProjection && this.m_InstanceId == other.m_InstanceId && this.m_TextureId == other.m_TextureId;
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x00043F78 File Offset: 0x00042178
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is VisibleReflectionProbe && this.Equals((VisibleReflectionProbe)obj);
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x00043FB0 File Offset: 0x000421B0
		public override int GetHashCode()
		{
			int num = this.m_Bounds.GetHashCode();
			num = (num * 397) ^ this.m_LocalToWorldMatrix.GetHashCode();
			num = (num * 397) ^ this.m_HdrData.GetHashCode();
			num = (num * 397) ^ this.m_Center.GetHashCode();
			num = (num * 397) ^ this.m_BlendDistance.GetHashCode();
			num = (num * 397) ^ this.m_Importance;
			num = (num * 397) ^ this.m_BoxProjection;
			num = (num * 397) ^ this.m_InstanceId;
			return (num * 397) ^ this.m_TextureId;
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x00044074 File Offset: 0x00042274
		public static bool operator ==(VisibleReflectionProbe left, VisibleReflectionProbe right)
		{
			return left.Equals(right);
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x00044090 File Offset: 0x00042290
		public static bool operator !=(VisibleReflectionProbe left, VisibleReflectionProbe right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000F00 RID: 3840
		private Bounds m_Bounds;

		// Token: 0x04000F01 RID: 3841
		private Matrix4x4 m_LocalToWorldMatrix;

		// Token: 0x04000F02 RID: 3842
		private Vector4 m_HdrData;

		// Token: 0x04000F03 RID: 3843
		private Vector3 m_Center;

		// Token: 0x04000F04 RID: 3844
		private float m_BlendDistance;

		// Token: 0x04000F05 RID: 3845
		private int m_Importance;

		// Token: 0x04000F06 RID: 3846
		private int m_BoxProjection;

		// Token: 0x04000F07 RID: 3847
		private int m_InstanceId;

		// Token: 0x04000F08 RID: 3848
		private int m_TextureId;
	}
}
