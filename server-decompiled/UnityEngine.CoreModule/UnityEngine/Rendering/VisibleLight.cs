using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x0200047F RID: 1151
	[UsedByNativeCode]
	public struct VisibleLight : IEquatable<VisibleLight>
	{
		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x0600276C RID: 10092 RVA: 0x00043A4A File Offset: 0x00041C4A
		public Light light
		{
			get
			{
				return (Light)Object.FindObjectFromInstanceID(this.m_InstanceId);
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x0600276D RID: 10093 RVA: 0x00043A5C File Offset: 0x00041C5C
		// (set) Token: 0x0600276E RID: 10094 RVA: 0x00043A74 File Offset: 0x00041C74
		public LightType lightType
		{
			get
			{
				return this.m_LightType;
			}
			set
			{
				this.m_LightType = value;
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x0600276F RID: 10095 RVA: 0x00043A80 File Offset: 0x00041C80
		// (set) Token: 0x06002770 RID: 10096 RVA: 0x00043A98 File Offset: 0x00041C98
		public Color finalColor
		{
			get
			{
				return this.m_FinalColor;
			}
			set
			{
				this.m_FinalColor = value;
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06002771 RID: 10097 RVA: 0x00043AA4 File Offset: 0x00041CA4
		// (set) Token: 0x06002772 RID: 10098 RVA: 0x00043ABC File Offset: 0x00041CBC
		public Rect screenRect
		{
			get
			{
				return this.m_ScreenRect;
			}
			set
			{
				this.m_ScreenRect = value;
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06002773 RID: 10099 RVA: 0x00043AC8 File Offset: 0x00041CC8
		// (set) Token: 0x06002774 RID: 10100 RVA: 0x00043AE0 File Offset: 0x00041CE0
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

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06002775 RID: 10101 RVA: 0x00043AEC File Offset: 0x00041CEC
		// (set) Token: 0x06002776 RID: 10102 RVA: 0x00043B04 File Offset: 0x00041D04
		public float range
		{
			get
			{
				return this.m_Range;
			}
			set
			{
				this.m_Range = value;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06002777 RID: 10103 RVA: 0x00043B10 File Offset: 0x00041D10
		// (set) Token: 0x06002778 RID: 10104 RVA: 0x00043B28 File Offset: 0x00041D28
		public float spotAngle
		{
			get
			{
				return this.m_SpotAngle;
			}
			set
			{
				this.m_SpotAngle = value;
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06002779 RID: 10105 RVA: 0x00043B34 File Offset: 0x00041D34
		// (set) Token: 0x0600277A RID: 10106 RVA: 0x00043B54 File Offset: 0x00041D54
		public bool intersectsNearPlane
		{
			get
			{
				return (this.m_Flags & VisibleLightFlags.IntersectsNearPlane) > (VisibleLightFlags)0;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= VisibleLightFlags.IntersectsNearPlane;
				}
				else
				{
					this.m_Flags &= ~VisibleLightFlags.IntersectsNearPlane;
				}
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x0600277B RID: 10107 RVA: 0x00043B88 File Offset: 0x00041D88
		// (set) Token: 0x0600277C RID: 10108 RVA: 0x00043BA8 File Offset: 0x00041DA8
		public bool intersectsFarPlane
		{
			get
			{
				return (this.m_Flags & VisibleLightFlags.IntersectsFarPlane) > (VisibleLightFlags)0;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= VisibleLightFlags.IntersectsFarPlane;
				}
				else
				{
					this.m_Flags &= ~VisibleLightFlags.IntersectsFarPlane;
				}
			}
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x00043BDC File Offset: 0x00041DDC
		public bool Equals(VisibleLight other)
		{
			return this.m_LightType == other.m_LightType && this.m_FinalColor.Equals(other.m_FinalColor) && this.m_ScreenRect.Equals(other.m_ScreenRect) && this.m_LocalToWorldMatrix.Equals(other.m_LocalToWorldMatrix) && this.m_Range.Equals(other.m_Range) && this.m_SpotAngle.Equals(other.m_SpotAngle) && this.m_InstanceId == other.m_InstanceId && this.m_Flags == other.m_Flags;
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x00043C7C File Offset: 0x00041E7C
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is VisibleLight && this.Equals((VisibleLight)obj);
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x00043CB4 File Offset: 0x00041EB4
		public override int GetHashCode()
		{
			int num = (int)this.m_LightType;
			num = (num * 397) ^ this.m_FinalColor.GetHashCode();
			num = (num * 397) ^ this.m_ScreenRect.GetHashCode();
			num = (num * 397) ^ this.m_LocalToWorldMatrix.GetHashCode();
			num = (num * 397) ^ this.m_Range.GetHashCode();
			num = (num * 397) ^ this.m_SpotAngle.GetHashCode();
			num = (num * 397) ^ this.m_InstanceId;
			return (num * 397) ^ (int)this.m_Flags;
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x00043D64 File Offset: 0x00041F64
		public static bool operator ==(VisibleLight left, VisibleLight right)
		{
			return left.Equals(right);
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x00043D80 File Offset: 0x00041F80
		public static bool operator !=(VisibleLight left, VisibleLight right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000EF5 RID: 3829
		private LightType m_LightType;

		// Token: 0x04000EF6 RID: 3830
		private Color m_FinalColor;

		// Token: 0x04000EF7 RID: 3831
		private Rect m_ScreenRect;

		// Token: 0x04000EF8 RID: 3832
		private Matrix4x4 m_LocalToWorldMatrix;

		// Token: 0x04000EF9 RID: 3833
		private float m_Range;

		// Token: 0x04000EFA RID: 3834
		private float m_SpotAngle;

		// Token: 0x04000EFB RID: 3835
		private int m_InstanceId;

		// Token: 0x04000EFC RID: 3836
		private VisibleLightFlags m_Flags;
	}
}
