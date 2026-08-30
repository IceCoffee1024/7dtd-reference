using System;

// Token: 0x02000CB1 RID: 3249
public struct ChunkKey : IEquatable<ChunkKey>
{
	// Token: 0x060063D8 RID: 25560 RVA: 0x002727F4 File Offset: 0x002709F4
	public ChunkKey(IChunk _chunk)
	{
		this.x = _chunk.X;
		this.z = _chunk.Z;
	}

	// Token: 0x060063D9 RID: 25561 RVA: 0x0027280E File Offset: 0x00270A0E
	public ChunkKey(int _x, int _z)
	{
		this.x = _x;
		this.z = _z;
	}

	// Token: 0x060063DA RID: 25562 RVA: 0x0027281E File Offset: 0x00270A1E
	public override int GetHashCode()
	{
		return WaterUtils.GetVoxelKey2D(this.x, this.z);
	}

	// Token: 0x060063DB RID: 25563 RVA: 0x00272831 File Offset: 0x00270A31
	public override bool Equals(object obj)
	{
		return base.Equals((ChunkKey)obj);
	}

	// Token: 0x060063DC RID: 25564 RVA: 0x0027284E File Offset: 0x00270A4E
	public bool Equals(ChunkKey other)
	{
		return this.x == other.x && this.z == other.z;
	}

	// Token: 0x04004DA2 RID: 19874
	public int x;

	// Token: 0x04004DA3 RID: 19875
	public int z;
}
