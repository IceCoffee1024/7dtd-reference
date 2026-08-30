using System;
using System.Collections.Generic;

namespace Platform.EOS
{
	// Token: 0x02001D0D RID: 7437
	[PublicizedFrom(EAccessModifier.Internal)]
	public struct SanctionsCheckResult
	{
		// Token: 0x17001B7E RID: 7038
		// (get) Token: 0x0600DC7A RID: 56442 RVA: 0x004ED26A File Offset: 0x004EB46A
		public readonly DateTime LongestExpiry { get; }

		// Token: 0x17001B7F RID: 7039
		// (get) Token: 0x0600DC7B RID: 56443 RVA: 0x004ED272 File Offset: 0x004EB472
		public readonly bool HasActiveSanctions { get; }

		// Token: 0x17001B80 RID: 7040
		// (get) Token: 0x0600DC7C RID: 56444 RVA: 0x004ED27A File Offset: 0x004EB47A
		public readonly bool Success { get; }

		// Token: 0x0600DC7D RID: 56445 RVA: 0x004ED284 File Offset: 0x004EB484
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

		// Token: 0x0600DC7E RID: 56446 RVA: 0x004ED448 File Offset: 0x004EB648
		public SanctionsCheckResult(DateTime banUntil, GameUtils.EKickReason reason, int apiResponseEnum, string customReason)
		{
			this.KickReason = new GameUtils.KickPlayerData(reason, apiResponseEnum, banUntil, customReason);
			this.LongestExpiry = default(DateTime);
			this.HasActiveSanctions = false;
			this.ReasonForSanction = SanctionsCheckResult.GetReasonMessage(banUntil, reason, apiResponseEnum, customReason);
			this.Success = false;
		}

		// Token: 0x0600DC7F RID: 56447 RVA: 0x004ED484 File Offset: 0x004EB684
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

		// Token: 0x0600DC80 RID: 56448 RVA: 0x004ED4BC File Offset: 0x004EB6BC
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

		// Token: 0x0600DC81 RID: 56449 RVA: 0x004ED550 File Offset: 0x004EB750
		[PublicizedFrom(EAccessModifier.Private)]
		public static string BannedMessage(DateTime banUntil, string customReason)
		{
			if (!(banUntil == default(DateTime)) && !(banUntil == DateTime.MaxValue))
			{
				return string.Format("\n" + Localization.Get("auth_sanctioned", false, null), banUntil.ToCultureInvariantString());
			}
			return Localization.Get("auth_sanctioned_forever", false, null);
		}

		// Token: 0x0400A6AB RID: 42667
		public GameUtils.KickPlayerData KickReason;

		// Token: 0x0400A6AC RID: 42668
		public string ReasonForSanction;
	}
}
