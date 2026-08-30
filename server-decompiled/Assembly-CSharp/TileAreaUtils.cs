using System;
using System.Runtime.CompilerServices;

// Token: 0x02000BCB RID: 3019
public static class TileAreaUtils
{
	// Token: 0x06005B0A RID: 23306 RVA: 0x00230200 File Offset: 0x0022E400
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint MakeKey(int _tileX, int _tileZ)
	{
		return (uint)((_tileX << 16) | (_tileZ & 65535));
	}

	// Token: 0x06005B0B RID: 23307 RVA: 0x0023020E File Offset: 0x0022E40E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetTileXPos(uint _key)
	{
		return (int)_key >> 16;
	}

	// Token: 0x06005B0C RID: 23308 RVA: 0x00230214 File Offset: 0x0022E414
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetTileZPos(uint _key)
	{
		return (int)((short)_key);
	}
}
