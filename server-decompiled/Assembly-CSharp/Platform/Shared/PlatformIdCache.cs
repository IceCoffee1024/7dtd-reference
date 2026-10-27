using System;
using System.IO;

namespace Platform.Shared
{
	// Token: 0x02001D10 RID: 7440
	public static class PlatformIdCache
	{
		// Token: 0x17001BFC RID: 7164
		// (get) Token: 0x0600DC58 RID: 56408 RVA: 0x004EA231 File Offset: 0x004E8431
		public static string IdFilePath
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return Path.Combine(GameIO.GetUserGameDataDir(), "PlatformIdCache.txt");
			}
		}

		// Token: 0x0600DC59 RID: 56409 RVA: 0x004EA244 File Offset: 0x004E8444
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

		// Token: 0x0600DC5A RID: 56410 RVA: 0x004EA328 File Offset: 0x004E8528
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

		// Token: 0x0400A6DD RID: 42717
		[PublicizedFrom(EAccessModifier.Private)]
		public const string idCacheFile = "PlatformIdCache.txt";
	}
}
