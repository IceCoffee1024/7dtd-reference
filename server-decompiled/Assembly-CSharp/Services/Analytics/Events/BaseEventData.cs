using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using BhvrAnalyticsServices.Interfaces;
using Newtonsoft.Json;
using Platform;
using UnityEngine;

namespace Services.Analytics.Events
{
	// Token: 0x020016A3 RID: 5795
	[JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
	public abstract class BaseEventData : IAnalyticsEventData
	{
		// Token: 0x170015F3 RID: 5619
		// (get) Token: 0x0600B529 RID: 46377
		public abstract string EventType { get; }

		// Token: 0x170015F4 RID: 5620
		// (get) Token: 0x0600B52A RID: 46378 RVA: 0x0043B20C File Offset: 0x0043940C
		[JsonProperty(PropertyName = "provider_user_id")]
		[Description("The unique player identifier based on the provider_id (anonymized)")]
		[KrakenIdTransform("$.provider_user_id")]
		public string ProviderUserId { get; }

		// Token: 0x170015F5 RID: 5621
		// (get) Token: 0x0600B52B RID: 46379 RVA: 0x0043B214 File Offset: 0x00439414
		[JsonProperty(PropertyName = "session_build")]
		[Description("The name of the build used for the game session initiated (patch/CL number)")]
		public string SessionBuild
		{
			get
			{
				return Constants.cVersionInformation.Build.ToString();
			}
		}

		// Token: 0x170015F6 RID: 5622
		// (get) Token: 0x0600B52C RID: 46380 RVA: 0x0043B233 File Offset: 0x00439433
		[JsonProperty(PropertyName = "game_version")]
		[Description("The name of the game version (Major.Minor.Hotfix)")]
		public string GameVersion
		{
			get
			{
				return Constants.cVersionInformation.ShortString;
			}
		}

		// Token: 0x170015F7 RID: 5623
		// (get) Token: 0x0600B52D RID: 46381 RVA: 0x0043B23F File Offset: 0x0043943F
		[JsonProperty(PropertyName = "easy_anti_cheat_enabled")]
		[Description("TRUE if the player launched with EAC activated")]
		public bool EasyAntiCheatEnabled { get; }

		// Token: 0x170015F8 RID: 5624
		// (get) Token: 0x0600B52E RID: 46382 RVA: 0x0043B247 File Offset: 0x00439447
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

		// Token: 0x170015F9 RID: 5625
		// (get) Token: 0x0600B52F RID: 46383 RVA: 0x0043B26F File Offset: 0x0043946F
		[JsonProperty(PropertyName = "platform")]
		[Description("The platform that the game session was initiated on.")]
		public string Platform { get; }

		// Token: 0x170015FA RID: 5626
		// (get) Token: 0x0600B530 RID: 46384 RVA: 0x0043B277 File Offset: 0x00439477
		[JsonProperty(PropertyName = "provider")]
		[Description("Provider Used to launch the game (Steam, XBL, PSN)")]
		public string Provider { get; }

		// Token: 0x0600B531 RID: 46385 RVA: 0x0043B280 File Offset: 0x00439480
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
