using System;
using System.Threading;
using UnityEngine.Events;

namespace UnityEngine.Device
{
	// Token: 0x020004B1 RID: 1201
	public static class Application
	{
		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x060029D8 RID: 10712 RVA: 0x00046EAC File Offset: 0x000450AC
		public static string absoluteURL
		{
			get
			{
				return Application.absoluteURL;
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x060029D9 RID: 10713 RVA: 0x00046EB3 File Offset: 0x000450B3
		// (set) Token: 0x060029DA RID: 10714 RVA: 0x00046EBA File Offset: 0x000450BA
		public static ThreadPriority backgroundLoadingPriority
		{
			get
			{
				return Application.backgroundLoadingPriority;
			}
			set
			{
				Application.backgroundLoadingPriority = value;
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x060029DB RID: 10715 RVA: 0x00046EC3 File Offset: 0x000450C3
		public static string buildGUID
		{
			get
			{
				return Application.buildGUID;
			}
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x060029DC RID: 10716 RVA: 0x00046ECA File Offset: 0x000450CA
		public static string cloudProjectId
		{
			get
			{
				return Application.cloudProjectId;
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x060029DD RID: 10717 RVA: 0x00046ED1 File Offset: 0x000450D1
		public static string companyName
		{
			get
			{
				return Application.companyName;
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x060029DE RID: 10718 RVA: 0x00046ED8 File Offset: 0x000450D8
		public static string consoleLogPath
		{
			get
			{
				return Application.consoleLogPath;
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x060029DF RID: 10719 RVA: 0x00046EDF File Offset: 0x000450DF
		public static string dataPath
		{
			get
			{
				return Application.dataPath;
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x060029E0 RID: 10720 RVA: 0x00046EE6 File Offset: 0x000450E6
		public static bool genuine
		{
			get
			{
				return Application.genuine;
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x060029E1 RID: 10721 RVA: 0x00046EED File Offset: 0x000450ED
		public static bool genuineCheckAvailable
		{
			get
			{
				return Application.genuineCheckAvailable;
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x060029E2 RID: 10722 RVA: 0x00046EF4 File Offset: 0x000450F4
		public static string identifier
		{
			get
			{
				return Application.identifier;
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x060029E3 RID: 10723 RVA: 0x00046EFB File Offset: 0x000450FB
		public static string installerName
		{
			get
			{
				return Application.installerName;
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x060029E4 RID: 10724 RVA: 0x00046F02 File Offset: 0x00045102
		public static ApplicationInstallMode installMode
		{
			get
			{
				return Application.installMode;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x060029E5 RID: 10725 RVA: 0x00046F09 File Offset: 0x00045109
		public static NetworkReachability internetReachability
		{
			get
			{
				return Application.internetReachability;
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x060029E6 RID: 10726 RVA: 0x00046F10 File Offset: 0x00045110
		public static bool isBatchMode
		{
			get
			{
				return Application.isBatchMode;
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x060029E7 RID: 10727 RVA: 0x00046F17 File Offset: 0x00045117
		public static bool isConsolePlatform
		{
			get
			{
				return Application.isConsolePlatform;
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x060029E8 RID: 10728 RVA: 0x00046F1E File Offset: 0x0004511E
		public static bool isEditor
		{
			get
			{
				return Application.isEditor;
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x060029E9 RID: 10729 RVA: 0x00046F25 File Offset: 0x00045125
		public static bool isFocused
		{
			get
			{
				return Application.isFocused;
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x060029EA RID: 10730 RVA: 0x00046F2C File Offset: 0x0004512C
		public static bool isMobilePlatform
		{
			get
			{
				return Application.isMobilePlatform;
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x060029EB RID: 10731 RVA: 0x00046F33 File Offset: 0x00045133
		public static bool isPlaying
		{
			get
			{
				return Application.isPlaying;
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x060029EC RID: 10732 RVA: 0x00046F3A File Offset: 0x0004513A
		public static string persistentDataPath
		{
			get
			{
				return Application.persistentDataPath;
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x060029ED RID: 10733 RVA: 0x00046F41 File Offset: 0x00045141
		public static RuntimePlatform platform
		{
			get
			{
				return Application.platform;
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x060029EE RID: 10734 RVA: 0x00046F48 File Offset: 0x00045148
		public static string productName
		{
			get
			{
				return Application.productName;
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x060029EF RID: 10735 RVA: 0x00046F4F File Offset: 0x0004514F
		// (set) Token: 0x060029F0 RID: 10736 RVA: 0x00046F56 File Offset: 0x00045156
		public static bool runInBackground
		{
			get
			{
				return Application.runInBackground;
			}
			set
			{
				Application.runInBackground = value;
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x060029F1 RID: 10737 RVA: 0x00046F5F File Offset: 0x0004515F
		public static ApplicationSandboxType sandboxType
		{
			get
			{
				return Application.sandboxType;
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x060029F2 RID: 10738 RVA: 0x00046F66 File Offset: 0x00045166
		public static string streamingAssetsPath
		{
			get
			{
				return Application.streamingAssetsPath;
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x060029F3 RID: 10739 RVA: 0x00046F6D File Offset: 0x0004516D
		public static SystemLanguage systemLanguage
		{
			get
			{
				return Application.systemLanguage;
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x060029F4 RID: 10740 RVA: 0x00046F74 File Offset: 0x00045174
		// (set) Token: 0x060029F5 RID: 10741 RVA: 0x00046F7B File Offset: 0x0004517B
		public static int targetFrameRate
		{
			get
			{
				return Application.targetFrameRate;
			}
			set
			{
				Application.targetFrameRate = value;
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x060029F6 RID: 10742 RVA: 0x00046F84 File Offset: 0x00045184
		public static string temporaryCachePath
		{
			get
			{
				return Application.temporaryCachePath;
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x060029F7 RID: 10743 RVA: 0x00046F8B File Offset: 0x0004518B
		public static string unityVersion
		{
			get
			{
				return Application.unityVersion;
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x060029F8 RID: 10744 RVA: 0x00046F92 File Offset: 0x00045192
		public static string version
		{
			get
			{
				return Application.version;
			}
		}

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x060029F9 RID: 10745 RVA: 0x00046F99 File Offset: 0x00045199
		// (remove) Token: 0x060029FA RID: 10746 RVA: 0x00046FA2 File Offset: 0x000451A2
		public static event Action<string> deepLinkActivated
		{
			add
			{
				Application.deepLinkActivated += value;
			}
			remove
			{
				Application.deepLinkActivated -= value;
			}
		}

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x060029FB RID: 10747 RVA: 0x00046FAB File Offset: 0x000451AB
		// (remove) Token: 0x060029FC RID: 10748 RVA: 0x00046FB4 File Offset: 0x000451B4
		public static event Action<bool> focusChanged
		{
			add
			{
				Application.focusChanged += value;
			}
			remove
			{
				Application.focusChanged -= value;
			}
		}

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x060029FD RID: 10749 RVA: 0x00046FBD File Offset: 0x000451BD
		// (remove) Token: 0x060029FE RID: 10750 RVA: 0x00046FC6 File Offset: 0x000451C6
		public static event Application.LogCallback logMessageReceived
		{
			add
			{
				Application.logMessageReceived += value;
			}
			remove
			{
				Application.logMessageReceived -= value;
			}
		}

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x060029FF RID: 10751 RVA: 0x00046FCF File Offset: 0x000451CF
		// (remove) Token: 0x06002A00 RID: 10752 RVA: 0x00046FD8 File Offset: 0x000451D8
		public static event Application.LogCallback logMessageReceivedThreaded
		{
			add
			{
				Application.logMessageReceivedThreaded += value;
			}
			remove
			{
				Application.logMessageReceivedThreaded -= value;
			}
		}

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06002A01 RID: 10753 RVA: 0x00046FE1 File Offset: 0x000451E1
		// (remove) Token: 0x06002A02 RID: 10754 RVA: 0x00046FEA File Offset: 0x000451EA
		public static event Application.LowMemoryCallback lowMemory
		{
			add
			{
				Application.lowMemory += value;
			}
			remove
			{
				Application.lowMemory -= value;
			}
		}

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06002A03 RID: 10755 RVA: 0x00046FF3 File Offset: 0x000451F3
		// (remove) Token: 0x06002A04 RID: 10756 RVA: 0x00046FFC File Offset: 0x000451FC
		public static event Application.MemoryUsageChangedCallback memoryUsageChanged
		{
			add
			{
				Application.memoryUsageChanged += value;
			}
			remove
			{
				Application.memoryUsageChanged -= value;
			}
		}

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06002A05 RID: 10757 RVA: 0x00047005 File Offset: 0x00045205
		// (remove) Token: 0x06002A06 RID: 10758 RVA: 0x0004700E File Offset: 0x0004520E
		public static event UnityAction onBeforeRender
		{
			add
			{
				Application.onBeforeRender += value;
			}
			remove
			{
				Application.onBeforeRender -= value;
			}
		}

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06002A07 RID: 10759 RVA: 0x00047017 File Offset: 0x00045217
		// (remove) Token: 0x06002A08 RID: 10760 RVA: 0x00047020 File Offset: 0x00045220
		public static event Action quitting
		{
			add
			{
				Application.quitting += value;
			}
			remove
			{
				Application.quitting -= value;
			}
		}

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06002A09 RID: 10761 RVA: 0x00047029 File Offset: 0x00045229
		// (remove) Token: 0x06002A0A RID: 10762 RVA: 0x00047032 File Offset: 0x00045232
		public static event Func<bool> wantsToQuit
		{
			add
			{
				Application.wantsToQuit += value;
			}
			remove
			{
				Application.wantsToQuit -= value;
			}
		}

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x06002A0B RID: 10763 RVA: 0x0004703B File Offset: 0x0004523B
		// (remove) Token: 0x06002A0C RID: 10764 RVA: 0x00047044 File Offset: 0x00045244
		public static event Action unloading
		{
			add
			{
				Application.unloading += value;
			}
			remove
			{
				Application.unloading -= value;
			}
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x00047050 File Offset: 0x00045250
		public static bool CanStreamedLevelBeLoaded(int levelIndex)
		{
			return Application.CanStreamedLevelBeLoaded(levelIndex);
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x00047068 File Offset: 0x00045268
		public static bool CanStreamedLevelBeLoaded(string levelName)
		{
			return Application.CanStreamedLevelBeLoaded(levelName);
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x00047080 File Offset: 0x00045280
		[Obsolete("Application.GetBuildTags is no longer supported and will be removed.", false)]
		public static string[] GetBuildTags()
		{
			return Application.GetBuildTags();
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x00047097 File Offset: 0x00045297
		[Obsolete("Application.SetBuildTags is no longer supported and will be removed.", false)]
		public static void SetBuildTags(string[] buildTags)
		{
			Application.SetBuildTags(buildTags);
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x000470A4 File Offset: 0x000452A4
		public static StackTraceLogType GetStackTraceLogType(LogType logType)
		{
			return Application.GetStackTraceLogType(logType);
		}

		// Token: 0x06002A12 RID: 10770 RVA: 0x000470BC File Offset: 0x000452BC
		public static bool HasProLicense()
		{
			return Application.HasProLicense();
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x000470D4 File Offset: 0x000452D4
		public static bool HasUserAuthorization(UserAuthorization mode)
		{
			return Application.HasUserAuthorization(mode);
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x000470EC File Offset: 0x000452EC
		public static bool IsPlaying(Object obj)
		{
			return Application.IsPlaying(obj);
		}

		// Token: 0x06002A15 RID: 10773 RVA: 0x00047104 File Offset: 0x00045304
		public static void OpenURL(string url)
		{
			Application.OpenURL(url);
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x0004710E File Offset: 0x0004530E
		public static void Quit()
		{
			Application.Quit();
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x00047117 File Offset: 0x00045317
		public static void Quit(int exitCode)
		{
			Application.Quit(exitCode);
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x00047124 File Offset: 0x00045324
		public static bool RequestAdvertisingIdentifierAsync(Application.AdvertisingIdentifierCallback delegateMethod)
		{
			return Application.RequestAdvertisingIdentifierAsync(delegateMethod);
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x0004713C File Offset: 0x0004533C
		public static AsyncOperation RequestUserAuthorization(UserAuthorization mode)
		{
			return Application.RequestUserAuthorization(mode);
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x00047154 File Offset: 0x00045354
		public static void SetStackTraceLogType(LogType logType, StackTraceLogType stackTraceType)
		{
			Application.SetStackTraceLogType(logType, stackTraceType);
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x0004715F File Offset: 0x0004535F
		public static void Unload()
		{
			Application.Unload();
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06002A1C RID: 10780 RVA: 0x00047168 File Offset: 0x00045368
		public static CancellationToken exitCancellationToken
		{
			get
			{
				return Application.exitCancellationToken;
			}
		}
	}
}
