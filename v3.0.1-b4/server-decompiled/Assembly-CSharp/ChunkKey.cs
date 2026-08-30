using System;

// Token: 0x02000C9F RID: 3231
public struct ChunkKey : IEquatable<ChunkKey>
{
	// Token: 0x0600634C RID: 25420 RVA: 0x0026F8F8 File Offset: 0x0026DAF8
	public ChunkKey(IChunk _chunk)
	{
		this.x = _chunk.X;
		this.z = _chunk.Z;
	}

	// Token: 0x0600634D RID: 25421 RVA: 0x0026F912 File Offset: 0x0026DB12
	public ChunkKey(int _x, int _z)
	{
		this.x = _x;
		this.z = _z;
	}

	// Token: 0x0600634E RID: 25422 RVA: 0x0026F922 File Offset: 0x0026DB22
	public override int GetHashCode()
	{
		return WaterUtils.GetVoxelKey2D(this.x, this.z);
	}

	// Token: 0x0600634F RID: 25423 RVA: 0x0026F935 File Offset: 0x0026DB35
	public override bool Equals(object obj)
	{
		return base.Equals((ChunkKey)obj);
	}

	// Token: 0x06006350 RID: 25424 RVA: 0x0026F952 File Offset: 0x0026DB52
	public bool Equals(ChunkKey other)
	{
		return this.x == other.x && this.z == other.z;
	}

	// Token: 0x04004D04 RID: 19716
	public int x;

	// Token: 0x04004D05 RID: 19717
	public int z;
}
