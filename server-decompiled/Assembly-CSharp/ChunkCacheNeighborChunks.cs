using System;

// Token: 0x02000B25 RID: 2853
public class ChunkCacheNeighborChunks
{
	// Token: 0x060055A9 RID: 21929 RVA: 0x0020E530 File Offset: 0x0020C730
	public ChunkCacheNeighborChunks(IChunkAccess _chunkAccess)
	{
		IChunk[,] array = new Chunk[3, 3];
		this.chunks = array;
		base..ctor();
		this.chunkAccess = _chunkAccess;
	}

	// Token: 0x060055AA RID: 21930 RVA: 0x0020E55C File Offset: 0x0020C75C
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

	// Token: 0x060055AB RID: 21931 RVA: 0x0020E5CA File Offset: 0x0020C7CA
	public void Clear()
	{
		Array.Clear(this.chunks, 0, this.chunks.GetLength(0) * this.chunks.GetLength(1));
	}

	// Token: 0x17000948 RID: 2376
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

	// Token: 0x0400428F RID: 17039
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunk[,] chunks;

	// Token: 0x04004290 RID: 17040
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunkAccess chunkAccess;
}
