using System;

// Token: 0x02000B60 RID: 2912
public interface IChunkAccess
{
	// Token: 0x0600579D RID: 22429
	IChunk GetChunkSync(int chunkX, int chunkZ);

	// Token: 0x0600579E RID: 22430
	IChunk GetChunkSync(long chunkKey);

	// Token: 0x0600579F RID: 22431 RVA: 0x001B24CC File Offset: 0x001B06CC
	IChunk GetChunkSync(int chunkX, int chunkY, int chunkZ)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkX, chunkY, chunkZ);
	}

	// Token: 0x060057A0 RID: 22432 RVA: 0x001B24D7 File Offset: 0x001B06D7
	IChunk GetChunkSync(Vector2i chunkPos)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkPos);
	}

	// Token: 0x060057A1 RID: 22433 RVA: 0x001B24E0 File Offset: 0x001B06E0
	IChunk GetChunkSync(PropRef propRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, propRef);
	}

	// Token: 0x060057A2 RID: 22434 RVA: 0x001B24E9 File Offset: 0x001B06E9
	IChunk GetChunkSync(BlockValueRef bvRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, bvRef);
	}

	// Token: 0x060057A3 RID: 22435 RVA: 0x001B24F2 File Offset: 0x001B06F2
	IChunk GetChunkFromWorldPos(int x, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z);
	}

	// Token: 0x060057A4 RID: 22436 RVA: 0x001B24FC File Offset: 0x001B06FC
	IChunk GetChunkFromWorldPos(int x, int y, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, y, z);
	}

	// Token: 0x060057A5 RID: 22437 RVA: 0x001B2507 File Offset: 0x001B0707
	IChunk GetChunkFromWorldPos(Vector3i blockPos)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos);
	}

	// Token: 0x060057A6 RID: 22438 RVA: 0x001B2510 File Offset: 0x001B0710
	bool GetChunkFromWorldPos(int x, int z, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z, ref chunk);
	}

	// Token: 0x060057A7 RID: 22439 RVA: 0x001B251B File Offset: 0x001B071B
	bool GetChunkFromWorldPos(Vector3i blockPos, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos, ref chunk);
	}

	// Token: 0x060057A8 RID: 22440 RVA: 0x0021CB3B File Offset: 0x0021AD3B
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkSync(IChunkAccess ica, int chunkX, int chunkY, int chunkZ)
	{
		return ica.GetChunkSync(chunkX, chunkZ);
	}

	// Token: 0x060057A9 RID: 22441 RVA: 0x0021CB45 File Offset: 0x0021AD45
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkSync(IChunkAccess ica, Vector2i chunkPos)
	{
		return ica.GetChunkSync(chunkPos.x, chunkPos.y);
	}

	// Token: 0x060057AA RID: 22442 RVA: 0x0021CB59 File Offset: 0x0021AD59
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkSync(IChunkAccess ica, PropRef propRef)
	{
		return ica.GetChunkSync(propRef.ChunkPos);
	}

	// Token: 0x060057AB RID: 22443 RVA: 0x0021CB68 File Offset: 0x0021AD68
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

	// Token: 0x060057AC RID: 22444 RVA: 0x0021CBB1 File Offset: 0x0021ADB1
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkFromWorldPos(IChunkAccess ica, int x, int z)
	{
		return ica.GetChunkSync(World.toChunkXZ(x), World.toChunkXZ(z));
	}

	// Token: 0x060057AD RID: 22445 RVA: 0x0021CBC5 File Offset: 0x0021ADC5
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkFromWorldPos(IChunkAccess ica, int x, int y, int z)
	{
		return ica.GetChunkFromWorldPos(x, z);
	}

	// Token: 0x060057AE RID: 22446 RVA: 0x0021CBCF File Offset: 0x0021ADCF
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkFromWorldPos(IChunkAccess ica, Vector3i blockPos)
	{
		return ica.GetChunkFromWorldPos(blockPos.x, blockPos.y, blockPos.z);
	}

	// Token: 0x060057AF RID: 22447 RVA: 0x0021CBEC File Offset: 0x0021ADEC
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

	// Token: 0x060057B0 RID: 22448 RVA: 0x0021CC2E File Offset: 0x0021AE2E
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool DefaultGetChunkFromWorldPos(IChunkAccess ica, Vector3i blockPos, ref IChunk chunk)
	{
		return ica.GetChunkFromWorldPos(blockPos.x, blockPos.z, ref chunk);
	}
}
