using System;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x02000163 RID: 355
[Serializable]
public struct BlockValue : IEquatable<BlockValue>
{
	// Token: 0x060009B8 RID: 2488 RVA: 0x00042DDF File Offset: 0x00040FDF
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BlockValue(uint _rawData)
	{
		this.rawData = _rawData;
		this.damage = 0;
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x00042DEF File Offset: 0x00040FEF
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BlockValue(uint _rawData, int _damage)
	{
		this.rawData = _rawData;
		this.damage = _damage;
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x00042DFF File Offset: 0x00040FFF
	public BlockValue set(int _type, byte _meta, byte _damage, byte _rotation)
	{
		this.type = _type;
		this.meta = _meta;
		this.damage = (int)_damage;
		this.rotation = _rotation;
		return this;
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x060009BB RID: 2491 RVA: 0x00042E24 File Offset: 0x00041024
	public Block Block
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return Block.list[this.type];
		}
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x00042E32 File Offset: 0x00041032
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint GetTypeMasked(uint _v)
	{
		return _v & 65535U;
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x060009BD RID: 2493 RVA: 0x00042E3B File Offset: 0x0004103B
	public bool isair
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.type == 0;
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x060009BE RID: 2494 RVA: 0x00042E46 File Offset: 0x00041046
	public bool isTerrain
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.type - 1 < 239;
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x060009BF RID: 2495 RVA: 0x00042E57 File Offset: 0x00041057
	public bool isWater
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.type == 240 || this.type == 241 || this.type == 242;
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x060009C0 RID: 2496 RVA: 0x00042E82 File Offset: 0x00041082
	public bool validCover
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return !this.isair && !this.isWater && this.Block.MaxDamage != 1;
		}
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x060009C1 RID: 2497 RVA: 0x00042EA7 File Offset: 0x000410A7
	// (set) Token: 0x060009C2 RID: 2498 RVA: 0x00042EB5 File Offset: 0x000410B5
	public int type
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (int)(this.rawData & 65535U);
		}
		set
		{
			this.rawData = (this.rawData & 4294901760U) | (uint)(value & 65535);
		}
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x060009C3 RID: 2499 RVA: 0x00042ED1 File Offset: 0x000410D1
	// (set) Token: 0x060009C4 RID: 2500 RVA: 0x00042EE0 File Offset: 0x000410E0
	public byte rotation
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (byte)((this.rawData >> 16) & 31U);
		}
		set
		{
			this.rawData = (this.rawData & 4292935679U) | (uint)((uint)(value & 31) << 16);
		}
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x060009C5 RID: 2501 RVA: 0x00042EFC File Offset: 0x000410FC
	public bool isRotated45Degrees
	{
		get
		{
			return this.rotation >= 24 && this.rotation <= 27;
		}
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x060009C6 RID: 2502 RVA: 0x00042F17 File Offset: 0x00041117
	// (set) Token: 0x060009C7 RID: 2503 RVA: 0x00042F26 File Offset: 0x00041126
	public byte meta
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (byte)((this.rawData >> 22) & 15U);
		}
		set
		{
			this.rawData = (this.rawData & 4232052735U) | (uint)((uint)(value & 15) << 22);
		}
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x060009C8 RID: 2504 RVA: 0x00042F42 File Offset: 0x00041142
	// (set) Token: 0x060009C9 RID: 2505 RVA: 0x00042F51 File Offset: 0x00041151
	public byte meta2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (byte)((this.rawData >> 26) & 15U);
		}
		set
		{
			this.rawData = (this.rawData & 3288334335U) | (uint)((uint)(value & 15) << 26);
		}
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x060009CA RID: 2506 RVA: 0x00042F6D File Offset: 0x0004116D
	// (set) Token: 0x060009CB RID: 2507 RVA: 0x00042F7F File Offset: 0x0004117F
	public byte meta2and1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (byte)((this.rawData >> 22) & 255U);
		}
		set
		{
			this.rawData = (this.rawData & 3225419775U) | (uint)((uint)value << 22);
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x060009CC RID: 2508 RVA: 0x00042F98 File Offset: 0x00041198
	public byte meta3
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return (byte)((this.rawData >> 21) & 1U);
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x060009CD RID: 2509 RVA: 0x00042FA6 File Offset: 0x000411A6
	// (set) Token: 0x060009CE RID: 2510 RVA: 0x00042FB5 File Offset: 0x000411B5
	public byte rotationAndMeta3
	{
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (byte)((this.rawData >> 16) & 63U);
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.rawData = (this.rawData & 4290838527U) | (uint)((uint)(value & 63) << 16);
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x060009CF RID: 2511 RVA: 0x00042FD1 File Offset: 0x000411D1
	// (set) Token: 0x060009D0 RID: 2512 RVA: 0x00042FE2 File Offset: 0x000411E2
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

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x060009D1 RID: 2513 RVA: 0x00043002 File Offset: 0x00041202
	public BlockFaceFlag rotatedWaterFlowMask
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return BlockFaceFlags.RotateFlags(this.Block.WaterFlowMask, this.rotation);
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x060009D2 RID: 2514 RVA: 0x00042F17 File Offset: 0x00041117
	// (set) Token: 0x060009D3 RID: 2515 RVA: 0x00042F26 File Offset: 0x00041126
	public BlockFace decalface
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (BlockFace)((this.rawData >> 22) & 15U);
		}
		set
		{
			this.rawData = (this.rawData & 4232052735U) | (uint)((uint)(value & (BlockFace)15) << 22);
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x060009D4 RID: 2516 RVA: 0x00042F42 File Offset: 0x00041142
	// (set) Token: 0x060009D5 RID: 2517 RVA: 0x00042F51 File Offset: 0x00041151
	public byte decaltex
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (byte)((this.rawData >> 26) & 15U);
		}
		set
		{
			this.rawData = (this.rawData & 3288334335U) | (uint)((uint)(value & 15) << 26);
		}
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x060009D6 RID: 2518 RVA: 0x0004301A File Offset: 0x0004121A
	// (set) Token: 0x060009D7 RID: 2519 RVA: 0x0004302B File Offset: 0x0004122B
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

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x060009D8 RID: 2520 RVA: 0x0004304B File Offset: 0x0004124B
	// (set) Token: 0x060009D9 RID: 2521 RVA: 0x00043055 File Offset: 0x00041255
	public int parentx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (int)(this.meta - 8);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			this.meta = (byte)(value + 8);
		}
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x060009DA RID: 2522 RVA: 0x00043061 File Offset: 0x00041261
	// (set) Token: 0x060009DB RID: 2523 RVA: 0x0004306C File Offset: 0x0004126C
	public int parenty
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (int)(this.rotationAndMeta3 - 32);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			this.rotationAndMeta3 = (byte)(value + 32);
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x060009DC RID: 2524 RVA: 0x00043079 File Offset: 0x00041279
	// (set) Token: 0x060009DD RID: 2525 RVA: 0x00043083 File Offset: 0x00041283
	public int parentz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (int)(this.meta2 - 8);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			this.meta2 = (byte)(value + 8);
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x060009DE RID: 2526 RVA: 0x0004308F File Offset: 0x0004128F
	// (set) Token: 0x060009DF RID: 2527 RVA: 0x000430A8 File Offset: 0x000412A8
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

	// Token: 0x060009E0 RID: 2528 RVA: 0x000430D0 File Offset: 0x000412D0
	public static BlockValue Read(BinaryReader br)
	{
		return new BlockValue
		{
			rawData = br.ReadUInt32(),
			damage = (int)br.ReadUInt16()
		};
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x00043100 File Offset: 0x00041300
	public readonly void Write(BinaryWriter bw)
	{
		bw.Write(this.rawData);
		bw.Write((ushort)this.damage);
	}

	// Token: 0x060009E2 RID: 2530 RVA: 0x0004311B File Offset: 0x0004131B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetForceToOtherBlock(BlockValue _other)
	{
		return Utils.FastMin(this.Block.blockMaterial.StabilityGlue, _other.Block.blockMaterial.StabilityGlue);
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x00043143 File Offset: 0x00041343
	public int ToItemType()
	{
		return this.type;
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x0004314B File Offset: 0x0004134B
	public ItemValue ToItemValue()
	{
		return new ItemValue
		{
			type = this.type
		};
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x00043143 File Offset: 0x00041343
	public override int GetHashCode()
	{
		return this.type;
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x00043160 File Offset: 0x00041360
	public override bool Equals(object _other)
	{
		return _other is BlockValue && ((BlockValue)_other).type == this.type;
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x0004318D File Offset: 0x0004138D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(BlockValue _other)
	{
		return _other.type == this.type;
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x000431A0 File Offset: 0x000413A0
	public bool EqualsExceptRotation(BlockValue _other)
	{
		uint num = this.rawData & 4292935679U;
		uint num2 = _other.rawData & 4292935679U;
		return num == num2;
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x000431CC File Offset: 0x000413CC
	public override string ToString()
	{
		if (!this.ischild)
		{
			string text = "id={0} r={1} d={2} m={3} m2={4} m3={5} name={6}";
			object[] array = new object[7];
			array[0] = this.type;
			array[1] = this.rotation;
			array[2] = this.damage;
			array[3] = this.meta;
			array[4] = this.meta2;
			array[5] = this.meta3;
			int num = 6;
			Block block = this.Block;
			array[num] = ((block != null) ? block.GetBlockName() : null) ?? "-null-";
			return string.Format(text, array);
		}
		string text2 = "id={0} px={1} py={2} pz={3} name={4}";
		object[] array2 = new object[5];
		array2[0] = this.type;
		array2[1] = this.parentx;
		array2[2] = this.parenty;
		array2[3] = this.parentz;
		int num2 = 4;
		Block block2 = this.Block;
		array2[num2] = ((block2 != null) ? block2.GetBlockName() : null) ?? "-null-";
		return string.Format(text2, array2);
	}

	// Token: 0x040009E5 RID: 2533
	public const uint TypeMask = 65535U;

	// Token: 0x040009E6 RID: 2534
	public const uint RotationMax = 31U;

	// Token: 0x040009E7 RID: 2535
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint RotationMask = 2031616U;

	// Token: 0x040009E8 RID: 2536
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int RotationShift = 16;

	// Token: 0x040009E9 RID: 2537
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint Metadata3Max = 1U;

	// Token: 0x040009EA RID: 2538
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint Metadata3Mask = 2097152U;

	// Token: 0x040009EB RID: 2539
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int Metadata3Shift = 21;

	// Token: 0x040009EC RID: 2540
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint RotationMeta3Max = 63U;

	// Token: 0x040009ED RID: 2541
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint RotationMeta3Mask = 4128768U;

	// Token: 0x040009EE RID: 2542
	public const uint MetadataMax = 15U;

	// Token: 0x040009EF RID: 2543
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint Metadata1Mask = 62914560U;

	// Token: 0x040009F0 RID: 2544
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int Metadata1Shift = 22;

	// Token: 0x040009F1 RID: 2545
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint Metadata2Mask = 1006632960U;

	// Token: 0x040009F2 RID: 2546
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int Metadata2Shift = 26;

	// Token: 0x040009F3 RID: 2547
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint Metadata12Max = 255U;

	// Token: 0x040009F4 RID: 2548
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint ChildMask = 1073741824U;

	// Token: 0x040009F5 RID: 2549
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int ChildShift = 30;

	// Token: 0x040009F6 RID: 2550
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint HasDecalMask = 2147483648U;

	// Token: 0x040009F7 RID: 2551
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int HasDecalShift = 31;

	// Token: 0x040009F8 RID: 2552
	public static BlockValue Air;

	// Token: 0x040009F9 RID: 2553
	public uint rawData;

	// Token: 0x040009FA RID: 2554
	public int damage;
}
