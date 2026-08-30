using System;
using System.Collections.Generic;

namespace Platform.Shared
{
	// Token: 0x02001CB4 RID: 7348
	public static class SaveGameProviderHelper
	{
		// Token: 0x0600DA0D RID: 55821 RVA: 0x004E25B7 File Offset: 0x004E07B7
		public static IEnumerable<SaveDataManagedPath> GetManagedPathsFromBaseAndSubPaths(SaveDataManagedPath path, string basePath, IEnumerable<string> subPaths)
		{
			foreach (string text in subPaths)
			{
				yield return SaveGameProviderHelper.GetManagedPathFromBaseAndSubPath(path, basePath, text);
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600DA0E RID: 55822 RVA: 0x004E25D8 File Offset: 0x004E07D8
		public static SaveDataManagedPath GetManagedPathFromBaseAndSubPath(SaveDataManagedPath path, string basePath, string subPath)
		{
			ReadOnlySpan<char> readOnlySpan = subPath.AsSpan(basePath.Length).TrimStart("\\/");
			return path.GetChildPath(readOnlySpan);
		}
	}
}
