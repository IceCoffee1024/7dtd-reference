using System;
using System.IO;
using System.Text;
using System.Threading;
using InControl;
using UnityEngine;

namespace Platform.Shared
{
	// Token: 0x02001CB7 RID: 7351
	public class Utils : IUtils
	{
		// Token: 0x0600DA1E RID: 55838 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DA1F RID: 55839 RVA: 0x004E27D8 File Offset: 0x004E09D8
		public virtual bool OpenBrowser(string _url)
		{
			return Utils.OpenSystemBrowser(_url);
		}

		// Token: 0x0600DA20 RID: 55840 RVA: 0x000027FC File Offset: 0x000009FC
		public void ControllerDisconnected(InputDevice inputDevice)
		{
		}

		// Token: 0x0600DA21 RID: 55841 RVA: 0x004E27E0 File Offset: 0x004E09E0
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

		// Token: 0x0600DA22 RID: 55842 RVA: 0x004E2857 File Offset: 0x004E0A57
		public virtual string GetAppLanguage()
		{
			return this.GetPlatformLanguage();
		}

		// Token: 0x0600DA23 RID: 55843 RVA: 0x004E285F File Offset: 0x004E0A5F
		public virtual string GetCountry()
		{
			return "??";
		}

		// Token: 0x0600DA24 RID: 55844 RVA: 0x004E2866 File Offset: 0x004E0A66
		public virtual string GetStoreBranchName()
		{
			return "none";
		}

		// Token: 0x0600DA25 RID: 55845 RVA: 0x004E11F7 File Offset: 0x004DF3F7
		public virtual void ClearTempFiles()
		{
			Utils.TryDeleteTempCacheContents();
		}

		// Token: 0x0600DA26 RID: 55846 RVA: 0x004E11FE File Offset: 0x004DF3FE
		public virtual string GetTempFileName(string prefix = "", string suffix = "")
		{
			return Utils.GetRandomTempCacheFileName(prefix, suffix);
		}

		// Token: 0x0600DA27 RID: 55847 RVA: 0x004E2870 File Offset: 0x004E0A70
		public bool? IsFamilyShare()
		{
			return null;
		}

		// Token: 0x0600DA28 RID: 55848 RVA: 0x004E2886 File Offset: 0x004E0A86
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void TryDeleteTempCacheContents()
		{
			Utils.TryDeleteTempDirectoryContentsExceptCrashes(PlatformApplicationManager.Application.temporaryCachePath);
		}

		// Token: 0x0600DA29 RID: 55849 RVA: 0x004E2898 File Offset: 0x004E0A98
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

		// Token: 0x0600DA2A RID: 55850 RVA: 0x004E29B8 File Offset: 0x004E0BB8
		[PublicizedFrom(EAccessModifier.Internal)]
		public static string GetRandomTempCacheFileName(string prefix, string suffix)
		{
			return Utils.GetRandomFileName(PlatformApplicationManager.Application.temporaryCachePath, prefix, suffix);
		}

		// Token: 0x0600DA2B RID: 55851 RVA: 0x004E29CC File Offset: 0x004E0BCC
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

		// Token: 0x0600DA2C RID: 55852 RVA: 0x004E2A48 File Offset: 0x004E0C48
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

		// Token: 0x0600DA2D RID: 55853 RVA: 0x0004E53C File Offset: 0x0004C73C
		public virtual string GetCrossplayPlayerIcon(EPlayGroup _playGroup, bool _fetchGenericIcons, EPlatformIdentifier _nativePlatform)
		{
			return string.Empty;
		}

		// Token: 0x0400A553 RID: 42323
		[PublicizedFrom(EAccessModifier.Private)]
		public static int Seed = Environment.TickCount;

		// Token: 0x0400A554 RID: 42324
		public static readonly ThreadLocal<global::System.Random> RandLocal = new ThreadLocal<global::System.Random>(() => new global::System.Random(Interlocked.Increment(ref Utils.Seed)));

		// Token: 0x0400A555 RID: 42325
		[PublicizedFrom(EAccessModifier.Private)]
		public string platformLanguageCache;
	}
}
