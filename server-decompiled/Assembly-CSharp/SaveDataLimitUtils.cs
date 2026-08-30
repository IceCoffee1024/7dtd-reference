using System;

// Token: 0x02000A13 RID: 2579
public static class SaveDataLimitUtils
{
	// Token: 0x06004D7A RID: 19834 RVA: 0x001DB194 File Offset: 0x001D9394
	public static long CalculatePlayerMapSize(Vector2i worldSize)
	{
		int num = worldSize.x * worldSize.y;
		if (num <= 0)
		{
			throw new ArgumentException(string.Format("Expected a positive value for the world area, but was: {0}", num), "worldSize");
		}
		return Math.Min((long)num / 256L * 516L, 270532608L);
	}

	// Token: 0x04003C1A RID: 15386
	[PublicizedFrom(EAccessModifier.Private)]
	public const long CHUNK_SIZE = 16L;

	// Token: 0x04003C1B RID: 15387
	[PublicizedFrom(EAccessModifier.Private)]
	public const long CHUNK_AREA = 256L;

	// Token: 0x04003C1C RID: 15388
	[PublicizedFrom(EAccessModifier.Private)]
	public const long PLAYER_MAP_OVERHEAD_PER_CHUNK = 516L;

	// Token: 0x04003C1D RID: 15389
	[PublicizedFrom(EAccessModifier.Private)]
	public const long PLAYER_MAP_MAX_OVERHEAD = 270532608L;
}
