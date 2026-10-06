using System;

// Token: 0x02000A1D RID: 2589
public static class SaveDataLimitUtils
{
	// Token: 0x06004D49 RID: 19785 RVA: 0x001DC444 File Offset: 0x001DA644
	public static long CalculatePlayerMapSize(Vector2i worldSize)
	{
		int num = worldSize.x * worldSize.y;
		if (num <= 0)
		{
			throw new ArgumentException(string.Format("Expected a positive value for the world area, but was: {0}", num), "worldSize");
		}
		return Math.Min((long)num / 256L * 516L, 270532608L);
	}

	// Token: 0x04003C27 RID: 15399
	[PublicizedFrom(EAccessModifier.Private)]
	public const long CHUNK_SIZE = 16L;

	// Token: 0x04003C28 RID: 15400
	[PublicizedFrom(EAccessModifier.Private)]
	public const long CHUNK_AREA = 256L;

	// Token: 0x04003C29 RID: 15401
	[PublicizedFrom(EAccessModifier.Private)]
	public const long PLAYER_MAP_OVERHEAD_PER_CHUNK = 516L;

	// Token: 0x04003C2A RID: 15402
	[PublicizedFrom(EAccessModifier.Private)]
	public const long PLAYER_MAP_MAX_OVERHEAD = 270532608L;
}
