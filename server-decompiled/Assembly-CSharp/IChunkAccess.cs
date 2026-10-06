using System;

// Token: 0x02000B55 RID: 2901
public interface IChunkAccess
{
	// Token: 0x06005778 RID: 22392
	IChunk GetChunkSync(int chunkX, int chunkZ);

	// Token: 0x06005779 RID: 22393
	IChunk GetChunkSync(long chunkKey);

	// Token: 0x0600577A RID: 22394 RVA: 0x001B1448 File Offset: 0x001AF648
	IChunk GetChunkSync(int chunkX, int chunkY, int chunkZ)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkX, chunkY, chunkZ);
	}

	// Token: 0x0600577B RID: 22395 RVA: 0x001B1453 File Offset: 0x001AF653
	IChunk GetChunkSync(Vector2i chunkPos)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkPos);
	}

	// Token: 0x0600577C RID: 22396 RVA: 0x001B145C File Offset: 0x001AF65C
	IChunk GetChunkSync(PropRef propRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, propRef);
	}

	// Token: 0x0600577D RID: 22397 RVA: 0x001B1465 File Offset: 0x001AF665
	IChunk GetChunkSync(BlockValueRef bvRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, bvRef);
	}

	// Token: 0x0600577E RID: 22398 RVA: 0x001B146E File Offset: 0x001AF66E
	IChunk GetChunkFromWorldPos(int x, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z);
	}

	// Token: 0x0600577F RID: 22399 RVA: 0x001B1478 File Offset: 0x001AF678
	IChunk GetChunkFromWorldPos(int x, int y, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, y, z);
	}

	// Token: 0x06005780 RID: 22400 RVA: 0x001B1483 File Offset: 0x001AF683
	IChunk GetChunkFromWorldPos(Vector3i blockPos)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos);
	}

	// Token: 0x06005781 RID: 22401 RVA: 0x001B148C File Offset: 0x001AF68C
	bool GetChunkFromWorldPos(int x, int z, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z, ref chunk);
	}

	// Token: 0x06005782 RID: 22402 RVA: 0x001B1497 File Offset: 0x001AF697
	bool GetChunkFromWorldPos(Vector3i blockPos, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos, ref chunk);
	}

	// Token: 0x06005783 RID: 22403 RVA: 0x0021B5DF File Offset: 0x002197DF
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkSync(IChunkAccess ica, int chunkX, int chunkY, int chunkZ)
	{
		return ica.GetChunkSync(chunkX, chunkZ);
	}

	// Token: 0x06005784 RID: 22404 RVA: 0x0021B5E9 File Offset: 0x002197E9
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkSync(IChunkAccess ica, Vector2i chunkPos)
	{
		return ica.GetChunkSync(chunkPos.x, chunkPos.y);
	}

	// Token: 0x06005785 RID: 22405 RVA: 0x0021B5FD File Offset: 0x002197FD
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkSync(IChunkAccess ica, PropRef propRef)
	{
		return ica.GetChunkSync(propRef.ChunkPos);
	}

	// Token: 0x06005786 RID: 22406 RVA: 0x0021B60C File Offset: 0x0021980C
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkSync(IChunkAccess ica, BlockValueRef bvRef)
	{
		switch (bvRef.Type)
		{
		case BlockValueRefType.None:
			return null;
		case BlockValueRefType.Block:
			return ica.GetChunkFromWorldPos(bvRef.BlockPosition);
		case BlockValueRefType.Prop:
			return ica.GetChunkSync(bvRef.PropReference);
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06005787 RID: 22407 RVA: 0x0021B655 File Offset: 0x00219855
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkFromWorldPos(IChunkAccess ica, int x, int z)
	{
		return ica.GetChunkSync(World.toChunkXZ(x), World.toChunkXZ(z));
	}

	// Token: 0x06005788 RID: 22408 RVA: 0x0021B669 File Offset: 0x00219869
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkFromWorldPos(IChunkAccess ica, int x, int y, int z)
	{
		return ica.GetChunkFromWorldPos(x, z);
	}

	// Token: 0x06005789 RID: 22409 RVA: 0x0021B673 File Offset: 0x00219873
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkFromWorldPos(IChunkAccess ica, Vector3i blockPos)
	{
		return ica.GetChunkFromWorldPos(blockPos.x, blockPos.y, blockPos.z);
	}

	// Token: 0x0600578A RID: 22410 RVA: 0x0021B690 File Offset: 0x00219890
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool DefaultGetChunkFromWorldPos(IChunkAccess ica, int x, int z, ref IChunk chunk)
	{
		int num = World.toChunkXZ(x);
		int num2 = World.toChunkXZ(z);
		if (chunk == null || chunk.X != num || chunk.Z != num2)
		{
			chunk = ica.GetChunkSync(num, num2);
		}
		return chunk != null;
	}

	// Token: 0x0600578B RID: 22411 RVA: 0x0021B6D2 File Offset: 0x002198D2
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool DefaultGetChunkFromWorldPos(IChunkAccess ica, Vector3i blockPos, ref IChunk chunk)
	{
		return ica.GetChunkFromWorldPos(blockPos.x, blockPos.z, ref chunk);
	}
}
