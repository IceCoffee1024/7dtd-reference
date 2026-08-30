using System;
using UnityEngine;

// Token: 0x02001424 RID: 5156
public static class MathUtils
{
	// Token: 0x0600A1D1 RID: 41425 RVA: 0x003C8FF9 File Offset: 0x003C71F9
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

	// Token: 0x0600A1D2 RID: 41426 RVA: 0x003C9008 File Offset: 0x003C7208
	public static float Lerp(float a, float b, float x)
	{
		return a + (b - a) * x;
	}

	// Token: 0x0600A1D3 RID: 41427 RVA: 0x003C9011 File Offset: 0x003C7211
	public static int Min(int a, int b)
	{
		if (a < b)
		{
			return a;
		}
		return b;
	}

	// Token: 0x0600A1D4 RID: 41428 RVA: 0x003C901A File Offset: 0x003C721A
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

	// Token: 0x0600A1D5 RID: 41429 RVA: 0x003C902D File Offset: 0x003C722D
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

	// Token: 0x0600A1D6 RID: 41430 RVA: 0x003C904E File Offset: 0x003C724E
	public static int Max(int a, int b)
	{
		if (a > b)
		{
			return a;
		}
		return b;
	}

	// Token: 0x0600A1D7 RID: 41431 RVA: 0x003C9057 File Offset: 0x003C7257
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

	// Token: 0x0600A1D8 RID: 41432 RVA: 0x003C906A File Offset: 0x003C726A
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

	// Token: 0x0600A1D9 RID: 41433 RVA: 0x003C908C File Offset: 0x003C728C
	public static double RoundToSignificantDigits(double value, int digits)
	{
		if (value == 0.0)
		{
			return 0.0;
		}
		double num = Math.Pow(10.0, Math.Floor(Math.Log10(Math.Abs(value))) + 1.0);
		return num * Math.Round(value / num, digits);
	}

	// Token: 0x0600A1DA RID: 41434 RVA: 0x003C90E4 File Offset: 0x003C72E4
	public static double TruncateToSignificantDigits(double value, int digits)
	{
		if (value == 0.0)
		{
			return 0.0;
		}
		double num = Math.Pow(10.0, Math.Floor(Math.Log10(Math.Abs(value))) + 1.0 - (double)digits);
		return num * Math.Truncate(value / num);
	}

	// Token: 0x0600A1DB RID: 41435 RVA: 0x003C9140 File Offset: 0x003C7340
	public static void Swap(ref int x, ref int z)
	{
		int num = x;
		x = z;
		z = num;
	}

	// Token: 0x0600A1DC RID: 41436 RVA: 0x003C9157 File Offset: 0x003C7357
	public static int Mod(int _value, int _modulus)
	{
		return (_value % _modulus + _modulus) % _modulus;
	}

	// Token: 0x0600A1DD RID: 41437 RVA: 0x003C9157 File Offset: 0x003C7357
	public static float Mod(float _value, float _modulus)
	{
		return (_value % _modulus + _modulus) % _modulus;
	}

	// Token: 0x0600A1DE RID: 41438 RVA: 0x003C9160 File Offset: 0x003C7360
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

	// Token: 0x0600A1DF RID: 41439 RVA: 0x003C9191 File Offset: 0x003C7391
	public static uint ToPreviousPowerOfTwo(uint _x, bool _allowCurrent = false)
	{
		return MathUtils.ToNextPowerOfTwo(_x, !_allowCurrent) >> 1;
	}

	// Token: 0x0600A1E0 RID: 41440 RVA: 0x003C919F File Offset: 0x003C739F
	public static float ClampAxis(float Angle)
	{
		Angle %= 360f;
		if (Angle < 0f)
		{
			Angle += 360f;
		}
		return Angle;
	}

	// Token: 0x0600A1E1 RID: 41441 RVA: 0x003C91BC File Offset: 0x003C73BC
	public static float NormalizeAxis(float Angle)
	{
		Angle = MathUtils.ClampAxis(Angle);
		if (Angle > 180f)
		{
			Angle -= 360f;
		}
		return Angle;
	}

	// Token: 0x0600A1E2 RID: 41442 RVA: 0x003C91D8 File Offset: 0x003C73D8
	public static float YawForDirection(Vector3 direction)
	{
		float x = direction.x;
		return -(float)(Math.Atan2((double)direction.z, (double)x) * 180.0 / 3.141592653589793) + 90f;
	}

	// Token: 0x0600A1E3 RID: 41443 RVA: 0x003C9218 File Offset: 0x003C7418
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
