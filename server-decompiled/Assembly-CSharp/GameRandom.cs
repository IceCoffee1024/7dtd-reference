using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000A0D RID: 2573
public class GameRandom : IMemoryPoolableObject
{
	// Token: 0x06004C1F RID: 19487 RVA: 0x001DA6AC File Offset: 0x001D88AC
	public void SetSeed(int _seed)
	{
		this.InternalSetSeed(_seed);
	}

	// Token: 0x06004C20 RID: 19488 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetLock()
	{
	}

	// Token: 0x06004C21 RID: 19489 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06004C22 RID: 19490 RVA: 0x000027FC File Offset: 0x000009FC
	public void Reset()
	{
	}

	// Token: 0x1700083F RID: 2111
	// (get) Token: 0x06004C23 RID: 19491 RVA: 0x001DA6B5 File Offset: 0x001D88B5
	public double RandomDouble
	{
		get
		{
			return this.NextDouble();
		}
	}

	// Token: 0x17000840 RID: 2112
	// (get) Token: 0x06004C24 RID: 19492 RVA: 0x001DA6BD File Offset: 0x001D88BD
	public float RandomFloat
	{
		get
		{
			return (float)this.NextDouble();
		}
	}

	// Token: 0x17000841 RID: 2113
	// (get) Token: 0x06004C25 RID: 19493 RVA: 0x001DA6C6 File Offset: 0x001D88C6
	public int RandomInt
	{
		get
		{
			return this.Next();
		}
	}

	// Token: 0x17000842 RID: 2114
	// (get) Token: 0x06004C26 RID: 19494 RVA: 0x001DA6D0 File Offset: 0x001D88D0
	public Vector2 RandomInsideUnitCircle
	{
		get
		{
			float num = (float)this.NextDouble() * 6.2831855f;
			return new Vector2(Mathf.Cos(num), Mathf.Sin(num)) * (float)Math.Sqrt(this.NextDouble());
		}
	}

	// Token: 0x17000843 RID: 2115
	// (get) Token: 0x06004C27 RID: 19495 RVA: 0x001DA710 File Offset: 0x001D8910
	public Vector2 RandomOnUnitCircle
	{
		get
		{
			float num = (float)this.NextDouble() * 6.2831855f;
			return new Vector2(Mathf.Cos(num), Mathf.Sin(num));
		}
	}

	// Token: 0x17000844 RID: 2116
	// (get) Token: 0x06004C28 RID: 19496 RVA: 0x001DA73C File Offset: 0x001D893C
	public Vector3 RandomOnUnitCircleXZ
	{
		get
		{
			float num = (float)this.NextDouble() * 6.2831855f;
			return new Vector3(Mathf.Cos(num), 0f, Mathf.Sin(num));
		}
	}

	// Token: 0x17000845 RID: 2117
	// (get) Token: 0x06004C29 RID: 19497 RVA: 0x001DA770 File Offset: 0x001D8970
	public Vector3 RandomInsideUnitSphere
	{
		get
		{
			return new Vector3((float)(this.NextDouble() - 0.5), (float)(this.NextDouble() - 0.5), (float)(this.NextDouble() - 0.5)).normalized * (float)Math.Sqrt(this.NextDouble());
		}
	}

	// Token: 0x17000846 RID: 2118
	// (get) Token: 0x06004C2A RID: 19498 RVA: 0x001DA7D0 File Offset: 0x001D89D0
	public Vector3 RandomOnUnitSphere
	{
		get
		{
			return new Vector3((float)(this.NextDouble() - 0.5), (float)(this.NextDouble() - 0.5), (float)(this.NextDouble() - 0.5)).normalized;
		}
	}

	// Token: 0x17000847 RID: 2119
	// (get) Token: 0x06004C2B RID: 19499 RVA: 0x001DA820 File Offset: 0x001D8A20
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

	// Token: 0x06004C2C RID: 19500 RVA: 0x001DA898 File Offset: 0x001D8A98
	public float RandomRange(float _maxExclusive)
	{
		return (float)(this.NextDouble() * (double)_maxExclusive);
	}

	// Token: 0x06004C2D RID: 19501 RVA: 0x001DA8A4 File Offset: 0x001D8AA4
	public float RandomRange(float _min, float _maxExclusive)
	{
		return (float)(this.NextDouble() * (double)(_maxExclusive - _min) + (double)_min);
	}

	// Token: 0x06004C2E RID: 19502 RVA: 0x001DA8B5 File Offset: 0x001D8AB5
	public int RandomRange(int _maxExclusive)
	{
		return this.Next(_maxExclusive);
	}

	// Token: 0x06004C2F RID: 19503 RVA: 0x001DA8BE File Offset: 0x001D8ABE
	public int RandomRange(int _min, int _maxExclusive)
	{
		return this.Next(_maxExclusive - _min) + _min;
	}

	// Token: 0x06004C30 RID: 19504 RVA: 0x001DA8CB File Offset: 0x001D8ACB
	[PublicizedFrom(EAccessModifier.Private)]
	public static void log(string _format, params object[] _values)
	{
		Log.Warning(string.Format("{0} GameRandom ", Time.time.ToCultureInvariantString()) + _format, _values);
	}

	// Token: 0x06004C31 RID: 19505 RVA: 0x001DA8F0 File Offset: 0x001D8AF0
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

	// Token: 0x06004C32 RID: 19506 RVA: 0x001DA9DA File Offset: 0x001D8BDA
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double Sample()
	{
		return (double)this.InternalSample() * 4.656612875245797E-10;
	}

	// Token: 0x06004C33 RID: 19507 RVA: 0x001DA9F0 File Offset: 0x001D8BF0
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

	// Token: 0x06004C34 RID: 19508 RVA: 0x001DAA64 File Offset: 0x001D8C64
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

	// Token: 0x06004C35 RID: 19509 RVA: 0x001DAAC0 File Offset: 0x001D8CC0
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int Next()
	{
		return this.InternalSample();
	}

	// Token: 0x06004C36 RID: 19510 RVA: 0x001DAAC8 File Offset: 0x001D8CC8
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

	// Token: 0x06004C37 RID: 19511 RVA: 0x001DAB08 File Offset: 0x001D8D08
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

	// Token: 0x06004C38 RID: 19512 RVA: 0x001DAB53 File Offset: 0x001D8D53
	[PublicizedFrom(EAccessModifier.Private)]
	public int Next(int maxValue)
	{
		if (maxValue < 0)
		{
			throw new ArgumentOutOfRangeException("maxValue", "ArgumentOutOfRange_MustBePositive");
		}
		return (int)(this.Sample() * (double)maxValue);
	}

	// Token: 0x06004C39 RID: 19513 RVA: 0x001DAB73 File Offset: 0x001D8D73
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double NextDouble()
	{
		return this.Sample();
	}

	// Token: 0x06004C3A RID: 19514 RVA: 0x001DAB7C File Offset: 0x001D8D7C
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

	// Token: 0x04003BFF RID: 15359
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MBIG = 2147483647;

	// Token: 0x04003C00 RID: 15360
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MSEED = 161803398;

	// Token: 0x04003C01 RID: 15361
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MZ = 0;

	// Token: 0x04003C02 RID: 15362
	[PublicizedFrom(EAccessModifier.Private)]
	public int inext;

	// Token: 0x04003C03 RID: 15363
	[PublicizedFrom(EAccessModifier.Private)]
	public int inextp;

	// Token: 0x04003C04 RID: 15364
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] SeedArray = new int[56];
}
