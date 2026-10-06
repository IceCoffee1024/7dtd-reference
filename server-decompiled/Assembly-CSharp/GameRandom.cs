using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000A03 RID: 2563
public class GameRandom : IMemoryPoolableObject
{
	// Token: 0x06004BFB RID: 19451 RVA: 0x001D95C4 File Offset: 0x001D77C4
	public void SetSeed(int _seed)
	{
		this.InternalSetSeed(_seed);
	}

	// Token: 0x06004BFC RID: 19452 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetLock()
	{
	}

	// Token: 0x06004BFD RID: 19453 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06004BFE RID: 19454 RVA: 0x000027FC File Offset: 0x000009FC
	public void Reset()
	{
	}

	// Token: 0x17000839 RID: 2105
	// (get) Token: 0x06004BFF RID: 19455 RVA: 0x001D95CD File Offset: 0x001D77CD
	public double RandomDouble
	{
		get
		{
			return this.NextDouble();
		}
	}

	// Token: 0x1700083A RID: 2106
	// (get) Token: 0x06004C00 RID: 19456 RVA: 0x001D95D5 File Offset: 0x001D77D5
	public float RandomFloat
	{
		get
		{
			return (float)this.NextDouble();
		}
	}

	// Token: 0x1700083B RID: 2107
	// (get) Token: 0x06004C01 RID: 19457 RVA: 0x001D95DE File Offset: 0x001D77DE
	public int RandomInt
	{
		get
		{
			return this.Next();
		}
	}

	// Token: 0x1700083C RID: 2108
	// (get) Token: 0x06004C02 RID: 19458 RVA: 0x001D95E8 File Offset: 0x001D77E8
	public Vector2 RandomInsideUnitCircle
	{
		get
		{
			float num = (float)this.NextDouble() * 6.2831855f;
			return new Vector2(Mathf.Cos(num), Mathf.Sin(num)) * (float)Math.Sqrt(this.NextDouble());
		}
	}

	// Token: 0x1700083D RID: 2109
	// (get) Token: 0x06004C03 RID: 19459 RVA: 0x001D9628 File Offset: 0x001D7828
	public Vector2 RandomOnUnitCircle
	{
		get
		{
			float num = (float)this.NextDouble() * 6.2831855f;
			return new Vector2(Mathf.Cos(num), Mathf.Sin(num));
		}
	}

	// Token: 0x1700083E RID: 2110
	// (get) Token: 0x06004C04 RID: 19460 RVA: 0x001D9654 File Offset: 0x001D7854
	public Vector3 RandomOnUnitCircleXZ
	{
		get
		{
			float num = (float)this.NextDouble() * 6.2831855f;
			return new Vector3(Mathf.Cos(num), 0f, Mathf.Sin(num));
		}
	}

	// Token: 0x1700083F RID: 2111
	// (get) Token: 0x06004C05 RID: 19461 RVA: 0x001D9688 File Offset: 0x001D7888
	public Vector3 RandomInsideUnitSphere
	{
		get
		{
			return new Vector3((float)(this.NextDouble() - 0.5), (float)(this.NextDouble() - 0.5), (float)(this.NextDouble() - 0.5)).normalized * (float)Math.Sqrt(this.NextDouble());
		}
	}

	// Token: 0x17000840 RID: 2112
	// (get) Token: 0x06004C06 RID: 19462 RVA: 0x001D96E8 File Offset: 0x001D78E8
	public Vector3 RandomOnUnitSphere
	{
		get
		{
			return new Vector3((float)(this.NextDouble() - 0.5), (float)(this.NextDouble() - 0.5), (float)(this.NextDouble() - 0.5)).normalized;
		}
	}

	// Token: 0x17000841 RID: 2113
	// (get) Token: 0x06004C07 RID: 19463 RVA: 0x001D9738 File Offset: 0x001D7938
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

	// Token: 0x06004C08 RID: 19464 RVA: 0x001D97B0 File Offset: 0x001D79B0
	public float RandomRange(float _maxExclusive)
	{
		return (float)(this.NextDouble() * (double)_maxExclusive);
	}

	// Token: 0x06004C09 RID: 19465 RVA: 0x001D97BC File Offset: 0x001D79BC
	public float RandomRange(float _min, float _maxExclusive)
	{
		return (float)(this.NextDouble() * (double)(_maxExclusive - _min) + (double)_min);
	}

	// Token: 0x06004C0A RID: 19466 RVA: 0x001D97CD File Offset: 0x001D79CD
	public int RandomRange(int _maxExclusive)
	{
		return this.Next(_maxExclusive);
	}

	// Token: 0x06004C0B RID: 19467 RVA: 0x001D97D6 File Offset: 0x001D79D6
	public int RandomRange(int _min, int _maxExclusive)
	{
		return this.Next(_maxExclusive - _min) + _min;
	}

	// Token: 0x06004C0C RID: 19468 RVA: 0x001D97E3 File Offset: 0x001D79E3
	[PublicizedFrom(EAccessModifier.Private)]
	public static void log(string _format, params object[] _values)
	{
		Log.Warning(string.Format("{0} GameRandom ", Time.time.ToCultureInvariantString()) + _format, _values);
	}

	// Token: 0x06004C0D RID: 19469 RVA: 0x001D9808 File Offset: 0x001D7A08
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

	// Token: 0x06004C0E RID: 19470 RVA: 0x001D98F2 File Offset: 0x001D7AF2
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double Sample()
	{
		return (double)this.InternalSample() * 4.656612875245797E-10;
	}

	// Token: 0x06004C0F RID: 19471 RVA: 0x001D9908 File Offset: 0x001D7B08
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

	// Token: 0x06004C10 RID: 19472 RVA: 0x001D997C File Offset: 0x001D7B7C
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

	// Token: 0x06004C11 RID: 19473 RVA: 0x001D99D8 File Offset: 0x001D7BD8
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int Next()
	{
		return this.InternalSample();
	}

	// Token: 0x06004C12 RID: 19474 RVA: 0x001D99E0 File Offset: 0x001D7BE0
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

	// Token: 0x06004C13 RID: 19475 RVA: 0x001D9A20 File Offset: 0x001D7C20
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

	// Token: 0x06004C14 RID: 19476 RVA: 0x001D9A6B File Offset: 0x001D7C6B
	[PublicizedFrom(EAccessModifier.Private)]
	public int Next(int maxValue)
	{
		if (maxValue < 0)
		{
			throw new ArgumentOutOfRangeException("maxValue", "ArgumentOutOfRange_MustBePositive");
		}
		return (int)(this.Sample() * (double)maxValue);
	}

	// Token: 0x06004C15 RID: 19477 RVA: 0x001D9A8B File Offset: 0x001D7C8B
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double NextDouble()
	{
		return this.Sample();
	}

	// Token: 0x06004C16 RID: 19478 RVA: 0x001D9A94 File Offset: 0x001D7C94
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

	// Token: 0x04003BDE RID: 15326
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MBIG = 2147483647;

	// Token: 0x04003BDF RID: 15327
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MSEED = 161803398;

	// Token: 0x04003BE0 RID: 15328
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MZ = 0;

	// Token: 0x04003BE1 RID: 15329
	[PublicizedFrom(EAccessModifier.Private)]
	public int inext;

	// Token: 0x04003BE2 RID: 15330
	[PublicizedFrom(EAccessModifier.Private)]
	public int inextp;

	// Token: 0x04003BE3 RID: 15331
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] SeedArray = new int[56];
}
