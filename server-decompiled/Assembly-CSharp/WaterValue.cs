using System;
using System.Runtime.CompilerServices;

// Token: 0x02000CF9 RID: 3321
public struct WaterValue
{
	// Token: 0x0600652E RID: 25902 RVA: 0x0027C0EC File Offset: 0x0027A2EC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool HasMass()
	{
		return this.mass > 195;
	}

	// Token: 0x0600652F RID: 25903 RVA: 0x0027C0FB File Offset: 0x0027A2FB
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetMass()
	{
		return (int)this.mass;
	}

	// Token: 0x06006530 RID: 25904 RVA: 0x0027C103 File Offset: 0x0027A303
	public float GetMassPercent()
	{
		if (this.mass <= 195)
		{
			return 0f;
		}
		if (this.mass >= 15600)
		{
			return 1f;
		}
		return (float)(this.mass - 195) / 15405f;
	}

	// Token: 0x06006531 RID: 25905 RVA: 0x0027C13E File Offset: 0x0027A33E
	public void SetMass(int value)
	{
		this.mass = (ushort)Utils.FastClamp(value, 0, 65535);
	}

	// Token: 0x06006532 RID: 25906 RVA: 0x0027C153 File Offset: 0x0027A353
	public override string ToString()
	{
		return string.Format("Raw Mass: {0:d}", this.mass);
	}

	// Token: 0x17000AA8 RID: 2728
	// (get) Token: 0x06006533 RID: 25907 RVA: 0x0027C16A File Offset: 0x0027A36A
	public long RawData
	{
		get
		{
			return (long)((ulong)this.mass);
		}
	}

	// Token: 0x06006534 RID: 25908 RVA: 0x0027C174 File Offset: 0x0027A374
	public static WaterValue FromRawData(long rawData)
	{
		return new WaterValue
		{
			mass = (ushort)rawData
		};
	}

	// Token: 0x06006535 RID: 25909 RVA: 0x0027C193 File Offset: 0x0027A393
	public static WaterValue FromBlockType(int type)
	{
		if (type == 240 || type == 241 || type == 242)
		{
			return new WaterValue(19500);
		}
		return WaterValue.Empty;
	}

	// Token: 0x06006536 RID: 25910 RVA: 0x0027C1C0 File Offset: 0x0027A3C0
	public static WaterValue FromStream(PooledBinaryReader _reader)
	{
		WaterValue waterValue = default(WaterValue);
		waterValue.Read(_reader);
		return waterValue;
	}

	// Token: 0x06006537 RID: 25911 RVA: 0x0027C1DE File Offset: 0x0027A3DE
	public WaterValue(BlockValue _bv)
	{
		this.mass = (_bv.isWater ? 19500 : 0);
	}

	// Token: 0x06006538 RID: 25912 RVA: 0x0027C13E File Offset: 0x0027A33E
	public WaterValue(int mass)
	{
		this.mass = (ushort)Utils.FastClamp(mass, 0, 65535);
	}

	// Token: 0x06006539 RID: 25913 RVA: 0x0027C1F8 File Offset: 0x0027A3F8
	public void Write(PooledBinaryWriter writer)
	{
		writer.Write(this.mass);
	}

	// Token: 0x0600653A RID: 25914 RVA: 0x0027C206 File Offset: 0x0027A406
	public void Read(PooledBinaryReader reader)
	{
		this.mass = reader.ReadUInt16();
	}

	// Token: 0x0600653B RID: 25915 RVA: 0x0002F260 File Offset: 0x0002D460
	public static int SerializedLength()
	{
		return 2;
	}

	// Token: 0x04004EF3 RID: 20211
	public const float cTopPerCap = 0.6f;

	// Token: 0x04004EF4 RID: 20212
	public const int MAX_MASS_VALUE = 65535;

	// Token: 0x04004EF5 RID: 20213
	public static readonly WaterValue Empty = new WaterValue(0);

	// Token: 0x04004EF6 RID: 20214
	public static readonly WaterValue Full = new WaterValue(19500);

	// Token: 0x04004EF7 RID: 20215
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort mass;
}
