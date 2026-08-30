using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001F8 RID: 504
	[NativeHeader("Runtime/Math/Vector4.h")]
	[RequiredByNativeCode(Optional = true, GenerateProxy = true)]
	[NativeClass("Vector4f")]
	[Il2CppEagerStaticClassConstruction]
	public struct Vector4 : IEquatable<Vector4>, IFormattable
	{
		// Token: 0x17000494 RID: 1172
		public float this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				float num;
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
				case 3:
					num = this.w;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid Vector4 index!");
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
				case 3:
					this.w = value;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid Vector4 index!");
				}
			}
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x00025B6D File Offset: 0x00023D6D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x00025B8D File Offset: 0x00023D8D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = 0f;
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x00025BB0 File Offset: 0x00023DB0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4(float x, float y)
		{
			this.x = x;
			this.y = y;
			this.z = 0f;
			this.w = 0f;
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x00025B6D File Offset: 0x00023D6D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Set(float newX, float newY, float newZ, float newW)
		{
			this.x = newX;
			this.y = newY;
			this.z = newZ;
			this.w = newW;
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x00025BD8 File Offset: 0x00023DD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 Lerp(Vector4 a, Vector4 b, float t)
		{
			t = Mathf.Clamp01(t);
			return new Vector4(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t, a.w + (b.w - a.w) * t);
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x00025C50 File Offset: 0x00023E50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 LerpUnclamped(Vector4 a, Vector4 b, float t)
		{
			return new Vector4(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t, a.w + (b.w - a.w) * t);
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x00025CC0 File Offset: 0x00023EC0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 MoveTowards(Vector4 current, Vector4 target, float maxDistanceDelta)
		{
			float num = target.x - current.x;
			float num2 = target.y - current.y;
			float num3 = target.z - current.z;
			float num4 = target.w - current.w;
			float num5 = num * num + num2 * num2 + num3 * num3 + num4 * num4;
			bool flag = num5 == 0f || (maxDistanceDelta >= 0f && num5 <= maxDistanceDelta * maxDistanceDelta);
			Vector4 vector;
			if (flag)
			{
				vector = target;
			}
			else
			{
				float num6 = (float)Math.Sqrt((double)num5);
				vector = new Vector4(current.x + num / num6 * maxDistanceDelta, current.y + num2 / num6 * maxDistanceDelta, current.z + num3 / num6 * maxDistanceDelta, current.w + num4 / num6 * maxDistanceDelta);
			}
			return vector;
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x00025D90 File Offset: 0x00023F90
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 Scale(Vector4 a, Vector4 b)
		{
			return new Vector4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x00025DDC File Offset: 0x00023FDC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Scale(Vector4 scale)
		{
			this.x *= scale.x;
			this.y *= scale.y;
			this.z *= scale.z;
			this.w *= scale.w;
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x00025E38 File Offset: 0x00024038
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ (this.y.GetHashCode() << 2) ^ (this.z.GetHashCode() >> 2) ^ (this.w.GetHashCode() >> 1);
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x00025E80 File Offset: 0x00024080
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object other)
		{
			bool flag = !(other is Vector4);
			return !flag && this.Equals((Vector4)other);
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x00025EB4 File Offset: 0x000240B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector4 other)
		{
			return this.x == other.x && this.y == other.y && this.z == other.z && this.w == other.w;
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x00025F04 File Offset: 0x00024104
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 Normalize(Vector4 a)
		{
			float num = Vector4.Magnitude(a);
			bool flag = num > 1E-05f;
			Vector4 vector;
			if (flag)
			{
				vector = a / num;
			}
			else
			{
				vector = Vector4.zero;
			}
			return vector;
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x00025F38 File Offset: 0x00024138
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Normalize()
		{
			float num = Vector4.Magnitude(this);
			bool flag = num > 1E-05f;
			if (flag)
			{
				this /= num;
			}
			else
			{
				this = Vector4.zero;
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x060016F9 RID: 5881 RVA: 0x00025F80 File Offset: 0x00024180
		public Vector4 normalized
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector4.Normalize(this);
			}
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x00025FA0 File Offset: 0x000241A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Dot(Vector4 a, Vector4 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x00025FEC File Offset: 0x000241EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 Project(Vector4 a, Vector4 b)
		{
			return b * (Vector4.Dot(a, b) / Vector4.Dot(b, b));
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x00026014 File Offset: 0x00024214
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Distance(Vector4 a, Vector4 b)
		{
			return Vector4.Magnitude(a - b);
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x00026034 File Offset: 0x00024234
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Magnitude(Vector4 a)
		{
			return (float)Math.Sqrt((double)Vector4.Dot(a, a));
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x060016FE RID: 5886 RVA: 0x00026054 File Offset: 0x00024254
		public float magnitude
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (float)Math.Sqrt((double)Vector4.Dot(this, this));
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x060016FF RID: 5887 RVA: 0x00026080 File Offset: 0x00024280
		public float sqrMagnitude
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector4.Dot(this, this);
			}
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x000260A4 File Offset: 0x000242A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 Min(Vector4 lhs, Vector4 rhs)
		{
			return new Vector4(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y), Mathf.Min(lhs.z, rhs.z), Mathf.Min(lhs.w, rhs.w));
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x00026100 File Offset: 0x00024300
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 Max(Vector4 lhs, Vector4 rhs)
		{
			return new Vector4(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y), Mathf.Max(lhs.z, rhs.z), Mathf.Max(lhs.w, rhs.w));
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001702 RID: 5890 RVA: 0x0002615C File Offset: 0x0002435C
		public static Vector4 zero
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector4.zeroVector;
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06001703 RID: 5891 RVA: 0x00026174 File Offset: 0x00024374
		public static Vector4 one
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector4.oneVector;
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001704 RID: 5892 RVA: 0x0002618C File Offset: 0x0002438C
		public static Vector4 positiveInfinity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector4.positiveInfinityVector;
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001705 RID: 5893 RVA: 0x000261A4 File Offset: 0x000243A4
		public static Vector4 negativeInfinity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector4.negativeInfinityVector;
			}
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x000261BC File Offset: 0x000243BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 operator +(Vector4 a, Vector4 b)
		{
			return new Vector4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x00026208 File Offset: 0x00024408
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 operator -(Vector4 a, Vector4 b)
		{
			return new Vector4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x00026254 File Offset: 0x00024454
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 operator -(Vector4 a)
		{
			return new Vector4(-a.x, -a.y, -a.z, -a.w);
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x00026288 File Offset: 0x00024488
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 operator *(Vector4 a, float d)
		{
			return new Vector4(a.x * d, a.y * d, a.z * d, a.w * d);
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x000262C0 File Offset: 0x000244C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 operator *(float d, Vector4 a)
		{
			return new Vector4(a.x * d, a.y * d, a.z * d, a.w * d);
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x000262F8 File Offset: 0x000244F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 operator /(Vector4 a, float d)
		{
			return new Vector4(a.x / d, a.y / d, a.z / d, a.w / d);
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x00026330 File Offset: 0x00024530
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector4 lhs, Vector4 rhs)
		{
			float num = lhs.x - rhs.x;
			float num2 = lhs.y - rhs.y;
			float num3 = lhs.z - rhs.z;
			float num4 = lhs.w - rhs.w;
			float num5 = num * num + num2 * num2 + num3 * num3 + num4 * num4;
			return num5 < 9.9999994E-11f;
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x00026398 File Offset: 0x00024598
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector4 lhs, Vector4 rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x000263B4 File Offset: 0x000245B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector4(Vector3 v)
		{
			return new Vector4(v.x, v.y, v.z, 0f);
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x000263E4 File Offset: 0x000245E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3(Vector4 v)
		{
			return new Vector3(v.x, v.y, v.z);
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x00026410 File Offset: 0x00024610
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector4(Vector2 v)
		{
			return new Vector4(v.x, v.y, 0f, 0f);
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x00026440 File Offset: 0x00024640
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2(Vector4 v)
		{
			return new Vector2(v.x, v.y);
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x00026464 File Offset: 0x00024664
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x00026480 File Offset: 0x00024680
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x0002649C File Offset: 0x0002469C
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
			return UnityString.Format("({0}, {1}, {2}, {3})", new object[]
			{
				this.x.ToString(format, formatProvider),
				this.y.ToString(format, formatProvider),
				this.z.ToString(format, formatProvider),
				this.w.ToString(format, formatProvider)
			});
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x00026524 File Offset: 0x00024724
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float SqrMagnitude(Vector4 a)
		{
			return Vector4.Dot(a, a);
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x00026540 File Offset: 0x00024740
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float SqrMagnitude()
		{
			return Vector4.Dot(this, this);
		}

		// Token: 0x04000841 RID: 2113
		public const float kEpsilon = 1E-05f;

		// Token: 0x04000842 RID: 2114
		public float x;

		// Token: 0x04000843 RID: 2115
		public float y;

		// Token: 0x04000844 RID: 2116
		public float z;

		// Token: 0x04000845 RID: 2117
		public float w;

		// Token: 0x04000846 RID: 2118
		private static readonly Vector4 zeroVector = new Vector4(0f, 0f, 0f, 0f);

		// Token: 0x04000847 RID: 2119
		private static readonly Vector4 oneVector = new Vector4(1f, 1f, 1f, 1f);

		// Token: 0x04000848 RID: 2120
		private static readonly Vector4 positiveInfinityVector = new Vector4(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

		// Token: 0x04000849 RID: 2121
		private static readonly Vector4 negativeInfinityVector = new Vector4(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
	}
}
