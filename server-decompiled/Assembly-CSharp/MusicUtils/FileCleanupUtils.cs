using System;
using System.Collections.Generic;

namespace MusicUtils
{
	// Token: 0x02001A74 RID: 6772
	public static class FileCleanupUtils
	{
		// Token: 0x0600CD81 RID: 52609 RVA: 0x004ADAFC File Offset: 0x004ABCFC
		public static void CleanUpAllWaveFiles()
		{
			for (int i = 0; i < FileCleanupUtils.paths.Count; i++)
			{
				FileCleanupUtils.CleanUpWaveFile(FileCleanupUtils.paths[i]);
			}
		}

		// Token: 0x0600CD82 RID: 52610 RVA: 0x004ADB2E File Offset: 0x004ABD2E
		public static void CleanUpWaveFile(string file)
		{
			WaveCleanUp.Create().GetComponent<WaveCleanUp>().FilePath = file;
		}

		// Token: 0x04009C03 RID: 39939
		public static List<string> paths = new List<string>();
	}
}
