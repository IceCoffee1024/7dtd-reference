using System;
using System.IO;
using System.Text;
using System.Threading;
using InControl;
using UnityEngine;

namespace Platform.Shared
{
	// Token: 0x02001D1A RID: 7450
	public class Utils : IUtils
	{
		// Token: 0x0600DCAC RID: 56492 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DCAD RID: 56493 RVA: 0x004EAEC0 File Offset: 0x004E90C0
		public virtual bool OpenBrowser(string _url)
		{
			return Utils.OpenSystemBrowser(_url);
		}

		// Token: 0x0600DCAE RID: 56494 RVA: 0x000027FC File Offset: 0x000009FC
		public void ControllerDisconnected(InputDevice inputDevice)
		{
		}

		// Token: 0x0600DCAF RID: 56495 RVA: 0x004EAEC8 File Offset: 0x004E90C8
		public virtual string GetPlatformLanguage()
		{
			if (this.platformLanguageCache == null)
			{
				string text = Application.systemLanguage.ToStringCached<SystemLanguage>().ToLower();
				string text2;
				if (!(text == "chinesesimplified"))
				{
					if (!(text == "chinesetraditional"))
					{
						if (!(text == "korean"))
						{
							text2 = text;
						}
						else
						{
							text2 = "koreana";
						}
					}
					else
					{
						text2 = "tchinese";
					}
				}
				else
				{
					text2 = "schinese";
				}
				text = text2;
				this.platformLanguageCache = text;
			}
			return this.platformLanguageCache;
		}

		// Token: 0x0600DCB0 RID: 56496 RVA: 0x004EAF3F File Offset: 0x004E913F
		public virtual string GetAppLanguage()
		{
			return this.GetPlatformLanguage();
		}

		// Token: 0x0600DCB1 RID: 56497 RVA: 0x004EAF47 File Offset: 0x004E9147
		public virtual string GetCountry()
		{
			return "??";
		}

		// Token: 0x0600DCB2 RID: 56498 RVA: 0x004EAF4E File Offset: 0x004E914E
		public virtual string GetStoreBranchName()
		{
			return "none";
		}

		// Token: 0x0600DCB3 RID: 56499 RVA: 0x004E98DF File Offset: 0x004E7ADF
		public virtual void ClearTempFiles()
		{
			Utils.TryDeleteTempCacheContents();
		}

		// Token: 0x0600DCB4 RID: 56500 RVA: 0x004E98E6 File Offset: 0x004E7AE6
		public virtual string GetTempFileName(string prefix = "", string suffix = "")
		{
			return Utils.GetRandomTempCacheFileName(prefix, suffix);
		}

		// Token: 0x0600DCB5 RID: 56501 RVA: 0x004EAF58 File Offset: 0x004E9158
		public bool? IsFamilyShare()
		{
			return null;
		}

		// Token: 0x0600DCB6 RID: 56502 RVA: 0x004EAF6E File Offset: 0x004E916E
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void TryDeleteTempCacheContents()
		{
			Utils.TryDeleteTempDirectoryContentsExceptCrashes(PlatformApplicationManager.Application.temporaryCachePath);
		}

		// Token: 0x0600DCB7 RID: 56503 RVA: 0x004EAF80 File Offset: 0x004E9180
		[PublicizedFrom(EAccessModifier.Private)]
		public static void TryDeleteTempDirectoryContentsExceptCrashes(string path)
		{
			try
			{
				if (Directory.Exists(path))
				{
					foreach (FileSystemInfo fileSystemInfo in new DirectoryInfo(path).EnumerateFileSystemInfos())
					{
						try
						{
							if (!fileSystemInfo.Name.EqualsCaseInsensitive("Crashes"))
							{
								DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;
								if (directoryInfo != null)
								{
									directoryInfo.Delete(true);
								}
								else
								{
									fileSystemInfo.Delete();
								}
							}
						}
						catch (Exception ex)
						{
							Log.Warning(string.Concat(new string[]
							{
								"[Platform.Shared.Utils] Could not delete '",
								fileSystemInfo.Name,
								"' from temp cache. ",
								ex.GetType().FullName,
								": ",
								ex.Message
							}));
						}
					}
				}
			}
			catch (Exception ex2)
			{
				Log.Warning("[Platform.Shared.Utils] Could not delete contents of temp cache. " + ex2.GetType().FullName + ": " + ex2.Message);
			}
		}

		// Token: 0x0600DCB8 RID: 56504 RVA: 0x004EB0A0 File Offset: 0x004E92A0
		[PublicizedFrom(EAccessModifier.Internal)]
		public static string GetRandomTempCacheFileName(string prefix, string suffix)
		{
			return Utils.GetRandomFileName(PlatformApplicationManager.Application.temporaryCachePath, prefix, suffix);
		}

		// Token: 0x0600DCB9 RID: 56505 RVA: 0x004EB0B4 File Offset: 0x004E92B4
		[PublicizedFrom(EAccessModifier.Private)]
		public static string GetRandomFileName(string parentDir, string prefix, string suffix)
		{
			for (int i = 0; i < 100; i++)
			{
				string randomName = Utils.GetRandomName(prefix, suffix);
				string text = Path.Join(parentDir, randomName);
				if (!File.Exists(text))
				{
					using (File.Open(text, FileMode.OpenOrCreate))
					{
						return text;
					}
				}
			}
			throw new IOException(string.Format("Failed to create a temporary file after {0} attempts.", 100));
		}

		// Token: 0x0600DCBA RID: 56506 RVA: 0x004EB130 File Offset: 0x004E9330
		[PublicizedFrom(EAccessModifier.Private)]
		public static string GetRandomName(string prefix, string suffix)
		{
			global::System.Random value = Utils.RandLocal.Value;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(prefix);
			for (int i = 0; i < 16; i++)
			{
				stringBuilder.Append("0123456789ABCDEFGHIJKLMNOPabcdefghijklmnop"[value.Next("0123456789ABCDEFGHIJKLMNOPabcdefghijklmnop".Length)]);
			}
			stringBuilder.Append(suffix);
			return stringBuilder.ToString();
		}

		// Token: 0x0600DCBB RID: 56507 RVA: 0x0004E500 File Offset: 0x0004C700
		public virtual string GetCrossplayPlayerIcon(EPlayGroup _playGroup, bool _fetchGenericIcons, EPlatformIdentifier _nativePlatform)
		{
			return string.Empty;
		}

		// Token: 0x0400A711 RID: 42769
		[PublicizedFrom(EAccessModifier.Private)]
		public static int Seed = Environment.TickCount;

		// Token: 0x0400A712 RID: 42770
		public static readonly ThreadLocal<global::System.Random> RandLocal = new ThreadLocal<global::System.Random>(() => new global::System.Random(Interlocked.Increment(ref Utils.Seed)));

		// Token: 0x0400A713 RID: 42771
		[PublicizedFrom(EAccessModifier.Private)]
		public string platformLanguageCache;
	}
}
