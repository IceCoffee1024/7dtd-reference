using System;
using System.Collections.Generic;

namespace Platform.EOS
{
	// Token: 0x02001D70 RID: 7536
	[PublicizedFrom(EAccessModifier.Internal)]
	public struct SanctionsCheckResult
	{
		// Token: 0x17001C6C RID: 7276
		// (get) Token: 0x0600DF09 RID: 57097 RVA: 0x004F5A52 File Offset: 0x004F3C52
		public readonly DateTime LongestExpiry { get; }

		// Token: 0x17001C6D RID: 7277
		// (get) Token: 0x0600DF0A RID: 57098 RVA: 0x004F5A5A File Offset: 0x004F3C5A
		public readonly bool HasActiveSanctions { get; }

		// Token: 0x17001C6E RID: 7278
		// (get) Token: 0x0600DF0B RID: 57099 RVA: 0x004F5A62 File Offset: 0x004F3C62
		public readonly bool Success { get; }

		// Token: 0x0600DF0C RID: 57100 RVA: 0x004F5A6C File Offset: 0x004F3C6C
		public SanctionsCheckResult(List<EOSSanction> sanctions)
		{
			this.Success = true;
			this.LongestExpiry = DateTime.MinValue;
			this.ReasonForSanction = string.Empty;
			this.KickReason = default(GameUtils.KickPlayerData);
			if (sanctions == null || sanctions.Count == 0)
			{
				this.HasActiveSanctions = false;
				return;
			}
			this.HasActiveSanctions = true;
			EOSSanction eossanction = sanctions[0];
			foreach (EOSSanction eossanction2 in sanctions)
			{
				if (eossanction2.expiry == DateTime.MaxValue || eossanction2.expiry == default(DateTime))
				{
					Log.Out("[EOS] Sanctioned Until: Forever");
					this.ReasonForSanction = SanctionsCheckResult.GetReasonMessage(default(DateTime), GameUtils.EKickReason.CrossPlatformAuthenticationFailed, 9, "Sanction: [" + eossanction2.ReferenceId + "]");
					this.ReasonForSanction = string.Format(Localization.Get("auth_banned_forever", false, null), Array.Empty<object>());
					this.KickReason = default(GameUtils.KickPlayerData);
					this.LongestExpiry = DateTime.MaxValue;
					break;
				}
				if (eossanction2.expiry > eossanction.expiry)
				{
					eossanction = eossanction2;
				}
			}
			Log.Out("[EOS] Sanctioned Until: " + this.LongestExpiry.ToLongDateString());
			this.ReasonForSanction = SanctionsCheckResult.GetReasonMessage(eossanction.expiry, GameUtils.EKickReason.CrossPlatformAuthenticationFailed, 9, "Sanction: [" + eossanction.ReferenceId + "]");
			this.KickReason = new GameUtils.KickPlayerData(GameUtils.EKickReason.CrossPlatformAuthenticationFailed, 9, eossanction.expiry, "Sanction: [" + eossanction.ReferenceId + "]");
			this.LongestExpiry = eossanction.expiry;
		}

		// Token: 0x0600DF0D RID: 57101 RVA: 0x004F5C30 File Offset: 0x004F3E30
		public SanctionsCheckResult(DateTime banUntil, GameUtils.EKickReason reason, int apiResponseEnum, string customReason)
		{
			this.KickReason = new GameUtils.KickPlayerData(reason, apiResponseEnum, banUntil, customReason);
			this.LongestExpiry = default(DateTime);
			this.HasActiveSanctions = false;
			this.ReasonForSanction = SanctionsCheckResult.GetReasonMessage(banUntil, reason, apiResponseEnum, customReason);
			this.Success = false;
		}

		// Token: 0x0600DF0E RID: 57102 RVA: 0x004F5C6C File Offset: 0x004F3E6C
		[PublicizedFrom(EAccessModifier.Private)]
		public static string GetReasonMessage(DateTime banUntil, GameUtils.EKickReason reason, int apiResponseEnum, string customReason)
		{
			if (reason == GameUtils.EKickReason.Banned)
			{
				return SanctionsCheckResult.BannedMessage(banUntil, customReason);
			}
			if (reason == GameUtils.EKickReason.PlatformAuthenticationFailed)
			{
				return SanctionsCheckResult.GetAuthFailedMessage(banUntil, apiResponseEnum, customReason);
			}
			if (reason == GameUtils.EKickReason.CrossPlatformAuthenticationFailed)
			{
				return SanctionsCheckResult.GetAuthFailedMessage(banUntil, apiResponseEnum, customReason);
			}
			return Localization.Get("auth_unknown", false, null);
		}

		// Token: 0x0600DF0F RID: 57103 RVA: 0x004F5CA4 File Offset: 0x004F3EA4
		[PublicizedFrom(EAccessModifier.Private)]
		public static string GetAuthFailedMessage(DateTime banUntil, int apiResponseEnum, string customReason)
		{
			switch (apiResponseEnum)
			{
			case 1:
			case 2:
			case 3:
			case 4:
			case 6:
			case 7:
			case 8:
				return string.Format(Localization.Get("platformauth_" + ((EUserAuthenticationResult)apiResponseEnum).ToStringCached<EUserAuthenticationResult>(), false, null), PlatformManager.NativePlatform.PlatformDisplayName);
			case 5:
				return string.Format(Localization.Get("auth_timeout", false, null), PlatformManager.CrossplatformPlatform.PlatformDisplayName);
			case 9:
				return SanctionsCheckResult.BannedMessage(banUntil, customReason);
			default:
				return Localization.Get("auth_unknown", false, null);
			}
		}

		// Token: 0x0600DF10 RID: 57104 RVA: 0x004F5D38 File Offset: 0x004F3F38
		[PublicizedFrom(EAccessModifier.Private)]
		public static string BannedMessage(DateTime banUntil, string customReason)
		{
			if (!(banUntil == default(DateTime)) && !(banUntil == DateTime.MaxValue))
			{
				return string.Format("\n" + Localization.Get("auth_sanctioned", false, null), banUntil.ToCultureInvariantString());
			}
			return Localization.Get("auth_sanctioned_forever", false, null);
		}

		// Token: 0x0400A86B RID: 43115
		public GameUtils.KickPlayerData KickReason;

		// Token: 0x0400A86C RID: 43116
		public string ReasonForSanction;
	}
}
