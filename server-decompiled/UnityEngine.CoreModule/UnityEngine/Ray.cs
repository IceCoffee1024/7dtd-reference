using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	// Token: 0x0200012F RID: 303
	public struct Ray : IFormattable
	{
		// Token: 0x060007C8 RID: 1992 RVA: 0x0000C5D7 File Offset: 0x0000A7D7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Ray(Vector3 origin, Vector3 direction)
		{
			this.m_Origin = origin;
			this.m_Direction = direction.normalized;
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x0000C5F0 File Offset: 0x0000A7F0
		// (set) Token: 0x060007CA RID: 1994 RVA: 0x0000C608 File Offset: 0x0000A808
		public Vector3 origin
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

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x0000C614 File Offset: 0x0000A814
		// (set) Token: 0x060007CC RID: 1996 RVA: 0x0000C62C File Offset: 0x0000A82C
		public Vector3 direction
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

		// Token: 0x060007CD RID: 1997 RVA: 0x0000C63C File Offset: 0x0000A83C
		public Vector3 GetPoint(float distance)
		{
			return this.m_Origin + this.m_Direction * distance;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0000C668 File Offset: 0x0000A868
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0000C684 File Offset: 0x0000A884
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0000C6A0 File Offset: 0x0000A8A0
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

		// Token: 0x040003EA RID: 1002
		private Vector3 m_Origin;

		// Token: 0x040003EB RID: 1003
		private Vector3 m_Direction;
	}
}
