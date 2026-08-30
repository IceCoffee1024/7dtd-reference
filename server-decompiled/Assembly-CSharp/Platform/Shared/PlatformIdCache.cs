using System;
using System.IO;

namespace Platform.Shared
{
	// Token: 0x02001CAD RID: 7341
	public static class PlatformIdCache
	{
		// Token: 0x17001B0E RID: 6926
		// (get) Token: 0x0600D9CA RID: 55754 RVA: 0x004E1B49 File Offset: 0x004DFD49
		public static string IdFilePath
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return Path.Combine(GameIO.GetUserGameDataDir(), "PlatformIdCache.txt");
			}
		}

		// Token: 0x0600D9CB RID: 55755 RVA: 0x004E1B5C File Offset: 0x004DFD5C
		public static bool TryGetCachedId<T>(out T _platformUserIdentifier) where T : PlatformUserIdentifierAbs
		{
			string idFilePath = PlatformIdCache.IdFilePath;
			if (SdFile.Exists(idFilePath))
			{
				using (Stream stream = SdFile.OpenRead(idFilePath))
				{
					using (StreamReader streamReader = new StreamReader(stream))
					{
						string text = streamReader.ReadLine();
						if (text == null)
						{
							Log.Out("[PlatformIdCache] no cached user id");
							_platformUserIdentifier = default(T);
							return false;
						}
						PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromCombinedString(text, true);
						if (!(platformUserIdentifierAbs is T))
						{
							Log.Error(string.Format("[PlatformIdCache] cannot retrieved cached id {0} as {1}", text, typeof(T)));
							_platformUserIdentifier = default(T);
							return false;
						}
						_platformUserIdentifier = (T)((object)platformUserIdentifierAbs);
						return true;
					}
				}
			}
			Log.Out("[PlatformIdCache] no id cache file at " + idFilePath);
			_platformUserIdentifier = default(T);
			return false;
		}

		// Token: 0x0600D9CC RID: 55756 RVA: 0x004E1C40 File Offset: 0x004DFE40
		public static void SetCachedId(PlatformUserIdentifierAbs _platformUserIdentifier)
		{
			using (Stream stream = SdFile.OpenWrite(PlatformIdCache.IdFilePath))
			{
				using (StreamWriter streamWriter = new StreamWriter(stream))
				{
					streamWriter.WriteLine(_platformUserIdentifier.CombinedString);
				}
			}
		}

		// Token: 0x0400A51F RID: 42271
		[PublicizedFrom(EAccessModifier.Private)]
		public const string idCacheFile = "PlatformIdCache.txt";
	}
}
