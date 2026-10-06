using System;
using System.IO;
using System.Text;
using System.Threading;
using InControl;
using UnityEngine;

namespace Platform.Shared
{
	// Token: 0x02001CE9 RID: 7401
	public class Utils : IUtils
	{
		// Token: 0x0600DB98 RID: 56216 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DB99 RID: 56217 RVA: 0x004E7320 File Offset: 0x004E5520
		public virtual bool OpenBrowser(string _url)
		{
			return Utils.OpenSystemBrowser(_url);
		}

		// Token: 0x0600DB9A RID: 56218 RVA: 0x000027FC File Offset: 0x000009FC
		public void ControllerDisconnected(InputDevice inputDevice)
		{
		}

		// Token: 0x0600DB9B RID: 56219 RVA: 0x004E7328 File Offset: 0x004E5528
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

		// Token: 0x0600DB9C RID: 56220 RVA: 0x004E739F File Offset: 0x004E559F
		public virtual string GetAppLanguage()
		{
			return this.GetPlatformLanguage();
		}

		// Token: 0x0600DB9D RID: 56221 RVA: 0x004E73A7 File Offset: 0x004E55A7
		public virtual string GetCountry()
		{
			return "??";
		}

		// Token: 0x0600DB9E RID: 56222 RVA: 0x004E73AE File Offset: 0x004E55AE
		public virtual string GetStoreBranchName()
		{
			return "none";
		}

		// Token: 0x0600DB9F RID: 56223 RVA: 0x004E5D3F File Offset: 0x004E3F3F
		public virtual void ClearTempFiles()
		{
			Utils.TryDeleteTempCacheContents();
		}

		// Token: 0x0600DBA0 RID: 56224 RVA: 0x004E5D46 File Offset: 0x004E3F46
		public virtual string GetTempFileName(string prefix = "", string suffix = "")
		{
			return Utils.GetRandomTempCacheFileName(prefix, suffix);
		}

		// Token: 0x0600DBA1 RID: 56225 RVA: 0x004E73B8 File Offset: 0x004E55B8
		public bool? IsFamilyShare()
		{
			return null;
		}

		// Token: 0x0600DBA2 RID: 56226 RVA: 0x004E73CE File Offset: 0x004E55CE
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void TryDeleteTempCacheContents()
		{
			Utils.TryDeleteTempDirectoryContentsExceptCrashes(PlatformApplicationManager.Application.temporaryCachePath);
		}

		// Token: 0x0600DBA3 RID: 56227 RVA: 0x004E73E0 File Offset: 0x004E55E0
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

		// Token: 0x0600DBA4 RID: 56228 RVA: 0x004E7500 File Offset: 0x004E5700
		[PublicizedFrom(EAccessModifier.Internal)]
		public static string GetRandomTempCacheFileName(string prefix, string suffix)
		{
			return Utils.GetRandomFileName(PlatformApplicationManager.Application.temporaryCachePath, prefix, suffix);
		}

		// Token: 0x0600DBA5 RID: 56229 RVA: 0x004E7514 File Offset: 0x004E5714
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

		// Token: 0x0600DBA6 RID: 56230 RVA: 0x004E7590 File Offset: 0x004E5790
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

		// Token: 0x0600DBA7 RID: 56231 RVA: 0x0004E540 File Offset: 0x0004C740
		public virtual string GetCrossplayPlayerIcon(EPlayGroup _playGroup, bool _fetchGenericIcons, EPlatformIdentifier _nativePlatform)
		{
			return string.Empty;
		}

		// Token: 0x0400A628 RID: 42536
		[PublicizedFrom(EAccessModifier.Private)]
		public static int Seed = Environment.TickCount;

		// Token: 0x0400A629 RID: 42537
		public static readonly ThreadLocal<global::System.Random> RandLocal = new ThreadLocal<global::System.Random>(() => new global::System.Random(Interlocked.Increment(ref Utils.Seed)));

		// Token: 0x0400A62A RID: 42538
		[PublicizedFrom(EAccessModifier.Private)]
		public string platformLanguageCache;
	}
}
