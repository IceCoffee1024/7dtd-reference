using System;
using System.IO;

namespace Platform.Shared
{
	// Token: 0x02001CDF RID: 7391
	public static class PlatformIdCache
	{
		// Token: 0x17001BB4 RID: 7092
		// (get) Token: 0x0600DB44 RID: 56132 RVA: 0x004E6691 File Offset: 0x004E4891
		public static string IdFilePath
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return Path.Combine(GameIO.GetUserGameDataDir(), "PlatformIdCache.txt");
			}
		}

		// Token: 0x0600DB45 RID: 56133 RVA: 0x004E66A4 File Offset: 0x004E48A4
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

		// Token: 0x0600DB46 RID: 56134 RVA: 0x004E6788 File Offset: 0x004E4988
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

		// Token: 0x0400A5F4 RID: 42484
		[PublicizedFrom(EAccessModifier.Private)]
		public const string idCacheFile = "PlatformIdCache.txt";
	}
}
