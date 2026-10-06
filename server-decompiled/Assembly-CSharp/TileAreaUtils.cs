using System;
using System.Runtime.CompilerServices;

// Token: 0x02000BDD RID: 3037
public static class TileAreaUtils
{
	// Token: 0x06005B23 RID: 23331 RVA: 0x002334FC File Offset: 0x002316FC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint MakeKey(int _tileX, int _tileZ)
	{
		return (uint)((_tileX << 16) | (_tileZ & 65535));
	}

	// Token: 0x06005B24 RID: 23332 RVA: 0x0023350A File Offset: 0x0023170A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetTileXPos(uint _key)
	{
		return (int)_key >> 16;
	}

	// Token: 0x06005B25 RID: 23333 RVA: 0x00233510 File Offset: 0x00231710
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetTileZPos(uint _key)
	{
		return (int)((short)_key);
	}
}
