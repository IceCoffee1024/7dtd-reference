using System;

// Token: 0x02000B30 RID: 2864
public class ChunkCacheNeighborChunks
{
	// Token: 0x060055CE RID: 21966 RVA: 0x0020FAAC File Offset: 0x0020DCAC
	public ChunkCacheNeighborChunks(IChunkAccess _chunkAccess)
	{
		IChunk[,] array = new Chunk[3, 3];
		this.chunks = array;
		base..ctor();
		this.chunkAccess = _chunkAccess;
	}

	// Token: 0x060055CF RID: 21967 RVA: 0x0020FAD8 File Offset: 0x0020DCD8
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

	// Token: 0x060055D0 RID: 21968 RVA: 0x0020FB46 File Offset: 0x0020DD46
	public void Clear()
	{
		Array.Clear(this.chunks, 0, this.chunks.GetLength(0) * this.chunks.GetLength(1));
	}

	// Token: 0x1700094E RID: 2382
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

	// Token: 0x040042B4 RID: 17076
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunk[,] chunks;

	// Token: 0x040042B5 RID: 17077
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunkAccess chunkAccess;
}
