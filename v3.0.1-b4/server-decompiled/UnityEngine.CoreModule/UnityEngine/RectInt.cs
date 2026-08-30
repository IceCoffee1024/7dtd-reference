using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000132 RID: 306
	[UsedByNativeCode]
	public struct RectInt : IEquatable<RectInt>, IFormattable
	{
		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x0000D1B4 File Offset: 0x0000B3B4
		// (set) Token: 0x0600080F RID: 2063 RVA: 0x0000D1CC File Offset: 0x0000B3CC
		public int x
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_XMin;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_XMin = value;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x0000D1D8 File Offset: 0x0000B3D8
		// (set) Token: 0x06000811 RID: 2065 RVA: 0x0000D1F0 File Offset: 0x0000B3F0
		public int y
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_YMin;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_YMin = value;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x0000D1FC File Offset: 0x0000B3FC
		public Vector2 center
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Vector2((float)this.x + (float)this.m_Width / 2f, (float)this.y + (float)this.m_Height / 2f);
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x0000D240 File Offset: 0x0000B440
		// (set) Token: 0x06000814 RID: 2068 RVA: 0x0000D263 File Offset: 0x0000B463
		public Vector2Int min
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Vector2Int(this.xMin, this.yMin);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.xMin = value.x;
				this.yMin = value.y;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x0000D284 File Offset: 0x0000B484
		// (set) Token: 0x06000816 RID: 2070 RVA: 0x0000D2A7 File Offset: 0x0000B4A7
		public Vector2Int max
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Vector2Int(this.xMax, this.yMax);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.xMax = value.x;
				this.yMax = value.y;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x0000D2C8 File Offset: 0x0000B4C8
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x0000D2E0 File Offset: 0x0000B4E0
		public int width
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Width;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Width = value;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x0000D2EC File Offset: 0x0000B4EC
		// (set) Token: 0x0600081A RID: 2074 RVA: 0x0000D304 File Offset: 0x0000B504
		public int height
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Height;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Height = value;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x0000D310 File Offset: 0x0000B510
		// (set) Token: 0x0600081C RID: 2076 RVA: 0x0000D33C File Offset: 0x0000B53C
		public int xMin
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Math.Min(this.m_XMin, this.m_XMin + this.m_Width);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				int xMax = this.xMax;
				this.m_XMin = value;
				this.m_Width = xMax - this.m_XMin;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x0000D368 File Offset: 0x0000B568
		// (set) Token: 0x0600081E RID: 2078 RVA: 0x0000D394 File Offset: 0x0000B594
		public int yMin
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Math.Min(this.m_YMin, this.m_YMin + this.m_Height);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				int yMax = this.yMax;
				this.m_YMin = value;
				this.m_Height = yMax - this.m_YMin;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x0000D3C0 File Offset: 0x0000B5C0
		// (set) Token: 0x06000820 RID: 2080 RVA: 0x0000D3EA File Offset: 0x0000B5EA
		public int xMax
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Math.Max(this.m_XMin, this.m_XMin + this.m_Width);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Width = value - this.m_XMin;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x0000D3FC File Offset: 0x0000B5FC
		// (set) Token: 0x06000822 RID: 2082 RVA: 0x0000D426 File Offset: 0x0000B626
		public int yMax
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Math.Max(this.m_YMin, this.m_YMin + this.m_Height);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Height = value - this.m_YMin;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x0000D438 File Offset: 0x0000B638
		// (set) Token: 0x06000824 RID: 2084 RVA: 0x0000D45B File Offset: 0x0000B65B
		public Vector2Int position
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Vector2Int(this.m_XMin, this.m_YMin);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_XMin = value.x;
				this.m_YMin = value.y;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x0000D478 File Offset: 0x0000B678
		// (set) Token: 0x06000826 RID: 2086 RVA: 0x0000D49B File Offset: 0x0000B69B
		public Vector2Int size
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Vector2Int(this.m_Width, this.m_Height);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Width = value.x;
				this.m_Height = value.y;
			}
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0000D4B8 File Offset: 0x0000B6B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetMinMax(Vector2Int minPosition, Vector2Int maxPosition)
		{
			this.min = minPosition;
			this.max = maxPosition;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0000D4CB File Offset: 0x0000B6CB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public RectInt(int xMin, int yMin, int width, int height)
		{
			this.m_XMin = xMin;
			this.m_YMin = yMin;
			this.m_Width = width;
			this.m_Height = height;
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0000D4EB File Offset: 0x0000B6EB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public RectInt(Vector2Int position, Vector2Int size)
		{
			this.m_XMin = position.x;
			this.m_YMin = position.y;
			this.m_Width = size.x;
			this.m_Height = size.y;
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0000D524 File Offset: 0x0000B724
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClampToBounds(RectInt bounds)
		{
			this.position = new Vector2Int(Math.Max(Math.Min(bounds.xMax, this.position.x), bounds.xMin), Math.Max(Math.Min(bounds.yMax, this.position.y), bounds.yMin));
			this.size = new Vector2Int(Math.Min(bounds.xMax - this.position.x, this.size.x), Math.Min(bounds.yMax - this.position.y, this.size.y));
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0000D5E8 File Offset: 0x0000B7E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(Vector2Int position)
		{
			return position.x >= this.xMin && position.y >= this.yMin && position.x < this.xMax && position.y < this.yMax;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0000D63C File Offset: 0x0000B83C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Overlaps(RectInt other)
		{
			return other.xMin < this.xMax && other.xMax > this.xMin && other.yMin < this.yMax && other.yMax > this.yMin;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0000D690 File Offset: 0x0000B890
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0000D6AC File Offset: 0x0000B8AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0000D6C8 File Offset: 0x0000B8C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			bool flag = formatProvider == null;
			if (flag)
			{
				formatProvider = CultureInfo.InvariantCulture.NumberFormat;
			}
			return UnityString.Format("(x:{0}, y:{1}, width:{2}, height:{3})", new object[]
			{
				this.x.ToString(format, formatProvider),
				this.y.ToString(format, formatProvider),
				this.width.ToString(format, formatProvider),
				this.height.ToString(format, formatProvider)
			});
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0000D74C File Offset: 0x0000B94C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(RectInt other)
		{
			return this.m_XMin == other.m_XMin && this.m_YMin == other.m_YMin && this.m_Width == other.m_Width && this.m_Height == other.m_Height;
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x0000D79C File Offset: 0x0000B99C
		public RectInt.PositionEnumerator allPositionsWithin
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new RectInt.PositionEnumerator(this.min, this.max);
			}
		}

		// Token: 0x040003F2 RID: 1010
		private int m_XMin;

		// Token: 0x040003F3 RID: 1011
		private int m_YMin;

		// Token: 0x040003F4 RID: 1012
		private int m_Width;

		// Token: 0x040003F5 RID: 1013
		private int m_Height;

		// Token: 0x02000133 RID: 307
		public struct PositionEnumerator : IEnumerator<Vector2Int>, IEnumerator, IDisposable
		{
			// Token: 0x06000832 RID: 2098 RVA: 0x0000D7C0 File Offset: 0x0000B9C0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public PositionEnumerator(Vector2Int min, Vector2Int max)
			{
				this._current = min;
				this._min = min;
				this._max = max;
				this.Reset();
			}

			// Token: 0x06000833 RID: 2099 RVA: 0x0000D7EC File Offset: 0x0000B9EC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public RectInt.PositionEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x06000834 RID: 2100 RVA: 0x0000D804 File Offset: 0x0000BA04
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				bool flag = this._current.y >= this._max.y;
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
							return false;
						}
					}
					flag2 = true;
				}
				return flag2;
			}

			// Token: 0x06000835 RID: 2101 RVA: 0x0000D900 File Offset: 0x0000BB00
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Reset()
			{
				this._current = this._min;
				int x = this._current.x;
				this._current.x = x - 1;
			}

			// Token: 0x170001C8 RID: 456
			// (get) Token: 0x06000836 RID: 2102 RVA: 0x0000D930 File Offset: 0x0000BB30
			public Vector2Int Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this._current;
				}
			}

			// Token: 0x170001C9 RID: 457
			// (get) Token: 0x06000837 RID: 2103 RVA: 0x0000D948 File Offset: 0x0000BB48
			object IEnumerator.Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000838 RID: 2104 RVA: 0x00002669 File Offset: 0x00000869
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			void IDisposable.Dispose()
			{
			}

			// Token: 0x040003F6 RID: 1014
			private readonly Vector2Int _min;

			// Token: 0x040003F7 RID: 1015
			private readonly Vector2Int _max;

			// Token: 0x040003F8 RID: 1016
			private Vector2Int _current;
		}
	}
}
