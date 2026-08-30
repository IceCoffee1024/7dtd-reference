using System;
using System.Collections.Generic;

namespace MusicUtils
{
	// Token: 0x020019F0 RID: 6640
	public static class FileCleanupUtils
	{
		// Token: 0x0600C98C RID: 51596 RVA: 0x0049F740 File Offset: 0x0049D940
		public static void CleanUpAllWaveFiles()
		{
			for (int i = 0; i < FileCleanupUtils.paths.Count; i++)
			{
				FileCleanupUtils.CleanUpWaveFile(FileCleanupUtils.paths[i]);
			}
		}

		// Token: 0x0600C98D RID: 51597 RVA: 0x0049F772 File Offset: 0x0049D972
		public static void CleanUpWaveFile(string file)
		{
			WaveCleanUp.Create().GetComponent<WaveCleanUp>().FilePath = file;
		}

		// Token: 0x04009923 RID: 39203
		public static List<string> paths = new List<string>();
	}
}
