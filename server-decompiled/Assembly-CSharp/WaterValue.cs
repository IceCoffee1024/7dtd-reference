using System;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x02000CEB RID: 3307
public struct WaterValue
{
	// Token: 0x0600653C RID: 25916 RVA: 0x00278D48 File Offset: 0x00276F48
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool HasMass()
	{
		return this.mass > 195;
	}

	// Token: 0x0600653D RID: 25917 RVA: 0x00278D57 File Offset: 0x00276F57
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetMass()
	{
		return (int)this.mass;
	}

	// Token: 0x0600653E RID: 25918 RVA: 0x00278D5F File Offset: 0x00276F5F
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

	// Token: 0x0600653F RID: 25919 RVA: 0x00278D9A File Offset: 0x00276F9A
	public void SetMass(int value)
	{
		this.mass = (ushort)Utils.FastClamp(value, 0, 65535);
	}

	// Token: 0x06006540 RID: 25920 RVA: 0x00278DAF File Offset: 0x00276FAF
	public override string ToString()
	{
		return string.Format("Raw Mass: {0:d}", this.mass);
	}

	// Token: 0x17000A92 RID: 2706
	// (get) Token: 0x06006541 RID: 25921 RVA: 0x00278DC6 File Offset: 0x00276FC6
	public long RawData
	{
		get
		{
			return (long)((ulong)this.mass);
		}
	}

	// Token: 0x06006542 RID: 25922 RVA: 0x00278DD0 File Offset: 0x00276FD0
	public static WaterValue FromRawData(long rawData)
	{
		return new WaterValue
		{
			mass = (ushort)rawData
		};
	}

	// Token: 0x06006543 RID: 25923 RVA: 0x00278DEF File Offset: 0x00276FEF
	public static WaterValue FromBlockType(int type)
	{
		if (type == 240 || type == 241 || type == 242)
		{
			return new WaterValue(19500);
		}
		return WaterValue.Empty;
	}

	// Token: 0x06006544 RID: 25924 RVA: 0x00278E1C File Offset: 0x0027701C
	public static WaterValue FromStream(BinaryReader _reader)
	{
		WaterValue waterValue = default(WaterValue);
		waterValue.Read(_reader);
		return waterValue;
	}

	// Token: 0x06006545 RID: 25925 RVA: 0x00278E3A File Offset: 0x0027703A
	public WaterValue(BlockValue _bv)
	{
		this.mass = (_bv.isWater ? 19500 : 0);
	}

	// Token: 0x06006546 RID: 25926 RVA: 0x00278D9A File Offset: 0x00276F9A
	public WaterValue(int mass)
	{
		this.mass = (ushort)Utils.FastClamp(mass, 0, 65535);
	}

	// Token: 0x06006547 RID: 25927 RVA: 0x00278E54 File Offset: 0x00277054
	public void Write(BinaryWriter writer)
	{
		writer.Write(this.mass);
	}

	// Token: 0x06006548 RID: 25928 RVA: 0x00278E62 File Offset: 0x00277062
	public void Read(BinaryReader reader)
	{
		this.mass = reader.ReadUInt16();
	}

	// Token: 0x06006549 RID: 25929 RVA: 0x0002F168 File Offset: 0x0002D368
	public static int SerializedLength()
	{
		return 2;
	}

	// Token: 0x04004EBD RID: 20157
	public const float cTopPerCap = 0.6f;

	// Token: 0x04004EBE RID: 20158
	public const int MAX_MASS_VALUE = 65535;

	// Token: 0x04004EBF RID: 20159
	public static readonly WaterValue Empty = new WaterValue(0);

	// Token: 0x04004EC0 RID: 20160
	public static readonly WaterValue Full = new WaterValue(19500);

	// Token: 0x04004EC1 RID: 20161
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort mass;
}
