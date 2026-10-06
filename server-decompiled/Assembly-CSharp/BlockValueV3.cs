using System;
using System.Runtime.CompilerServices;

// Token: 0x02000167 RID: 359
public struct BlockValueV3
{
	// Token: 0x060009FA RID: 2554 RVA: 0x00043608 File Offset: 0x00041808
	public static uint ConvertOldRawData(uint _rawData)
	{
		BlockValueV3.convertBV3.rawData = _rawData;
		int type = BlockValueV3.convertBV3.type;
		BlockValueV3.convertBV.type = type;
		if (!BlockValueV3.convertBV3.ischild)
		{
			BlockValueV3.convertBV.rotation = BlockValueV3.convertBV3.rotation;
			BlockValueV3.convertBV.meta = BlockValueV3.convertBV3.meta;
			BlockValueV3.convertBV.meta2 = BlockValueV3.convertBV3.meta2;
		}
		else
		{
			BlockValueV3.convertBV.parent = BlockValueV3.convertBV3.parent;
			BlockValueV3.convertBV.ischild = true;
		}
		BlockValueV3.convertBV.hasdecal = BlockValueV3.convertBV3.hasdecal;
		return BlockValueV3.convertBV.rawData;
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x000436BD File Offset: 0x000418BD
	public BlockValueV3(uint _rawData)
	{
		this.rawData = _rawData;
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x060009FC RID: 2556 RVA: 0x000436C6 File Offset: 0x000418C6
	public Block Block
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return Block.list[this.type];
		}
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x000436D4 File Offset: 0x000418D4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint GetTypeMasked(uint _v)
	{
		return _v & 32767U;
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x060009FE RID: 2558 RVA: 0x000436DD File Offset: 0x000418DD
	public bool isair
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.type == 0;
		}
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x060009FF RID: 2559 RVA: 0x000436E8 File Offset: 0x000418E8
	public bool isWater
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.type == 240 || this.type == 241 || this.type == 242;
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06000A00 RID: 2560 RVA: 0x00043713 File Offset: 0x00041913
	// (set) Token: 0x06000A01 RID: 2561 RVA: 0x00043721 File Offset: 0x00041921
	public int type
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (int)(this.rawData & 32767U);
		}
		set
		{
			this.rawData = (this.rawData & 4294934528U) | (uint)((long)value & 32767L);
		}
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x06000A02 RID: 2562 RVA: 0x00043740 File Offset: 0x00041940
	// (set) Token: 0x06000A03 RID: 2563 RVA: 0x00043752 File Offset: 0x00041952
	public byte rotation
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (byte)((this.rawData & 1015808U) >> 15);
		}
		set
		{
			this.rawData = (this.rawData & 4293951487U) | (uint)((uint)(value & 31) << 15);
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0004376E File Offset: 0x0004196E
	// (set) Token: 0x06000A05 RID: 2565 RVA: 0x00043780 File Offset: 0x00041980
	public byte meta
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (byte)((this.rawData & 15728640U) >> 20);
		}
		set
		{
			this.rawData = (this.rawData & 4279238655U) | (uint)((uint)(value & 15) << 20);
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x06000A06 RID: 2566 RVA: 0x0004379C File Offset: 0x0004199C
	// (set) Token: 0x06000A07 RID: 2567 RVA: 0x000437AE File Offset: 0x000419AE
	public byte meta2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (byte)((this.rawData & 251658240U) >> 24);
		}
		set
		{
			this.rawData = (this.rawData & 4043309055U) | (uint)((uint)(value & 15) << 24);
		}
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x06000A08 RID: 2568 RVA: 0x000437CA File Offset: 0x000419CA
	// (set) Token: 0x06000A09 RID: 2569 RVA: 0x000437DC File Offset: 0x000419DC
	public byte meta3
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return (byte)((this.rawData & 805306368U) >> 28);
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.rawData = (this.rawData & 3489660927U) | (uint)((uint)(value & 3) << 28);
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x06000A0A RID: 2570 RVA: 0x000437F7 File Offset: 0x000419F7
	// (set) Token: 0x06000A0B RID: 2571 RVA: 0x00043809 File Offset: 0x00041A09
	public byte meta2and1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (byte)(((int)this.meta2 << 4) | (int)this.meta);
		}
		set
		{
			this.meta2 = (byte)((value >> 4) & 15);
			this.meta = value & 15;
		}
	}

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x06000A0C RID: 2572 RVA: 0x00043823 File Offset: 0x00041A23
	// (set) Token: 0x06000A0D RID: 2573 RVA: 0x00043835 File Offset: 0x00041A35
	public byte rotationAndMeta3
	{
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (byte)(((int)this.rotation << 2) | (int)this.meta3);
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.rotation = (byte)((long)(value >> 2) & 31L);
			this.meta3 = value;
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x06000A0E RID: 2574 RVA: 0x0004384D File Offset: 0x00041A4D
	// (set) Token: 0x06000A0F RID: 2575 RVA: 0x0004385E File Offset: 0x00041A5E
	public bool hasdecal
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.rawData & 2147483648U) > 0U;
		}
		set
		{
			this.rawData = (this.rawData & 2147483647U) | (value ? 2147483648U : 0U);
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0004387E File Offset: 0x00041A7E
	public BlockFaceFlag rotatedWaterFlowMask
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return BlockFaceFlags.RotateFlags(this.Block.WaterFlowMask, this.rotation);
		}
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000A11 RID: 2577 RVA: 0x0004376E File Offset: 0x0004196E
	// (set) Token: 0x06000A12 RID: 2578 RVA: 0x00043896 File Offset: 0x00041A96
	public BlockFace decalface
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (BlockFace)((this.rawData & 15728640U) >> 20);
		}
		set
		{
			this.rawData = (this.rawData & 4279238655U) | (uint)((uint)value << 20);
		}
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x06000A13 RID: 2579 RVA: 0x0004379C File Offset: 0x0004199C
	// (set) Token: 0x06000A14 RID: 2580 RVA: 0x000438AF File Offset: 0x00041AAF
	public byte decaltex
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (byte)((this.rawData & 251658240U) >> 24);
		}
		set
		{
			this.rawData = (this.rawData & 4043309055U) | (uint)((uint)value << 24);
		}
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x06000A15 RID: 2581 RVA: 0x000438C8 File Offset: 0x00041AC8
	// (set) Token: 0x06000A16 RID: 2582 RVA: 0x000438D9 File Offset: 0x00041AD9
	public bool ischild
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.rawData & 1073741824U) > 0U;
		}
		set
		{
			this.rawData = (this.rawData & 3221225471U) | (value ? 1073741824U : 0U);
		}
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x06000A17 RID: 2583 RVA: 0x000438FC File Offset: 0x00041AFC
	// (set) Token: 0x06000A18 RID: 2584 RVA: 0x0004392C File Offset: 0x00041B2C
	public int parentx
	{
		get
		{
			int num = (int)((this.rawData & 251658240U) >> 24);
			return ((num & 8) != 0) ? (-(num & 7)) : (num & 7);
		}
		set
		{
			int num = ((value < 0) ? (8 | (-value & 7)) : (value & 7));
			this.rawData = (this.rawData & 4043309055U) | (uint)((uint)num << 24);
		}
	}

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x06000A19 RID: 2585 RVA: 0x00043960 File Offset: 0x00041B60
	// (set) Token: 0x06000A1A RID: 2586 RVA: 0x0004398C File Offset: 0x00041B8C
	public int parenty
	{
		get
		{
			int rotationAndMeta = (int)this.rotationAndMeta3;
			return ((rotationAndMeta & 32) != 0) ? (-(rotationAndMeta & 31)) : (rotationAndMeta & 31);
		}
		set
		{
			int num = ((value < 0) ? (32 | (-value & 31)) : (value & 31));
			this.rotationAndMeta3 = (byte)num;
		}
	}

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x06000A1B RID: 2587 RVA: 0x000439B4 File Offset: 0x00041BB4
	// (set) Token: 0x06000A1C RID: 2588 RVA: 0x000439E4 File Offset: 0x00041BE4
	public int parentz
	{
		get
		{
			int num = (int)((this.rawData & 15728640U) >> 20);
			return ((num & 8) != 0) ? (-(num & 7)) : (num & 7);
		}
		set
		{
			int num = ((value < 0) ? (8 | (-value & 7)) : (value & 7));
			this.rawData = (this.rawData & 4279238655U) | (uint)((uint)num << 20);
		}
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x06000A1D RID: 2589 RVA: 0x00043A18 File Offset: 0x00041C18
	// (set) Token: 0x06000A1E RID: 2590 RVA: 0x00043A31 File Offset: 0x00041C31
	public Vector3i parent
	{
		get
		{
			return new Vector3i(this.parentx, this.parenty, this.parentz);
		}
		set
		{
			this.parentx = value.x;
			this.parenty = value.y;
			this.parentz = value.z;
		}
	}

	// Token: 0x04000A00 RID: 2560
	public const uint TypeMask = 32767U;

	// Token: 0x04000A01 RID: 2561
	public const uint RotationMax = 31U;

	// Token: 0x04000A02 RID: 2562
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint RotationMask = 1015808U;

	// Token: 0x04000A03 RID: 2563
	[PublicizedFrom(EAccessModifier.Private)]
	public const int RotationShift = 15;

	// Token: 0x04000A04 RID: 2564
	public const uint MetadataMax = 15U;

	// Token: 0x04000A05 RID: 2565
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint MetadataMask = 15728640U;

	// Token: 0x04000A06 RID: 2566
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MetadataShift = 20;

	// Token: 0x04000A07 RID: 2567
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint Metadata2Mask = 251658240U;

	// Token: 0x04000A08 RID: 2568
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Metadata2Shift = 24;

	// Token: 0x04000A09 RID: 2569
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint Metadata3Max = 3U;

	// Token: 0x04000A0A RID: 2570
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint Metadata3Mask = 805306368U;

	// Token: 0x04000A0B RID: 2571
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Metadata3Shift = 28;

	// Token: 0x04000A0C RID: 2572
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint ChildMask = 1073741824U;

	// Token: 0x04000A0D RID: 2573
	[PublicizedFrom(EAccessModifier.Private)]
	public const int ChildShift = 30;

	// Token: 0x04000A0E RID: 2574
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint HasDecalMask = 2147483648U;

	// Token: 0x04000A0F RID: 2575
	[PublicizedFrom(EAccessModifier.Private)]
	public const int HasDecalShift = 31;

	// Token: 0x04000A10 RID: 2576
	public uint rawData;

	// Token: 0x04000A11 RID: 2577
	[PublicizedFrom(EAccessModifier.Private)]
	public static BlockValueV3 convertBV3;

	// Token: 0x04000A12 RID: 2578
	[PublicizedFrom(EAccessModifier.Private)]
	public static BlockValue convertBV;
}
