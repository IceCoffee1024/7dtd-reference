using System;
using System.Runtime.CompilerServices;

// Token: 0x02000166 RID: 358
[Serializable]
public struct BlockValue : IEquatable<BlockValue>
{
	// Token: 0x060009C8 RID: 2504 RVA: 0x00043117 File Offset: 0x00041317
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BlockValue(uint _rawData)
	{
		this.rawData = _rawData;
		this.damage = 0;
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x00043127 File Offset: 0x00041327
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BlockValue(uint _rawData, int _damage)
	{
		this.rawData = _rawData;
		this.damage = _damage;
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x00043137 File Offset: 0x00041337
	public BlockValue set(int _type, byte _meta, byte _damage, byte _rotation)
	{
		this.type = _type;
		this.meta = _meta;
		this.damage = (int)_damage;
		this.rotation = _rotation;
		return this;
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x060009CB RID: 2507 RVA: 0x0004315C File Offset: 0x0004135C
	public Block Block
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return Block.list[this.type];
		}
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x0004316A File Offset: 0x0004136A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint GetTypeMasked(uint _v)
	{
		return _v & 65535U;
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x060009CD RID: 2509 RVA: 0x00043173 File Offset: 0x00041373
	public bool isair
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.type == 0;
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x060009CE RID: 2510 RVA: 0x0004317E File Offset: 0x0004137E
	public bool isTerrain
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.type - 1 < 239;
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x060009CF RID: 2511 RVA: 0x0004318F File Offset: 0x0004138F
	public bool isWater
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.type == 240 || this.type == 241 || this.type == 242;
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x060009D0 RID: 2512 RVA: 0x000431BA File Offset: 0x000413BA
	public bool validCover
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return !this.isair && !this.isWater && this.Block.MaxDamage != 1;
		}
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x060009D1 RID: 2513 RVA: 0x000431DF File Offset: 0x000413DF
	// (set) Token: 0x060009D2 RID: 2514 RVA: 0x000431ED File Offset: 0x000413ED
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
	// (get) Token: 0x060009D3 RID: 2515 RVA: 0x00043209 File Offset: 0x00041409
	// (set) Token: 0x060009D4 RID: 2516 RVA: 0x00043218 File Offset: 0x00041418
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
	// (get) Token: 0x060009D5 RID: 2517 RVA: 0x00043234 File Offset: 0x00041434
	public bool isRotated45Degrees
	{
		get
		{
			return this.rotation >= 24 && this.rotation <= 27;
		}
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x060009D6 RID: 2518 RVA: 0x0004324F File Offset: 0x0004144F
	// (set) Token: 0x060009D7 RID: 2519 RVA: 0x0004325E File Offset: 0x0004145E
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
	// (get) Token: 0x060009D8 RID: 2520 RVA: 0x0004327A File Offset: 0x0004147A
	// (set) Token: 0x060009D9 RID: 2521 RVA: 0x00043289 File Offset: 0x00041489
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
	// (get) Token: 0x060009DA RID: 2522 RVA: 0x000432A5 File Offset: 0x000414A5
	// (set) Token: 0x060009DB RID: 2523 RVA: 0x000432B7 File Offset: 0x000414B7
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
	// (get) Token: 0x060009DC RID: 2524 RVA: 0x000432D0 File Offset: 0x000414D0
	public byte meta3
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return (byte)((this.rawData >> 21) & 1U);
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x060009DD RID: 2525 RVA: 0x000432DE File Offset: 0x000414DE
	// (set) Token: 0x060009DE RID: 2526 RVA: 0x000432ED File Offset: 0x000414ED
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
	// (get) Token: 0x060009DF RID: 2527 RVA: 0x00043309 File Offset: 0x00041509
	// (set) Token: 0x060009E0 RID: 2528 RVA: 0x0004331A File Offset: 0x0004151A
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
	// (get) Token: 0x060009E1 RID: 2529 RVA: 0x0004333A File Offset: 0x0004153A
	public BlockFaceFlag rotatedWaterFlowMask
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return BlockFaceFlags.RotateFlags(this.Block.WaterFlowMask, this.rotation);
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x060009E2 RID: 2530 RVA: 0x0004324F File Offset: 0x0004144F
	// (set) Token: 0x060009E3 RID: 2531 RVA: 0x0004325E File Offset: 0x0004145E
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
	// (get) Token: 0x060009E4 RID: 2532 RVA: 0x0004327A File Offset: 0x0004147A
	// (set) Token: 0x060009E5 RID: 2533 RVA: 0x00043289 File Offset: 0x00041489
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
	// (get) Token: 0x060009E6 RID: 2534 RVA: 0x00043352 File Offset: 0x00041552
	// (set) Token: 0x060009E7 RID: 2535 RVA: 0x00043363 File Offset: 0x00041563
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
	// (get) Token: 0x060009E8 RID: 2536 RVA: 0x00043383 File Offset: 0x00041583
	// (set) Token: 0x060009E9 RID: 2537 RVA: 0x0004338D File Offset: 0x0004158D
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
	// (get) Token: 0x060009EA RID: 2538 RVA: 0x00043399 File Offset: 0x00041599
	// (set) Token: 0x060009EB RID: 2539 RVA: 0x000433A4 File Offset: 0x000415A4
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
	// (get) Token: 0x060009EC RID: 2540 RVA: 0x000433B1 File Offset: 0x000415B1
	// (set) Token: 0x060009ED RID: 2541 RVA: 0x000433BB File Offset: 0x000415BB
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
	// (get) Token: 0x060009EE RID: 2542 RVA: 0x000433C7 File Offset: 0x000415C7
	// (set) Token: 0x060009EF RID: 2543 RVA: 0x000433E0 File Offset: 0x000415E0
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

	// Token: 0x060009F0 RID: 2544 RVA: 0x00043408 File Offset: 0x00041608
	public static BlockValue Read(PooledBinaryReader br)
	{
		return new BlockValue
		{
			rawData = br.ReadUInt32(),
			damage = (int)br.ReadUInt16()
		};
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x00043438 File Offset: 0x00041638
	public readonly void Write(PooledBinaryWriter bw)
	{
		bw.Write(this.rawData);
		bw.Write((ushort)this.damage);
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x00043453 File Offset: 0x00041653
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetForceToOtherBlock(BlockValue _other)
	{
		return Utils.FastMin(this.Block.blockMaterial.StabilityGlue, _other.Block.blockMaterial.StabilityGlue);
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x0004347B File Offset: 0x0004167B
	public int ToItemType()
	{
		return this.type;
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x00043483 File Offset: 0x00041683
	public ItemValue ToItemValue()
	{
		return new ItemValue
		{
			type = this.type
		};
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x0004347B File Offset: 0x0004167B
	public override int GetHashCode()
	{
		return this.type;
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x00043498 File Offset: 0x00041698
	public override bool Equals(object _other)
	{
		return _other is BlockValue && ((BlockValue)_other).type == this.type;
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x000434C5 File Offset: 0x000416C5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(BlockValue _other)
	{
		return _other.type == this.type;
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x000434D8 File Offset: 0x000416D8
	public bool EqualsExceptRotation(BlockValue _other)
	{
		uint num = this.rawData & 4292935679U;
		uint num2 = _other.rawData & 4292935679U;
		return num == num2;
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x00043504 File Offset: 0x00041704
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

	// Token: 0x040009EA RID: 2538
	public const uint TypeMask = 65535U;

	// Token: 0x040009EB RID: 2539
	public const uint RotationMax = 31U;

	// Token: 0x040009EC RID: 2540
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint RotationMask = 2031616U;

	// Token: 0x040009ED RID: 2541
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int RotationShift = 16;

	// Token: 0x040009EE RID: 2542
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint Metadata3Max = 1U;

	// Token: 0x040009EF RID: 2543
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint Metadata3Mask = 2097152U;

	// Token: 0x040009F0 RID: 2544
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int Metadata3Shift = 21;

	// Token: 0x040009F1 RID: 2545
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint RotationMeta3Max = 63U;

	// Token: 0x040009F2 RID: 2546
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint RotationMeta3Mask = 4128768U;

	// Token: 0x040009F3 RID: 2547
	public const uint MetadataMax = 15U;

	// Token: 0x040009F4 RID: 2548
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint Metadata1Mask = 62914560U;

	// Token: 0x040009F5 RID: 2549
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int Metadata1Shift = 22;

	// Token: 0x040009F6 RID: 2550
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint Metadata2Mask = 1006632960U;

	// Token: 0x040009F7 RID: 2551
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int Metadata2Shift = 26;

	// Token: 0x040009F8 RID: 2552
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint Metadata12Max = 255U;

	// Token: 0x040009F9 RID: 2553
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint ChildMask = 1073741824U;

	// Token: 0x040009FA RID: 2554
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int ChildShift = 30;

	// Token: 0x040009FB RID: 2555
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const uint HasDecalMask = 2147483648U;

	// Token: 0x040009FC RID: 2556
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int HasDecalShift = 31;

	// Token: 0x040009FD RID: 2557
	public static BlockValue Air;

	// Token: 0x040009FE RID: 2558
	public uint rawData;

	// Token: 0x040009FF RID: 2559
	public int damage;
}
