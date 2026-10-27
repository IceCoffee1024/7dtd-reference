using System;

// Token: 0x02000B18 RID: 2840
public sealed class BlockChangeInfo
{
	// Token: 0x060053FB RID: 21499 RVA: 0x0020523D File Offset: 0x0020343D
	public BlockChangeInfo()
	{
		this.blockValueRef = BlockValueRef.None;
		this.blockValue = BlockValue.Air;
		this.density = MarchingCubes.DensityAir;
		this.changedByEntityId = -1;
	}

	// Token: 0x060053FC RID: 21500 RVA: 0x00205274 File Offset: 0x00203474
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue)
	{
		this.blockValueRef = _bvRef;
		this.blockValue = _blockValue;
		this.bChangeBlockValue = true;
		this.bUpdateLight = false;
	}

	// Token: 0x060053FD RID: 21501 RVA: 0x0020529F File Offset: 0x0020349F
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, bool _updateLight, bool _bOnlyDamage = false)
		: this(_bvRef, _blockValue, _updateLight)
	{
		this.bChangeDamage = _bOnlyDamage;
	}

	// Token: 0x060053FE RID: 21502 RVA: 0x002052B2 File Offset: 0x002034B2
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, int _changedEntityId)
		: this(_bvRef, _blockValue)
	{
		this.changedByEntityId = _changedEntityId;
	}

	// Token: 0x060053FF RID: 21503 RVA: 0x002052C3 File Offset: 0x002034C3
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, bool _updateLight)
		: this(_bvRef, _blockValue)
	{
		this.bUpdateLight = _updateLight;
	}

	// Token: 0x06005400 RID: 21504 RVA: 0x002052D4 File Offset: 0x002034D4
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, bool _updateLight, int _changingEntityId)
		: this(_bvRef, _blockValue, _updateLight)
	{
		this.changedByEntityId = _changingEntityId;
	}

	// Token: 0x06005401 RID: 21505 RVA: 0x002052E7 File Offset: 0x002034E7
	public BlockChangeInfo(BlockValueRef _bvRef, sbyte _density, bool _bForceDensityChange = false)
	{
		this.blockValueRef = _bvRef;
		this.density = _density;
		this.bChangeDensity = true;
		this.bForceDensity = _bForceDensityChange;
		this.changedByEntityId = -1;
	}

	// Token: 0x06005402 RID: 21506 RVA: 0x00205319 File Offset: 0x00203519
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

	// Token: 0x06005403 RID: 21507 RVA: 0x00205359 File Offset: 0x00203559
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, sbyte _density, int _changedByEntityId)
		: this(_bvRef, _blockValue, _density)
	{
		this.changedByEntityId = _changedByEntityId;
	}

	// Token: 0x06005404 RID: 21508 RVA: 0x0020536C File Offset: 0x0020356C
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, sbyte _density, TextureFullArray _tex)
		: this(_bvRef, _blockValue, _density)
	{
		this.bChangeTexture = true;
		this.textureFull = _tex;
		this.changedByEntityId = -1;
	}

	// Token: 0x06005405 RID: 21509 RVA: 0x00205390 File Offset: 0x00203590
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

	// Token: 0x06005406 RID: 21510 RVA: 0x00205444 File Offset: 0x00203644
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

	// Token: 0x040041DE RID: 16862
	public static BlockChangeInfo Empty = new BlockChangeInfo
	{
		blockValueRef = BlockValueRef.None
	};

	// Token: 0x040041DF RID: 16863
	public BlockValueRef blockValueRef;

	// Token: 0x040041E0 RID: 16864
	public bool bChangeBlockValue;

	// Token: 0x040041E1 RID: 16865
	public bool bChangeDamage;

	// Token: 0x040041E2 RID: 16866
	public BlockValue blockValue;

	// Token: 0x040041E3 RID: 16867
	public bool bChangeDensity;

	// Token: 0x040041E4 RID: 16868
	public bool bForceDensity;

	// Token: 0x040041E5 RID: 16869
	public sbyte density;

	// Token: 0x040041E6 RID: 16870
	public bool bUpdateLight;

	// Token: 0x040041E7 RID: 16871
	public bool bChangeTexture;

	// Token: 0x040041E8 RID: 16872
	public TextureFullArray textureFull;

	// Token: 0x040041E9 RID: 16873
	public int changedByEntityId = -1;

	// Token: 0x02000B19 RID: 2841
	[Flags]
	[PublicizedFrom(EAccessModifier.Private)]
	public enum Flags : byte
	{
		// Token: 0x040041EB RID: 16875
		ChangeBlockValue = 1,
		// Token: 0x040041EC RID: 16876
		ChangeDamage = 2,
		// Token: 0x040041ED RID: 16877
		ChangeDensity = 4,
		// Token: 0x040041EE RID: 16878
		ForceDensity = 8,
		// Token: 0x040041EF RID: 16879
		UpdateLight = 16,
		// Token: 0x040041F0 RID: 16880
		ChangeTexture = 32
	}
}
