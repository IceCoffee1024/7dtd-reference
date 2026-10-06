using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using BhvrAnalyticsServices;
using BhvrAnalyticsServices.Interfaces;
using BhvrAnalyticsServices.Services;
using Platform;
using SandboxOptions;
using Services;
using Services.Analytics;
using UnityEngine;

// Token: 0x020011B4 RID: 4532
public class GameEntrypoint : MonoBehaviour
{
	// Token: 0x17001185 RID: 4485
	// (get) Token: 0x060090A6 RID: 37030 RVA: 0x003673DC File Offset: 0x003655DC
	// (set) Token: 0x060090A7 RID: 37031 RVA: 0x003673E3 File Offset: 0x003655E3
	public static bool EntrypointSuccess
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060090A8 RID: 37032 RVA: 0x003673EB File Offset: 0x003655EB
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		ThreadManager.RunCoroutineSync(GameEntrypoint.EntrypointCoroutine());
	}

	// Token: 0x060090A9 RID: 37033 RVA: 0x003673F8 File Offset: 0x003655F8
	public static bool FirstFrameInit()
	{
		Cursor.visible = false;
		ThreadManager.SetMainThreadRef(Thread.CurrentThread);
		PlatformOptimizations.Init();
		if (GameEntrypoint.HasPrefCollisions())
		{
			return false;
		}
		GamePrefs.InitPropertyDeclarations();
		if (!GameStartupHelper.Instance.InitCommandLine())
		{
			return false;
		}
		if (!string.IsNullOrEmpty(LaunchPrefs.RunAutomation.Value))
		{
			AutomationRunner.InitialiseLogging();
		}
		GameIO.InitializeUserDataPaths(LaunchPrefs.UserDataFolder.Value);
		if (!PlatformApplicationManager.Init())
		{
			return false;
		}
		if (!PlatformManager.Init())
		{
			return false;
		}
		ServiceProvider.Init();
		LogLevel logLevel = LogLevel.None;
		string launchArgument = GameUtils.GetLaunchArgument("analyticsLogLevel");
		if (launchArgument != null)
		{
			EnumUtils.TryParse<LogLevel>(launchArgument, out logLevel, true);
		}
		ServiceProvider.Instance.Register(typeof(IAnalyticsService), AnalyticsServiceFactory.CreateAnalyticsService(AnalyticsServiceFactory.LoadServiceConfiguration(Helper.IsDevConfig()), EventTypes.VersionDictionary, logLevel));
		ServiceProvider.Instance.Get<IAnalyticsService>().StartService();
		Application.targetFrameRate = (int)PlatformApplicationManager.Application.GetCurrentRefreshRate().value;
		return true;
	}

	// Token: 0x060090AA RID: 37034 RVA: 0x003674DD File Offset: 0x003656DD
	public static IEnumerator EntrypointCoroutine()
	{
		if (GameEntrypoint.s_entrypointEntered)
		{
			while (!GameEntrypoint.s_entrypointFinished)
			{
				yield return null;
			}
			yield break;
		}
		GameEntrypoint.s_entrypointEntered = true;
		try
		{
			if (!GameEntrypoint.FirstFrameInit())
			{
				yield break;
			}
			yield return GameEntrypoint.EntrypointCoroutineInternal();
		}
		finally
		{
			GameEntrypoint.s_entrypointFinished = true;
			if (!GameEntrypoint.EntrypointSuccess)
			{
				Log.Error("[GameEntrypoint] Failed initializing core systems, shutting down");
				Application.Quit();
			}
		}
		yield break;
		yield break;
	}

	// Token: 0x060090AB RID: 37035 RVA: 0x003674E5 File Offset: 0x003656E5
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator EntrypointCoroutineInternal()
	{
		yield return SaveDataUtils.InitStaticCoroutine();
		yield return null;
		GameOptionsReset.Init();
		RoamingPrefs.Init();
		PrefVersionStore store = RoamingPrefs.Store;
		if (store != null)
		{
			store.Apply();
		}
		GamePrefs.InitPrefs();
		SandboxOptionManager.Current.Init();
		GamePrefs.SetupSandboxReferences();
		GameStats.SetupSandboxReferences();
		if (!GameStartupHelper.Instance.InitGamePrefs())
		{
			yield break;
		}
		PlatformManager.MultiPlatform.UserDataRoaming.ValidateRoamingMode();
		yield return null;
		try
		{
			Localization.Init();
		}
		catch (Exception ex)
		{
			Log.Error(string.Format("[GameEntrypoint] Failed initializing localization: {0}", ex.GetType()));
			Log.Exception(ex);
			yield break;
		}
		ProfileSDF.InitializeStatics();
		Archetype.InitializeStatics();
		GameEntrypoint.EntrypointSuccess = true;
		yield break;
	}

	// Token: 0x060090AC RID: 37036 RVA: 0x003674F0 File Offset: 0x003656F0
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool HasPrefCollisions()
	{
		foreach (string text in EnumUtils.Names<EnumGamePrefs>())
		{
			ILaunchPref launchPref;
			if (LaunchPrefs.All.TryGetValue(text, out launchPref))
			{
				Log.Error(string.Concat(new string[] { "Name collision between LaunchPref '", launchPref.Name, "' and GamePref '", text, "'." }));
				return true;
			}
		}
		return false;
	}

	// Token: 0x060090AD RID: 37037 RVA: 0x00367580 File Offset: 0x00365780
	[Conditional("NEVER_DEFINED")]
	public static void ProfileSection(string identifier)
	{
		if (GameEntrypoint.s_profileTotal == null)
		{
			GameEntrypoint.s_profileTotal = new MicroStopwatch(true);
		}
		if (GameEntrypoint.s_profileSection == null)
		{
			GameEntrypoint.s_profileSection = new MicroStopwatch(true);
		}
		GameEntrypoint.s_profileIdentifier = identifier;
	}

	// Token: 0x060090AE RID: 37038 RVA: 0x003675AC File Offset: 0x003657AC
	[Conditional("PROFILE_GAME_ENTRYPOINT")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ProfileSectionEnd()
	{
		if (GameEntrypoint.s_profileIdentifier == null)
		{
			return;
		}
		Log.Out(string.Format("[GameEntrypoint: Profile] Section {0} {1:F3} ms", GameEntrypoint.s_profileIdentifier, GameEntrypoint.s_profileSection.Elapsed.TotalMilliseconds));
		GameEntrypoint.s_profileSection.Restart();
		GameEntrypoint.s_profileIdentifier = null;
	}

	// Token: 0x060090AF RID: 37039 RVA: 0x003675FC File Offset: 0x003657FC
	[Conditional("PROFILE_GAME_ENTRYPOINT")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ProfileEnd()
	{
		if (GameEntrypoint.s_profileTotal == null)
		{
			return;
		}
		Log.Out(string.Format("[GameEntrypoint: Profile] TOTAL {0:F3} ms", GameEntrypoint.s_profileTotal.Elapsed.TotalMilliseconds));
		GameEntrypoint.s_profileTotal = null;
	}

	// Token: 0x04006B0D RID: 27405
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool s_entrypointEntered;

	// Token: 0x04006B0E RID: 27406
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool s_entrypointFinished;

	// Token: 0x04006B10 RID: 27408
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static MicroStopwatch s_profileTotal;

	// Token: 0x04006B11 RID: 27409
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static MicroStopwatch s_profileSection;

	// Token: 0x04006B12 RID: 27410
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static string s_profileIdentifier;
}
