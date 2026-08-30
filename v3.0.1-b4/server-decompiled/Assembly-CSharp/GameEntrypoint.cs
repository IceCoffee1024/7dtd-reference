using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using BhvrAnalyticsServices.Interfaces;
using BhvrAnalyticsServices.Services;
using Platform;
using SandboxOptions;
using Services;
using UnityEngine;

// Token: 0x02001182 RID: 4482
public class GameEntrypoint : MonoBehaviour
{
	// Token: 0x17001108 RID: 4360
	// (get) Token: 0x06008F1E RID: 36638 RVA: 0x0035DF9C File Offset: 0x0035C19C
	// (set) Token: 0x06008F1F RID: 36639 RVA: 0x0035DFA3 File Offset: 0x0035C1A3
	public static bool EntrypointSuccess
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06008F20 RID: 36640 RVA: 0x0035DFAB File Offset: 0x0035C1AB
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		ThreadManager.RunCoroutineSync(GameEntrypoint.EntrypointCoroutine());
	}

	// Token: 0x06008F21 RID: 36641 RVA: 0x0035DFB8 File Offset: 0x0035C1B8
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
		ServiceProvider.Instance.Register(typeof(IAnalyticsService), AnalyticsServiceFactory.CreateAnalyticsService(AnalyticsServiceFactory.LoadServiceConfiguration()));
		ServiceProvider.Instance.Get<IAnalyticsService>().StartService();
		Application.targetFrameRate = (int)PlatformApplicationManager.Application.GetCurrentRefreshRate().value;
		return true;
	}

	// Token: 0x06008F22 RID: 36642 RVA: 0x0035E078 File Offset: 0x0035C278
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

	// Token: 0x06008F23 RID: 36643 RVA: 0x0035E080 File Offset: 0x0035C280
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

	// Token: 0x06008F24 RID: 36644 RVA: 0x0035E088 File Offset: 0x0035C288
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

	// Token: 0x06008F25 RID: 36645 RVA: 0x0035E118 File Offset: 0x0035C318
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

	// Token: 0x06008F26 RID: 36646 RVA: 0x0035E144 File Offset: 0x0035C344
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

	// Token: 0x06008F27 RID: 36647 RVA: 0x0035E194 File Offset: 0x0035C394
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

	// Token: 0x040069D4 RID: 27092
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool s_entrypointEntered;

	// Token: 0x040069D5 RID: 27093
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool s_entrypointFinished;

	// Token: 0x040069D7 RID: 27095
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static MicroStopwatch s_profileTotal;

	// Token: 0x040069D8 RID: 27096
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static MicroStopwatch s_profileSection;

	// Token: 0x040069D9 RID: 27097
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static string s_profileIdentifier;
}
