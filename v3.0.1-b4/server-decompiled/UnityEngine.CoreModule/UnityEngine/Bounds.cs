using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200012A RID: 298
	[NativeType(Header = "Runtime/Geometry/AABB.h")]
	[NativeHeader("Runtime/Math/MathScripting.h")]
	[NativeHeader("Runtime/Geometry/Intersection.h")]
	[NativeClass("AABB")]
	[NativeHeader("Runtime/Geometry/Ray.h")]
	[RequiredByNativeCode(Optional = true, GenerateProxy = true)]
	[NativeHeader("Runtime/Geometry/AABB.h")]
	public struct Bounds : IEquatable<Bounds>, IFormattable
	{
		// Token: 0x06000753 RID: 1875 RVA: 0x0000B10B File Offset: 0x0000930B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Bounds(Vector3 center, Vector3 size)
		{
			this.m_Center = center;
			this.m_Extents = size * 0.5f;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0000B128 File Offset: 0x00009328
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return this.center.GetHashCode() ^ (this.extents.GetHashCode() << 2);
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0000B168 File Offset: 0x00009368
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object other)
		{
			bool flag = !(other is Bounds);
			return !flag && this.Equals((Bounds)other);
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0000B19C File Offset: 0x0000939C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Bounds other)
		{
			return this.center.Equals(other.center) && this.extents.Equals(other.extents);
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x0000B1E0 File Offset: 0x000093E0
		// (set) Token: 0x06000758 RID: 1880 RVA: 0x0000B1F8 File Offset: 0x000093F8
		public Vector3 center
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Center;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Center = value;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x0000B204 File Offset: 0x00009404
		// (set) Token: 0x0600075A RID: 1882 RVA: 0x0000B226 File Offset: 0x00009426
		public Vector3 size
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Extents * 2f;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Extents = value * 0.5f;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x0000B23C File Offset: 0x0000943C
		// (set) Token: 0x0600075C RID: 1884 RVA: 0x0000B254 File Offset: 0x00009454
		public Vector3 extents
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Extents;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.m_Extents = value;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x0000B260 File Offset: 0x00009460
		// (set) Token: 0x0600075E RID: 1886 RVA: 0x0000B283 File Offset: 0x00009483
		public Vector3 min
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.center - this.extents;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.SetMinMax(value, this.max);
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x0000B294 File Offset: 0x00009494
		// (set) Token: 0x06000760 RID: 1888 RVA: 0x0000B2B7 File Offset: 0x000094B7
		public Vector3 max
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.center + this.extents;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.SetMinMax(this.min, value);
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0000B2C8 File Offset: 0x000094C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Bounds lhs, Bounds rhs)
		{
			return lhs.center == rhs.center && lhs.extents == rhs.extents;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0000B308 File Offset: 0x00009508
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Bounds lhs, Bounds rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0000B324 File Offset: 0x00009524
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetMinMax(Vector3 min, Vector3 max)
		{
			this.extents = (max - min) * 0.5f;
			this.center = min + this.extents;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0000B352 File Offset: 0x00009552
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Encapsulate(Vector3 point)
		{
			this.SetMinMax(Vector3.Min(this.min, point), Vector3.Max(this.max, point));
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0000B374 File Offset: 0x00009574
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Encapsulate(Bounds bounds)
		{
			this.Encapsulate(bounds.center - bounds.extents);
			this.Encapsulate(bounds.center + bounds.extents);
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0000B3AB File Offset: 0x000095AB
		public void Expand(float amount)
		{
			amount *= 0.5f;
			this.extents += new Vector3(amount, amount, amount);
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0000B3D1 File Offset: 0x000095D1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Expand(Vector3 amount)
		{
			this.extents += amount * 0.5f;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0000B3F4 File Offset: 0x000095F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Intersects(Bounds bounds)
		{
			return this.min.x <= bounds.max.x && this.max.x >= bounds.min.x && this.min.y <= bounds.max.y && this.max.y >= bounds.min.y && this.min.z <= bounds.max.z && this.max.z >= bounds.min.z;
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0000B4A8 File Offset: 0x000096A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IntersectRay(Ray ray)
		{
			float num;
			return Bounds.IntersectRayAABB(ray, this, out num);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0000B4C8 File Offset: 0x000096C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IntersectRay(Ray ray, out float distance)
		{
			return Bounds.IntersectRayAABB(ray, this, out distance);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0000B4E8 File Offset: 0x000096E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0000B504 File Offset: 0x00009704
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0000B520 File Offset: 0x00009720
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
			return UnityString.Format("Center: {0}, Extents: {1}", new object[]
			{
				this.m_Center.ToString(format, formatProvider),
				this.m_Extents.ToString(format, formatProvider)
			});
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0000B587 File Offset: 0x00009787
		[NativeMethod("IsInside", IsThreadSafe = true)]
		public bool Contains(Vector3 point)
		{
			return Bounds.Contains_Injected(ref this, ref point);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0000B591 File Offset: 0x00009791
		[FreeFunction("BoundsScripting::SqrDistance", HasExplicitThis = true, IsThreadSafe = true)]
		public float SqrDistance(Vector3 point)
		{
			return Bounds.SqrDistance_Injected(ref this, ref point);
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0000B59B File Offset: 0x0000979B
		[FreeFunction("IntersectRayAABB", IsThreadSafe = true)]
		private static bool IntersectRayAABB(Ray ray, Bounds bounds, out float dist)
		{
			return Bounds.IntersectRayAABB_Injected(ref ray, ref bounds, out dist);
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0000B5A8 File Offset: 0x000097A8
		[FreeFunction("BoundsScripting::ClosestPoint", HasExplicitThis = true, IsThreadSafe = true)]
		public Vector3 ClosestPoint(Vector3 point)
		{
			Vector3 vector;
			Bounds.ClosestPoint_Injected(ref this, ref point, out vector);
			return vector;
		}

		// Token: 0x06000772 RID: 1906
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Contains_Injected(ref Bounds _unity_self, ref Vector3 point);

		// Token: 0x06000773 RID: 1907
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float SqrDistance_Injected(ref Bounds _unity_self, ref Vector3 point);

		// Token: 0x06000774 RID: 1908
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IntersectRayAABB_Injected(ref Ray ray, ref Bounds bounds, out float dist);

		// Token: 0x06000775 RID: 1909
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClosestPoint_Injected(ref Bounds _unity_self, ref Vector3 point, out Vector3 ret);

		// Token: 0x040003E0 RID: 992
		private Vector3 m_Center;

		// Token: 0x040003E1 RID: 993
		[NativeName("m_Extent")]
		private Vector3 m_Extents;
	}
}
