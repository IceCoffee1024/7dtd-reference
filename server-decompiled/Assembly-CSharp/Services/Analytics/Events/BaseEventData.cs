using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using BhvrAnalyticsServices.Interfaces;
using Newtonsoft.Json;
using Platform;
using UnityEngine;

namespace Services.Analytics.Events
{
	// Token: 0x02001683 RID: 5763
	[JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
	public abstract class BaseEventData : IAnalyticsEventData
	{
		// Token: 0x170015AE RID: 5550
		// (get) Token: 0x0600B48F RID: 46223
		public abstract string EventType { get; }

		// Token: 0x170015AF RID: 5551
		// (get) Token: 0x0600B490 RID: 46224 RVA: 0x004374F8 File Offset: 0x004356F8
		[JsonProperty(PropertyName = "provider_user_id")]
		[Description("The unique player identifier based on the provider_id (anonymized)")]
		[KrakenIdTransform("$.provider_user_id")]
		public string ProviderUserId { get; }

		// Token: 0x170015B0 RID: 5552
		// (get) Token: 0x0600B491 RID: 46225 RVA: 0x00437500 File Offset: 0x00435700
		[JsonProperty(PropertyName = "session_build")]
		[Description("The name of the build used for the game session initiated (patch/CL number)")]
		public string SessionBuild
		{
			get
			{
				return Constants.cVersionInformation.Build.ToString();
			}
		}

		// Token: 0x170015B1 RID: 5553
		// (get) Token: 0x0600B492 RID: 46226 RVA: 0x0043751F File Offset: 0x0043571F
		[JsonProperty(PropertyName = "game_version")]
		[Description("The name of the game version (Major.Minor.Hotfix)")]
		public string GameVersion
		{
			get
			{
				return Constants.cVersionInformation.ShortString;
			}
		}

		// Token: 0x170015B2 RID: 5554
		// (get) Token: 0x0600B493 RID: 46227 RVA: 0x0043752B File Offset: 0x0043572B
		[JsonProperty(PropertyName = "easy_anti_cheat_enabled")]
		[Description("TRUE if the player launched with EAC activated")]
		public bool EasyAntiCheatEnabled { get; }

		// Token: 0x170015B3 RID: 5555
		// (get) Token: 0x0600B494 RID: 46228 RVA: 0x00437533 File Offset: 0x00435733
		[JsonProperty(PropertyName = "steam_branch_name")]
		[Description("The name of the steam branch")]
		public string SteamBranchName
		{
			get
			{
				IPlatform nativePlatform = PlatformManager.NativePlatform;
				string text;
				if (nativePlatform == null)
				{
					text = null;
				}
				else
				{
					IUtils utils = nativePlatform.Utils;
					text = ((utils != null) ? utils.GetStoreBranchName() : null);
				}
				return text ?? "none";
			}
		}

		// Token: 0x170015B4 RID: 5556
		// (get) Token: 0x0600B495 RID: 46229 RVA: 0x0043755B File Offset: 0x0043575B
		[JsonProperty(PropertyName = "platform")]
		[Description("The platform that the game session was initiated on.")]
		public string Platform { get; }

		// Token: 0x170015B5 RID: 5557
		// (get) Token: 0x0600B496 RID: 46230 RVA: 0x00437563 File Offset: 0x00435763
		[JsonProperty(PropertyName = "provider")]
		[Description("Provider Used to launch the game (Steam, XBL, PSN)")]
		public string Provider { get; }

		// Token: 0x0600B497 RID: 46231 RVA: 0x0043756C File Offset: 0x0043576C
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseEventData()
		{
			IPlatform nativePlatform = PlatformManager.NativePlatform;
			string text;
			if (nativePlatform == null)
			{
				text = null;
			}
			else
			{
				IUserClient user = nativePlatform.User;
				if (user == null)
				{
					text = null;
				}
				else
				{
					PlatformUserIdentifierAbs platformUserId = user.PlatformUserId;
					text = ((platformUserId != null) ? platformUserId.ReadablePlatformUserIdentifier : null);
				}
			}
			this.ProviderUserId = text;
			IPlatform multiPlatform = PlatformManager.MultiPlatform;
			bool? flag;
			if (multiPlatform == null)
			{
				flag = null;
			}
			else
			{
				IAntiCheatClient antiCheatClient = multiPlatform.AntiCheatClient;
				flag = ((antiCheatClient != null) ? new bool?(antiCheatClient.ClientAntiCheatEnabled()) : null);
			}
			bool? flag2 = flag;
			this.EasyAntiCheatEnabled = flag2.GetValueOrDefault();
			this.Platform = Application.platform.ToString();
			IPlatform nativePlatform2 = PlatformManager.NativePlatform;
			this.Provider = ((nativePlatform2 != null) ? nativePlatform2.PlatformIdentifier.ToString() : null) ?? "Unknown";
			base..ctor();
		}
	}
}
