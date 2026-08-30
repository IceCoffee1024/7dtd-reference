using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200012B RID: 299
	[UsedByNativeCode]
	public struct BoundsInt : IEquatable<BoundsInt>, IFormattable
	{
		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x0000B5C0 File Offset: 0x000097C0
		// (set) Token: 0x06000777 RID: 1911 RVA: 0x0000B5DD File Offset: 0x000097DD
		public int x
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Position.x;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Position.x = value;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x0000B5F0 File Offset: 0x000097F0
		// (set) Token: 0x06000779 RID: 1913 RVA: 0x0000B60D File Offset: 0x0000980D
		public int y
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Position.y;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Position.y = value;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x0000B620 File Offset: 0x00009820
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x0000B63D File Offset: 0x0000983D
		public int z
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Position.z;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Position.z = value;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x0000B650 File Offset: 0x00009850
		public Vector3 center
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Vector3((float)this.x + (float)this.m_Size.x / 2f, (float)this.y + (float)this.m_Size.y / 2f, (float)this.z + (float)this.m_Size.z / 2f);
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x0000B6B8 File Offset: 0x000098B8
		// (set) Token: 0x0600077E RID: 1918 RVA: 0x0000B6E1 File Offset: 0x000098E1
		public Vector3Int min
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Vector3Int(this.xMin, this.yMin, this.zMin);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.xMin = value.x;
				this.yMin = value.y;
				this.zMin = value.z;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x0000B710 File Offset: 0x00009910
		// (set) Token: 0x06000780 RID: 1920 RVA: 0x0000B739 File Offset: 0x00009939
		public Vector3Int max
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Vector3Int(this.xMax, this.yMax, this.zMax);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.xMax = value.x;
				this.yMax = value.y;
				this.zMax = value.z;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x0000B768 File Offset: 0x00009968
		// (set) Token: 0x06000782 RID: 1922 RVA: 0x0000B7A4 File Offset: 0x000099A4
		public int xMin
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Math.Min(this.m_Position.x, this.m_Position.x + this.m_Size.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				int xMax = this.xMax;
				this.m_Position.x = value;
				this.m_Size.x = xMax - this.m_Position.x;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x0000B7E0 File Offset: 0x000099E0
		// (set) Token: 0x06000784 RID: 1924 RVA: 0x0000B81C File Offset: 0x00009A1C
		public int yMin
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Math.Min(this.m_Position.y, this.m_Position.y + this.m_Size.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				int yMax = this.yMax;
				this.m_Position.y = value;
				this.m_Size.y = yMax - this.m_Position.y;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x0000B858 File Offset: 0x00009A58
		// (set) Token: 0x06000786 RID: 1926 RVA: 0x0000B894 File Offset: 0x00009A94
		public int zMin
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Math.Min(this.m_Position.z, this.m_Position.z + this.m_Size.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				int zMax = this.zMax;
				this.m_Position.z = value;
				this.m_Size.z = zMax - this.m_Position.z;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x0000B8D0 File Offset: 0x00009AD0
		// (set) Token: 0x06000788 RID: 1928 RVA: 0x0000B909 File Offset: 0x00009B09
		public int xMax
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Math.Max(this.m_Position.x, this.m_Position.x + this.m_Size.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Size.x = value - this.m_Position.x;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x0000B928 File Offset: 0x00009B28
		// (set) Token: 0x0600078A RID: 1930 RVA: 0x0000B961 File Offset: 0x00009B61
		public int yMax
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Math.Max(this.m_Position.y, this.m_Position.y + this.m_Size.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Size.y = value - this.m_Position.y;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x0000B980 File Offset: 0x00009B80
		// (set) Token: 0x0600078C RID: 1932 RVA: 0x0000B9B9 File Offset: 0x00009BB9
		public int zMax
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Math.Max(this.m_Position.z, this.m_Position.z + this.m_Size.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Size.z = value - this.m_Position.z;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x0000B9D8 File Offset: 0x00009BD8
		// (set) Token: 0x0600078E RID: 1934 RVA: 0x0000B9F0 File Offset: 0x00009BF0
		public Vector3Int position
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Position;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Position = value;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x0000B9FC File Offset: 0x00009BFC
		// (set) Token: 0x06000790 RID: 1936 RVA: 0x0000BA14 File Offset: 0x00009C14
		public Vector3Int size
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Size;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Size = value;
			}
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0000BA1E File Offset: 0x00009C1E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BoundsInt(int xMin, int yMin, int zMin, int sizeX, int sizeY, int sizeZ)
		{
			this.m_Position = new Vector3Int(xMin, yMin, zMin);
			this.m_Size = new Vector3Int(sizeX, sizeY, sizeZ);
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0000BA40 File Offset: 0x00009C40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BoundsInt(Vector3Int position, Vector3Int size)
		{
			this.m_Position = position;
			this.m_Size = size;
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0000BA51 File Offset: 0x00009C51
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetMinMax(Vector3Int minPosition, Vector3Int maxPosition)
		{
			this.min = minPosition;
			this.max = maxPosition;
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0000BA64 File Offset: 0x00009C64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClampToBounds(BoundsInt bounds)
		{
			this.position = new Vector3Int(Math.Max(Math.Min(bounds.xMax, this.position.x), bounds.xMin), Math.Max(Math.Min(bounds.yMax, this.position.y), bounds.yMin), Math.Max(Math.Min(bounds.zMax, this.position.z), bounds.zMin));
			this.size = new Vector3Int(Math.Min(bounds.xMax - this.position.x, this.size.x), Math.Min(bounds.yMax - this.position.y, this.size.y), Math.Min(bounds.zMax - this.position.z, this.size.z));
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0000BB78 File Offset: 0x00009D78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(Vector3Int position)
		{
			return position.x >= this.xMin && position.y >= this.yMin && position.z >= this.zMin && position.x < this.xMax && position.y < this.yMax && position.z < this.zMax;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0000BBE8 File Offset: 0x00009DE8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0000BC04 File Offset: 0x00009E04
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0000BC20 File Offset: 0x00009E20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			bool flag = formatProvider == null;
			if (flag)
			{
				formatProvider = CultureInfo.InvariantCulture.NumberFormat;
			}
			return UnityString.Format("Position: {0}, Size: {1}", new object[]
			{
				this.m_Position.ToString(format, formatProvider),
				this.m_Size.ToString(format, formatProvider)
			});
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0000BC78 File Offset: 0x00009E78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(BoundsInt lhs, BoundsInt rhs)
		{
			return lhs.m_Position == rhs.m_Position && lhs.m_Size == rhs.m_Size;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0000BCB4 File Offset: 0x00009EB4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(BoundsInt lhs, BoundsInt rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0000BCD0 File Offset: 0x00009ED0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object other)
		{
			bool flag = !(other is BoundsInt);
			return !flag && this.Equals((BoundsInt)other);
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0000BD04 File Offset: 0x00009F04
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(BoundsInt other)
		{
			return this.m_Position.Equals(other.m_Position) && this.m_Size.Equals(other.m_Size);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0000BD40 File Offset: 0x00009F40
		public override int GetHashCode()
		{
			return this.m_Position.GetHashCode() ^ (this.m_Size.GetHashCode() << 2);
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x0000BD78 File Offset: 0x00009F78
		public BoundsInt.PositionEnumerator allPositionsWithin
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new BoundsInt.PositionEnumerator(this.min, this.max);
			}
		}

		// Token: 0x040003E2 RID: 994
		private Vector3Int m_Position;

		// Token: 0x040003E3 RID: 995
		private Vector3Int m_Size;

		// Token: 0x0200012C RID: 300
		public struct PositionEnumerator : IEnumerator<Vector3Int>, IEnumerator, IDisposable
		{
			// Token: 0x0600079F RID: 1951 RVA: 0x0000BD9C File Offset: 0x00009F9C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public PositionEnumerator(Vector3Int min, Vector3Int max)
			{
				this._current = min;
				this._min = min;
				this._max = max;
				this.Reset();
			}

			// Token: 0x060007A0 RID: 1952 RVA: 0x0000BDC8 File Offset: 0x00009FC8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public BoundsInt.PositionEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x060007A1 RID: 1953 RVA: 0x0000BDE0 File Offset: 0x00009FE0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				bool flag = this._current.z >= this._max.z || this._current.y >= this._max.y;
				bool flag2;
				if (flag)
				{
					flag2 = false;
				}
				else
				{
					int num = this._current.x;
					this._current.x = num + 1;
					bool flag3 = this._current.x >= this._max.x;
					if (flag3)
					{
						this._current.x = this._min.x;
						bool flag4 = this._current.x >= this._max.x;
						if (flag4)
						{
							return false;
						}
						num = this._current.y;
						this._current.y = num + 1;
						bool flag5 = this._current.y >= this._max.y;
						if (flag5)
						{
							this._current.y = this._min.y;
							num = this._current.z;
							this._current.z = num + 1;
							bool flag6 = this._current.z >= this._max.z;
							if (flag6)
							{
								return false;
							}
						}
					}
					flag2 = true;
				}
				return flag2;
			}

			// Token: 0x060007A2 RID: 1954 RVA: 0x0000BF54 File Offset: 0x0000A154
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Reset()
			{
				this._current = this._min;
				int x = this._current.x;
				this._current.x = x - 1;
			}

			// Token: 0x1700019F RID: 415
			// (get) Token: 0x060007A3 RID: 1955 RVA: 0x0000BF84 File Offset: 0x0000A184
			public Vector3Int Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this._current;
				}
			}

			// Token: 0x170001A0 RID: 416
			// (get) Token: 0x060007A4 RID: 1956 RVA: 0x0000BF9C File Offset: 0x0000A19C
			object IEnumerator.Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.Current;
				}
			}

			// Token: 0x060007A5 RID: 1957 RVA: 0x00002669 File Offset: 0x00000869
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			void IDisposable.Dispose()
			{
			}

			// Token: 0x040003E4 RID: 996
			private readonly Vector3Int _min;

			// Token: 0x040003E5 RID: 997
			private readonly Vector3Int _max;

			// Token: 0x040003E6 RID: 998
			private Vector3Int _current;
		}
	}
}
