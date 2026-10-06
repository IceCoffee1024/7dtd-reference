using System;
using System.Collections.Generic;

namespace Platform.EOS
{
	// Token: 0x02001D3F RID: 7487
	[PublicizedFrom(EAccessModifier.Internal)]
	public struct SanctionsCheckResult
	{
		// Token: 0x17001C24 RID: 7204
		// (get) Token: 0x0600DDF5 RID: 56821 RVA: 0x004F1EB2 File Offset: 0x004F00B2
		public readonly DateTime LongestExpiry { get; }

		// Token: 0x17001C25 RID: 7205
		// (get) Token: 0x0600DDF6 RID: 56822 RVA: 0x004F1EBA File Offset: 0x004F00BA
		public readonly bool HasActiveSanctions { get; }

		// Token: 0x17001C26 RID: 7206
		// (get) Token: 0x0600DDF7 RID: 56823 RVA: 0x004F1EC2 File Offset: 0x004F00C2
		public readonly bool Success { get; }

		// Token: 0x0600DDF8 RID: 56824 RVA: 0x004F1ECC File Offset: 0x004F00CC
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

		// Token: 0x0600DDF9 RID: 56825 RVA: 0x004F2090 File Offset: 0x004F0290
		public SanctionsCheckResult(DateTime banUntil, GameUtils.EKickReason reason, int apiResponseEnum, string customReason)
		{
			this.KickReason = new GameUtils.KickPlayerData(reason, apiResponseEnum, banUntil, customReason);
			this.LongestExpiry = default(DateTime);
			this.HasActiveSanctions = false;
			this.ReasonForSanction = SanctionsCheckResult.GetReasonMessage(banUntil, reason, apiResponseEnum, customReason);
			this.Success = false;
		}

		// Token: 0x0600DDFA RID: 56826 RVA: 0x004F20CC File Offset: 0x004F02CC
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

		// Token: 0x0600DDFB RID: 56827 RVA: 0x004F2104 File Offset: 0x004F0304
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

		// Token: 0x0600DDFC RID: 56828 RVA: 0x004F2198 File Offset: 0x004F0398
		[PublicizedFrom(EAccessModifier.Private)]
		public static string BannedMessage(DateTime banUntil, string customReason)
		{
			if (!(banUntil == default(DateTime)) && !(banUntil == DateTime.MaxValue))
			{
				return string.Format("\n" + Localization.Get("auth_sanctioned", false, null), banUntil.ToCultureInvariantString());
			}
			return Localization.Get("auth_sanctioned_forever", false, null);
		}

		// Token: 0x0400A782 RID: 42882
		public GameUtils.KickPlayerData KickReason;

		// Token: 0x0400A783 RID: 42883
		public string ReasonForSanction;
	}
}
