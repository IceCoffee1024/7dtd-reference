using System;

// Token: 0x02000B43 RID: 2883
public interface IChunkAccess
{
	// Token: 0x0600575E RID: 22366
	IChunk GetChunkSync(int chunkX, int chunkZ);

	// Token: 0x0600575F RID: 22367
	IChunk GetChunkSync(long chunkKey);

	// Token: 0x06005760 RID: 22368 RVA: 0x001B1240 File Offset: 0x001AF440
	IChunk GetChunkSync(int chunkX, int chunkY, int chunkZ)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkX, chunkY, chunkZ);
	}

	// Token: 0x06005761 RID: 22369 RVA: 0x001B124B File Offset: 0x001AF44B
	IChunk GetChunkSync(Vector2i chunkPos)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkPos);
	}

	// Token: 0x06005762 RID: 22370 RVA: 0x001B1254 File Offset: 0x001AF454
	IChunk GetChunkSync(PropRef propRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, propRef);
	}

	// Token: 0x06005763 RID: 22371 RVA: 0x001B125D File Offset: 0x001AF45D
	IChunk GetChunkSync(BlockValueRef bvRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, bvRef);
	}

	// Token: 0x06005764 RID: 22372 RVA: 0x001B1266 File Offset: 0x001AF466
	IChunk GetChunkFromWorldPos(int x, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z);
	}

	// Token: 0x06005765 RID: 22373 RVA: 0x001B1270 File Offset: 0x001AF470
	IChunk GetChunkFromWorldPos(int x, int y, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, y, z);
	}

	// Token: 0x06005766 RID: 22374 RVA: 0x001B127B File Offset: 0x001AF47B
	IChunk GetChunkFromWorldPos(Vector3i blockPos)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos);
	}

	// Token: 0x06005767 RID: 22375 RVA: 0x001B1284 File Offset: 0x001AF484
	bool GetChunkFromWorldPos(int x, int z, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z, ref chunk);
	}

	// Token: 0x06005768 RID: 22376 RVA: 0x001B128F File Offset: 0x001AF48F
	bool GetChunkFromWorldPos(Vector3i blockPos, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos, ref chunk);
	}

	// Token: 0x06005769 RID: 22377 RVA: 0x00218353 File Offset: 0x00216553
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkSync(IChunkAccess ica, int chunkX, int chunkY, int chunkZ)
	{
		return ica.GetChunkSync(chunkX, chunkZ);
	}

	// Token: 0x0600576A RID: 22378 RVA: 0x0021835D File Offset: 0x0021655D
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkSync(IChunkAccess ica, Vector2i chunkPos)
	{
		return ica.GetChunkSync(chunkPos.x, chunkPos.y);
	}

	// Token: 0x0600576B RID: 22379 RVA: 0x00218371 File Offset: 0x00216571
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkSync(IChunkAccess ica, PropRef propRef)
	{
		return ica.GetChunkSync(propRef.ChunkPos);
	}

	// Token: 0x0600576C RID: 22380 RVA: 0x00218380 File Offset: 0x00216580
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

	// Token: 0x0600576D RID: 22381 RVA: 0x002183C9 File Offset: 0x002165C9
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkFromWorldPos(IChunkAccess ica, int x, int z)
	{
		return ica.GetChunkSync(World.toChunkXZ(x), World.toChunkXZ(z));
	}

	// Token: 0x0600576E RID: 22382 RVA: 0x002183DD File Offset: 0x002165DD
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkFromWorldPos(IChunkAccess ica, int x, int y, int z)
	{
		return ica.GetChunkFromWorldPos(x, z);
	}

	// Token: 0x0600576F RID: 22383 RVA: 0x002183E7 File Offset: 0x002165E7
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkFromWorldPos(IChunkAccess ica, Vector3i blockPos)
	{
		return ica.GetChunkFromWorldPos(blockPos.x, blockPos.y, blockPos.z);
	}

	// Token: 0x06005770 RID: 22384 RVA: 0x00218404 File Offset: 0x00216604
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

	// Token: 0x06005771 RID: 22385 RVA: 0x00218446 File Offset: 0x00216646
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool DefaultGetChunkFromWorldPos(IChunkAccess ica, Vector3i blockPos, ref IChunk chunk)
	{
		return ica.GetChunkFromWorldPos(blockPos.x, blockPos.z, ref chunk);
	}
}
