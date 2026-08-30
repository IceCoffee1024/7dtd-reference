using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	// Token: 0x02000130 RID: 304
	public struct Ray2D : IFormattable
	{
		// Token: 0x060007D1 RID: 2001 RVA: 0x0000C707 File Offset: 0x0000A907
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Ray2D(Vector2 origin, Vector2 direction)
		{
			this.m_Origin = origin;
			this.m_Direction = direction.normalized;
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x0000C720 File Offset: 0x0000A920
		// (set) Token: 0x060007D3 RID: 2003 RVA: 0x0000C738 File Offset: 0x0000A938
		public Vector2 origin
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Origin;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Origin = value;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x0000C744 File Offset: 0x0000A944
		// (set) Token: 0x060007D5 RID: 2005 RVA: 0x0000C75C File Offset: 0x0000A95C
		public Vector2 direction
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Direction;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Direction = value.normalized;
			}
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0000C76C File Offset: 0x0000A96C
		public Vector2 GetPoint(float distance)
		{
			return this.m_Origin + this.m_Direction * distance;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0000C798 File Offset: 0x0000A998
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0000C7B4 File Offset: 0x0000A9B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0000C7D0 File Offset: 0x0000A9D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			bool flag = string.IsNullOrEmpty(format);
			if (flag)
			{
				format = "F2";
			}
			bool flag2 = formatProvider == null;
			if (flag2)
			{
				formatProvider = CultureInfo.InvariantCulture.NumberFormat;
			}
			return UnityString.Format("Origin: {0}, Dir: {1}", new object[]
			{
				this.m_Origin.ToString(format, formatProvider),
				this.m_Direction.ToString(format, formatProvider)
			});
		}

		// Token: 0x040003EC RID: 1004
		private Vector2 m_Origin;

		// Token: 0x040003ED RID: 1005
		private Vector2 m_Direction;
	}
}
