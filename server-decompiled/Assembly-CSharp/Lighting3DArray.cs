using System;

// Token: 0x02000B61 RID: 2913
public class Lighting3DArray : Array3DWithOffset<Lighting>
{
	// Token: 0x060057EF RID: 22511 RVA: 0x0021BB06 File Offset: 0x00219D06
	public Lighting3DArray()
		: base(3, 3, 3)
	{
	}

	// Token: 0x060057F0 RID: 22512 RVA: 0x0021BB11 File Offset: 0x00219D11
	public void SetBlockCache(INeighborBlockCache _nBlocks)
	{
		this.nBlocks = _nBlocks;
	}

	// Token: 0x060057F1 RID: 22513 RVA: 0x0021BB1C File Offset: 0x00219D1C
	public void SetPosition(Vector3i _blockPos)
	{
		this.stab = this.nBlocks.GetChunk(0, 0).GetStability(_blockPos.x, _blockPos.y, _blockPos.z);
		if (this.blockPos.x != _blockPos.x || this.blockPos.z != _blockPos.z || this.blockPos.y != _blockPos.y + 1)
		{
			this.available = 0;
		}
		else
		{
			int num = 26;
			for (int i = 0; i < 18; i++)
			{
				this.data[num] = this.data[num - 9];
				num--;
			}
			this.available <<= 9;
		}
		this.blockPos = _blockPos;
	}

	// Token: 0x060057F2 RID: 22514 RVA: 0x0021BBDC File Offset: 0x00219DDC
	[PublicizedFrom(EAccessModifier.Private)]
	public Lighting GetLight(IChunk _c, int _x, int _y, int _z)
	{
		if (_c == null)
		{
			return Lighting.one;
		}
		_x &= 15;
		_z &= 15;
		return new Lighting(_c.GetLight(_x, _y, _z, Chunk.LIGHT_TYPE.SUN), 0, this.stab);
	}

	// Token: 0x1700096B RID: 2411
	public override Lighting this[int _x, int _y, int _z]
	{
		get
		{
			int index = base.GetIndex(_x, _y, _z);
			int num = 1 << index;
			if ((this.available & num) == 0)
			{
				this.data[index] = this.GetLight(this.nBlocks.GetChunk(_x, _z), this.blockPos.x + _x, this.blockPos.y + _y, this.blockPos.z + _z);
				this.available |= num;
			}
			return this.data[index];
		}
		set
		{
			int index = base.GetIndex(_x, _y, _z);
			this.data[index] = value;
			this.available |= 1 << index;
		}
	}

	// Token: 0x0400440D RID: 17421
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSize = 3;

	// Token: 0x0400440E RID: 17422
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSize2d = 9;

	// Token: 0x0400440F RID: 17423
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSize3d = 27;

	// Token: 0x04004410 RID: 17424
	[PublicizedFrom(EAccessModifier.Private)]
	public int available;

	// Token: 0x04004411 RID: 17425
	[PublicizedFrom(EAccessModifier.Private)]
	public INeighborBlockCache nBlocks;

	// Token: 0x04004412 RID: 17426
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x04004413 RID: 17427
	[PublicizedFrom(EAccessModifier.Private)]
	public byte stab;
}
