using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001F6 RID: 502
	[Il2CppEagerStaticClassConstruction]
	[NativeType("Runtime/Math/Vector2Int.h")]
	[UsedByNativeCode]
	public struct Vector2Int : IEquatable<Vector2Int>, IFormattable
	{
		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x0600168E RID: 5774 RVA: 0x000249D0 File Offset: 0x00022BD0
		// (set) Token: 0x0600168F RID: 5775 RVA: 0x000249E8 File Offset: 0x00022BE8
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

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001690 RID: 5776 RVA: 0x000249F4 File Offset: 0x00022BF4
		// (set) Token: 0x06001691 RID: 5777 RVA: 0x00024A0C File Offset: 0x00022C0C
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

		// Token: 0x06001692 RID: 5778 RVA: 0x00024A16 File Offset: 0x00022C16
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2Int(int x, int y)
		{
			this.m_X = x;
			this.m_Y = y;
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x00024A16 File Offset: 0x00022C16
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Set(int x, int y)
		{
			this.m_X = x;
			this.m_Y = y;
		}

		// Token: 0x1700047D RID: 1149
		public int this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				int num;
				if (index != 0)
				{
					if (index != 1)
					{
						throw new IndexOutOfRangeException(string.Format("Invalid Vector2Int index addressed: {0}!", index));
					}
					num = this.y;
				}
				else
				{
					num = this.x;
				}
				return num;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				if (index != 0)
				{
					if (index != 1)
					{
						throw new IndexOutOfRangeException(string.Format("Invalid Vector2Int index addressed: {0}!", index));
					}
					this.y = value;
				}
				else
				{
					this.x = value;
				}
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001696 RID: 5782 RVA: 0x00024AB8 File Offset: 0x00022CB8
		public float magnitude
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.y * this.y));
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001697 RID: 5783 RVA: 0x00024AEC File Offset: 0x00022CEC
		public int sqrMagnitude
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.x * this.x + this.y * this.y;
			}
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x00024B1C File Offset: 0x00022D1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Distance(Vector2Int a, Vector2Int b)
		{
			float num = (float)(a.x - b.x);
			float num2 = (float)(a.y - b.y);
			return (float)Math.Sqrt((double)(num * num + num2 * num2));
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x00024B60 File Offset: 0x00022D60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int Min(Vector2Int lhs, Vector2Int rhs)
		{
			return new Vector2Int(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y));
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x00024BA0 File Offset: 0x00022DA0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int Max(Vector2Int lhs, Vector2Int rhs)
		{
			return new Vector2Int(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y));
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x00024BE0 File Offset: 0x00022DE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int Scale(Vector2Int a, Vector2Int b)
		{
			return new Vector2Int(a.x * b.x, a.y * b.y);
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x00024C15 File Offset: 0x00022E15
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Scale(Vector2Int scale)
		{
			this.x *= scale.x;
			this.y *= scale.y;
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x00024C44 File Offset: 0x00022E44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clamp(Vector2Int min, Vector2Int max)
		{
			this.x = Math.Max(min.x, this.x);
			this.x = Math.Min(max.x, this.x);
			this.y = Math.Max(min.y, this.y);
			this.y = Math.Min(max.y, this.y);
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x00024CB8 File Offset: 0x00022EB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2(Vector2Int v)
		{
			return new Vector2((float)v.x, (float)v.y);
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x00024CE0 File Offset: 0x00022EE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3Int(Vector2Int v)
		{
			return new Vector3Int(v.x, v.y, 0);
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x00024D08 File Offset: 0x00022F08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int FloorToInt(Vector2 v)
		{
			return new Vector2Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y));
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x00024D38 File Offset: 0x00022F38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int CeilToInt(Vector2 v)
		{
			return new Vector2Int(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y));
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x00024D68 File Offset: 0x00022F68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int RoundToInt(Vector2 v)
		{
			return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x00024D98 File Offset: 0x00022F98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator -(Vector2Int v)
		{
			return new Vector2Int(-v.x, -v.y);
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x00024DC0 File Offset: 0x00022FC0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator +(Vector2Int a, Vector2Int b)
		{
			return new Vector2Int(a.x + b.x, a.y + b.y);
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x00024DF8 File Offset: 0x00022FF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator -(Vector2Int a, Vector2Int b)
		{
			return new Vector2Int(a.x - b.x, a.y - b.y);
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x00024E30 File Offset: 0x00023030
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator *(Vector2Int a, Vector2Int b)
		{
			return new Vector2Int(a.x * b.x, a.y * b.y);
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x00024E68 File Offset: 0x00023068
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator *(int a, Vector2Int b)
		{
			return new Vector2Int(a * b.x, a * b.y);
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x00024E94 File Offset: 0x00023094
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator *(Vector2Int a, int b)
		{
			return new Vector2Int(a.x * b, a.y * b);
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x00024EC0 File Offset: 0x000230C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator /(Vector2Int a, int b)
		{
			return new Vector2Int(a.x / b, a.y / b);
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x00024EEC File Offset: 0x000230EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector2Int lhs, Vector2Int rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y;
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x00024F24 File Offset: 0x00023124
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2Int lhs, Vector2Int rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x00024F40 File Offset: 0x00023140
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object other)
		{
			bool flag = !(other is Vector2Int);
			return !flag && this.Equals((Vector2Int)other);
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x00024F74 File Offset: 0x00023174
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector2Int other)
		{
			return this.x == other.x && this.y == other.y;
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x00024FA8 File Offset: 0x000231A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ (this.y.GetHashCode() << 2);
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x00024FDC File Offset: 0x000231DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x00024FF8 File Offset: 0x000231F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x00025014 File Offset: 0x00023214
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			bool flag = formatProvider == null;
			if (flag)
			{
				formatProvider = CultureInfo.InvariantCulture.NumberFormat;
			}
			return UnityString.Format("({0}, {1})", new object[]
			{
				this.x.ToString(format, formatProvider),
				this.y.ToString(format, formatProvider)
			});
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x060016B2 RID: 5810 RVA: 0x00025070 File Offset: 0x00023270
		public static Vector2Int zero
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector2Int.s_Zero;
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x060016B3 RID: 5811 RVA: 0x00025088 File Offset: 0x00023288
		public static Vector2Int one
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector2Int.s_One;
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x060016B4 RID: 5812 RVA: 0x000250A0 File Offset: 0x000232A0
		public static Vector2Int up
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector2Int.s_Up;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x060016B5 RID: 5813 RVA: 0x000250B8 File Offset: 0x000232B8
		public static Vector2Int down
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector2Int.s_Down;
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x060016B6 RID: 5814 RVA: 0x000250D0 File Offset: 0x000232D0
		public static Vector2Int left
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector2Int.s_Left;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x060016B7 RID: 5815 RVA: 0x000250E8 File Offset: 0x000232E8
		public static Vector2Int right
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector2Int.s_Right;
			}
		}

		// Token: 0x0400082E RID: 2094
		private int m_X;

		// Token: 0x0400082F RID: 2095
		private int m_Y;

		// Token: 0x04000830 RID: 2096
		private static readonly Vector2Int s_Zero = new Vector2Int(0, 0);

		// Token: 0x04000831 RID: 2097
		private static readonly Vector2Int s_One = new Vector2Int(1, 1);

		// Token: 0x04000832 RID: 2098
		private static readonly Vector2Int s_Up = new Vector2Int(0, 1);

		// Token: 0x04000833 RID: 2099
		private static readonly Vector2Int s_Down = new Vector2Int(0, -1);

		// Token: 0x04000834 RID: 2100
		private static readonly Vector2Int s_Left = new Vector2Int(-1, 0);

		// Token: 0x04000835 RID: 2101
		private static readonly Vector2Int s_Right = new Vector2Int(1, 0);
	}
}
