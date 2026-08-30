using System;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x02000CD9 RID: 3289
public struct WaterValue
{
	// Token: 0x060064B0 RID: 25776 RVA: 0x00275E4C File Offset: 0x0027404C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool HasMass()
	{
		return this.mass > 195;
	}

	// Token: 0x060064B1 RID: 25777 RVA: 0x00275E5B File Offset: 0x0027405B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetMass()
	{
		return (int)this.mass;
	}

	// Token: 0x060064B2 RID: 25778 RVA: 0x00275E63 File Offset: 0x00274063
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

	// Token: 0x060064B3 RID: 25779 RVA: 0x00275E9E File Offset: 0x0027409E
	public void SetMass(int value)
	{
		this.mass = (ushort)Utils.FastClamp(value, 0, 65535);
	}

	// Token: 0x060064B4 RID: 25780 RVA: 0x00275EB3 File Offset: 0x002740B3
	public override string ToString()
	{
		return string.Format("Raw Mass: {0:d}", this.mass);
	}

	// Token: 0x17000A7F RID: 2687
	// (get) Token: 0x060064B5 RID: 25781 RVA: 0x00275ECA File Offset: 0x002740CA
	public long RawData
	{
		get
		{
			return (long)((ulong)this.mass);
		}
	}

	// Token: 0x060064B6 RID: 25782 RVA: 0x00275ED4 File Offset: 0x002740D4
	public static WaterValue FromRawData(long rawData)
	{
		return new WaterValue
		{
			mass = (ushort)rawData
		};
	}

	// Token: 0x060064B7 RID: 25783 RVA: 0x00275EF3 File Offset: 0x002740F3
	public static WaterValue FromBlockType(int type)
	{
		if (type == 240 || type == 241 || type == 242)
		{
			return new WaterValue(19500);
		}
		return WaterValue.Empty;
	}

	// Token: 0x060064B8 RID: 25784 RVA: 0x00275F20 File Offset: 0x00274120
	public static WaterValue FromStream(BinaryReader _reader)
	{
		WaterValue waterValue = default(WaterValue);
		waterValue.Read(_reader);
		return waterValue;
	}

	// Token: 0x060064B9 RID: 25785 RVA: 0x00275F3E File Offset: 0x0027413E
	public WaterValue(BlockValue _bv)
	{
		this.mass = (_bv.isWater ? 19500 : 0);
	}

	// Token: 0x060064BA RID: 25786 RVA: 0x00275E9E File Offset: 0x0027409E
	public WaterValue(int mass)
	{
		this.mass = (ushort)Utils.FastClamp(mass, 0, 65535);
	}

	// Token: 0x060064BB RID: 25787 RVA: 0x00275F58 File Offset: 0x00274158
	public void Write(BinaryWriter writer)
	{
		writer.Write(this.mass);
	}

	// Token: 0x060064BC RID: 25788 RVA: 0x00275F66 File Offset: 0x00274166
	public void Read(BinaryReader reader)
	{
		this.mass = reader.ReadUInt16();
	}

	// Token: 0x060064BD RID: 25789 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public static int SerializedLength()
	{
		return 2;
	}

	// Token: 0x04004E1F RID: 19999
	public const float cTopPerCap = 0.6f;

	// Token: 0x04004E20 RID: 20000
	public const int MAX_MASS_VALUE = 65535;

	// Token: 0x04004E21 RID: 20001
	public static readonly WaterValue Empty = new WaterValue(0);

	// Token: 0x04004E22 RID: 20002
	public static readonly WaterValue Full = new WaterValue(19500);

	// Token: 0x04004E23 RID: 20003
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort mass;
}
