using System;

// Token: 0x02000A02 RID: 2562
public static class SaveDataLimitUtils
{
	// Token: 0x06004CF4 RID: 19700 RVA: 0x001D86FC File Offset: 0x001D68FC
	public static long CalculatePlayerMapSize(Vector2i worldSize)
	{
		int num = worldSize.x * worldSize.y;
		if (num <= 0)
		{
			throw new ArgumentException(string.Format("Expected a positive value for the world area, but was: {0}", num), "worldSize");
		}
		return Math.Min((long)num / 256L * 516L, 270532608L);
	}

	// Token: 0x04003B89 RID: 15241
	[PublicizedFrom(EAccessModifier.Private)]
	public const long CHUNK_SIZE = 16L;

	// Token: 0x04003B8A RID: 15242
	[PublicizedFrom(EAccessModifier.Private)]
	public const long CHUNK_AREA = 256L;

	// Token: 0x04003B8B RID: 15243
	[PublicizedFrom(EAccessModifier.Private)]
	public const long PLAYER_MAP_OVERHEAD_PER_CHUNK = 516L;

	// Token: 0x04003B8C RID: 15244
	[PublicizedFrom(EAccessModifier.Private)]
	public const long PLAYER_MAP_MAX_OVERHEAD = 270532608L;
}
