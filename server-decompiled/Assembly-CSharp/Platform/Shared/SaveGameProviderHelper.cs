using System;
using System.Collections.Generic;

namespace Platform.Shared
{
	// Token: 0x02001D17 RID: 7447
	public static class SaveGameProviderHelper
	{
		// Token: 0x0600DC9B RID: 56475 RVA: 0x004EAC9F File Offset: 0x004E8E9F
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

		// Token: 0x0600DC9C RID: 56476 RVA: 0x004EACC0 File Offset: 0x004E8EC0
		public static SaveDataManagedPath GetManagedPathFromBaseAndSubPath(SaveDataManagedPath path, string basePath, string subPath)
		{
			ReadOnlySpan<char> readOnlySpan = subPath.AsSpan(basePath.Length).TrimStart("\\/");
			return path.GetChildPath(readOnlySpan);
		}
	}
}
