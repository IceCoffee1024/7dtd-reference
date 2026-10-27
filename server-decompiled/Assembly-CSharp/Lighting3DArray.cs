using System;

// Token: 0x02000B6C RID: 2924
public class Lighting3DArray : Array3DWithOffset<Lighting>
{
	// Token: 0x06005814 RID: 22548 RVA: 0x0021D062 File Offset: 0x0021B262
	public Lighting3DArray()
		: base(3, 3, 3)
	{
	}

	// Token: 0x06005815 RID: 22549 RVA: 0x0021D06D File Offset: 0x0021B26D
	public void SetBlockCache(INeighborBlockCache _nBlocks)
	{
		this.nBlocks = _nBlocks;
	}

	// Token: 0x06005816 RID: 22550 RVA: 0x0021D078 File Offset: 0x0021B278
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

	// Token: 0x06005817 RID: 22551 RVA: 0x0021D138 File Offset: 0x0021B338
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

	// Token: 0x17000971 RID: 2417
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

	// Token: 0x04004432 RID: 17458
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSize = 3;

	// Token: 0x04004433 RID: 17459
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSize2d = 9;

	// Token: 0x04004434 RID: 17460
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSize3d = 27;

	// Token: 0x04004435 RID: 17461
	[PublicizedFrom(EAccessModifier.Private)]
	public int available;

	// Token: 0x04004436 RID: 17462
	[PublicizedFrom(EAccessModifier.Private)]
	public INeighborBlockCache nBlocks;

	// Token: 0x04004437 RID: 17463
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x04004438 RID: 17464
	[PublicizedFrom(EAccessModifier.Private)]
	public byte stab;
}
