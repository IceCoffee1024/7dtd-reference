using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200045D RID: 1117
	public struct LODParameters : IEquatable<LODParameters>
	{
		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x0600257A RID: 9594 RVA: 0x0003FEA4 File Offset: 0x0003E0A4
		// (set) Token: 0x0600257B RID: 9595 RVA: 0x0003FEC1 File Offset: 0x0003E0C1
		public bool isOrthographic
		{
			get
			{
				return Convert.ToBoolean(this.m_IsOrthographic);
			}
			set
			{
				this.m_IsOrthographic = Convert.ToInt32(value);
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x0600257C RID: 9596 RVA: 0x0003FED0 File Offset: 0x0003E0D0
		// (set) Token: 0x0600257D RID: 9597 RVA: 0x0003FEE8 File Offset: 0x0003E0E8
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

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x0600257E RID: 9598 RVA: 0x0003FEF4 File Offset: 0x0003E0F4
		// (set) Token: 0x0600257F RID: 9599 RVA: 0x0003FF0C File Offset: 0x0003E10C
		public float fieldOfView
		{
			get
			{
				return this.m_FieldOfView;
			}
			set
			{
				this.m_FieldOfView = value;
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06002580 RID: 9600 RVA: 0x0003FF18 File Offset: 0x0003E118
		// (set) Token: 0x06002581 RID: 9601 RVA: 0x0003FF30 File Offset: 0x0003E130
		public float orthoSize
		{
			get
			{
				return this.m_OrthoSize;
			}
			set
			{
				this.m_OrthoSize = value;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002582 RID: 9602 RVA: 0x0003FF3C File Offset: 0x0003E13C
		// (set) Token: 0x06002583 RID: 9603 RVA: 0x0003FF54 File Offset: 0x0003E154
		public int cameraPixelHeight
		{
			get
			{
				return this.m_CameraPixelHeight;
			}
			set
			{
				this.m_CameraPixelHeight = value;
			}
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x0003FF60 File Offset: 0x0003E160
		public bool Equals(LODParameters other)
		{
			return this.m_IsOrthographic == other.m_IsOrthographic && this.m_CameraPosition.Equals(other.m_CameraPosition) && this.m_FieldOfView.Equals(other.m_FieldOfView) && this.m_OrthoSize.Equals(other.m_OrthoSize) && this.m_CameraPixelHeight == other.m_CameraPixelHeight;
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x0003FFCC File Offset: 0x0003E1CC
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is LODParameters && this.Equals((LODParameters)obj);
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x00040004 File Offset: 0x0003E204
		public override int GetHashCode()
		{
			int num = this.m_IsOrthographic;
			num = (num * 397) ^ this.m_CameraPosition.GetHashCode();
			num = (num * 397) ^ this.m_FieldOfView.GetHashCode();
			num = (num * 397) ^ this.m_OrthoSize.GetHashCode();
			return (num * 397) ^ this.m_CameraPixelHeight;
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x00040070 File Offset: 0x0003E270
		public static bool operator ==(LODParameters left, LODParameters right)
		{
			return left.Equals(right);
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x0004008C File Offset: 0x0003E28C
		public static bool operator !=(LODParameters left, LODParameters right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000E2D RID: 3629
		private int m_IsOrthographic;

		// Token: 0x04000E2E RID: 3630
		private Vector3 m_CameraPosition;

		// Token: 0x04000E2F RID: 3631
		private float m_FieldOfView;

		// Token: 0x04000E30 RID: 3632
		private float m_OrthoSize;

		// Token: 0x04000E31 RID: 3633
		private int m_CameraPixelHeight;
	}
}
