using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001F7 RID: 503
	[UsedByNativeCode]
	[Il2CppEagerStaticClassConstruction]
	public struct Vector3Int : IEquatable<Vector3Int>, IFormattable
	{
		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x060016B9 RID: 5817 RVA: 0x00025158 File Offset: 0x00023358
		// (set) Token: 0x060016BA RID: 5818 RVA: 0x00025170 File Offset: 0x00023370
		public int x
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_X;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_X = value;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060016BB RID: 5819 RVA: 0x0002517C File Offset: 0x0002337C
		// (set) Token: 0x060016BC RID: 5820 RVA: 0x00025194 File Offset: 0x00023394
		public int y
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Y;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Y = value;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x060016BD RID: 5821 RVA: 0x000251A0 File Offset: 0x000233A0
		// (set) Token: 0x060016BE RID: 5822 RVA: 0x000251B8 File Offset: 0x000233B8
		public int z
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Z;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Z = value;
			}
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x000251C2 File Offset: 0x000233C2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3Int(int x, int y)
		{
			this.m_X = x;
			this.m_Y = y;
			this.m_Z = 0;
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x000251DA File Offset: 0x000233DA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3Int(int x, int y, int z)
		{
			this.m_X = x;
			this.m_Y = y;
			this.m_Z = z;
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x000251DA File Offset: 0x000233DA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Set(int x, int y, int z)
		{
			this.m_X = x;
			this.m_Y = y;
			this.m_Z = z;
		}

		// Token: 0x17000489 RID: 1161
		public int this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				int num;
				switch (index)
				{
				case 0:
					num = this.x;
					break;
				case 1:
					num = this.y;
					break;
				case 2:
					num = this.z;
					break;
				default:
					throw new IndexOutOfRangeException(UnityString.Format("Invalid Vector3Int index addressed: {0}!", new object[] { index }));
				}
				return num;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				switch (index)
				{
				case 0:
					this.x = value;
					break;
				case 1:
					this.y = value;
					break;
				case 2:
					this.z = value;
					break;
				default:
					throw new IndexOutOfRangeException(UnityString.Format("Invalid Vector3Int index addressed: {0}!", new object[] { index }));
				}
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x060016C4 RID: 5828 RVA: 0x000252BC File Offset: 0x000234BC
		public float magnitude
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.y * this.y + this.z * this.z));
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x060016C5 RID: 5829 RVA: 0x00025300 File Offset: 0x00023500
		public int sqrMagnitude
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.x * this.x + this.y * this.y + this.z * this.z;
			}
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x0002533C File Offset: 0x0002353C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Distance(Vector3Int a, Vector3Int b)
		{
			return (a - b).magnitude;
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x00025360 File Offset: 0x00023560
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int Min(Vector3Int lhs, Vector3Int rhs)
		{
			return new Vector3Int(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y), Mathf.Min(lhs.z, rhs.z));
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x000253B0 File Offset: 0x000235B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int Max(Vector3Int lhs, Vector3Int rhs)
		{
			return new Vector3Int(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y), Mathf.Max(lhs.z, rhs.z));
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x00025400 File Offset: 0x00023600
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int Scale(Vector3Int a, Vector3Int b)
		{
			return new Vector3Int(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x00025444 File Offset: 0x00023644
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Scale(Vector3Int scale)
		{
			this.x *= scale.x;
			this.y *= scale.y;
			this.z *= scale.z;
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x00025494 File Offset: 0x00023694
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clamp(Vector3Int min, Vector3Int max)
		{
			this.x = Math.Max(min.x, this.x);
			this.x = Math.Min(max.x, this.x);
			this.y = Math.Max(min.y, this.y);
			this.y = Math.Min(max.y, this.y);
			this.z = Math.Max(min.z, this.z);
			this.z = Math.Min(max.z, this.z);
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x00025538 File Offset: 0x00023738
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3(Vector3Int v)
		{
			return new Vector3((float)v.x, (float)v.y, (float)v.z);
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x00025568 File Offset: 0x00023768
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector2Int(Vector3Int v)
		{
			return new Vector2Int(v.x, v.y);
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x00025590 File Offset: 0x00023790
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int FloorToInt(Vector3 v)
		{
			return new Vector3Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y), Mathf.FloorToInt(v.z));
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x000255C8 File Offset: 0x000237C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int CeilToInt(Vector3 v)
		{
			return new Vector3Int(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y), Mathf.CeilToInt(v.z));
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00025600 File Offset: 0x00023800
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int RoundToInt(Vector3 v)
		{
			return new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x00025638 File Offset: 0x00023838
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int operator +(Vector3Int a, Vector3Int b)
		{
			return new Vector3Int(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x0002567C File Offset: 0x0002387C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int operator -(Vector3Int a, Vector3Int b)
		{
			return new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x000256C0 File Offset: 0x000238C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int operator *(Vector3Int a, Vector3Int b)
		{
			return new Vector3Int(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x00025704 File Offset: 0x00023904
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int operator -(Vector3Int a)
		{
			return new Vector3Int(-a.x, -a.y, -a.z);
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x00025734 File Offset: 0x00023934
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int operator *(Vector3Int a, int b)
		{
			return new Vector3Int(a.x * b, a.y * b, a.z * b);
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x00025768 File Offset: 0x00023968
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int operator *(int a, Vector3Int b)
		{
			return new Vector3Int(a * b.x, a * b.y, a * b.z);
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x0002579C File Offset: 0x0002399C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int operator /(Vector3Int a, int b)
		{
			return new Vector3Int(a.x / b, a.y / b, a.z / b);
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x000257D0 File Offset: 0x000239D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector3Int lhs, Vector3Int rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x00025818 File Offset: 0x00023A18
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector3Int lhs, Vector3Int rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x00025834 File Offset: 0x00023A34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object other)
		{
			bool flag = !(other is Vector3Int);
			return !flag && this.Equals((Vector3Int)other);
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x00025868 File Offset: 0x00023A68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector3Int other)
		{
			return this == other;
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x00025888 File Offset: 0x00023A88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			int hashCode = this.y.GetHashCode();
			int hashCode2 = this.z.GetHashCode();
			return this.x.GetHashCode() ^ (hashCode << 4) ^ (hashCode >> 28) ^ (hashCode2 >> 4) ^ (hashCode2 << 28);
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x000258D8 File Offset: 0x00023AD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x000258F4 File Offset: 0x00023AF4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x00025910 File Offset: 0x00023B10
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			bool flag = formatProvider == null;
			if (flag)
			{
				formatProvider = CultureInfo.InvariantCulture.NumberFormat;
			}
			return UnityString.Format("({0}, {1}, {2})", new object[]
			{
				this.x.ToString(format, formatProvider),
				this.y.ToString(format, formatProvider),
				this.z.ToString(format, formatProvider)
			});
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x060016E0 RID: 5856 RVA: 0x00025980 File Offset: 0x00023B80
		public static Vector3Int zero
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector3Int.s_Zero;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x060016E1 RID: 5857 RVA: 0x00025998 File Offset: 0x00023B98
		public static Vector3Int one
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector3Int.s_One;
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x060016E2 RID: 5858 RVA: 0x000259B0 File Offset: 0x00023BB0
		public static Vector3Int up
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector3Int.s_Up;
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x060016E3 RID: 5859 RVA: 0x000259C8 File Offset: 0x00023BC8
		public static Vector3Int down
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector3Int.s_Down;
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x060016E4 RID: 5860 RVA: 0x000259E0 File Offset: 0x00023BE0
		public static Vector3Int left
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector3Int.s_Left;
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x060016E5 RID: 5861 RVA: 0x000259F8 File Offset: 0x00023BF8
		public static Vector3Int right
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector3Int.s_Right;
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x060016E6 RID: 5862 RVA: 0x00025A10 File Offset: 0x00023C10
		public static Vector3Int forward
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector3Int.s_Forward;
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x060016E7 RID: 5863 RVA: 0x00025A28 File Offset: 0x00023C28
		public static Vector3Int back
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector3Int.s_Back;
			}
		}

		// Token: 0x04000836 RID: 2102
		private int m_X;

		// Token: 0x04000837 RID: 2103
		private int m_Y;

		// Token: 0x04000838 RID: 2104
		private int m_Z;

		// Token: 0x04000839 RID: 2105
		private static readonly Vector3Int s_Zero = new Vector3Int(0, 0, 0);

		// Token: 0x0400083A RID: 2106
		private static readonly Vector3Int s_One = new Vector3Int(1, 1, 1);

		// Token: 0x0400083B RID: 2107
		private static readonly Vector3Int s_Up = new Vector3Int(0, 1, 0);

		// Token: 0x0400083C RID: 2108
		private static readonly Vector3Int s_Down = new Vector3Int(0, -1, 0);

		// Token: 0x0400083D RID: 2109
		private static readonly Vector3Int s_Left = new Vector3Int(-1, 0, 0);

		// Token: 0x0400083E RID: 2110
		private static readonly Vector3Int s_Right = new Vector3Int(1, 0, 0);

		// Token: 0x0400083F RID: 2111
		private static readonly Vector3Int s_Forward = new Vector3Int(0, 0, 1);

		// Token: 0x04000840 RID: 2112
		private static readonly Vector3Int s_Back = new Vector3Int(0, 0, -1);
	}
}
