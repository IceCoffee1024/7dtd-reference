using System;
using System.Collections.Generic;

namespace Platform.Shared
{
	// Token: 0x02001CE6 RID: 7398
	public static class SaveGameProviderHelper
	{
		// Token: 0x0600DB87 RID: 56199 RVA: 0x004E70FF File Offset: 0x004E52FF
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

		// Token: 0x0600DB88 RID: 56200 RVA: 0x004E7120 File Offset: 0x004E5320
		public static SaveDataManagedPath GetManagedPathFromBaseAndSubPath(SaveDataManagedPath path, string basePath, string subPath)
		{
			ReadOnlySpan<char> readOnlySpan = subPath.AsSpan(basePath.Length).TrimStart("\\/");
			return path.GetChildPath(readOnlySpan);
		}
	}
}
