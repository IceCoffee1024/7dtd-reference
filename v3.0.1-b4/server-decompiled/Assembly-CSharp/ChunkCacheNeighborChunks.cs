using System;

// Token: 0x02000B01 RID: 2817
public class ChunkCacheNeighborChunks
{
	// Token: 0x06005508 RID: 21768 RVA: 0x002085A4 File Offset: 0x002067A4
	public ChunkCacheNeighborChunks(IChunkAccess _chunkAccess)
	{
		IChunk[,] array = new Chunk[3, 3];
		this.chunks = array;
		base..ctor();
		this.chunkAccess = _chunkAccess;
	}

	// Token: 0x06005509 RID: 21769 RVA: 0x002085D0 File Offset: 0x002067D0
	public void Init(IChunk _chunk, IChunk[] _chunkArr)
	{
		this[0, 0] = _chunk;
		this[-1, 0] = _chunkArr[1];
		this[1, 0] = _chunkArr[0];
		this[0, -1] = _chunkArr[3];
		this[0, 1] = _chunkArr[2];
		this[-1, -1] = _chunkArr[5];
		this[1, -1] = _chunkArr[7];
		this[-1, 1] = _chunkArr[6];
		this[1, 1] = _chunkArr[4];
	}

	// Token: 0x0600550A RID: 21770 RVA: 0x0020863E File Offset: 0x0020683E
	public void Clear()
	{
		Array.Clear(this.chunks, 0, this.chunks.GetLength(0) * this.chunks.GetLength(1));
	}

	// Token: 0x17000914 RID: 2324
	public IChunk this[int x, int y]
	{
		get
		{
			return this.chunks[x + 1, y + 1];
		}
		set
		{
			this.chunks[x + 1, y + 1] = value;
		}
	}

	// Token: 0x040041B7 RID: 16823
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunk[,] chunks;

	// Token: 0x040041B8 RID: 16824
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunkAccess chunkAccess;
}
