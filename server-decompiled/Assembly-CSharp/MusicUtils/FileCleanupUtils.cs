using System;
using System.Collections.Generic;

namespace MusicUtils
{
	// Token: 0x02001A17 RID: 6679
	public static class FileCleanupUtils
	{
		// Token: 0x0600CB15 RID: 51989 RVA: 0x004A5B34 File Offset: 0x004A3D34
		public static void CleanUpAllWaveFiles()
		{
			for (int i = 0; i < FileCleanupUtils.paths.Count; i++)
			{
				FileCleanupUtils.CleanUpWaveFile(FileCleanupUtils.paths[i]);
			}
		}

		// Token: 0x0600CB16 RID: 51990 RVA: 0x004A5B66 File Offset: 0x004A3D66
		public static void CleanUpWaveFile(string file)
		{
			WaveCleanUp.Create().GetComponent<WaveCleanUp>().FilePath = file;
		}

		// Token: 0x04009A54 RID: 39508
		public static List<string> paths = new List<string>();
	}
}
