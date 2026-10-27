using System;
using UnityEngine;

// Token: 0x02001471 RID: 5233
public static class MathUtils
{
	// Token: 0x0600A3D2 RID: 41938 RVA: 0x003D5351 File Offset: 0x003D3551
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

	// Token: 0x0600A3D3 RID: 41939 RVA: 0x003D5360 File Offset: 0x003D3560
	public static float Lerp(float a, float b, float x)
	{
		return a + (b - a) * x;
	}

	// Token: 0x0600A3D4 RID: 41940 RVA: 0x003D5369 File Offset: 0x003D3569
	public static int Min(int a, int b)
	{
		if (a < b)
		{
			return a;
		}
		return b;
	}

	// Token: 0x0600A3D5 RID: 41941 RVA: 0x003D5372 File Offset: 0x003D3572
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

	// Token: 0x0600A3D6 RID: 41942 RVA: 0x003D5385 File Offset: 0x003D3585
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

	// Token: 0x0600A3D7 RID: 41943 RVA: 0x003D53A6 File Offset: 0x003D35A6
	public static int Max(int a, int b)
	{
		if (a > b)
		{
			return a;
		}
		return b;
	}

	// Token: 0x0600A3D8 RID: 41944 RVA: 0x003D53AF File Offset: 0x003D35AF
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

	// Token: 0x0600A3D9 RID: 41945 RVA: 0x003D53C2 File Offset: 0x003D35C2
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

	// Token: 0x0600A3DA RID: 41946 RVA: 0x003D53E4 File Offset: 0x003D35E4
	public static double RoundToSignificantDigits(double value, int digits)
	{
		if (value == 0.0)
		{
			return 0.0;
		}
		double num = Math.Pow(10.0, Math.Floor(Math.Log10(Math.Abs(value))) + 1.0);
		return num * Math.Round(value / num, digits);
	}

	// Token: 0x0600A3DB RID: 41947 RVA: 0x003D543C File Offset: 0x003D363C
	public static double TruncateToSignificantDigits(double value, int digits)
	{
		if (value == 0.0)
		{
			return 0.0;
		}
		double num = Math.Pow(10.0, Math.Floor(Math.Log10(Math.Abs(value))) + 1.0 - (double)digits);
		return num * Math.Truncate(value / num);
	}

	// Token: 0x0600A3DC RID: 41948 RVA: 0x003D5498 File Offset: 0x003D3698
	public static void Swap(ref int x, ref int z)
	{
		int num = x;
		x = z;
		z = num;
	}

	// Token: 0x0600A3DD RID: 41949 RVA: 0x003D54AF File Offset: 0x003D36AF
	public static int Mod(int _value, int _modulus)
	{
		return (_value % _modulus + _modulus) % _modulus;
	}

	// Token: 0x0600A3DE RID: 41950 RVA: 0x003D54AF File Offset: 0x003D36AF
	public static float Mod(float _value, float _modulus)
	{
		return (_value % _modulus + _modulus) % _modulus;
	}

	// Token: 0x0600A3DF RID: 41951 RVA: 0x003D54B8 File Offset: 0x003D36B8
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

	// Token: 0x0600A3E0 RID: 41952 RVA: 0x003D54E9 File Offset: 0x003D36E9
	public static uint ToPreviousPowerOfTwo(uint _x, bool _allowCurrent = false)
	{
		return MathUtils.ToNextPowerOfTwo(_x, !_allowCurrent) >> 1;
	}

	// Token: 0x0600A3E1 RID: 41953 RVA: 0x003D54F7 File Offset: 0x003D36F7
	public static float ClampAxis(float Angle)
	{
		Angle %= 360f;
		if (Angle < 0f)
		{
			Angle += 360f;
		}
		return Angle;
	}

	// Token: 0x0600A3E2 RID: 41954 RVA: 0x003D5514 File Offset: 0x003D3714
	public static float NormalizeAxis(float Angle)
	{
		Angle = MathUtils.ClampAxis(Angle);
		if (Angle > 180f)
		{
			Angle -= 360f;
		}
		return Angle;
	}

	// Token: 0x0600A3E3 RID: 41955 RVA: 0x003D5530 File Offset: 0x003D3730
	public static float YawForDirection(Vector3 direction)
	{
		float x = direction.x;
		return -(float)(Math.Atan2((double)direction.z, (double)x) * 180.0 / 3.141592653589793) + 90f;
	}

	// Token: 0x0600A3E4 RID: 41956 RVA: 0x003D5570 File Offset: 0x003D3770
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
