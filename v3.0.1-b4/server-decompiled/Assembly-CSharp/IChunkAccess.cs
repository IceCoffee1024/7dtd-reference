using System;

// Token: 0x02000B31 RID: 2865
public interface IChunkAccess
{
	// Token: 0x060056D6 RID: 22230
	IChunk GetChunkSync(int chunkX, int chunkZ);

	// Token: 0x060056D7 RID: 22231
	IChunk GetChunkSync(long chunkKey);

	// Token: 0x060056D8 RID: 22232 RVA: 0x001AEC68 File Offset: 0x001ACE68
	IChunk GetChunkSync(int chunkX, int chunkY, int chunkZ)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkX, chunkY, chunkZ);
	}

	// Token: 0x060056D9 RID: 22233 RVA: 0x001AEC73 File Offset: 0x001ACE73
	IChunk GetChunkSync(Vector2i chunkPos)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkPos);
	}

	// Token: 0x060056DA RID: 22234 RVA: 0x001AEC7C File Offset: 0x001ACE7C
	IChunk GetChunkSync(PropRef propRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, propRef);
	}

	// Token: 0x060056DB RID: 22235 RVA: 0x001AEC85 File Offset: 0x001ACE85
	IChunk GetChunkSync(BlockValueRef bvRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, bvRef);
	}

	// Token: 0x060056DC RID: 22236 RVA: 0x001AEC8E File Offset: 0x001ACE8E
	IChunk GetChunkFromWorldPos(int x, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z);
	}

	// Token: 0x060056DD RID: 22237 RVA: 0x001AEC98 File Offset: 0x001ACE98
	IChunk GetChunkFromWorldPos(int x, int y, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, y, z);
	}

	// Token: 0x060056DE RID: 22238 RVA: 0x001AECA3 File Offset: 0x001ACEA3
	IChunk GetChunkFromWorldPos(Vector3i blockPos)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos);
	}

	// Token: 0x060056DF RID: 22239 RVA: 0x001AECAC File Offset: 0x001ACEAC
	bool GetChunkFromWorldPos(int x, int z, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z, ref chunk);
	}

	// Token: 0x060056E0 RID: 22240 RVA: 0x001AECB7 File Offset: 0x001ACEB7
	bool GetChunkFromWorldPos(Vector3i blockPos, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos, ref chunk);
	}

	// Token: 0x060056E1 RID: 22241 RVA: 0x00215637 File Offset: 0x00213837
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkSync(IChunkAccess ica, int chunkX, int chunkY, int chunkZ)
	{
		return ica.GetChunkSync(chunkX, chunkZ);
	}

	// Token: 0x060056E2 RID: 22242 RVA: 0x00215641 File Offset: 0x00213841
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkSync(IChunkAccess ica, Vector2i chunkPos)
	{
		return ica.GetChunkSync(chunkPos.x, chunkPos.y);
	}

	// Token: 0x060056E3 RID: 22243 RVA: 0x00215655 File Offset: 0x00213855
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkSync(IChunkAccess ica, PropRef propRef)
	{
		return ica.GetChunkSync(propRef.ChunkPos);
	}

	// Token: 0x060056E4 RID: 22244 RVA: 0x00215664 File Offset: 0x00213864
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

	// Token: 0x060056E5 RID: 22245 RVA: 0x002156AD File Offset: 0x002138AD
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkFromWorldPos(IChunkAccess ica, int x, int z)
	{
		return ica.GetChunkSync(World.toChunkXZ(x), World.toChunkXZ(z));
	}

	// Token: 0x060056E6 RID: 22246 RVA: 0x002156C1 File Offset: 0x002138C1
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkFromWorldPos(IChunkAccess ica, int x, int y, int z)
	{
		return ica.GetChunkFromWorldPos(x, z);
	}

	// Token: 0x060056E7 RID: 22247 RVA: 0x002156CB File Offset: 0x002138CB
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IChunk DefaultGetChunkFromWorldPos(IChunkAccess ica, Vector3i blockPos)
	{
		return ica.GetChunkFromWorldPos(blockPos.x, blockPos.y, blockPos.z);
	}

	// Token: 0x060056E8 RID: 22248 RVA: 0x002156E8 File Offset: 0x002138E8
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

	// Token: 0x060056E9 RID: 22249 RVA: 0x0021572A File Offset: 0x0021392A
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool DefaultGetChunkFromWorldPos(IChunkAccess ica, Vector3i blockPos, ref IChunk chunk)
	{
		return ica.GetChunkFromWorldPos(blockPos.x, blockPos.z, ref chunk);
	}
}
