using System;
using System.Runtime.CompilerServices;

// Token: 0x02000167 RID: 359
public struct BlockValueV3
{
	// Token: 0x060009FA RID: 2554 RVA: 0x000435C8 File Offset: 0x000417C8
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

	// Token: 0x060009FB RID: 2555 RVA: 0x0004367D File Offset: 0x0004187D
	public BlockValueV3(uint _rawData)
	{
		this.rawData = _rawData;
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x060009FC RID: 2556 RVA: 0x00043686 File Offset: 0x00041886
	public Block Block
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return Block.list[this.type];
		}
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x00043694 File Offset: 0x00041894
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint GetTypeMasked(uint _v)
	{
		return _v & 32767U;
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x060009FE RID: 2558 RVA: 0x0004369D File Offset: 0x0004189D
	public bool isair
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.type == 0;
		}
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x060009FF RID: 2559 RVA: 0x000436A8 File Offset: 0x000418A8
	public bool isWater
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.type == 240 || this.type == 241 || this.type == 242;
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06000A00 RID: 2560 RVA: 0x000436D3 File Offset: 0x000418D3
	// (set) Token: 0x06000A01 RID: 2561 RVA: 0x000436E1 File Offset: 0x000418E1
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
	// (get) Token: 0x06000A02 RID: 2562 RVA: 0x00043700 File Offset: 0x00041900
	// (set) Token: 0x06000A03 RID: 2563 RVA: 0x00043712 File Offset: 0x00041912
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
	// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0004372E File Offset: 0x0004192E
	// (set) Token: 0x06000A05 RID: 2565 RVA: 0x00043740 File Offset: 0x00041940
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
	// (get) Token: 0x06000A06 RID: 2566 RVA: 0x0004375C File Offset: 0x0004195C
	// (set) Token: 0x06000A07 RID: 2567 RVA: 0x0004376E File Offset: 0x0004196E
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
	// (get) Token: 0x06000A08 RID: 2568 RVA: 0x0004378A File Offset: 0x0004198A
	// (set) Token: 0x06000A09 RID: 2569 RVA: 0x0004379C File Offset: 0x0004199C
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
	// (get) Token: 0x06000A0A RID: 2570 RVA: 0x000437B7 File Offset: 0x000419B7
	// (set) Token: 0x06000A0B RID: 2571 RVA: 0x000437C9 File Offset: 0x000419C9
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
	// (get) Token: 0x06000A0C RID: 2572 RVA: 0x000437E3 File Offset: 0x000419E3
	// (set) Token: 0x06000A0D RID: 2573 RVA: 0x000437F5 File Offset: 0x000419F5
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
	// (get) Token: 0x06000A0E RID: 2574 RVA: 0x0004380D File Offset: 0x00041A0D
	// (set) Token: 0x06000A0F RID: 2575 RVA: 0x0004381E File Offset: 0x00041A1E
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
	// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0004383E File Offset: 0x00041A3E
	public BlockFaceFlag rotatedWaterFlowMask
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return BlockFaceFlags.RotateFlags(this.Block.WaterFlowMask, this.rotation);
		}
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000A11 RID: 2577 RVA: 0x0004372E File Offset: 0x0004192E
	// (set) Token: 0x06000A12 RID: 2578 RVA: 0x00043856 File Offset: 0x00041A56
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
	// (get) Token: 0x06000A13 RID: 2579 RVA: 0x0004375C File Offset: 0x0004195C
	// (set) Token: 0x06000A14 RID: 2580 RVA: 0x0004386F File Offset: 0x00041A6F
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
	// (get) Token: 0x06000A15 RID: 2581 RVA: 0x00043888 File Offset: 0x00041A88
	// (set) Token: 0x06000A16 RID: 2582 RVA: 0x00043899 File Offset: 0x00041A99
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
	// (get) Token: 0x06000A17 RID: 2583 RVA: 0x000438BC File Offset: 0x00041ABC
	// (set) Token: 0x06000A18 RID: 2584 RVA: 0x000438EC File Offset: 0x00041AEC
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
	// (get) Token: 0x06000A19 RID: 2585 RVA: 0x00043920 File Offset: 0x00041B20
	// (set) Token: 0x06000A1A RID: 2586 RVA: 0x0004394C File Offset: 0x00041B4C
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
	// (get) Token: 0x06000A1B RID: 2587 RVA: 0x00043974 File Offset: 0x00041B74
	// (set) Token: 0x06000A1C RID: 2588 RVA: 0x000439A4 File Offset: 0x00041BA4
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
	// (get) Token: 0x06000A1D RID: 2589 RVA: 0x000439D8 File Offset: 0x00041BD8
	// (set) Token: 0x06000A1E RID: 2590 RVA: 0x000439F1 File Offset: 0x00041BF1
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
