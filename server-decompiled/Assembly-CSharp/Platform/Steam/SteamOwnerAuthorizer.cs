using System;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001C83 RID: 7299
	[Preserve]
	public class SteamOwnerAuthorizer : AuthorizerAbs
	{
		// Token: 0x17001AD9 RID: 6873
		// (get) Token: 0x0600D86F RID: 55407 RVA: 0x004DC929 File Offset: 0x004DAB29
		public override int Order
		{
			get
			{
				return 430;
			}
		}

		// Token: 0x17001ADA RID: 6874
		// (get) Token: 0x0600D870 RID: 55408 RVA: 0x004DC930 File Offset: 0x004DAB30
		public override string AuthorizerName
		{
			get
			{
				return "SteamFamily";
			}
		}

		// Token: 0x17001ADB RID: 6875
		// (get) Token: 0x0600D871 RID: 55409 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public override string StateLocalizationKey
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001ADC RID: 6876
		// (get) Token: 0x0600D872 RID: 55410 RVA: 0x00046EDA File Offset: 0x000450DA
		public override EPlatformIdentifier PlatformRestriction
		{
			get
			{
				return EPlatformIdentifier.Steam;
			}
		}

		// Token: 0x0600D873 RID: 55411 RVA: 0x004DC938 File Offset: 0x004DAB38
		public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
		{
			UserIdentifierSteam userIdentifierSteam = (UserIdentifierSteam)_clientInfo.PlatformId;
			UserIdentifierSteam ownerId = userIdentifierSteam.OwnerId;
			DateTime dateTime;
			string text;
			if (GameManager.Instance.adminTools != null && ownerId != null && !userIdentifierSteam.Equals(ownerId) && GameManager.Instance.adminTools.Blacklist.IsBanned(ownerId, out dateTime, out text))
			{
				return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.Banned, 0, dateTime, text)));
			}
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
		}
	}
}
