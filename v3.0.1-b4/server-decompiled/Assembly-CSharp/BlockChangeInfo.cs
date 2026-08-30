using System;
using System.IO;

// Token: 0x02000AE9 RID: 2793
public sealed class BlockChangeInfo
{
	// Token: 0x06005335 RID: 21301 RVA: 0x001FDDE9 File Offset: 0x001FBFE9
	public BlockChangeInfo()
	{
		this.blockValueRef = BlockValueRef.None;
		this.blockValue = BlockValue.Air;
		this.density = MarchingCubes.DensityAir;
		this.changedByEntityId = -1;
	}

	// Token: 0x06005336 RID: 21302 RVA: 0x001FDE20 File Offset: 0x001FC020
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue)
	{
		this.blockValueRef = _bvRef;
		this.blockValue = _blockValue;
		this.bChangeBlockValue = true;
		this.bUpdateLight = false;
	}

	// Token: 0x06005337 RID: 21303 RVA: 0x001FDE4B File Offset: 0x001FC04B
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, bool _updateLight, bool _bOnlyDamage = false)
		: this(_bvRef, _blockValue, _updateLight)
	{
		this.bChangeDamage = _bOnlyDamage;
	}

	// Token: 0x06005338 RID: 21304 RVA: 0x001FDE5E File Offset: 0x001FC05E
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, int _changedEntityId)
		: this(_bvRef, _blockValue)
	{
		this.changedByEntityId = _changedEntityId;
	}

	// Token: 0x06005339 RID: 21305 RVA: 0x001FDE6F File Offset: 0x001FC06F
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, bool _updateLight)
		: this(_bvRef, _blockValue)
	{
		this.bUpdateLight = _updateLight;
	}

	// Token: 0x0600533A RID: 21306 RVA: 0x001FDE80 File Offset: 0x001FC080
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, bool _updateLight, int _changingEntityId)
		: this(_bvRef, _blockValue, _updateLight)
	{
		this.changedByEntityId = _changingEntityId;
	}

	// Token: 0x0600533B RID: 21307 RVA: 0x001FDE93 File Offset: 0x001FC093
	public BlockChangeInfo(BlockValueRef _bvRef, sbyte _density, bool _bForceDensityChange = false)
	{
		this.blockValueRef = _bvRef;
		this.density = _density;
		this.bChangeDensity = true;
		this.bForceDensity = _bForceDensityChange;
		this.changedByEntityId = -1;
	}

	// Token: 0x0600533C RID: 21308 RVA: 0x001FDEC5 File Offset: 0x001FC0C5
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

	// Token: 0x0600533D RID: 21309 RVA: 0x001FDF05 File Offset: 0x001FC105
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, sbyte _density, int _changedByEntityId)
		: this(_bvRef, _blockValue, _density)
	{
		this.changedByEntityId = _changedByEntityId;
	}

	// Token: 0x0600533E RID: 21310 RVA: 0x001FDF18 File Offset: 0x001FC118
	public BlockChangeInfo(BlockValueRef _bvRef, BlockValue _blockValue, sbyte _density, TextureFullArray _tex)
		: this(_bvRef, _blockValue, _density)
	{
		this.bChangeTexture = true;
		this.textureFull = _tex;
		this.changedByEntityId = -1;
	}

	// Token: 0x0600533F RID: 21311 RVA: 0x001FDF3C File Offset: 0x001FC13C
	public void Read(BinaryReader _br)
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

	// Token: 0x06005340 RID: 21312 RVA: 0x001FDFF0 File Offset: 0x001FC1F0
	public void Write(BinaryWriter _bw)
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

	// Token: 0x040040E6 RID: 16614
	public static BlockChangeInfo Empty = new BlockChangeInfo
	{
		blockValueRef = BlockValueRef.None
	};

	// Token: 0x040040E7 RID: 16615
	public BlockValueRef blockValueRef;

	// Token: 0x040040E8 RID: 16616
	public bool bChangeBlockValue;

	// Token: 0x040040E9 RID: 16617
	public bool bChangeDamage;

	// Token: 0x040040EA RID: 16618
	public BlockValue blockValue;

	// Token: 0x040040EB RID: 16619
	public bool bChangeDensity;

	// Token: 0x040040EC RID: 16620
	public bool bForceDensity;

	// Token: 0x040040ED RID: 16621
	public sbyte density;

	// Token: 0x040040EE RID: 16622
	public bool bUpdateLight;

	// Token: 0x040040EF RID: 16623
	public bool bChangeTexture;

	// Token: 0x040040F0 RID: 16624
	public TextureFullArray textureFull;

	// Token: 0x040040F1 RID: 16625
	public int changedByEntityId = -1;

	// Token: 0x02000AEA RID: 2794
	[Flags]
	[PublicizedFrom(EAccessModifier.Private)]
	public enum Flags : byte
	{
		// Token: 0x040040F3 RID: 16627
		ChangeBlockValue = 1,
		// Token: 0x040040F4 RID: 16628
		ChangeDamage = 2,
		// Token: 0x040040F5 RID: 16629
		ChangeDensity = 4,
		// Token: 0x040040F6 RID: 16630
		ForceDensity = 8,
		// Token: 0x040040F7 RID: 16631
		UpdateLight = 16,
		// Token: 0x040040F8 RID: 16632
		ChangeTexture = 32
	}
}
