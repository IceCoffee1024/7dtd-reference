using System;
using UnityEngine;

namespace MapRendering
{
	// Token: 0x02001AD8 RID: 6872
	public static class Constants
	{
		// Token: 0x17001A07 RID: 6663
		// (get) Token: 0x0600CF59 RID: 53081 RVA: 0x004B2041 File Offset: 0x004B0241
		public static int MAP_BLOCK_TO_CHUNK_DIV
		{
			get
			{
				return Constants.MapBlockSize / 16;
			}
		}

		// Token: 0x17001A08 RID: 6664
		// (get) Token: 0x0600CF5A RID: 53082 RVA: 0x0022779D File Offset: 0x0022599D
		public static int MAP_REGION_TO_CHUNK_DIV
		{
			get
			{
				return 32;
			}
		}

		// Token: 0x04009CD1 RID: 40145
		public static readonly TextureFormat DefaultTextureFormat = TextureFormat.ARGB32;

		// Token: 0x04009CD2 RID: 40146
		public static int MapBlockSize = 128;

		// Token: 0x04009CD3 RID: 40147
		public const int MapChunkSize = 16;

		// Token: 0x04009CD4 RID: 40148
		public const int MapRegionSize = 512;

		// Token: 0x04009CD5 RID: 40149
		public static int Zoomlevels = 5;

		// Token: 0x04009CD6 RID: 40150
		public static string MapDirectory = string.Empty;
	}
}
