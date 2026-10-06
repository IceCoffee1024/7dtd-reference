using System;
using UnityEngine;

// Token: 0x0200145F RID: 5215
public static class MathUtils
{
	// Token: 0x0600A37A RID: 41850 RVA: 0x003D2F4D File Offset: 0x003D114D
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

	// Token: 0x0600A37B RID: 41851 RVA: 0x003D2F5C File Offset: 0x003D115C
	public static float Lerp(float a, float b, float x)
	{
		return a + (b - a) * x;
	}

	// Token: 0x0600A37C RID: 41852 RVA: 0x003D2F65 File Offset: 0x003D1165
	public static int Min(int a, int b)
	{
		if (a < b)
		{
			return a;
		}
		return b;
	}

	// Token: 0x0600A37D RID: 41853 RVA: 0x003D2F6E File Offset: 0x003D116E
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

	// Token: 0x0600A37E RID: 41854 RVA: 0x003D2F81 File Offset: 0x003D1181
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

	// Token: 0x0600A37F RID: 41855 RVA: 0x003D2FA2 File Offset: 0x003D11A2
	public static int Max(int a, int b)
	{
		if (a > b)
		{
			return a;
		}
		return b;
	}

	// Token: 0x0600A380 RID: 41856 RVA: 0x003D2FAB File Offset: 0x003D11AB
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

	// Token: 0x0600A381 RID: 41857 RVA: 0x003D2FBE File Offset: 0x003D11BE
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

	// Token: 0x0600A382 RID: 41858 RVA: 0x003D2FE0 File Offset: 0x003D11E0
	public static double RoundToSignificantDigits(double value, int digits)
	{
		if (value == 0.0)
		{
			return 0.0;
		}
		double num = Math.Pow(10.0, Math.Floor(Math.Log10(Math.Abs(value))) + 1.0);
		return num * Math.Round(value / num, digits);
	}

	// Token: 0x0600A383 RID: 41859 RVA: 0x003D3038 File Offset: 0x003D1238
	public static double TruncateToSignificantDigits(double value, int digits)
	{
		if (value == 0.0)
		{
			return 0.0;
		}
		double num = Math.Pow(10.0, Math.Floor(Math.Log10(Math.Abs(value))) + 1.0 - (double)digits);
		return num * Math.Truncate(value / num);
	}

	// Token: 0x0600A384 RID: 41860 RVA: 0x003D3094 File Offset: 0x003D1294
	public static void Swap(ref int x, ref int z)
	{
		int num = x;
		x = z;
		z = num;
	}

	// Token: 0x0600A385 RID: 41861 RVA: 0x003D30AB File Offset: 0x003D12AB
	public static int Mod(int _value, int _modulus)
	{
		return (_value % _modulus + _modulus) % _modulus;
	}

	// Token: 0x0600A386 RID: 41862 RVA: 0x003D30AB File Offset: 0x003D12AB
	public static float Mod(float _value, float _modulus)
	{
		return (_value % _modulus + _modulus) % _modulus;
	}

	// Token: 0x0600A387 RID: 41863 RVA: 0x003D30B4 File Offset: 0x003D12B4
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

	// Token: 0x0600A388 RID: 41864 RVA: 0x003D30E5 File Offset: 0x003D12E5
	public static uint ToPreviousPowerOfTwo(uint _x, bool _allowCurrent = false)
	{
		return MathUtils.ToNextPowerOfTwo(_x, !_allowCurrent) >> 1;
	}

	// Token: 0x0600A389 RID: 41865 RVA: 0x003D30F3 File Offset: 0x003D12F3
	public static float ClampAxis(float Angle)
	{
		Angle %= 360f;
		if (Angle < 0f)
		{
			Angle += 360f;
		}
		return Angle;
	}

	// Token: 0x0600A38A RID: 41866 RVA: 0x003D3110 File Offset: 0x003D1310
	public static float NormalizeAxis(float Angle)
	{
		Angle = MathUtils.ClampAxis(Angle);
		if (Angle > 180f)
		{
			Angle -= 360f;
		}
		return Angle;
	}

	// Token: 0x0600A38B RID: 41867 RVA: 0x003D312C File Offset: 0x003D132C
	public static float YawForDirection(Vector3 direction)
	{
		float x = direction.x;
		return -(float)(Math.Atan2((double)direction.z, (double)x) * 180.0 / 3.141592653589793) + 90f;
	}

	// Token: 0x0600A38C RID: 41868 RVA: 0x003D316C File Offset: 0x003D136C
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
