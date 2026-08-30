using System;
using UnityEngine;

namespace MapRendering
{
	// Token: 0x02001AAC RID: 6828
	public static class Constants
	{
		// Token: 0x17001965 RID: 6501
		// (get) Token: 0x0600CE01 RID: 52737 RVA: 0x004ADB75 File Offset: 0x004ABD75
		public static int MAP_BLOCK_TO_CHUNK_DIV
		{
			get
			{
				return Constants.MapBlockSize / 16;
			}
		}

		// Token: 0x17001966 RID: 6502
		// (get) Token: 0x0600CE02 RID: 52738 RVA: 0x00183C79 File Offset: 0x00181E79
		public static int MAP_REGION_TO_CHUNK_DIV
		{
			get
			{
				return 32;
			}
		}

		// Token: 0x04009C0B RID: 39947
		public static readonly TextureFormat DefaultTextureFormat = TextureFormat.ARGB32;

		// Token: 0x04009C0C RID: 39948
		public static int MapBlockSize = 128;

		// Token: 0x04009C0D RID: 39949
		public const int MapChunkSize = 16;

		// Token: 0x04009C0E RID: 39950
		public const int MapRegionSize = 512;

		// Token: 0x04009C0F RID: 39951
		public static int Zoomlevels = 5;

		// Token: 0x04009C10 RID: 39952
		public static string MapDirectory = string.Empty;
	}
}
