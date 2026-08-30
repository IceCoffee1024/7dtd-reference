using System;

namespace WorldGenerationEngineFinal
{
	// Token: 0x020016F0 RID: 5872
	public struct PathTile
	{
		// Token: 0x04008867 RID: 34919
		public PathTile.PathTileStates TileState;

		// Token: 0x020016F1 RID: 5873
		public enum PathTileStates : byte
		{
			// Token: 0x04008869 RID: 34921
			Free,
			// Token: 0x0400886A RID: 34922
			Blocked,
			// Token: 0x0400886B RID: 34923
			Highway,
			// Token: 0x0400886C RID: 34924
			Country
		}
	}
}
