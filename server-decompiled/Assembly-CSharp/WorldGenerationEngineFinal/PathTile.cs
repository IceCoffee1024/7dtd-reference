using System;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001716 RID: 5910
	public struct PathTile
	{
		// Token: 0x04008983 RID: 35203
		public PathTile.PathTileStates TileState;

		// Token: 0x02001717 RID: 5911
		public enum PathTileStates : byte
		{
			// Token: 0x04008985 RID: 35205
			Free,
			// Token: 0x04008986 RID: 35206
			Blocked,
			// Token: 0x04008987 RID: 35207
			Highway,
			// Token: 0x04008988 RID: 35208
			Country
		}
	}
}
