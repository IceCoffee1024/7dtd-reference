using System;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001742 RID: 5954
	public struct PathTile
	{
		// Token: 0x04008A48 RID: 35400
		public PathTile.PathTileStates TileState;

		// Token: 0x02001743 RID: 5955
		public enum PathTileStates : byte
		{
			// Token: 0x04008A4A RID: 35402
			Free,
			// Token: 0x04008A4B RID: 35403
			Blocked,
			// Token: 0x04008A4C RID: 35404
			Highway,
			// Token: 0x04008A4D RID: 35405
			Country
		}
	}
}
