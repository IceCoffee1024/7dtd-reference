using System;
using System.Collections.Generic;

namespace MusicUtils
{
	// Token: 0x02001A43 RID: 6723
	public static class FileCleanupUtils
	{
		// Token: 0x0600CC6D RID: 52333 RVA: 0x004AA000 File Offset: 0x004A8200
		public static void CleanUpAllWaveFiles()
		{
			for (int i = 0; i < FileCleanupUtils.paths.Count; i++)
			{
				FileCleanupUtils.CleanUpWaveFile(FileCleanupUtils.paths[i]);
			}
		}

		// Token: 0x0600CC6E RID: 52334 RVA: 0x004AA032 File Offset: 0x004A8232
		public static void CleanUpWaveFile(string file)
		{
			WaveCleanUp.Create().GetComponent<WaveCleanUp>().FilePath = file;
		}

		// Token: 0x04009B1A RID: 39706
		public static List<string> paths = new List<string>();
	}
}
