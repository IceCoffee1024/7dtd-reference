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
using UnityEngine.Networking;

namespace Services.Analytics
{
	// Token: 0x0200167F RID: 5759
	public static class Helper
	{
		// Token: 0x0600B475 RID: 46197 RVA: 0x00436F04 File Offset: 0x00435104
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

		// Token: 0x0600B476 RID: 46198 RVA: 0x00436F38 File Offset: 0x00435138
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

		// Token: 0x0600B477 RID: 46199 RVA: 0x00436FBB File Offset: 0x004351BB
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

		// Token: 0x0600B478 RID: 46200 RVA: 0x00436FE9 File Offset: 0x004351E9
		public static IList<Mod> GetTruncatedLoadedMods(int modCountLimit)
		{
			return ModManager.GetLoadedMods().GetRange(0, Math.Min(ModManager.GetLoadedMods().Count, modCountLimit));
		}

		// Token: 0x0600B479 RID: 46201 RVA: 0x00437008 File Offset: 0x00435208
		public static Dictionary<string, string> GetSandboxSettingsDelta(SandboxOptionPreset preset)
		{
			return preset.PresetValues.ToDictionary<KeyValuePair<global::SandboxOptions.SandboxOptions, int>, string, string>((KeyValuePair<global::SandboxOptions.SandboxOptions, int> kvp) => SandboxOptionManager.Current.SandboxOptionsDict[kvp.Key].OptionName, (KeyValuePair<global::SandboxOptions.SandboxOptions, int> kvp) => SandboxOptionManager.Current.SandboxOptionsDict[kvp.Key].GetValueTextFromIndex(kvp.Value, "english"));
		}

		// Token: 0x0600B47A RID: 46202 RVA: 0x0043705E File Offset: 0x0043525E
		public static IEnumerator LoginEventAnalyticCoroutine(bool isNewLogin)
		{
			string ipAddress = "Unknown";
			using (UnityWebRequest webRequest = UnityWebRequest.Get("https://api.ipify.org"))
			{
				webRequest.timeout = 10;
				yield return webRequest.SendWebRequest();
				if (webRequest.result == UnityWebRequest.Result.Success)
				{
					ipAddress = webRequest.downloadHandler.text;
				}
				else
				{
					Log.Out("[Kraken] Error: Failed to retrieve IP address: " + webRequest.error);
				}
			}
			UnityWebRequest webRequest = null;
			LoginEventData loginEventData = new LoginEventData();
			loginEventData.SessionStartTimeStamp = DateTime.UtcNow.ToString("O");
			loginEventData.IP = ipAddress;
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
					PlatformManager.NativePlatform.Api.ClientApiInitialized += Helper.<LoginEventAnalyticCoroutine>g__OnClientApiInitialized|6_0;
				}
				else
				{
					Helper.LogDlcEntitlementsAnalytic();
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600B47B RID: 46203 RVA: 0x00437070 File Offset: 0x00435270
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogDlcEntitlementsAnalytic()
		{
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
		}

		// Token: 0x0600B47C RID: 46204 RVA: 0x0043714E File Offset: 0x0043534E
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <LoginEventAnalyticCoroutine>g__OnClientApiInitialized|6_0()
		{
			PlatformManager.NativePlatform.Api.ClientApiInitialized -= Helper.<LoginEventAnalyticCoroutine>g__OnClientApiInitialized|6_0;
			Helper.LogDlcEntitlementsAnalytic();
		}

		// Token: 0x0400875A RID: 34650
		public const int ModCountLimit = 100;
	}
}
