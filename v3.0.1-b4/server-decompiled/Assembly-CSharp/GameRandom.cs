using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020009E8 RID: 2536
public class GameRandom : IMemoryPoolableObject
{
	// Token: 0x06004BA6 RID: 19366 RVA: 0x001D5B2C File Offset: 0x001D3D2C
	public void SetSeed(int _seed)
	{
		this.InternalSetSeed(_seed);
	}

	// Token: 0x06004BA7 RID: 19367 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetLock()
	{
	}

	// Token: 0x06004BA8 RID: 19368 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06004BA9 RID: 19369 RVA: 0x000027FC File Offset: 0x000009FC
	public void Reset()
	{
	}

	// Token: 0x17000807 RID: 2055
	// (get) Token: 0x06004BAA RID: 19370 RVA: 0x001D5B35 File Offset: 0x001D3D35
	public double RandomDouble
	{
		get
		{
			return this.NextDouble();
		}
	}

	// Token: 0x17000808 RID: 2056
	// (get) Token: 0x06004BAB RID: 19371 RVA: 0x001D5B3D File Offset: 0x001D3D3D
	public float RandomFloat
	{
		get
		{
			return (float)this.NextDouble();
		}
	}

	// Token: 0x17000809 RID: 2057
	// (get) Token: 0x06004BAC RID: 19372 RVA: 0x001D5B46 File Offset: 0x001D3D46
	public int RandomInt
	{
		get
		{
			return this.Next();
		}
	}

	// Token: 0x1700080A RID: 2058
	// (get) Token: 0x06004BAD RID: 19373 RVA: 0x001D5B50 File Offset: 0x001D3D50
	public Vector2 RandomInsideUnitCircle
	{
		get
		{
			float num = (float)this.NextDouble() * 6.2831855f;
			return new Vector2(Mathf.Cos(num), Mathf.Sin(num)) * (float)Math.Sqrt(this.NextDouble());
		}
	}

	// Token: 0x1700080B RID: 2059
	// (get) Token: 0x06004BAE RID: 19374 RVA: 0x001D5B90 File Offset: 0x001D3D90
	public Vector2 RandomOnUnitCircle
	{
		get
		{
			float num = (float)this.NextDouble() * 6.2831855f;
			return new Vector2(Mathf.Cos(num), Mathf.Sin(num));
		}
	}

	// Token: 0x1700080C RID: 2060
	// (get) Token: 0x06004BAF RID: 19375 RVA: 0x001D5BBC File Offset: 0x001D3DBC
	public Vector3 RandomOnUnitCircleXZ
	{
		get
		{
			float num = (float)this.NextDouble() * 6.2831855f;
			return new Vector3(Mathf.Cos(num), 0f, Mathf.Sin(num));
		}
	}

	// Token: 0x1700080D RID: 2061
	// (get) Token: 0x06004BB0 RID: 19376 RVA: 0x001D5BF0 File Offset: 0x001D3DF0
	public Vector3 RandomInsideUnitSphere
	{
		get
		{
			return new Vector3((float)(this.NextDouble() - 0.5), (float)(this.NextDouble() - 0.5), (float)(this.NextDouble() - 0.5)).normalized * (float)Math.Sqrt(this.NextDouble());
		}
	}

	// Token: 0x1700080E RID: 2062
	// (get) Token: 0x06004BB1 RID: 19377 RVA: 0x001D5C50 File Offset: 0x001D3E50
	public Vector3 RandomOnUnitSphere
	{
		get
		{
			return new Vector3((float)(this.NextDouble() - 0.5), (float)(this.NextDouble() - 0.5), (float)(this.NextDouble() - 0.5)).normalized;
		}
	}

	// Token: 0x1700080F RID: 2063
	// (get) Token: 0x06004BB2 RID: 19378 RVA: 0x001D5CA0 File Offset: 0x001D3EA0
	public float RandomGaussian
	{
		get
		{
			float num;
			float num3;
			do
			{
				num = 2f * this.RandomRange(0f, 1f) - 1f;
				float num2 = 2f * this.RandomRange(0f, 1f) - 1f;
				num3 = num * num + num2 * num2;
			}
			while (num3 >= 1f || num3 == 0f);
			num3 = Mathf.Sqrt(-2f * Mathf.Log(num3) / num3);
			return num3 * num;
		}
	}

	// Token: 0x06004BB3 RID: 19379 RVA: 0x001D5D18 File Offset: 0x001D3F18
	public float RandomRange(float _maxExclusive)
	{
		return (float)(this.NextDouble() * (double)_maxExclusive);
	}

	// Token: 0x06004BB4 RID: 19380 RVA: 0x001D5D24 File Offset: 0x001D3F24
	public float RandomRange(float _min, float _maxExclusive)
	{
		return (float)(this.NextDouble() * (double)(_maxExclusive - _min) + (double)_min);
	}

	// Token: 0x06004BB5 RID: 19381 RVA: 0x001D5D35 File Offset: 0x001D3F35
	public int RandomRange(int _maxExclusive)
	{
		return this.Next(_maxExclusive);
	}

	// Token: 0x06004BB6 RID: 19382 RVA: 0x001D5D3E File Offset: 0x001D3F3E
	public int RandomRange(int _min, int _maxExclusive)
	{
		return this.Next(_maxExclusive - _min) + _min;
	}

	// Token: 0x06004BB7 RID: 19383 RVA: 0x001D5D4B File Offset: 0x001D3F4B
	[PublicizedFrom(EAccessModifier.Private)]
	public static void log(string _format, params object[] _values)
	{
		Log.Warning(string.Format("{0} GameRandom ", Time.time.ToCultureInvariantString()) + _format, _values);
	}

	// Token: 0x06004BB8 RID: 19384 RVA: 0x001D5D70 File Offset: 0x001D3F70
	[PublicizedFrom(EAccessModifier.Private)]
	public void InternalSetSeed(int Seed)
	{
		int num = ((Seed == int.MinValue) ? int.MaxValue : Math.Abs(Seed));
		int num2 = 161803398 - num;
		this.SeedArray[55] = num2;
		int num3 = 1;
		for (int i = 1; i < 55; i++)
		{
			int num4 = 21 * i % 55;
			this.SeedArray[num4] = num3;
			num3 = num2 - num3;
			if (num3 < 0)
			{
				num3 += int.MaxValue;
			}
			num2 = this.SeedArray[num4];
		}
		for (int j = 1; j < 5; j++)
		{
			for (int k = 1; k < 56; k++)
			{
				this.SeedArray[k] -= this.SeedArray[1 + (k + 30) % 55];
				if (this.SeedArray[k] < 0)
				{
					this.SeedArray[k] += int.MaxValue;
				}
			}
		}
		this.inext = 0;
		this.inextp = 21;
		Seed = 1;
	}

	// Token: 0x06004BB9 RID: 19385 RVA: 0x001D5E5A File Offset: 0x001D405A
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double Sample()
	{
		return (double)this.InternalSample() * 4.656612875245797E-10;
	}

	// Token: 0x06004BBA RID: 19386 RVA: 0x001D5E70 File Offset: 0x001D4070
	[PublicizedFrom(EAccessModifier.Private)]
	public int InternalSample()
	{
		int num = this.inext;
		int num2 = this.inextp;
		if (++num >= 56)
		{
			num = 1;
		}
		if (++num2 >= 56)
		{
			num2 = 1;
		}
		int num3 = this.SeedArray[num] - this.SeedArray[num2];
		if (num3 == 2147483647)
		{
			num3--;
		}
		if (num3 < 0)
		{
			num3 += int.MaxValue;
		}
		this.SeedArray[num] = num3;
		this.inext = num;
		this.inextp = num2;
		return num3;
	}

	// Token: 0x06004BBB RID: 19387 RVA: 0x001D5EE4 File Offset: 0x001D40E4
	public int PeekSample()
	{
		int num = this.inext;
		int num2 = this.inextp;
		if (++num >= 56)
		{
			num = 1;
		}
		if (++num2 >= 56)
		{
			num2 = 1;
		}
		int num3 = this.SeedArray[num] - this.SeedArray[num2];
		if (num3 == 2147483647)
		{
			num3--;
		}
		if (num3 < 0)
		{
			num3 += int.MaxValue;
		}
		return num3;
	}

	// Token: 0x06004BBC RID: 19388 RVA: 0x001D5F40 File Offset: 0x001D4140
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int Next()
	{
		return this.InternalSample();
	}

	// Token: 0x06004BBD RID: 19389 RVA: 0x001D5F48 File Offset: 0x001D4148
	[PublicizedFrom(EAccessModifier.Private)]
	public double GetSampleForLargeRange()
	{
		int num = this.InternalSample();
		if (this.InternalSample() % 2 == 0)
		{
			num = -num;
		}
		return ((double)num + 2147483646.0) / 4294967293.0;
	}

	// Token: 0x06004BBE RID: 19390 RVA: 0x001D5F88 File Offset: 0x001D4188
	[PublicizedFrom(EAccessModifier.Private)]
	public int Next(int minValue, int maxValue)
	{
		if (minValue > maxValue)
		{
			throw new ArgumentOutOfRangeException("minValue", "Argument_MinMaxValue");
		}
		long num = (long)maxValue - (long)minValue;
		if (num <= 2147483647L)
		{
			return (int)(this.Sample() * (double)num) + minValue;
		}
		return (int)((long)(this.GetSampleForLargeRange() * (double)num) + (long)minValue);
	}

	// Token: 0x06004BBF RID: 19391 RVA: 0x001D5FD3 File Offset: 0x001D41D3
	[PublicizedFrom(EAccessModifier.Private)]
	public int Next(int maxValue)
	{
		if (maxValue < 0)
		{
			throw new ArgumentOutOfRangeException("maxValue", "ArgumentOutOfRange_MustBePositive");
		}
		return (int)(this.Sample() * (double)maxValue);
	}

	// Token: 0x06004BC0 RID: 19392 RVA: 0x001D5FF3 File Offset: 0x001D41F3
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double NextDouble()
	{
		return this.Sample();
	}

	// Token: 0x06004BC1 RID: 19393 RVA: 0x001D5FFC File Offset: 0x001D41FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void NextBytes(byte[] buffer)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		for (int i = 0; i < buffer.Length; i++)
		{
			buffer[i] = (byte)(this.InternalSample() % 256);
		}
	}

	// Token: 0x04003B40 RID: 15168
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MBIG = 2147483647;

	// Token: 0x04003B41 RID: 15169
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MSEED = 161803398;

	// Token: 0x04003B42 RID: 15170
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MZ = 0;

	// Token: 0x04003B43 RID: 15171
	[PublicizedFrom(EAccessModifier.Private)]
	public int inext;

	// Token: 0x04003B44 RID: 15172
	[PublicizedFrom(EAccessModifier.Private)]
	public int inextp;

	// Token: 0x04003B45 RID: 15173
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] SeedArray = new int[56];
}
