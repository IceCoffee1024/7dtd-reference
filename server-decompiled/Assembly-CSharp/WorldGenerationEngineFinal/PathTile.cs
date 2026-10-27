using System;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001773 RID: 6003
	public struct PathTile
	{
		// Token: 0x04008B31 RID: 35633
		public PathTile.PathTileStates TileState;

		// Token: 0x02001774 RID: 6004
		public enum PathTileStates : byte
		{
			// Token: 0x04008B33 RID: 35635
			Free,
			// Token: 0x04008B34 RID: 35636
			Blocked,
			// Token: 0x04008B35 RID: 35637
			Highway,
			// Token: 0x04008B36 RID: 35638
			Country
		}
	}
}
