using System;

// Token: 0x02000CBF RID: 3263
public struct ChunkKey : IEquatable<ChunkKey>
{
	// Token: 0x060063CB RID: 25547 RVA: 0x00275BF8 File Offset: 0x00273DF8
	public ChunkKey(IChunk _chunk)
	{
		this.x = _chunk.X;
		this.z = _chunk.Z;
	}

	// Token: 0x060063CC RID: 25548 RVA: 0x00275C12 File Offset: 0x00273E12
	public ChunkKey(int _x, int _z)
	{
		this.x = _x;
		this.z = _z;
	}

	// Token: 0x060063CD RID: 25549 RVA: 0x00275C22 File Offset: 0x00273E22
	public override int GetHashCode()
	{
		return WaterUtils.GetVoxelKey2D(this.x, this.z);
	}

	// Token: 0x060063CE RID: 25550 RVA: 0x00275C35 File Offset: 0x00273E35
	public override bool Equals(object obj)
	{
		return base.Equals((ChunkKey)obj);
	}

	// Token: 0x060063CF RID: 25551 RVA: 0x00275C52 File Offset: 0x00273E52
	public bool Equals(ChunkKey other)
	{
		return this.x == other.x && this.z == other.z;
	}

	// Token: 0x04004DD8 RID: 19928
	public int x;

	// Token: 0x04004DD9 RID: 19929
	public int z;
}
