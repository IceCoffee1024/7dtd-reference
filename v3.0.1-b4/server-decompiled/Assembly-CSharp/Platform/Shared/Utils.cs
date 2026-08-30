using System;
using System.IO;
using System.Text;
using System.Threading;
using InControl;
using UnityEngine;

namespace Platform.Shared
{
	// Token: 0x02001C90 RID: 7312
	public class Utils : IUtils
	{
		// Token: 0x0600D888 RID: 55432 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600D889 RID: 55433 RVA: 0x004DC1FC File Offset: 0x004DA3FC
		public virtual bool OpenBrowser(string _url)
		{
			return Utils.OpenSystemBrowser(_url);
		}

		// Token: 0x0600D88A RID: 55434 RVA: 0x000027FC File Offset: 0x000009FC
		public void ControllerDisconnected(InputDevice inputDevice)
		{
		}

		// Token: 0x0600D88B RID: 55435 RVA: 0x004DC204 File Offset: 0x004DA404
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

		// Token: 0x0600D88C RID: 55436 RVA: 0x004DC27B File Offset: 0x004DA47B
		public virtual string GetAppLanguage()
		{
			return this.GetPlatformLanguage();
		}

		// Token: 0x0600D88D RID: 55437 RVA: 0x004DC283 File Offset: 0x004DA483
		public virtual string GetCountry()
		{
			return "??";
		}

		// Token: 0x0600D88E RID: 55438 RVA: 0x004DAC6F File Offset: 0x004D8E6F
		public virtual void ClearTempFiles()
		{
			Utils.TryDeleteTempCacheContents();
		}

		// Token: 0x0600D88F RID: 55439 RVA: 0x004DAC76 File Offset: 0x004D8E76
		public virtual string GetTempFileName(string prefix = "", string suffix = "")
		{
			return Utils.GetRandomTempCacheFileName(prefix, suffix);
		}

		// Token: 0x0600D890 RID: 55440 RVA: 0x004DC28A File Offset: 0x004DA48A
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void TryDeleteTempCacheContents()
		{
			Utils.TryDeleteTempDirectoryContentsExceptCrashes(PlatformApplicationManager.Application.temporaryCachePath);
		}

		// Token: 0x0600D891 RID: 55441 RVA: 0x004DC29C File Offset: 0x004DA49C
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

		// Token: 0x0600D892 RID: 55442 RVA: 0x004DC3BC File Offset: 0x004DA5BC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static string GetRandomTempCacheFileName(string prefix, string suffix)
		{
			return Utils.GetRandomFileName(PlatformApplicationManager.Application.temporaryCachePath, prefix, suffix);
		}

		// Token: 0x0600D893 RID: 55443 RVA: 0x004DC3D0 File Offset: 0x004DA5D0
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

		// Token: 0x0600D894 RID: 55444 RVA: 0x004DC44C File Offset: 0x004DA64C
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

		// Token: 0x0600D895 RID: 55445 RVA: 0x0004E124 File Offset: 0x0004C324
		public virtual string GetCrossplayPlayerIcon(EPlayGroup _playGroup, bool _fetchGenericIcons, EPlatformIdentifier _nativePlatform)
		{
			return string.Empty;
		}

		// Token: 0x0400A420 RID: 42016
		[PublicizedFrom(EAccessModifier.Private)]
		public static int Seed = Environment.TickCount;

		// Token: 0x0400A421 RID: 42017
		public static readonly ThreadLocal<global::System.Random> RandLocal = new ThreadLocal<global::System.Random>(() => new global::System.Random(Interlocked.Increment(ref Utils.Seed)));

		// Token: 0x0400A422 RID: 42018
		[PublicizedFrom(EAccessModifier.Private)]
		public string platformLanguageCache;
	}
}
