using System;
using System.Collections.Generic;

namespace Platform.EOS
{
	// Token: 0x02001CE6 RID: 7398
	[PublicizedFrom(EAccessModifier.Internal)]
	public struct SanctionsCheckResult
	{
		// Token: 0x17001B0C RID: 6924
		// (get) Token: 0x0600DADE RID: 56030 RVA: 0x004E6C2A File Offset: 0x004E4E2A
		public readonly DateTime LongestExpiry { get; }

		// Token: 0x17001B0D RID: 6925
		// (get) Token: 0x0600DADF RID: 56031 RVA: 0x004E6C32 File Offset: 0x004E4E32
		public readonly bool HasActiveSanctions { get; }

		// Token: 0x17001B0E RID: 6926
		// (get) Token: 0x0600DAE0 RID: 56032 RVA: 0x004E6C3A File Offset: 0x004E4E3A
		public readonly bool Success { get; }

		// Token: 0x0600DAE1 RID: 56033 RVA: 0x004E6C44 File Offset: 0x004E4E44
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

		// Token: 0x0600DAE2 RID: 56034 RVA: 0x004E6E08 File Offset: 0x004E5008
		public SanctionsCheckResult(DateTime banUntil, GameUtils.EKickReason reason, int apiResponseEnum, string customReason)
		{
			this.KickReason = new GameUtils.KickPlayerData(reason, apiResponseEnum, banUntil, customReason);
			this.LongestExpiry = default(DateTime);
			this.HasActiveSanctions = false;
			this.ReasonForSanction = SanctionsCheckResult.GetReasonMessage(banUntil, reason, apiResponseEnum, customReason);
			this.Success = false;
		}

		// Token: 0x0600DAE3 RID: 56035 RVA: 0x004E6E44 File Offset: 0x004E5044
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

		// Token: 0x0600DAE4 RID: 56036 RVA: 0x004E6E7C File Offset: 0x004E507C
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

		// Token: 0x0600DAE5 RID: 56037 RVA: 0x004E6F10 File Offset: 0x004E5110
		[PublicizedFrom(EAccessModifier.Private)]
		public static string BannedMessage(DateTime banUntil, string customReason)
		{
			if (!(banUntil == default(DateTime)) && !(banUntil == DateTime.MaxValue))
			{
				return string.Format("\n" + Localization.Get("auth_sanctioned", false, null), banUntil.ToCultureInvariantString());
			}
			return Localization.Get("auth_sanctioned_forever", false, null);
		}

		// Token: 0x0400A578 RID: 42360
		public GameUtils.KickPlayerData KickReason;

		// Token: 0x0400A579 RID: 42361
		public string ReasonForSanction;
	}
}
