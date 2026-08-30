using System;
using System.Runtime.CompilerServices;

// Token: 0x02000BB9 RID: 3001
public static class TileAreaUtils
{
	// Token: 0x06005A82 RID: 23170 RVA: 0x0022D498 File Offset: 0x0022B698
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint MakeKey(int _tileX, int _tileZ)
	{
		return (uint)((_tileX << 16) | (_tileZ & 65535));
	}

	// Token: 0x06005A83 RID: 23171 RVA: 0x0022D4A6 File Offset: 0x0022B6A6
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetTileXPos(uint _key)
	{
		return (int)_key >> 16;
	}

	// Token: 0x06005A84 RID: 23172 RVA: 0x0022D4AC File Offset: 0x0022B6AC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetTileZPos(uint _key)
	{
		return (int)((short)_key);
	}
}
