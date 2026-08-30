using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200012E RID: 302
	[UsedByNativeCode]
	public struct Plane : IFormattable
	{
		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x0000C2A0 File Offset: 0x0000A4A0
		// (set) Token: 0x060007B4 RID: 1972 RVA: 0x0000C2B8 File Offset: 0x0000A4B8
		public Vector3 normal
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Normal;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Normal = value;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x0000C2C4 File Offset: 0x0000A4C4
		// (set) Token: 0x060007B6 RID: 1974 RVA: 0x0000C2DC File Offset: 0x0000A4DC
		public float distance
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Distance;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Distance = value;
			}
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0000C2E6 File Offset: 0x0000A4E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Plane(Vector3 inNormal, Vector3 inPoint)
		{
			this.m_Normal = Vector3.Normalize(inNormal);
			this.m_Distance = -Vector3.Dot(this.m_Normal, inPoint);
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0000C308 File Offset: 0x0000A508
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Plane(Vector3 inNormal, float d)
		{
			this.m_Normal = Vector3.Normalize(inNormal);
			this.m_Distance = d;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0000C31E File Offset: 0x0000A51E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Plane(Vector3 a, Vector3 b, Vector3 c)
		{
			this.m_Normal = Vector3.Normalize(Vector3.Cross(b - a, c - a));
			this.m_Distance = -Vector3.Dot(this.m_Normal, a);
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0000C2E6 File Offset: 0x0000A4E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetNormalAndPosition(Vector3 inNormal, Vector3 inPoint)
		{
			this.m_Normal = Vector3.Normalize(inNormal);
			this.m_Distance = -Vector3.Dot(this.m_Normal, inPoint);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0000C31E File Offset: 0x0000A51E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Set3Points(Vector3 a, Vector3 b, Vector3 c)
		{
			this.m_Normal = Vector3.Normalize(Vector3.Cross(b - a, c - a));
			this.m_Distance = -Vector3.Dot(this.m_Normal, a);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0000C352 File Offset: 0x0000A552
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Flip()
		{
			this.m_Normal = -this.m_Normal;
			this.m_Distance = -this.m_Distance;
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x0000C374 File Offset: 0x0000A574
		public Plane flipped
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Plane(-this.m_Normal, -this.m_Distance);
			}
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0000C39D File Offset: 0x0000A59D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Translate(Vector3 translation)
		{
			this.m_Distance += Vector3.Dot(this.m_Normal, translation);
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0000C3BC File Offset: 0x0000A5BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Plane Translate(Plane plane, Vector3 translation)
		{
			return new Plane(plane.m_Normal, plane.m_Distance += Vector3.Dot(plane.m_Normal, translation));
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0000C3F4 File Offset: 0x0000A5F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3 ClosestPointOnPlane(Vector3 point)
		{
			float num = Vector3.Dot(this.m_Normal, point) + this.m_Distance;
			return point - this.m_Normal * num;
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0000C42C File Offset: 0x0000A62C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetDistanceToPoint(Vector3 point)
		{
			return Vector3.Dot(this.m_Normal, point) + this.m_Distance;
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0000C454 File Offset: 0x0000A654
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool GetSide(Vector3 point)
		{
			return Vector3.Dot(this.m_Normal, point) + this.m_Distance > 0f;
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0000C480 File Offset: 0x0000A680
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool SameSide(Vector3 inPt0, Vector3 inPt1)
		{
			float distanceToPoint = this.GetDistanceToPoint(inPt0);
			float distanceToPoint2 = this.GetDistanceToPoint(inPt1);
			return (distanceToPoint > 0f && distanceToPoint2 > 0f) || (distanceToPoint <= 0f && distanceToPoint2 <= 0f);
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0000C4CC File Offset: 0x0000A6CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Raycast(Ray ray, out float enter)
		{
			float num = Vector3.Dot(ray.direction, this.m_Normal);
			float num2 = -Vector3.Dot(ray.origin, this.m_Normal) - this.m_Distance;
			bool flag = Mathf.Approximately(num, 0f);
			bool flag2;
			if (flag)
			{
				enter = 0f;
				flag2 = false;
			}
			else
			{
				enter = num2 / num;
				flag2 = enter > 0f;
			}
			return flag2;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0000C538 File Offset: 0x0000A738
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0000C554 File Offset: 0x0000A754
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0000C570 File Offset: 0x0000A770
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
			return UnityString.Format("(normal:{0}, distance:{1})", new object[]
			{
				this.m_Normal.ToString(format, formatProvider),
				this.m_Distance.ToString(format, formatProvider)
			});
		}

		// Token: 0x040003E7 RID: 999
		internal const int size = 16;

		// Token: 0x040003E8 RID: 1000
		private Vector3 m_Normal;

		// Token: 0x040003E9 RID: 1001
		private float m_Distance;
	}
}
