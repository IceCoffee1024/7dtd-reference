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
	// Token: 0x020016B0 RID: 5808
	public static class Helper
	{
		// Token: 0x0600B55F RID: 46431 RVA: 0x0043D074 File Offset: 0x0043B274
		public static bool IsBuffEventValidOrNew(BuffValue buffV)
		{
			DateTime dateTime;
			return !Helper.DictionaryStatusTime.TryGetValue(buffV, out dateTime) || (DateTime.UtcNow - dateTime).TotalSeconds >= 60.0;
		}

		// Token: 0x0600B560 RID: 46432 RVA: 0x0043D0B3 File Offset: 0x0043B2B3
		public static void SetUseDateTimeForBuff(BuffValue buffV, DateTime time)
		{
			Helper.DictionaryStatusTime[buffV] = time;
		}

		// Token: 0x0600B561 RID: 46433 RVA: 0x0043D0C4 File Offset: 0x0043B2C4
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

		// Token: 0x0600B562 RID: 46434 RVA: 0x0043D0F5 File Offset: 0x0043B2F5
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

		// Token: 0x0600B563 RID: 46435 RVA: 0x0043D134 File Offset: 0x0043B334
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

		// Token: 0x0600B564 RID: 46436 RVA: 0x0043D1B8 File Offset: 0x0043B3B8
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

		// Token: 0x0600B565 RID: 46437 RVA: 0x0043D22C File Offset: 0x0043B42C
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

		// Token: 0x0600B566 RID: 46438 RVA: 0x0043D25A File Offset: 0x0043B45A
		public static IList<Mod> GetTruncatedLoadedMods(int modCountLimit)
		{
			return ModManager.GetLoadedMods().GetRange(0, Math.Min(ModManager.GetLoadedMods().Count, modCountLimit));
		}

		// Token: 0x0600B567 RID: 46439 RVA: 0x0043D278 File Offset: 0x0043B478
		public static Dictionary<string, string> GetSandboxSettingsDelta(SandboxOptionPreset preset)
		{
			return preset.PresetValues.ToDictionary<KeyValuePair<global::SandboxOptions.SandboxOptions, int>, string, string>((KeyValuePair<global::SandboxOptions.SandboxOptions, int> kvp) => SandboxOptionManager.Current.SandboxOptionsDict[kvp.Key].OptionName, (KeyValuePair<global::SandboxOptions.SandboxOptions, int> kvp) => SandboxOptionManager.Current.SandboxOptionsDict[kvp.Key].GetValueTextFromIndex(kvp.Value, "english"));
		}

		// Token: 0x0600B568 RID: 46440 RVA: 0x0043D2CE File Offset: 0x0043B4CE
		public static bool IsEditingOrPlaytesting()
		{
			return GameManager.Instance.IsEditMode() || GameUtils.IsPlaytesting();
		}

		// Token: 0x0600B569 RID: 46441 RVA: 0x0043D2E3 File Offset: 0x0043B4E3
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
					PlatformManager.NativePlatform.Api.ClientApiInitialized += Helper.<LoginEventAnalyticCoroutine>g__OnClientApiInitialized|14_0;
				}
				else
				{
					GameManager.Instance.StartCoroutine(Helper.LogDlcEntitlementsAnalytic());
				}
			}
			yield break;
		}

		// Token: 0x0600B56A RID: 46442 RVA: 0x0043D2F2 File Offset: 0x0043B4F2
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

		// Token: 0x0600B56C RID: 46444 RVA: 0x0043D306 File Offset: 0x0043B506
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <LoginEventAnalyticCoroutine>g__OnClientApiInitialized|14_0()
		{
			PlatformManager.NativePlatform.Api.ClientApiInitialized -= Helper.<LoginEventAnalyticCoroutine>g__OnClientApiInitialized|14_0;
			GameManager.Instance.StartCoroutine(Helper.LogDlcEntitlementsAnalytic());
		}

		// Token: 0x040087F1 RID: 34801
		public const int ModCountLimit = 100;

		// Token: 0x040087F2 RID: 34802
		[PublicizedFrom(EAccessModifier.Private)]
		public const string TestingPrefix = "testing_";

		// Token: 0x040087F3 RID: 34803
		public const int MinIntervalStatusUpdate = 60;

		// Token: 0x040087F4 RID: 34804
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<BuffValue, DateTime> DictionaryStatusTime = new Dictionary<BuffValue, DateTime>();
	}
}
