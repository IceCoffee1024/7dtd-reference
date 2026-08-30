using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngineInternal;

namespace UnityEngine
{
	// Token: 0x020001F3 RID: 499
	[NativeHeader("Runtime/Math/PerlinNoise.h")]
	[NativeHeader("Runtime/Utilities/BitUtility.h")]
	[Il2CppEagerStaticClassConstruction]
	[NativeHeader("Runtime/Math/FloatConversion.h")]
	[NativeHeader("Runtime/Math/ColorSpaceConversion.h")]
	public struct Mathf
	{
		// Token: 0x0600160B RID: 5643
		[FreeFunction(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int ClosestPowerOfTwo(int value);

		// Token: 0x0600160C RID: 5644
		[FreeFunction(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsPowerOfTwo(int value);

		// Token: 0x0600160D RID: 5645
		[FreeFunction(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int NextPowerOfTwo(int value);

		// Token: 0x0600160E RID: 5646
		[FreeFunction(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GammaToLinearSpace(float value);

		// Token: 0x0600160F RID: 5647
		[FreeFunction(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float LinearToGammaSpace(float value);

		// Token: 0x06001610 RID: 5648 RVA: 0x00022FD8 File Offset: 0x000211D8
		[FreeFunction(IsThreadSafe = true)]
		public static Color CorrelatedColorTemperatureToRGB(float kelvin)
		{
			Color color;
			Mathf.CorrelatedColorTemperatureToRGB_Injected(kelvin, out color);
			return color;
		}

		// Token: 0x06001611 RID: 5649
		[FreeFunction(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ushort FloatToHalf(float val);

		// Token: 0x06001612 RID: 5650
		[FreeFunction(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float HalfToFloat(ushort val);

		// Token: 0x06001613 RID: 5651
		[FreeFunction("PerlinNoise::NoiseNormalized", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float PerlinNoise(float x, float y);

		// Token: 0x06001614 RID: 5652
		[FreeFunction("PerlinNoise::NoiseNormalized", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float PerlinNoise1D(float x);

		// Token: 0x06001615 RID: 5653 RVA: 0x00022FF0 File Offset: 0x000211F0
		public static float Sin(float f)
		{
			return (float)Math.Sin((double)f);
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x0002300C File Offset: 0x0002120C
		public static float Cos(float f)
		{
			return (float)Math.Cos((double)f);
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x00023028 File Offset: 0x00021228
		public static float Tan(float f)
		{
			return (float)Math.Tan((double)f);
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x00023044 File Offset: 0x00021244
		public static float Asin(float f)
		{
			return (float)Math.Asin((double)f);
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x00023060 File Offset: 0x00021260
		public static float Acos(float f)
		{
			return (float)Math.Acos((double)f);
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x0002307C File Offset: 0x0002127C
		public static float Atan(float f)
		{
			return (float)Math.Atan((double)f);
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x00023098 File Offset: 0x00021298
		public static float Atan2(float y, float x)
		{
			return (float)Math.Atan2((double)y, (double)x);
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x000230B4 File Offset: 0x000212B4
		public static float Sqrt(float f)
		{
			return (float)Math.Sqrt((double)f);
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x000230D0 File Offset: 0x000212D0
		public static float Abs(float f)
		{
			return Math.Abs(f);
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x000230E8 File Offset: 0x000212E8
		public static int Abs(int value)
		{
			return Math.Abs(value);
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x00023100 File Offset: 0x00021300
		public static float Min(float a, float b)
		{
			return (a < b) ? a : b;
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x0002311C File Offset: 0x0002131C
		public static float Min(params float[] values)
		{
			int num = values.Length;
			bool flag = num == 0;
			float num2;
			if (flag)
			{
				num2 = 0f;
			}
			else
			{
				float num3 = values[0];
				for (int i = 1; i < num; i++)
				{
					bool flag2 = values[i] < num3;
					if (flag2)
					{
						num3 = values[i];
					}
				}
				num2 = num3;
			}
			return num2;
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x00023174 File Offset: 0x00021374
		public static int Min(int a, int b)
		{
			return (a < b) ? a : b;
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x00023190 File Offset: 0x00021390
		public static int Min(params int[] values)
		{
			int num = values.Length;
			bool flag = num == 0;
			int num2;
			if (flag)
			{
				num2 = 0;
			}
			else
			{
				int num3 = values[0];
				for (int i = 1; i < num; i++)
				{
					bool flag2 = values[i] < num3;
					if (flag2)
					{
						num3 = values[i];
					}
				}
				num2 = num3;
			}
			return num2;
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x000231E4 File Offset: 0x000213E4
		public static float Max(float a, float b)
		{
			return (a > b) ? a : b;
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x00023200 File Offset: 0x00021400
		public static float Max(params float[] values)
		{
			int num = values.Length;
			bool flag = num == 0;
			float num2;
			if (flag)
			{
				num2 = 0f;
			}
			else
			{
				float num3 = values[0];
				for (int i = 1; i < num; i++)
				{
					bool flag2 = values[i] > num3;
					if (flag2)
					{
						num3 = values[i];
					}
				}
				num2 = num3;
			}
			return num2;
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x00023258 File Offset: 0x00021458
		public static int Max(int a, int b)
		{
			return (a > b) ? a : b;
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x00023274 File Offset: 0x00021474
		public static int Max(params int[] values)
		{
			int num = values.Length;
			bool flag = num == 0;
			int num2;
			if (flag)
			{
				num2 = 0;
			}
			else
			{
				int num3 = values[0];
				for (int i = 1; i < num; i++)
				{
					bool flag2 = values[i] > num3;
					if (flag2)
					{
						num3 = values[i];
					}
				}
				num2 = num3;
			}
			return num2;
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x000232C8 File Offset: 0x000214C8
		public static float Pow(float f, float p)
		{
			return (float)Math.Pow((double)f, (double)p);
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x000232E4 File Offset: 0x000214E4
		public static float Exp(float power)
		{
			return (float)Math.Exp((double)power);
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x00023300 File Offset: 0x00021500
		public static float Log(float f, float p)
		{
			return (float)Math.Log((double)f, (double)p);
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x0002331C File Offset: 0x0002151C
		public static float Log(float f)
		{
			return (float)Math.Log((double)f);
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x00023338 File Offset: 0x00021538
		public static float Log10(float f)
		{
			return (float)Math.Log10((double)f);
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x00023354 File Offset: 0x00021554
		public static float Ceil(float f)
		{
			return (float)Math.Ceiling((double)f);
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x00023370 File Offset: 0x00021570
		public static float Floor(float f)
		{
			return (float)Math.Floor((double)f);
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x0002338C File Offset: 0x0002158C
		public static float Round(float f)
		{
			return (float)Math.Round((double)f);
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x000233A8 File Offset: 0x000215A8
		public static int CeilToInt(float f)
		{
			return (int)Math.Ceiling((double)f);
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x000233C4 File Offset: 0x000215C4
		public static int FloorToInt(float f)
		{
			return (int)Math.Floor((double)f);
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x000233E0 File Offset: 0x000215E0
		public static int RoundToInt(float f)
		{
			return (int)Math.Round((double)f);
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x000233FC File Offset: 0x000215FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Sign(float f)
		{
			return (f >= 0f) ? 1f : (-1f);
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x00023424 File Offset: 0x00021624
		public static float Clamp(float value, float min, float max)
		{
			bool flag = value < min;
			if (flag)
			{
				value = min;
			}
			else
			{
				bool flag2 = value > max;
				if (flag2)
				{
					value = max;
				}
			}
			return value;
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x00023450 File Offset: 0x00021650
		public static int Clamp(int value, int min, int max)
		{
			bool flag = value < min;
			if (flag)
			{
				value = min;
			}
			else
			{
				bool flag2 = value > max;
				if (flag2)
				{
					value = max;
				}
			}
			return value;
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x0002347C File Offset: 0x0002167C
		public static float Clamp01(float value)
		{
			bool flag = value < 0f;
			float num;
			if (flag)
			{
				num = 0f;
			}
			else
			{
				bool flag2 = value > 1f;
				if (flag2)
				{
					num = 1f;
				}
				else
				{
					num = value;
				}
			}
			return num;
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x000234B8 File Offset: 0x000216B8
		public static float Lerp(float a, float b, float t)
		{
			return a + (b - a) * Mathf.Clamp01(t);
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x000234D8 File Offset: 0x000216D8
		public static float LerpUnclamped(float a, float b, float t)
		{
			return a + (b - a) * t;
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x000234F4 File Offset: 0x000216F4
		public static float LerpAngle(float a, float b, float t)
		{
			float num = Mathf.Repeat(b - a, 360f);
			bool flag = num > 180f;
			if (flag)
			{
				num -= 360f;
			}
			return a + num * Mathf.Clamp01(t);
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x00023534 File Offset: 0x00021734
		public static float MoveTowards(float current, float target, float maxDelta)
		{
			bool flag = Mathf.Abs(target - current) <= maxDelta;
			float num;
			if (flag)
			{
				num = target;
			}
			else
			{
				num = current + Mathf.Sign(target - current) * maxDelta;
			}
			return num;
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x00023568 File Offset: 0x00021768
		public static float MoveTowardsAngle(float current, float target, float maxDelta)
		{
			float num = Mathf.DeltaAngle(current, target);
			bool flag = -maxDelta < num && num < maxDelta;
			float num2;
			if (flag)
			{
				num2 = target;
			}
			else
			{
				target = current + num;
				num2 = Mathf.MoveTowards(current, target, maxDelta);
			}
			return num2;
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x000235A4 File Offset: 0x000217A4
		public static float SmoothStep(float from, float to, float t)
		{
			t = Mathf.Clamp01(t);
			t = -2f * t * t * t + 3f * t * t;
			return to * t + from * (1f - t);
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x000235E4 File Offset: 0x000217E4
		public static float Gamma(float value, float absmax, float gamma)
		{
			bool flag = value < 0f;
			float num = Mathf.Abs(value);
			bool flag2 = num > absmax;
			float num2;
			if (flag2)
			{
				num2 = (flag ? (-num) : num);
			}
			else
			{
				float num3 = Mathf.Pow(num / absmax, gamma) * absmax;
				num2 = (flag ? (-num3) : num3);
			}
			return num2;
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x00023630 File Offset: 0x00021830
		public static bool Approximately(float a, float b)
		{
			return Mathf.Abs(b - a) < Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)), Mathf.Epsilon * 8f);
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x00023674 File Offset: 0x00021874
		[ExcludeFromDocs]
		public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
		{
			float deltaTime = Time.deltaTime;
			return Mathf.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x00023698 File Offset: 0x00021898
		[ExcludeFromDocs]
		public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime)
		{
			float deltaTime = Time.deltaTime;
			float positiveInfinity = float.PositiveInfinity;
			return Mathf.SmoothDamp(current, target, ref currentVelocity, smoothTime, positiveInfinity, deltaTime);
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x000236C4 File Offset: 0x000218C4
		public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, [DefaultValue("Mathf.Infinity")] float maxSpeed, [DefaultValue("Time.deltaTime")] float deltaTime)
		{
			smoothTime = Mathf.Max(0.0001f, smoothTime);
			float num = 2f / smoothTime;
			float num2 = num * deltaTime;
			float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
			float num4 = current - target;
			float num5 = target;
			float num6 = maxSpeed * smoothTime;
			num4 = Mathf.Clamp(num4, -num6, num6);
			target = current - num4;
			float num7 = (currentVelocity + num * num4) * deltaTime;
			currentVelocity = (currentVelocity - num * num7) * num3;
			float num8 = target + (num4 + num7) * num3;
			bool flag = num5 - current > 0f == num8 > num5;
			if (flag)
			{
				num8 = num5;
				currentVelocity = (num8 - num5) / deltaTime;
			}
			return num8;
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x00023780 File Offset: 0x00021980
		[ExcludeFromDocs]
		public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
		{
			float deltaTime = Time.deltaTime;
			return Mathf.SmoothDampAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x000237A4 File Offset: 0x000219A4
		[ExcludeFromDocs]
		public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime)
		{
			float deltaTime = Time.deltaTime;
			float positiveInfinity = float.PositiveInfinity;
			return Mathf.SmoothDampAngle(current, target, ref currentVelocity, smoothTime, positiveInfinity, deltaTime);
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x000237D0 File Offset: 0x000219D0
		public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, [DefaultValue("Mathf.Infinity")] float maxSpeed, [DefaultValue("Time.deltaTime")] float deltaTime)
		{
			target = current + Mathf.DeltaAngle(current, target);
			return Mathf.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x000237FC File Offset: 0x000219FC
		public static float Repeat(float t, float length)
		{
			return Mathf.Clamp(t - Mathf.Floor(t / length) * length, 0f, length);
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x00023828 File Offset: 0x00021A28
		public static float PingPong(float t, float length)
		{
			t = Mathf.Repeat(t, length * 2f);
			return length - Mathf.Abs(t - length);
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x00023854 File Offset: 0x00021A54
		public static float InverseLerp(float a, float b, float value)
		{
			bool flag = a != b;
			float num;
			if (flag)
			{
				num = Mathf.Clamp01((value - a) / (b - a));
			}
			else
			{
				num = 0f;
			}
			return num;
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x00023888 File Offset: 0x00021A88
		public static float DeltaAngle(float current, float target)
		{
			float num = Mathf.Repeat(target - current, 360f);
			bool flag = num > 180f;
			if (flag)
			{
				num -= 360f;
			}
			return num;
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x000238C0 File Offset: 0x00021AC0
		internal static bool LineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, ref Vector2 result)
		{
			float num = p2.x - p1.x;
			float num2 = p2.y - p1.y;
			float num3 = p4.x - p3.x;
			float num4 = p4.y - p3.y;
			float num5 = num * num4 - num2 * num3;
			bool flag = num5 == 0f;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				float num6 = p3.x - p1.x;
				float num7 = p3.y - p1.y;
				float num8 = (num6 * num4 - num7 * num3) / num5;
				result.x = p1.x + num8 * num;
				result.y = p1.y + num8 * num2;
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x0002397C File Offset: 0x00021B7C
		internal static bool LineSegmentIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, ref Vector2 result)
		{
			float num = p2.x - p1.x;
			float num2 = p2.y - p1.y;
			float num3 = p4.x - p3.x;
			float num4 = p4.y - p3.y;
			float num5 = num * num4 - num2 * num3;
			bool flag = num5 == 0f;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				float num6 = p3.x - p1.x;
				float num7 = p3.y - p1.y;
				float num8 = (num6 * num4 - num7 * num3) / num5;
				bool flag3 = num8 < 0f || num8 > 1f;
				if (flag3)
				{
					flag2 = false;
				}
				else
				{
					float num9 = (num6 * num2 - num7 * num) / num5;
					bool flag4 = num9 < 0f || num9 > 1f;
					if (flag4)
					{
						flag2 = false;
					}
					else
					{
						result.x = p1.x + num8 * num;
						result.y = p1.y + num8 * num2;
						flag2 = true;
					}
				}
			}
			return flag2;
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x00023A8C File Offset: 0x00021C8C
		internal static long RandomToLong(Random r)
		{
			byte[] array = new byte[8];
			r.NextBytes(array);
			return (long)(BitConverter.ToUInt64(array, 0) & 9223372036854775807UL);
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x00023AC0 File Offset: 0x00021CC0
		internal static float ClampToFloat(double value)
		{
			bool flag = double.IsPositiveInfinity(value);
			float num;
			if (flag)
			{
				num = float.PositiveInfinity;
			}
			else
			{
				bool flag2 = double.IsNegativeInfinity(value);
				if (flag2)
				{
					num = float.NegativeInfinity;
				}
				else
				{
					bool flag3 = value < -3.4028234663852886E+38;
					if (flag3)
					{
						num = float.MinValue;
					}
					else
					{
						bool flag4 = value > 3.4028234663852886E+38;
						if (flag4)
						{
							num = float.MaxValue;
						}
						else
						{
							num = (float)value;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x00023B2C File Offset: 0x00021D2C
		internal static int ClampToInt(long value)
		{
			bool flag = value < -2147483648L;
			int num;
			if (flag)
			{
				num = int.MinValue;
			}
			else
			{
				bool flag2 = value > 2147483647L;
				if (flag2)
				{
					num = int.MaxValue;
				}
				else
				{
					num = (int)value;
				}
			}
			return num;
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x00023B6C File Offset: 0x00021D6C
		internal static uint ClampToUInt(long value)
		{
			bool flag = value < 0L;
			uint num;
			if (flag)
			{
				num = 0U;
			}
			else
			{
				bool flag2 = value > (long)((ulong)(-1));
				if (flag2)
				{
					num = uint.MaxValue;
				}
				else
				{
					num = (uint)value;
				}
			}
			return num;
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x00023B9C File Offset: 0x00021D9C
		internal static float RoundToMultipleOf(float value, float roundingValue)
		{
			bool flag = roundingValue == 0f;
			float num;
			if (flag)
			{
				num = value;
			}
			else
			{
				num = Mathf.Round(value / roundingValue) * roundingValue;
			}
			return num;
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x00023BC8 File Offset: 0x00021DC8
		internal static float GetClosestPowerOfTen(float positiveNumber)
		{
			bool flag = positiveNumber <= 0f;
			float num;
			if (flag)
			{
				num = 1f;
			}
			else
			{
				num = Mathf.Pow(10f, (float)Mathf.RoundToInt(Mathf.Log10(positiveNumber)));
			}
			return num;
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x00023C08 File Offset: 0x00021E08
		internal static int GetNumberOfDecimalsForMinimumDifference(float minDifference)
		{
			return Mathf.Clamp(-Mathf.FloorToInt(Mathf.Log10(Mathf.Abs(minDifference))), 0, 15);
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x00023C34 File Offset: 0x00021E34
		internal static int GetNumberOfDecimalsForMinimumDifference(double minDifference)
		{
			return (int)Math.Max(0.0, -Math.Floor(Math.Log10(Math.Abs(minDifference))));
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x00023C68 File Offset: 0x00021E68
		internal static float RoundBasedOnMinimumDifference(float valueToRound, float minDifference)
		{
			bool flag = minDifference == 0f;
			float num;
			if (flag)
			{
				num = Mathf.DiscardLeastSignificantDecimal(valueToRound);
			}
			else
			{
				num = (float)Math.Round((double)valueToRound, Mathf.GetNumberOfDecimalsForMinimumDifference(minDifference), MidpointRounding.AwayFromZero);
			}
			return num;
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x00023CA0 File Offset: 0x00021EA0
		internal static double RoundBasedOnMinimumDifference(double valueToRound, double minDifference)
		{
			bool flag = minDifference == 0.0;
			double num;
			if (flag)
			{
				num = Mathf.DiscardLeastSignificantDecimal(valueToRound);
			}
			else
			{
				num = Math.Round(valueToRound, Mathf.GetNumberOfDecimalsForMinimumDifference(minDifference), MidpointRounding.AwayFromZero);
			}
			return num;
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x00023CD8 File Offset: 0x00021ED8
		internal static float DiscardLeastSignificantDecimal(float v)
		{
			int num = Mathf.Clamp((int)(5f - Mathf.Log10(Mathf.Abs(v))), 0, 15);
			return (float)Math.Round((double)v, num, MidpointRounding.AwayFromZero);
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x00023D10 File Offset: 0x00021F10
		internal static double DiscardLeastSignificantDecimal(double v)
		{
			int num = Math.Max(0, (int)(5.0 - Math.Log10(Math.Abs(v))));
			double num2;
			try
			{
				num2 = Math.Round(v, num);
			}
			catch (ArgumentOutOfRangeException)
			{
				num2 = 0.0;
			}
			return num2;
		}

		// Token: 0x06001657 RID: 5719
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CorrelatedColorTemperatureToRGB_Injected(float kelvin, out Color ret);

		// Token: 0x0400081A RID: 2074
		public const float PI = 3.1415927f;

		// Token: 0x0400081B RID: 2075
		public const float Infinity = float.PositiveInfinity;

		// Token: 0x0400081C RID: 2076
		public const float NegativeInfinity = float.NegativeInfinity;

		// Token: 0x0400081D RID: 2077
		public const float Deg2Rad = 0.017453292f;

		// Token: 0x0400081E RID: 2078
		public const float Rad2Deg = 57.29578f;

		// Token: 0x0400081F RID: 2079
		internal const int kMaxDecimals = 15;

		// Token: 0x04000820 RID: 2080
		public static readonly float Epsilon = (MathfInternal.IsFlushToZeroEnabled ? MathfInternal.FloatMinNormal : MathfInternal.FloatMinDenormal);
	}
}
