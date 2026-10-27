using System;
using System.Runtime.CompilerServices;

// Token: 0x02000D04 RID: 3332
public struct WaterValue
{
	// Token: 0x06006553 RID: 25939 RVA: 0x0027D614 File Offset: 0x0027B814
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool HasMass()
	{
		return this.mass > 195;
	}

	// Token: 0x06006554 RID: 25940 RVA: 0x0027D623 File Offset: 0x0027B823
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetMass()
	{
		return (int)this.mass;
	}

	// Token: 0x06006555 RID: 25941 RVA: 0x0027D62B File Offset: 0x0027B82B
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

	// Token: 0x06006556 RID: 25942 RVA: 0x0027D666 File Offset: 0x0027B866
	public void SetMass(int value)
	{
		this.mass = (ushort)Utils.FastClamp(value, 0, 65535);
	}

	// Token: 0x06006557 RID: 25943 RVA: 0x0027D67B File Offset: 0x0027B87B
	public override string ToString()
	{
		return string.Format("Raw Mass: {0:d}", this.mass);
	}

	// Token: 0x17000AAE RID: 2734
	// (get) Token: 0x06006558 RID: 25944 RVA: 0x0027D692 File Offset: 0x0027B892
	public long RawData
	{
		get
		{
			return (long)((ulong)this.mass);
		}
	}

	// Token: 0x06006559 RID: 25945 RVA: 0x0027D69C File Offset: 0x0027B89C
	public static WaterValue FromRawData(long rawData)
	{
		return new WaterValue
		{
			mass = (ushort)rawData
		};
	}

	// Token: 0x0600655A RID: 25946 RVA: 0x0027D6BB File Offset: 0x0027B8BB
	public static WaterValue FromBlockType(int type)
	{
		if (type == 240 || type == 241 || type == 242)
		{
			return new WaterValue(19500);
		}
		return WaterValue.Empty;
	}

	// Token: 0x0600655B RID: 25947 RVA: 0x0027D6E8 File Offset: 0x0027B8E8
	public static WaterValue FromStream(PooledBinaryReader _reader)
	{
		WaterValue waterValue = default(WaterValue);
		waterValue.Read(_reader);
		return waterValue;
	}

	// Token: 0x0600655C RID: 25948 RVA: 0x0027D706 File Offset: 0x0027B906
	public WaterValue(BlockValue _bv)
	{
		this.mass = (_bv.isWater ? 19500 : 0);
	}

	// Token: 0x0600655D RID: 25949 RVA: 0x0027D666 File Offset: 0x0027B866
	public WaterValue(int mass)
	{
		this.mass = (ushort)Utils.FastClamp(mass, 0, 65535);
	}

	// Token: 0x0600655E RID: 25950 RVA: 0x0027D720 File Offset: 0x0027B920
	public void Write(PooledBinaryWriter writer)
	{
		writer.Write(this.mass);
	}

	// Token: 0x0600655F RID: 25951 RVA: 0x0027D72E File Offset: 0x0027B92E
	public void Read(PooledBinaryReader reader)
	{
		this.mass = reader.ReadUInt16();
	}

	// Token: 0x06006560 RID: 25952 RVA: 0x0002F260 File Offset: 0x0002D460
	public static int SerializedLength()
	{
		return 2;
	}

	// Token: 0x04004F18 RID: 20248
	public const float cTopPerCap = 0.6f;

	// Token: 0x04004F19 RID: 20249
	public const int MAX_MASS_VALUE = 65535;

	// Token: 0x04004F1A RID: 20250
	public static readonly WaterValue Empty = new WaterValue(0);

	// Token: 0x04004F1B RID: 20251
	public static readonly WaterValue Full = new WaterValue(19500);

	// Token: 0x04004F1C RID: 20252
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort mass;
}
