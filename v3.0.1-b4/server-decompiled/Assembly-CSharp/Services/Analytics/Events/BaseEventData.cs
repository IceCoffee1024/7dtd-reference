using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Interfaces;
using Newtonsoft.Json;
using Platform;

namespace Services.Analytics.Events
{
	// Token: 0x02001663 RID: 5731
	[JsonObject(MemberSerialization.OptIn)]
	public abstract class BaseEventData : IAnalyticsEventData
	{
		// Token: 0x17001573 RID: 5491
		// (get) Token: 0x0600B377 RID: 45943
		public abstract string EventType { get; }

		// Token: 0x17001574 RID: 5492
		// (get) Token: 0x0600B378 RID: 45944 RVA: 0x00431B3F File Offset: 0x0042FD3F
		[JsonProperty(PropertyName = "provider_user_id")]
		[Description("The unique player identifier based on the provider_id (anonymized)")]
		public string ProviderUserId { get; }

		// Token: 0x17001575 RID: 5493
		// (get) Token: 0x0600B379 RID: 45945 RVA: 0x00431B48 File Offset: 0x0042FD48
		[JsonProperty(PropertyName = "session_build")]
		[Description("The name of the build used for the game session initiated (patch/CL number)")]
		public string SessionBuild
		{
			get
			{
				return Constants.cVersionInformation.Build.ToString();
			}
		}

		// Token: 0x17001576 RID: 5494
		// (get) Token: 0x0600B37A RID: 45946 RVA: 0x00431B67 File Offset: 0x0042FD67
		[JsonProperty(PropertyName = "game_version")]
		[Description("The name of the game version (Major.Minor.Hotfix)")]
		public string GameVersion
		{
			get
			{
				return Constants.cVersionInformation.ShortString;
			}
		}

		// Token: 0x17001577 RID: 5495
		// (get) Token: 0x0600B37B RID: 45947 RVA: 0x00431B73 File Offset: 0x0042FD73
		[JsonProperty(PropertyName = "easy_anti_cheat_enabled")]
		[Description("TRUE if the player launched with EAC activated")]
		public bool EasyAntiCheatEnabled { get; }

		// Token: 0x0600B37C RID: 45948 RVA: 0x00431B7C File Offset: 0x0042FD7C
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
			base..ctor();
		}
	}
}
