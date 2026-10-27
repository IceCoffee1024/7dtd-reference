using System;

// Token: 0x02000CCA RID: 3274
public struct ChunkKey : IEquatable<ChunkKey>
{
	// Token: 0x060063F0 RID: 25584 RVA: 0x00277120 File Offset: 0x00275320
	public ChunkKey(IChunk _chunk)
	{
		this.x = _chunk.X;
		this.z = _chunk.Z;
	}

	// Token: 0x060063F1 RID: 25585 RVA: 0x0027713A File Offset: 0x0027533A
	public ChunkKey(int _x, int _z)
	{
		this.x = _x;
		this.z = _z;
	}

	// Token: 0x060063F2 RID: 25586 RVA: 0x0027714A File Offset: 0x0027534A
	public override int GetHashCode()
	{
		return WaterUtils.GetVoxelKey2D(this.x, this.z);
	}

	// Token: 0x060063F3 RID: 25587 RVA: 0x0027715D File Offset: 0x0027535D
	public override bool Equals(object obj)
	{
		return base.Equals((ChunkKey)obj);
	}

	// Token: 0x060063F4 RID: 25588 RVA: 0x0027717A File Offset: 0x0027537A
	public bool Equals(ChunkKey other)
	{
		return this.x == other.x && this.z == other.z;
	}

	// Token: 0x04004DFD RID: 19965
	public int x;

	// Token: 0x04004DFE RID: 19966
	public int z;
}
