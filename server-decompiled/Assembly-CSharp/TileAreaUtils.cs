using System;
using System.Runtime.CompilerServices;

// Token: 0x02000BE8 RID: 3048
public static class TileAreaUtils
{
	// Token: 0x06005B48 RID: 23368 RVA: 0x00234A58 File Offset: 0x00232C58
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint MakeKey(int _tileX, int _tileZ)
	{
		return (uint)((_tileX << 16) | (_tileZ & 65535));
	}

	// Token: 0x06005B49 RID: 23369 RVA: 0x00234A66 File Offset: 0x00232C66
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetTileXPos(uint _key)
	{
		return (int)_key >> 16;
	}

	// Token: 0x06005B4A RID: 23370 RVA: 0x00234A6C File Offset: 0x00232C6C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetTileZPos(uint _key)
	{
		return (int)((short)_key);
	}
}
