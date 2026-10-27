using System;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001CE6 RID: 7398
	[Preserve]
	public class SteamOwnerAuthorizer : AuthorizerAbs
	{
		// Token: 0x17001BC7 RID: 7111
		// (get) Token: 0x0600DAFC RID: 56060 RVA: 0x004E4FB5 File Offset: 0x004E31B5
		public override int Order
		{
			get
			{
				return 430;
			}
		}

		// Token: 0x17001BC8 RID: 7112
		// (get) Token: 0x0600DAFD RID: 56061 RVA: 0x004E4FBC File Offset: 0x004E31BC
		public override string AuthorizerName
		{
			get
			{
				return "SteamFamily";
			}
		}

		// Token: 0x17001BC9 RID: 7113
		// (get) Token: 0x0600DAFE RID: 56062 RVA: 0x000200DA File Offset: 0x0001E2DA
		public override string StateLocalizationKey
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001BCA RID: 7114
		// (get) Token: 0x0600DAFF RID: 56063 RVA: 0x00047076 File Offset: 0x00045276
		public override EPlatformIdentifier PlatformRestriction
		{
			get
			{
				return EPlatformIdentifier.Steam;
			}
		}

		// Token: 0x0600DB00 RID: 56064 RVA: 0x004E4FC4 File Offset: 0x004E31C4
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
