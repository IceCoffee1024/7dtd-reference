using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001F5 RID: 501
	[Il2CppEagerStaticClassConstruction]
	[NativeClass("Vector2f")]
	[RequiredByNativeCode(Optional = true, GenerateProxy = true)]
	public struct Vector2 : IEquatable<Vector2>, IFormattable
	{
		// Token: 0x1700046F RID: 1135
		public float this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				float num;
				if (index != 0)
				{
					if (index != 1)
					{
						throw new IndexOutOfRangeException("Invalid Vector2 index!");
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
						throw new IndexOutOfRangeException("Invalid Vector2 index!");
					}
					this.y = value;
				}
				else
				{
					this.x = value;
				}
			}
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00023DFE File Offset: 0x00021FFE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x00023DFE File Offset: 0x00021FFE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Set(float newX, float newY)
		{
			this.x = newX;
			this.y = newY;
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00023E10 File Offset: 0x00022010
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
		{
			t = Mathf.Clamp01(t);
			return new Vector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x00023E5C File Offset: 0x0002205C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 LerpUnclamped(Vector2 a, Vector2 b, float t)
		{
			return new Vector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x00023EA0 File Offset: 0x000220A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta)
		{
			float num = target.x - current.x;
			float num2 = target.y - current.y;
			float num3 = num * num + num2 * num2;
			bool flag = num3 == 0f || (maxDistanceDelta >= 0f && num3 <= maxDistanceDelta * maxDistanceDelta);
			Vector2 vector;
			if (flag)
			{
				vector = target;
			}
			else
			{
				float num4 = (float)Math.Sqrt((double)num3);
				vector = new Vector2(current.x + num / num4 * maxDistanceDelta, current.y + num2 / num4 * maxDistanceDelta);
			}
			return vector;
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x00023F28 File Offset: 0x00022128
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Scale(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x * b.x, a.y * b.y);
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00023F59 File Offset: 0x00022159
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Scale(Vector2 scale)
		{
			this.x *= scale.x;
			this.y *= scale.y;
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x00023F84 File Offset: 0x00022184
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Normalize()
		{
			float magnitude = this.magnitude;
			bool flag = magnitude > 1E-05f;
			if (flag)
			{
				this /= magnitude;
			}
			else
			{
				this = Vector2.zero;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001662 RID: 5730 RVA: 0x00023FC4 File Offset: 0x000221C4
		public Vector2 normalized
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				Vector2 vector = new Vector2(this.x, this.y);
				vector.Normalize();
				return vector;
			}
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x00023FF4 File Offset: 0x000221F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x00024010 File Offset: 0x00022210
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x0002402C File Offset: 0x0002222C
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
			return UnityString.Format("({0}, {1})", new object[]
			{
				this.x.ToString(format, formatProvider),
				this.y.ToString(format, formatProvider)
			});
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x00024094 File Offset: 0x00022294
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ (this.y.GetHashCode() << 2);
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x000240C0 File Offset: 0x000222C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object other)
		{
			bool flag = !(other is Vector2);
			return !flag && this.Equals((Vector2)other);
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x000240F4 File Offset: 0x000222F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector2 other)
		{
			return this.x == other.x && this.y == other.y;
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x00024128 File Offset: 0x00022328
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Reflect(Vector2 inDirection, Vector2 inNormal)
		{
			float num = -2f * Vector2.Dot(inNormal, inDirection);
			return new Vector2(num * inNormal.x + inDirection.x, num * inNormal.y + inDirection.y);
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x0002416C File Offset: 0x0002236C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Perpendicular(Vector2 inDirection)
		{
			return new Vector2(-inDirection.y, inDirection.x);
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x00024190 File Offset: 0x00022390
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Dot(Vector2 lhs, Vector2 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y;
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x0600166C RID: 5740 RVA: 0x000241C0 File Offset: 0x000223C0
		public float magnitude
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (float)Math.Sqrt((double)(this.x * this.x + this.y * this.y));
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x0600166D RID: 5741 RVA: 0x000241F4 File Offset: 0x000223F4
		public float sqrMagnitude
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.x * this.x + this.y * this.y;
			}
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x00024224 File Offset: 0x00022424
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Angle(Vector2 from, Vector2 to)
		{
			float num = (float)Math.Sqrt((double)(from.sqrMagnitude * to.sqrMagnitude));
			bool flag = num < 1E-15f;
			float num2;
			if (flag)
			{
				num2 = 0f;
			}
			else
			{
				float num3 = Mathf.Clamp(Vector2.Dot(from, to) / num, -1f, 1f);
				num2 = (float)Math.Acos((double)num3) * 57.29578f;
			}
			return num2;
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x00024288 File Offset: 0x00022488
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float SignedAngle(Vector2 from, Vector2 to)
		{
			float num = Vector2.Angle(from, to);
			float num2 = Mathf.Sign(from.x * to.y - from.y * to.x);
			return num * num2;
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x000242C8 File Offset: 0x000224C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Distance(Vector2 a, Vector2 b)
		{
			float num = a.x - b.x;
			float num2 = a.y - b.y;
			return (float)Math.Sqrt((double)(num * num + num2 * num2));
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00024304 File Offset: 0x00022504
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 ClampMagnitude(Vector2 vector, float maxLength)
		{
			float sqrMagnitude = vector.sqrMagnitude;
			bool flag = sqrMagnitude > maxLength * maxLength;
			Vector2 vector2;
			if (flag)
			{
				float num = (float)Math.Sqrt((double)sqrMagnitude);
				float num2 = vector.x / num;
				float num3 = vector.y / num;
				vector2 = new Vector2(num2 * maxLength, num3 * maxLength);
			}
			else
			{
				vector2 = vector;
			}
			return vector2;
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x00024358 File Offset: 0x00022558
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float SqrMagnitude(Vector2 a)
		{
			return a.x * a.x + a.y * a.y;
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x00024388 File Offset: 0x00022588
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float SqrMagnitude()
		{
			return this.x * this.x + this.y * this.y;
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x000243B8 File Offset: 0x000225B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Min(Vector2 lhs, Vector2 rhs)
		{
			return new Vector2(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y));
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x000243F4 File Offset: 0x000225F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Max(Vector2 lhs, Vector2 rhs)
		{
			return new Vector2(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y));
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x00024430 File Offset: 0x00022630
		[ExcludeFromDocs]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 SmoothDamp(Vector2 current, Vector2 target, ref Vector2 currentVelocity, float smoothTime, float maxSpeed)
		{
			float deltaTime = Time.deltaTime;
			return Vector2.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x00024454 File Offset: 0x00022654
		[ExcludeFromDocs]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 SmoothDamp(Vector2 current, Vector2 target, ref Vector2 currentVelocity, float smoothTime)
		{
			float deltaTime = Time.deltaTime;
			float positiveInfinity = float.PositiveInfinity;
			return Vector2.SmoothDamp(current, target, ref currentVelocity, smoothTime, positiveInfinity, deltaTime);
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x00024480 File Offset: 0x00022680
		public static Vector2 SmoothDamp(Vector2 current, Vector2 target, ref Vector2 currentVelocity, float smoothTime, [DefaultValue("Mathf.Infinity")] float maxSpeed, [DefaultValue("Time.deltaTime")] float deltaTime)
		{
			smoothTime = Mathf.Max(0.0001f, smoothTime);
			float num = 2f / smoothTime;
			float num2 = num * deltaTime;
			float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
			float num4 = current.x - target.x;
			float num5 = current.y - target.y;
			Vector2 vector = target;
			float num6 = maxSpeed * smoothTime;
			float num7 = num6 * num6;
			float num8 = num4 * num4 + num5 * num5;
			bool flag = num8 > num7;
			if (flag)
			{
				float num9 = (float)Math.Sqrt((double)num8);
				num4 = num4 / num9 * num6;
				num5 = num5 / num9 * num6;
			}
			target.x = current.x - num4;
			target.y = current.y - num5;
			float num10 = (currentVelocity.x + num * num4) * deltaTime;
			float num11 = (currentVelocity.y + num * num5) * deltaTime;
			currentVelocity.x = (currentVelocity.x - num * num10) * num3;
			currentVelocity.y = (currentVelocity.y - num * num11) * num3;
			float num12 = target.x + (num4 + num10) * num3;
			float num13 = target.y + (num5 + num11) * num3;
			float num14 = vector.x - current.x;
			float num15 = vector.y - current.y;
			float num16 = num12 - vector.x;
			float num17 = num13 - vector.y;
			bool flag2 = num14 * num16 + num15 * num17 > 0f;
			if (flag2)
			{
				num12 = vector.x;
				num13 = vector.y;
				currentVelocity.x = (num12 - vector.x) / deltaTime;
				currentVelocity.y = (num13 - vector.y) / deltaTime;
			}
			return new Vector2(num12, num13);
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x0002464C File Offset: 0x0002284C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator +(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x + b.x, a.y + b.y);
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x00024680 File Offset: 0x00022880
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator -(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x - b.x, a.y - b.y);
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x000246B4 File Offset: 0x000228B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x * b.x, a.y * b.y);
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x000246E8 File Offset: 0x000228E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator /(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x / b.x, a.y / b.y);
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x0002471C File Offset: 0x0002291C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator -(Vector2 a)
		{
			return new Vector2(-a.x, -a.y);
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x00024744 File Offset: 0x00022944
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(Vector2 a, float d)
		{
			return new Vector2(a.x * d, a.y * d);
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x0002476C File Offset: 0x0002296C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(float d, Vector2 a)
		{
			return new Vector2(a.x * d, a.y * d);
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x00024794 File Offset: 0x00022994
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator /(Vector2 a, float d)
		{
			return new Vector2(a.x / d, a.y / d);
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x000247BC File Offset: 0x000229BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector2 lhs, Vector2 rhs)
		{
			float num = lhs.x - rhs.x;
			float num2 = lhs.y - rhs.y;
			return num * num + num2 * num2 < 9.9999994E-11f;
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x000247F8 File Offset: 0x000229F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2 lhs, Vector2 rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x00024814 File Offset: 0x00022A14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2(Vector3 v)
		{
			return new Vector2(v.x, v.y);
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x00024838 File Offset: 0x00022A38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3(Vector2 v)
		{
			return new Vector3(v.x, v.y, 0f);
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001685 RID: 5765 RVA: 0x00024860 File Offset: 0x00022A60
		public static Vector2 zero
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector2.zeroVector;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001686 RID: 5766 RVA: 0x00024878 File Offset: 0x00022A78
		public static Vector2 one
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector2.oneVector;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001687 RID: 5767 RVA: 0x00024890 File Offset: 0x00022A90
		public static Vector2 up
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector2.upVector;
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001688 RID: 5768 RVA: 0x000248A8 File Offset: 0x00022AA8
		public static Vector2 down
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector2.downVector;
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001689 RID: 5769 RVA: 0x000248C0 File Offset: 0x00022AC0
		public static Vector2 left
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector2.leftVector;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x0600168A RID: 5770 RVA: 0x000248D8 File Offset: 0x00022AD8
		public static Vector2 right
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector2.rightVector;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x0600168B RID: 5771 RVA: 0x000248F0 File Offset: 0x00022AF0
		public static Vector2 positiveInfinity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector2.positiveInfinityVector;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x0600168C RID: 5772 RVA: 0x00024908 File Offset: 0x00022B08
		public static Vector2 negativeInfinity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Vector2.negativeInfinityVector;
			}
		}

		// Token: 0x04000822 RID: 2082
		public float x;

		// Token: 0x04000823 RID: 2083
		public float y;

		// Token: 0x04000824 RID: 2084
		private static readonly Vector2 zeroVector = new Vector2(0f, 0f);

		// Token: 0x04000825 RID: 2085
		private static readonly Vector2 oneVector = new Vector2(1f, 1f);

		// Token: 0x04000826 RID: 2086
		private static readonly Vector2 upVector = new Vector2(0f, 1f);

		// Token: 0x04000827 RID: 2087
		private static readonly Vector2 downVector = new Vector2(0f, -1f);

		// Token: 0x04000828 RID: 2088
		private static readonly Vector2 leftVector = new Vector2(-1f, 0f);

		// Token: 0x04000829 RID: 2089
		private static readonly Vector2 rightVector = new Vector2(1f, 0f);

		// Token: 0x0400082A RID: 2090
		private static readonly Vector2 positiveInfinityVector = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

		// Token: 0x0400082B RID: 2091
		private static readonly Vector2 negativeInfinityVector = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

		// Token: 0x0400082C RID: 2092
		public const float kEpsilon = 1E-05f;

		// Token: 0x0400082D RID: 2093
		public const float kEpsilonNormalSqrt = 1E-15f;
	}
}
