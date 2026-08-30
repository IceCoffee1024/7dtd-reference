using System;
using System.Runtime.CompilerServices;

// Token: 0x02000165 RID: 357
public struct BlockValueV3
{
	// Token: 0x060009F4 RID: 2548 RVA: 0x00043530 File Offset: 0x00041730
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

	// Token: 0x060009F5 RID: 2549 RVA: 0x000435E5 File Offset: 0x000417E5
	public BlockValueV3(uint _rawData)
	{
		this.rawData = _rawData;
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x060009F6 RID: 2550 RVA: 0x000435EE File Offset: 0x000417EE
	public Block Block
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return Block.list[this.type];
		}
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x000435FC File Offset: 0x000417FC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint GetTypeMasked(uint _v)
	{
		return _v & 32767U;
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x060009F8 RID: 2552 RVA: 0x00043605 File Offset: 0x00041805
	public bool isair
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.type == 0;
		}
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x060009F9 RID: 2553 RVA: 0x00043610 File Offset: 0x00041810
	public bool isWater
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.type == 240 || this.type == 241 || this.type == 242;
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x060009FA RID: 2554 RVA: 0x0004363B File Offset: 0x0004183B
	// (set) Token: 0x060009FB RID: 2555 RVA: 0x00043649 File Offset: 0x00041849
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
	// (get) Token: 0x060009FC RID: 2556 RVA: 0x00043668 File Offset: 0x00041868
	// (set) Token: 0x060009FD RID: 2557 RVA: 0x0004367A File Offset: 0x0004187A
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
	// (get) Token: 0x060009FE RID: 2558 RVA: 0x00043696 File Offset: 0x00041896
	// (set) Token: 0x060009FF RID: 2559 RVA: 0x000436A8 File Offset: 0x000418A8
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
	// (get) Token: 0x06000A00 RID: 2560 RVA: 0x000436C4 File Offset: 0x000418C4
	// (set) Token: 0x06000A01 RID: 2561 RVA: 0x000436D6 File Offset: 0x000418D6
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
	// (get) Token: 0x06000A02 RID: 2562 RVA: 0x000436F2 File Offset: 0x000418F2
	// (set) Token: 0x06000A03 RID: 2563 RVA: 0x00043704 File Offset: 0x00041904
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
	// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0004371F File Offset: 0x0004191F
	// (set) Token: 0x06000A05 RID: 2565 RVA: 0x00043731 File Offset: 0x00041931
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
	// (get) Token: 0x06000A06 RID: 2566 RVA: 0x0004374B File Offset: 0x0004194B
	// (set) Token: 0x06000A07 RID: 2567 RVA: 0x0004375D File Offset: 0x0004195D
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
	// (get) Token: 0x06000A08 RID: 2568 RVA: 0x00043775 File Offset: 0x00041975
	// (set) Token: 0x06000A09 RID: 2569 RVA: 0x00043786 File Offset: 0x00041986
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
	// (get) Token: 0x06000A0A RID: 2570 RVA: 0x000437A6 File Offset: 0x000419A6
	public BlockFaceFlag rotatedWaterFlowMask
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return BlockFaceFlags.RotateFlags(this.Block.WaterFlowMask, this.rotation);
		}
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000A0B RID: 2571 RVA: 0x00043696 File Offset: 0x00041896
	// (set) Token: 0x06000A0C RID: 2572 RVA: 0x000437BE File Offset: 0x000419BE
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
	// (get) Token: 0x06000A0D RID: 2573 RVA: 0x000436C4 File Offset: 0x000418C4
	// (set) Token: 0x06000A0E RID: 2574 RVA: 0x000437D7 File Offset: 0x000419D7
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
	// (get) Token: 0x06000A0F RID: 2575 RVA: 0x000437F0 File Offset: 0x000419F0
	// (set) Token: 0x06000A10 RID: 2576 RVA: 0x00043801 File Offset: 0x00041A01
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
	// (get) Token: 0x06000A11 RID: 2577 RVA: 0x00043824 File Offset: 0x00041A24
	// (set) Token: 0x06000A12 RID: 2578 RVA: 0x00043854 File Offset: 0x00041A54
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
	// (get) Token: 0x06000A13 RID: 2579 RVA: 0x00043888 File Offset: 0x00041A88
	// (set) Token: 0x06000A14 RID: 2580 RVA: 0x000438B4 File Offset: 0x00041AB4
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
	// (get) Token: 0x06000A15 RID: 2581 RVA: 0x000438DC File Offset: 0x00041ADC
	// (set) Token: 0x06000A16 RID: 2582 RVA: 0x0004390C File Offset: 0x00041B0C
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
	// (get) Token: 0x06000A17 RID: 2583 RVA: 0x00043940 File Offset: 0x00041B40
	// (set) Token: 0x06000A18 RID: 2584 RVA: 0x00043959 File Offset: 0x00041B59
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

	// Token: 0x040009FD RID: 2557
	public const uint TypeMask = 32767U;

	// Token: 0x040009FE RID: 2558
	public const uint RotationMax = 31U;

	// Token: 0x040009FF RID: 2559
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint RotationMask = 1015808U;

	// Token: 0x04000A00 RID: 2560
	[PublicizedFrom(EAccessModifier.Private)]
	public const int RotationShift = 15;

	// Token: 0x04000A01 RID: 2561
	public const uint MetadataMax = 15U;

	// Token: 0x04000A02 RID: 2562
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint MetadataMask = 15728640U;

	// Token: 0x04000A03 RID: 2563
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MetadataShift = 20;

	// Token: 0x04000A04 RID: 2564
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint Metadata2Mask = 251658240U;

	// Token: 0x04000A05 RID: 2565
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Metadata2Shift = 24;

	// Token: 0x04000A06 RID: 2566
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint Metadata3Max = 3U;

	// Token: 0x04000A07 RID: 2567
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint Metadata3Mask = 805306368U;

	// Token: 0x04000A08 RID: 2568
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Metadata3Shift = 28;

	// Token: 0x04000A09 RID: 2569
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint ChildMask = 1073741824U;

	// Token: 0x04000A0A RID: 2570
	[PublicizedFrom(EAccessModifier.Private)]
	public const int ChildShift = 30;

	// Token: 0x04000A0B RID: 2571
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint HasDecalMask = 2147483648U;

	// Token: 0x04000A0C RID: 2572
	[PublicizedFrom(EAccessModifier.Private)]
	public const int HasDecalShift = 31;

	// Token: 0x04000A0D RID: 2573
	public uint rawData;

	// Token: 0x04000A0E RID: 2574
	[PublicizedFrom(EAccessModifier.Private)]
	public static BlockValueV3 convertBV3;

	// Token: 0x04000A0F RID: 2575
	[PublicizedFrom(EAccessModifier.Private)]
	public static BlockValue convertBV;
}
