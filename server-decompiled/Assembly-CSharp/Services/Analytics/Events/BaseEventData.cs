using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using BhvrAnalyticsServices.Interfaces;
using Newtonsoft.Json;
using Platform;
using UnityEngine;

namespace Services.Analytics.Events
{
	// Token: 0x020016B5 RID: 5813
	[JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
	public abstract class BaseEventData : IAnalyticsEventData
	{
		// Token: 0x170015FF RID: 5631
		// (get) Token: 0x0600B584 RID: 46468
		public abstract string EventType { get; }

		// Token: 0x17001600 RID: 5632
		// (get) Token: 0x0600B585 RID: 46469 RVA: 0x0043D6C0 File Offset: 0x0043B8C0
		[JsonProperty(PropertyName = "provider_user_id")]
		[Description("The unique player identifier based on the provider_id (anonymized)")]
		[KrakenIdTransform("$.provider_user_id")]
		public string ProviderUserId { get; }

		// Token: 0x17001601 RID: 5633
		// (get) Token: 0x0600B586 RID: 46470 RVA: 0x0043D6C8 File Offset: 0x0043B8C8
		[JsonProperty(PropertyName = "session_build")]
		[Description("The name of the build used for the game session initiated (patch/CL number)")]
		public string SessionBuild
		{
			get
			{
				return Constants.cVersionInformation.Build.ToString();
			}
		}

		// Token: 0x17001602 RID: 5634
		// (get) Token: 0x0600B587 RID: 46471 RVA: 0x0043D6E7 File Offset: 0x0043B8E7
		[JsonProperty(PropertyName = "game_version")]
		[Description("The name of the game version (Major.Minor.Hotfix)")]
		public string GameVersion
		{
			get
			{
				return Constants.cVersionInformation.ShortString;
			}
		}

		// Token: 0x17001603 RID: 5635
		// (get) Token: 0x0600B588 RID: 46472 RVA: 0x0043D6F3 File Offset: 0x0043B8F3
		[JsonProperty(PropertyName = "easy_anti_cheat_enabled")]
		[Description("TRUE if the player launched with EAC activated")]
		public bool EasyAntiCheatEnabled { get; }

		// Token: 0x17001604 RID: 5636
		// (get) Token: 0x0600B589 RID: 46473 RVA: 0x0043D6FB File Offset: 0x0043B8FB
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

		// Token: 0x17001605 RID: 5637
		// (get) Token: 0x0600B58A RID: 46474 RVA: 0x0043D723 File Offset: 0x0043B923
		[JsonProperty(PropertyName = "platform")]
		[Description("The platform that the game session was initiated on.")]
		public string Platform { get; }

		// Token: 0x17001606 RID: 5638
		// (get) Token: 0x0600B58B RID: 46475 RVA: 0x0043D72B File Offset: 0x0043B92B
		[JsonProperty(PropertyName = "provider")]
		[Description("Provider Used to launch the game (Steam, XBL, PSN)")]
		public string Provider { get; }

		// Token: 0x0600B58C RID: 46476 RVA: 0x0043D734 File Offset: 0x0043B934
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
