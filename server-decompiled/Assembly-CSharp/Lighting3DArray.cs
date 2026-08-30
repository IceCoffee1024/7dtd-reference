using System;

// Token: 0x02000B4F RID: 2895
public class Lighting3DArray : Array3DWithOffset<Lighting>
{
	// Token: 0x060057D5 RID: 22485 RVA: 0x0021887A File Offset: 0x00216A7A
	public Lighting3DArray()
		: base(3, 3, 3)
	{
	}

	// Token: 0x060057D6 RID: 22486 RVA: 0x00218885 File Offset: 0x00216A85
	public void SetBlockCache(INeighborBlockCache _nBlocks)
	{
		this.nBlocks = _nBlocks;
	}

	// Token: 0x060057D7 RID: 22487 RVA: 0x00218890 File Offset: 0x00216A90
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

	// Token: 0x060057D8 RID: 22488 RVA: 0x00218950 File Offset: 0x00216B50
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

	// Token: 0x17000949 RID: 2377
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

	// Token: 0x040043CF RID: 17359
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSize = 3;

	// Token: 0x040043D0 RID: 17360
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSize2d = 9;

	// Token: 0x040043D1 RID: 17361
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSize3d = 27;

	// Token: 0x040043D2 RID: 17362
	[PublicizedFrom(EAccessModifier.Private)]
	public int available;

	// Token: 0x040043D3 RID: 17363
	[PublicizedFrom(EAccessModifier.Private)]
	public INeighborBlockCache nBlocks;

	// Token: 0x040043D4 RID: 17364
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x040043D5 RID: 17365
	[PublicizedFrom(EAccessModifier.Private)]
	public byte stab;
}
