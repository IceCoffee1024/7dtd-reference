using System;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001CB5 RID: 7349
	[Preserve]
	public class SteamOwnerAuthorizer : AuthorizerAbs
	{
		// Token: 0x17001B7F RID: 7039
		// (get) Token: 0x0600D9E8 RID: 55784 RVA: 0x004E1465 File Offset: 0x004DF665
		public override int Order
		{
			get
			{
				return 430;
			}
		}

		// Token: 0x17001B80 RID: 7040
		// (get) Token: 0x0600D9E9 RID: 55785 RVA: 0x004E146C File Offset: 0x004DF66C
		public override string AuthorizerName
		{
			get
			{
				return "SteamFamily";
			}
		}

		// Token: 0x17001B81 RID: 7041
		// (get) Token: 0x0600D9EA RID: 55786 RVA: 0x000200DA File Offset: 0x0001E2DA
		public override string StateLocalizationKey
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B82 RID: 7042
		// (get) Token: 0x0600D9EB RID: 55787 RVA: 0x000470B6 File Offset: 0x000452B6
		public override EPlatformIdentifier PlatformRestriction
		{
			get
			{
				return EPlatformIdentifier.Steam;
			}
		}

		// Token: 0x0600D9EC RID: 55788 RVA: 0x004E1474 File Offset: 0x004DF674
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
