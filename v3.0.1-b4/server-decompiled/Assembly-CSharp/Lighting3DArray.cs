using System;

// Token: 0x02000B3D RID: 2877
public class Lighting3DArray : Array3DWithOffset<Lighting>
{
	// Token: 0x0600574D RID: 22349 RVA: 0x00215B5E File Offset: 0x00213D5E
	public Lighting3DArray()
		: base(3, 3, 3)
	{
	}

	// Token: 0x0600574E RID: 22350 RVA: 0x00215B69 File Offset: 0x00213D69
	public void SetBlockCache(INeighborBlockCache _nBlocks)
	{
		this.nBlocks = _nBlocks;
	}

	// Token: 0x0600574F RID: 22351 RVA: 0x00215B74 File Offset: 0x00213D74
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

	// Token: 0x06005750 RID: 22352 RVA: 0x00215C34 File Offset: 0x00213E34
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

	// Token: 0x17000937 RID: 2359
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

	// Token: 0x04004334 RID: 17204
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSize = 3;

	// Token: 0x04004335 RID: 17205
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSize2d = 9;

	// Token: 0x04004336 RID: 17206
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSize3d = 27;

	// Token: 0x04004337 RID: 17207
	[PublicizedFrom(EAccessModifier.Private)]
	public int available;

	// Token: 0x04004338 RID: 17208
	[PublicizedFrom(EAccessModifier.Private)]
	public INeighborBlockCache nBlocks;

	// Token: 0x04004339 RID: 17209
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x0400433A RID: 17210
	[PublicizedFrom(EAccessModifier.Private)]
	public byte stab;
}
