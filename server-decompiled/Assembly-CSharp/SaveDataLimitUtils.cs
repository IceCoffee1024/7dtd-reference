using System;

// Token: 0x02000A27 RID: 2599
public static class SaveDataLimitUtils
{
	// Token: 0x06004D6D RID: 19821 RVA: 0x001DD52C File Offset: 0x001DB72C
	public static long CalculatePlayerMapSize(Vector2i worldSize)
	{
		int num = worldSize.x * worldSize.y;
		if (num <= 0)
		{
			throw new ArgumentException(string.Format("Expected a positive value for the world area, but was: {0}", num), "worldSize");
		}
		return Math.Min((long)num / 256L * 516L, 270532608L);
	}

	// Token: 0x04003C48 RID: 15432
	[PublicizedFrom(EAccessModifier.Private)]
	public const long CHUNK_SIZE = 16L;

	// Token: 0x04003C49 RID: 15433
	[PublicizedFrom(EAccessModifier.Private)]
	public const long CHUNK_AREA = 256L;

	// Token: 0x04003C4A RID: 15434
	[PublicizedFrom(EAccessModifier.Private)]
	public const long PLAYER_MAP_OVERHEAD_PER_CHUNK = 516L;

	// Token: 0x04003C4B RID: 15435
	[PublicizedFrom(EAccessModifier.Private)]
	public const long PLAYER_MAP_MAX_OVERHEAD = 270532608L;
}
