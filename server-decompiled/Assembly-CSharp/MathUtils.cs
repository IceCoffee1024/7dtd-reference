using System;
using UnityEngine;

// Token: 0x0200143F RID: 5183
public static class MathUtils
{
	// Token: 0x0600A2CE RID: 41678 RVA: 0x003CE33D File Offset: 0x003CC53D
	public static float Clamp(float value, float min, float max)
	{
		if (value < min)
		{
			return min;
		}
		if (value <= max)
		{
			return value;
		}
		return max;
	}

	// Token: 0x0600A2CF RID: 41679 RVA: 0x003CE34C File Offset: 0x003CC54C
	public static float Lerp(float a, float b, float x)
	{
		return a + (b - a) * x;
	}

	// Token: 0x0600A2D0 RID: 41680 RVA: 0x003CE355 File Offset: 0x003CC555
	public static int Min(int a, int b)
	{
		if (a < b)
		{
			return a;
		}
		return b;
	}

	// Token: 0x0600A2D1 RID: 41681 RVA: 0x003CE35E File Offset: 0x003CC55E
	public static int Min(int a, int b, int c)
	{
		if (a >= c && b >= c)
		{
			return c;
		}
		if (a < b)
		{
			return a;
		}
		return b;
	}

	// Token: 0x0600A2D2 RID: 41682 RVA: 0x003CE371 File Offset: 0x003CC571
	public static int Min(int a, int b, int c, int d)
	{
		if (a >= d && b >= d && c >= d)
		{
			return d;
		}
		if (a >= c && b >= c)
		{
			return c;
		}
		if (a < b)
		{
			return a;
		}
		return b;
	}

	// Token: 0x0600A2D3 RID: 41683 RVA: 0x003CE392 File Offset: 0x003CC592
	public static int Max(int a, int b)
	{
		if (a > b)
		{
			return a;
		}
		return b;
	}

	// Token: 0x0600A2D4 RID: 41684 RVA: 0x003CE39B File Offset: 0x003CC59B
	public static int Max(int a, int b, int c)
	{
		if (a <= c && b <= c)
		{
			return c;
		}
		if (a > b)
		{
			return a;
		}
		return b;
	}

	// Token: 0x0600A2D5 RID: 41685 RVA: 0x003CE3AE File Offset: 0x003CC5AE
	public static int Max(int a, int b, int c, int d)
	{
		if (a <= d && b <= d && c <= d)
		{
			return d;
		}
		if (a <= c && b <= c)
		{
			return c;
		}
		if (a > b)
		{
			return a;
		}
		return b;
	}

	// Token: 0x0600A2D6 RID: 41686 RVA: 0x003CE3D0 File Offset: 0x003CC5D0
	public static double RoundToSignificantDigits(double value, int digits)
	{
		if (value == 0.0)
		{
			return 0.0;
		}
		double num = Math.Pow(10.0, Math.Floor(Math.Log10(Math.Abs(value))) + 1.0);
		return num * Math.Round(value / num, digits);
	}

	// Token: 0x0600A2D7 RID: 41687 RVA: 0x003CE428 File Offset: 0x003CC628
	public static double TruncateToSignificantDigits(double value, int digits)
	{
		if (value == 0.0)
		{
			return 0.0;
		}
		double num = Math.Pow(10.0, Math.Floor(Math.Log10(Math.Abs(value))) + 1.0 - (double)digits);
		return num * Math.Truncate(value / num);
	}

	// Token: 0x0600A2D8 RID: 41688 RVA: 0x003CE484 File Offset: 0x003CC684
	public static void Swap(ref int x, ref int z)
	{
		int num = x;
		x = z;
		z = num;
	}

	// Token: 0x0600A2D9 RID: 41689 RVA: 0x003CE49B File Offset: 0x003CC69B
	public static int Mod(int _value, int _modulus)
	{
		return (_value % _modulus + _modulus) % _modulus;
	}

	// Token: 0x0600A2DA RID: 41690 RVA: 0x003CE49B File Offset: 0x003CC69B
	public static float Mod(float _value, float _modulus)
	{
		return (_value % _modulus + _modulus) % _modulus;
	}

	// Token: 0x0600A2DB RID: 41691 RVA: 0x003CE4A4 File Offset: 0x003CC6A4
	public static uint ToNextPowerOfTwo(uint _x, bool _allowCurrent = false)
	{
		if (_allowCurrent)
		{
			_x -= 1U;
		}
		_x |= _x >> 1;
		_x |= _x >> 2;
		_x |= _x >> 4;
		_x |= _x >> 8;
		_x |= _x >> 16;
		return _x + 1U;
	}

	// Token: 0x0600A2DC RID: 41692 RVA: 0x003CE4D5 File Offset: 0x003CC6D5
	public static uint ToPreviousPowerOfTwo(uint _x, bool _allowCurrent = false)
	{
		return MathUtils.ToNextPowerOfTwo(_x, !_allowCurrent) >> 1;
	}

	// Token: 0x0600A2DD RID: 41693 RVA: 0x003CE4E3 File Offset: 0x003CC6E3
	public static float ClampAxis(float Angle)
	{
		Angle %= 360f;
		if (Angle < 0f)
		{
			Angle += 360f;
		}
		return Angle;
	}

	// Token: 0x0600A2DE RID: 41694 RVA: 0x003CE500 File Offset: 0x003CC700
	public static float NormalizeAxis(float Angle)
	{
		Angle = MathUtils.ClampAxis(Angle);
		if (Angle > 180f)
		{
			Angle -= 360f;
		}
		return Angle;
	}

	// Token: 0x0600A2DF RID: 41695 RVA: 0x003CE51C File Offset: 0x003CC71C
	public static float YawForDirection(Vector3 direction)
	{
		float x = direction.x;
		return -(float)(Math.Atan2((double)direction.z, (double)x) * 180.0 / 3.141592653589793) + 90f;
	}

	// Token: 0x0600A2E0 RID: 41696 RVA: 0x003CE55C File Offset: 0x003CC75C
	public static float DistanceToSegment(Vector3 point, Vector3 start, Vector3 end)
	{
		Vector3 vector = end - start;
		float sqrMagnitude = vector.sqrMagnitude;
		if (sqrMagnitude == 0f)
		{
			return Vector3.Distance(point, start);
		}
		float num = Vector3.Dot(point - start, vector) / sqrMagnitude;
		num = Mathf.Clamp01(num);
		Vector3 vector2 = start + num * vector;
		return Vector3.Distance(point, vector2);
	}
}
