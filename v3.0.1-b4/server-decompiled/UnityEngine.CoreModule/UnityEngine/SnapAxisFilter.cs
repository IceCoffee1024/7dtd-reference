using System;

namespace UnityEngine
{
	// Token: 0x02000282 RID: 642
	internal struct SnapAxisFilter : IEquatable<SnapAxisFilter>
	{
		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001ABB RID: 6843 RVA: 0x0002CDCC File Offset: 0x0002AFCC
		public float x
		{
			get
			{
				return ((this.m_Mask & SnapAxis.X) == SnapAxis.X) ? 1f : 0f;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001ABC RID: 6844 RVA: 0x0002CDF8 File Offset: 0x0002AFF8
		public float y
		{
			get
			{
				return ((this.m_Mask & SnapAxis.Y) == SnapAxis.Y) ? 1f : 0f;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001ABD RID: 6845 RVA: 0x0002CE24 File Offset: 0x0002B024
		public float z
		{
			get
			{
				return ((this.m_Mask & SnapAxis.Z) == SnapAxis.Z) ? 1f : 0f;
			}
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x0002CE50 File Offset: 0x0002B050
		public SnapAxisFilter(Vector3 v)
		{
			this.m_Mask = SnapAxis.None;
			float num = 1E-06f;
			bool flag = Mathf.Abs(v.x) > num;
			if (flag)
			{
				this.m_Mask |= SnapAxis.X;
			}
			bool flag2 = Mathf.Abs(v.y) > num;
			if (flag2)
			{
				this.m_Mask |= SnapAxis.Y;
			}
			bool flag3 = Mathf.Abs(v.z) > num;
			if (flag3)
			{
				this.m_Mask |= SnapAxis.Z;
			}
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x0002CECC File Offset: 0x0002B0CC
		public SnapAxisFilter(SnapAxis axis)
		{
			this.m_Mask = SnapAxis.None;
			bool flag = (axis & SnapAxis.X) == SnapAxis.X;
			if (flag)
			{
				this.m_Mask |= SnapAxis.X;
			}
			bool flag2 = (axis & SnapAxis.Y) == SnapAxis.Y;
			if (flag2)
			{
				this.m_Mask |= SnapAxis.Y;
			}
			bool flag3 = (axis & SnapAxis.Z) == SnapAxis.Z;
			if (flag3)
			{
				this.m_Mask |= SnapAxis.Z;
			}
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x0002CF2C File Offset: 0x0002B12C
		public override string ToString()
		{
			return string.Format("{{{0}, {1}, {2}}}", this.x, this.y, this.z);
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x0002CF6C File Offset: 0x0002B16C
		public int active
		{
			get
			{
				int num = 0;
				bool flag = (this.m_Mask & SnapAxis.X) > SnapAxis.None;
				if (flag)
				{
					num++;
				}
				bool flag2 = (this.m_Mask & SnapAxis.Y) > SnapAxis.None;
				if (flag2)
				{
					num++;
				}
				bool flag3 = (this.m_Mask & SnapAxis.Z) > SnapAxis.None;
				if (flag3)
				{
					num++;
				}
				return num;
			}
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x0002CFBC File Offset: 0x0002B1BC
		public static implicit operator Vector3(SnapAxisFilter mask)
		{
			return new Vector3(mask.x, mask.y, mask.z);
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x0002CFE8 File Offset: 0x0002B1E8
		public static explicit operator SnapAxisFilter(Vector3 v)
		{
			return new SnapAxisFilter(v);
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x0002D000 File Offset: 0x0002B200
		public static explicit operator SnapAxis(SnapAxisFilter mask)
		{
			return mask.m_Mask;
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x0002D018 File Offset: 0x0002B218
		public static SnapAxisFilter operator |(SnapAxisFilter left, SnapAxisFilter right)
		{
			return new SnapAxisFilter(left.m_Mask | right.m_Mask);
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x0002D03C File Offset: 0x0002B23C
		public static SnapAxisFilter operator &(SnapAxisFilter left, SnapAxisFilter right)
		{
			return new SnapAxisFilter(left.m_Mask & right.m_Mask);
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x0002D060 File Offset: 0x0002B260
		public static SnapAxisFilter operator ^(SnapAxisFilter left, SnapAxisFilter right)
		{
			return new SnapAxisFilter(left.m_Mask ^ right.m_Mask);
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x0002D084 File Offset: 0x0002B284
		public static SnapAxisFilter operator ~(SnapAxisFilter left)
		{
			return new SnapAxisFilter(~left.m_Mask);
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x0002D0A4 File Offset: 0x0002B2A4
		public static Vector3 operator *(SnapAxisFilter mask, float value)
		{
			return new Vector3(mask.x * value, mask.y * value, mask.z * value);
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x0002D0D8 File Offset: 0x0002B2D8
		public static Vector3 operator *(SnapAxisFilter mask, Vector3 right)
		{
			return new Vector3(mask.x * right.x, mask.y * right.y, mask.z * right.z);
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x0002D11C File Offset: 0x0002B31C
		public static Vector3 operator *(Quaternion rotation, SnapAxisFilter mask)
		{
			int active = mask.active;
			bool flag = active > 2;
			Vector3 vector;
			if (flag)
			{
				vector = mask;
			}
			else
			{
				Vector3 vector2 = rotation * mask;
				vector2 = new Vector3(Mathf.Abs(vector2.x), Mathf.Abs(vector2.y), Mathf.Abs(vector2.z));
				bool flag2 = active > 1;
				if (flag2)
				{
					vector = new Vector3((float)((vector2.x > vector2.y || vector2.x > vector2.z) ? 1 : 0), (float)((vector2.y > vector2.x || vector2.y > vector2.z) ? 1 : 0), (float)((vector2.z > vector2.x || vector2.z > vector2.y) ? 1 : 0));
				}
				else
				{
					vector = new Vector3((float)((vector2.x > vector2.y && vector2.x > vector2.z) ? 1 : 0), (float)((vector2.y > vector2.z && vector2.y > vector2.x) ? 1 : 0), (float)((vector2.z > vector2.x && vector2.z > vector2.y) ? 1 : 0));
				}
			}
			return vector;
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x0002D260 File Offset: 0x0002B460
		public static bool operator ==(SnapAxisFilter left, SnapAxisFilter right)
		{
			return left.m_Mask == right.m_Mask;
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x0002D280 File Offset: 0x0002B480
		public static bool operator !=(SnapAxisFilter left, SnapAxisFilter right)
		{
			return !(left == right);
		}

		// Token: 0x170004F7 RID: 1271
		public float this[int i]
		{
			get
			{
				bool flag = i < 0 || i > 2;
				if (flag)
				{
					throw new IndexOutOfRangeException();
				}
				return (float)(SnapAxis.X & (this.m_Mask >> (i & 31))) * 1f;
			}
			set
			{
				bool flag = i < 0 || i > 2;
				if (flag)
				{
					throw new IndexOutOfRangeException();
				}
				this.m_Mask &= (SnapAxis)(~(SnapAxis)(1 << i));
				this.m_Mask |= (SnapAxis)(((value > 0f) ? 1 : 0) << (i & 31));
			}
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x0002D330 File Offset: 0x0002B530
		public bool Equals(SnapAxisFilter other)
		{
			return this.m_Mask == other.m_Mask;
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x0002D350 File Offset: 0x0002B550
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is SnapAxisFilter && this.Equals((SnapAxisFilter)obj);
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x0002D388 File Offset: 0x0002B588
		public override int GetHashCode()
		{
			return this.m_Mask.GetHashCode();
		}

		// Token: 0x0400092E RID: 2350
		private const SnapAxis X = SnapAxis.X;

		// Token: 0x0400092F RID: 2351
		private const SnapAxis Y = SnapAxis.Y;

		// Token: 0x04000930 RID: 2352
		private const SnapAxis Z = SnapAxis.Z;

		// Token: 0x04000931 RID: 2353
		public static readonly SnapAxisFilter all = new SnapAxisFilter(SnapAxis.All);

		// Token: 0x04000932 RID: 2354
		private SnapAxis m_Mask;
	}
}
