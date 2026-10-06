using System;

// Token: 0x02000B0D RID: 2829
public sealed class BlockChangeInfo
{
	// Token: 0x060053D6 RID: 21462 RVA: 0x00203CB9 File Offset: 0x00201EB9
	public BlockChangeInfo()
	{
		this.blockValueRef = BlockValueRef.None;
		this.blockValue = BlockValue.Air;
		this.density = MarchingCubes.DensityAir;
		this.changedByEntityId = -1;
	}

	// Token: 0x060053D7 RID: 21463 RVA: 0x00203CF0 File Offset: 0x00201EF0
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue)
	{
		this.blockValueRef = _bvRef;
		this.blockValue = _blockValue;
		this.bChangeBlockValue = true;
		this.bUpdateLight = false;
	}

	// Token: 0x060053D8 RID: 21464 RVA: 0x00203D1B File Offset: 0x00201F1B
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, bool _updateLight, bool _bOnlyDamage = false)
		: this(_bvRef, _blockValue, _updateLight)
	{
		this.bChangeDamage = _bOnlyDamage;
	}

	// Token: 0x060053D9 RID: 21465 RVA: 0x00203D2E File Offset: 0x00201F2E
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, int _changedEntityId)
		: this(_bvRef, _blockValue)
	{
		this.changedByEntityId = _changedEntityId;
	}

	// Token: 0x060053DA RID: 21466 RVA: 0x00203D3F File Offset: 0x00201F3F
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, bool _updateLight)
		: this(_bvRef, _blockValue)
	{
		this.bUpdateLight = _updateLight;
	}

	// Token: 0x060053DB RID: 21467 RVA: 0x00203D50 File Offset: 0x00201F50
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, bool _updateLight, int _changingEntityId)
		: this(_bvRef, _blockValue, _updateLight)
	{
		this.changedByEntityId = _changingEntityId;
	}

	// Token: 0x060053DC RID: 21468 RVA: 0x00203D63 File Offset: 0x00201F63
	public BlockChangeInfo(BlockValueRef _bvRef, sbyte _density, bool _bForceDensityChange = false)
	{
		this.blockValueRef = _bvRef;
		this.density = _density;
		this.bChangeDensity = true;
		this.bForceDensity = _bForceDensityChange;
		this.changedByEntityId = -1;
	}

	// Token: 0x060053DD RID: 21469 RVA: 0x00203D95 File Offset: 0x00201F95
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, sbyte _density)
	{
		this.blockValueRef = _bvRef;
		this.blockValue = _blockValue;
		this.bChangeBlockValue = true;
		this.density = _density;
		this.bChangeDensity = true;
		this.bUpdateLight = true;
		this.changedByEntityId = -1;
	}

	// Token: 0x060053DE RID: 21470 RVA: 0x00203DD5 File Offset: 0x00201FD5
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, sbyte _density, int _changedByEntityId)
		: this(_bvRef, _blockValue, _density)
	{
		this.changedByEntityId = _changedByEntityId;
	}

	// Token: 0x060053DF RID: 21471 RVA: 0x00203DE8 File Offset: 0x00201FE8
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, sbyte _density, TextureFullArray _tex)
		: this(_bvRef, _blockValue, _density)
	{
		this.bChangeTexture = true;
		this.textureFull = _tex;
		this.changedByEntityId = -1;
	}

	// Token: 0x060053E0 RID: 21472 RVA: 0x00203E0C File Offset: 0x0020200C
	public void Read(PooledBinaryReader _br)
	{
		this.blockValueRef = BlockValueRef.Read(_br);
		this.changedByEntityId = _br.ReadInt32();
		BlockChangeInfo.Flags flags = (BlockChangeInfo.Flags)_br.ReadByte();
		this.bChangeBlockValue = (flags & BlockChangeInfo.Flags.ChangeBlockValue) > (BlockChangeInfo.Flags)0;
		this.bChangeDensity = (flags & BlockChangeInfo.Flags.ChangeDensity) > (BlockChangeInfo.Flags)0;
		this.bForceDensity = (flags & BlockChangeInfo.Flags.ForceDensity) > (BlockChangeInfo.Flags)0;
		this.bUpdateLight = (flags & BlockChangeInfo.Flags.UpdateLight) > (BlockChangeInfo.Flags)0;
		this.bChangeDamage = (flags & BlockChangeInfo.Flags.ChangeDamage) > (BlockChangeInfo.Flags)0;
		this.bChangeTexture = (flags & BlockChangeInfo.Flags.ChangeTexture) > (BlockChangeInfo.Flags)0;
		if (this.bChangeBlockValue)
		{
			this.blockValue = BlockValue.Read(_br);
		}
		if (this.bChangeDensity)
		{
			this.density = _br.ReadSByte();
		}
		if (this.bChangeTexture)
		{
			this.textureFull.Read(_br, 1);
		}
	}

	// Token: 0x060053E1 RID: 21473 RVA: 0x00203EC0 File Offset: 0x002020C0
	public void Write(PooledBinaryWriter _bw)
	{
		this.blockValueRef.Write(_bw);
		_bw.Write(this.changedByEntityId);
		BlockChangeInfo.Flags flags = (BlockChangeInfo.Flags)0;
		flags |= (this.bChangeBlockValue ? BlockChangeInfo.Flags.ChangeBlockValue : ((BlockChangeInfo.Flags)0));
		flags |= (this.bChangeDensity ? BlockChangeInfo.Flags.ChangeDensity : ((BlockChangeInfo.Flags)0));
		flags |= (this.bForceDensity ? BlockChangeInfo.Flags.ForceDensity : ((BlockChangeInfo.Flags)0));
		flags |= (this.bUpdateLight ? BlockChangeInfo.Flags.UpdateLight : ((BlockChangeInfo.Flags)0));
		flags |= (this.bChangeDamage ? BlockChangeInfo.Flags.ChangeDamage : ((BlockChangeInfo.Flags)0));
		flags |= (this.bChangeTexture ? BlockChangeInfo.Flags.ChangeTexture : ((BlockChangeInfo.Flags)0));
		_bw.Write((byte)flags);
		if (this.bChangeBlockValue)
		{
			this.blockValue.Write(_bw);
		}
		if (this.bChangeDensity)
		{
			_bw.Write(this.density);
		}
		if (this.bChangeTexture)
		{
			this.textureFull.Write(_bw);
		}
	}

	// Token: 0x040041B9 RID: 16825
	public static BlockChangeInfo Empty = new BlockChangeInfo
	{
		blockValueRef = BlockValueRef.None
	};

	// Token: 0x040041BA RID: 16826
	public BlockValueRef blockValueRef;

	// Token: 0x040041BB RID: 16827
	public bool bChangeBlockValue;

	// Token: 0x040041BC RID: 16828
	public bool bChangeDamage;

	// Token: 0x040041BD RID: 16829
	public BlockValue blockValue;

	// Token: 0x040041BE RID: 16830
	public bool bChangeDensity;

	// Token: 0x040041BF RID: 16831
	public bool bForceDensity;

	// Token: 0x040041C0 RID: 16832
	public sbyte density;

	// Token: 0x040041C1 RID: 16833
	public bool bUpdateLight;

	// Token: 0x040041C2 RID: 16834
	public bool bChangeTexture;

	// Token: 0x040041C3 RID: 16835
	public TextureFullArray textureFull;

	// Token: 0x040041C4 RID: 16836
	public int changedByEntityId = -1;

	// Token: 0x02000B0E RID: 2830
	[Flags]
	[PublicizedFrom(EAccessModifier.Private)]
	public enum Flags : byte
	{
		// Token: 0x040041C6 RID: 16838
		ChangeBlockValue = 1,
		// Token: 0x040041C7 RID: 16839
		ChangeDamage = 2,
		// Token: 0x040041C8 RID: 16840
		ChangeDensity = 4,
		// Token: 0x040041C9 RID: 16841
		ForceDensity = 8,
		// Token: 0x040041CA RID: 16842
		UpdateLight = 16,
		// Token: 0x040041CB RID: 16843
		ChangeTexture = 32
	}
}
