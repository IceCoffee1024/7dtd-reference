using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BhvrAnalyticsServices.Interfaces;
using Platform;
using Platform.EOS;
using SandboxOptions;
using Services.Analytics.Events;
using Steamworks;

namespace Services.Analytics
{
	// Token: 0x0200169E RID: 5790
	public static class Helper
	{
		// Token: 0x0600B507 RID: 46343 RVA: 0x0043AC1C File Offset: 0x00438E1C
		public static string GetServerId()
		{
			GameServerInfo currentGameServerInfoServerOrClient = SingletonMonoBehaviour<ConnectionManager>.Instance.CurrentGameServerInfoServerOrClient;
			string text = ((currentGameServerInfoServerOrClient != null) ? currentGameServerInfoServerOrClient.GetValue(GameInfoString.UniqueId) : null);
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return null;
		}

		// Token: 0x0600B508 RID: 46344 RVA: 0x0043AC4D File Offset: 0x00438E4D
		public static string GetSaveId()
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				return GamePrefs.GetString(EnumGamePrefs.GameGuidClient);
			}
			if (!(GameManager.Instance != null))
			{
				return null;
			}
			World world = GameManager.Instance.World;
			if (world == null)
			{
				return null;
			}
			return world.Guid;
		}

		// Token: 0x0600B509 RID: 46345 RVA: 0x0043AC8C File Offset: 0x00438E8C
		public static int? GetServerPlayerCount()
		{
			World world = GameManager.Instance.World;
			if (world == null)
			{
				return null;
			}
			if (GameManager.IsDedicatedServer)
			{
				return new int?(world.Players.dict.Values.Count);
			}
			if (world.Players.dict.Values.Count <= 0)
			{
				return null;
			}
			return new int?(world.Players.dict.Values.Count);
		}

		// Token: 0x0600B50A RID: 46346 RVA: 0x0043AD10 File Offset: 0x00438F10
		public static bool IsDevConfig()
		{
			if (!GameManager.IsDedicatedServer && PlatformManager.NativePlatform.PlatformIdentifier == EPlatformIdentifier.Steam)
			{
				try
				{
					string text;
					if (SteamApps.GetCurrentBetaName(out text, 128) && text.StartsWith("testing_", StringComparison.Ordinal))
					{
						return true;
					}
					return false;
				}
				catch (Exception ex)
				{
					Log.Out(string.Format("[Analytics] Failed to find current Steam Branch\n{0}\n{1}", ex, ex.Message));
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600B50B RID: 46347 RVA: 0x0043AD84 File Offset: 0x00438F84
		public static string GetSaveID()
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				return GamePrefs.GetString(EnumGamePrefs.GameGuidClient);
			}
			World world = GameManager.Instance.World;
			if (world == null)
			{
				return null;
			}
			return world.Guid;
		}

		// Token: 0x0600B50C RID: 46348 RVA: 0x0043ADB2 File Offset: 0x00438FB2
		public static IList<Mod> GetTruncatedLoadedMods(int modCountLimit)
		{
			return ModManager.GetLoadedMods().GetRange(0, Math.Min(ModManager.GetLoadedMods().Count, modCountLimit));
		}

		// Token: 0x0600B50D RID: 46349 RVA: 0x0043ADD0 File Offset: 0x00438FD0
		public static Dictionary<string, string> GetSandboxSettingsDelta(SandboxOptionPreset preset)
		{
			return preset.PresetValues.ToDictionary<KeyValuePair<global::SandboxOptions.SandboxOptions, int>, string, string>((KeyValuePair<global::SandboxOptions.SandboxOptions, int> kvp) => SandboxOptionManager.Current.SandboxOptionsDict[kvp.Key].OptionName, (KeyValuePair<global::SandboxOptions.SandboxOptions, int> kvp) => SandboxOptionManager.Current.SandboxOptionsDict[kvp.Key].GetValueTextFromIndex(kvp.Value, "english"));
		}

		// Token: 0x0600B50E RID: 46350 RVA: 0x0043AE26 File Offset: 0x00439026
		public static bool IsEditingOrPlaytesting()
		{
			return GameManager.Instance.IsEditMode() || GameUtils.IsPlaytesting();
		}

		// Token: 0x0600B50F RID: 46351 RVA: 0x0043AE3B File Offset: 0x0043903B
		public static IEnumerator LoginEventAnalyticCoroutine(bool isNewLogin)
		{
			LoginEventData loginEventData = new LoginEventData();
			loginEventData.SessionStartTimeStamp = DateTime.UtcNow.ToString("O");
			loginEventData.CrossplayEnabled = GamePrefs.GetBool(EnumGamePrefs.OptionsCrossplay);
			LoginEventData loginEventData2 = loginEventData;
			EUserAccountState userAccountState = EosHelpers.UserAccountState;
			bool? flag;
			if (userAccountState != EUserAccountState.NewUser)
			{
				if (userAccountState == EUserAccountState.Unknown)
				{
					flag = null;
				}
				else
				{
					flag = new bool?(false);
				}
			}
			else
			{
				flag = new bool?(true);
			}
			loginEventData2.IsFirstLaunchEos = flag;
			loginEventData.IsNewLogin = isNewLogin;
			LoginEventData loginEventData3 = loginEventData;
			IAnalyticsService analyticsService = ServiceProvider.Instance.Get<IAnalyticsService>();
			analyticsService.LogEvent(loginEventData3);
			SingletonMonoBehaviour<ConnectionManager>.Instance.BeginHeartbeat((float)analyticsService.Configuration.HeartbeatInSeconds);
			if (isNewLogin)
			{
				analyticsService.LogEvent(new HardwareInfoEventData());
				if (PlatformManager.NativePlatform.Api.ClientApiStatus != EApiStatus.Ok)
				{
					PlatformManager.NativePlatform.Api.ClientApiInitialized += Helper.<LoginEventAnalyticCoroutine>g__OnClientApiInitialized|10_0;
				}
				else
				{
					GameManager.Instance.StartCoroutine(Helper.LogDlcEntitlementsAnalytic());
				}
			}
			yield break;
		}

		// Token: 0x0600B510 RID: 46352 RVA: 0x0043AE4A File Offset: 0x0043904A
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerator LogDlcEntitlementsAnalytic()
		{
			while (EntitlementManager.Instance.IsFetchingEntitlements())
			{
				yield return null;
			}
			List<DlcOwnershipEventData.EntitlementData> list = (from entitlement in EnumUtils.Values<EntitlementSetEnum>()
				where entitlement > EntitlementSetEnum.None
				where EntitlementManager.Instance.HasEntitlement(entitlement)
				select entitlement).Select<EntitlementSetEnum, DlcOwnershipEventData.EntitlementData>(delegate(EntitlementSetEnum entitlement)
			{
				DateTime? dateTime;
				return new DlcOwnershipEventData.EntitlementData
				{
					Name = entitlement.ToStringCached<EntitlementSetEnum>(),
					ID = EntitlementManager.Instance.GetEntitlementSetId(entitlement),
					AcquiredDate = ((EntitlementManager.Instance.GetAcquiredDate(entitlement) != null) ? dateTime.GetValueOrDefault().ToUniversalTime().ToString("o") : null)
				};
			}).ToList<DlcOwnershipEventData.EntitlementData>();
			IAnalyticsService analyticsService = ServiceProvider.Instance.Get<IAnalyticsService>();
			DlcOwnershipEventData dlcOwnershipEventData = new DlcOwnershipEventData();
			dlcOwnershipEventData.DlcOwned = ((list.Count > 0) ? list : null);
			IPlatform nativePlatform = PlatformManager.NativePlatform;
			bool? flag;
			if (nativePlatform == null)
			{
				flag = null;
			}
			else
			{
				IUtils utils = nativePlatform.Utils;
				flag = ((utils != null) ? utils.IsFamilyShare() : null);
			}
			dlcOwnershipEventData.IsFamilyShare = flag;
			analyticsService.LogEvent(dlcOwnershipEventData);
			yield break;
		}

		// Token: 0x0600B511 RID: 46353 RVA: 0x0043AE52 File Offset: 0x00439052
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <LoginEventAnalyticCoroutine>g__OnClientApiInitialized|10_0()
		{
			PlatformManager.NativePlatform.Api.ClientApiInitialized -= Helper.<LoginEventAnalyticCoroutine>g__OnClientApiInitialized|10_0;
			GameManager.Instance.StartCoroutine(Helper.LogDlcEntitlementsAnalytic());
		}

		// Token: 0x040087B0 RID: 34736
		public const int ModCountLimit = 100;

		// Token: 0x040087B1 RID: 34737
		[PublicizedFrom(EAccessModifier.Private)]
		public const string TestingPrefix = "testing_";
	}
}
