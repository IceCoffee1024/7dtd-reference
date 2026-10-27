using System;
using UnityEngine;

namespace MapRendering
{
	// Token: 0x02001B09 RID: 6921
	public static class Constants
	{
		// Token: 0x17001A4F RID: 6735
		// (get) Token: 0x0600D06D RID: 53357 RVA: 0x004B5B3D File Offset: 0x004B3D3D
		public static int MAP_BLOCK_TO_CHUNK_DIV
		{
			get
			{
				return Constants.MapBlockSize / 16;
			}
		}

		// Token: 0x17001A50 RID: 6736
		// (get) Token: 0x0600D06E RID: 53358 RVA: 0x00228CF9 File Offset: 0x00226EF9
		public static int MAP_REGION_TO_CHUNK_DIV
		{
			get
			{
				return 32;
			}
		}

		// Token: 0x04009DBA RID: 40378
		public static readonly TextureFormat DefaultTextureFormat = TextureFormat.ARGB32;

		// Token: 0x04009DBB RID: 40379
		public static int MapBlockSize = 128;

		// Token: 0x04009DBC RID: 40380
		public const int MapChunkSize = 16;

		// Token: 0x04009DBD RID: 40381
		public const int MapRegionSize = 512;

		// Token: 0x04009DBE RID: 40382
		public static int Zoomlevels = 5;

		// Token: 0x04009DBF RID: 40383
		public static string MapDirectory = string.Empty;
	}
}
