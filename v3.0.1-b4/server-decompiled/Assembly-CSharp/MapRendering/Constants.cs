using System;
using UnityEngine;

namespace MapRendering
{
	// Token: 0x02001A85 RID: 6789
	public static class Constants
	{
		// Token: 0x170018F3 RID: 6387
		// (get) Token: 0x0600CC78 RID: 52344 RVA: 0x004A7781 File Offset: 0x004A5981
		public static int MAP_BLOCK_TO_CHUNK_DIV
		{
			get
			{
				return Constants.MapBlockSize / 16;
			}
		}

		// Token: 0x170018F4 RID: 6388
		// (get) Token: 0x0600CC79 RID: 52345 RVA: 0x001818A1 File Offset: 0x0017FAA1
		public static int MAP_REGION_TO_CHUNK_DIV
		{
			get
			{
				return 32;
			}
		}

		// Token: 0x04009ADA RID: 39642
		public static readonly TextureFormat DefaultTextureFormat = TextureFormat.ARGB32;

		// Token: 0x04009ADB RID: 39643
		public static int MapBlockSize = 128;

		// Token: 0x04009ADC RID: 39644
		public const int MapChunkSize = 16;

		// Token: 0x04009ADD RID: 39645
		public const int MapRegionSize = 512;

		// Token: 0x04009ADE RID: 39646
		public static int Zoomlevels = 5;

		// Token: 0x04009ADF RID: 39647
		public static string MapDirectory = string.Empty;
	}
}
