using System;
using System.Collections.Generic;

namespace Platform.Shared
{
	// Token: 0x02001C8D RID: 7309
	public static class SaveGameProviderHelper
	{
		// Token: 0x0600D877 RID: 55415 RVA: 0x004DBFDB File Offset: 0x004DA1DB
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

		// Token: 0x0600D878 RID: 55416 RVA: 0x004DBFFC File Offset: 0x004DA1FC
		public static SaveDataManagedPath GetManagedPathFromBaseAndSubPath(SaveDataManagedPath path, string basePath, string subPath)
		{
			ReadOnlySpan<char> readOnlySpan = subPath.AsSpan(basePath.Length).TrimStart("\\/");
			return path.GetChildPath(readOnlySpan);
		}
	}
}
