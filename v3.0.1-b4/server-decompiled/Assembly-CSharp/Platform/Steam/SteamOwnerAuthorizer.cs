using System;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001C5C RID: 7260
	[Preserve]
	public class SteamOwnerAuthorizer : AuthorizerAbs
	{
		// Token: 0x17001A67 RID: 6759
		// (get) Token: 0x0600D6DF RID: 55007 RVA: 0x004D6505 File Offset: 0x004D4705
		public override int Order
		{
			get
			{
				return 430;
			}
		}

		// Token: 0x17001A68 RID: 6760
		// (get) Token: 0x0600D6E0 RID: 55008 RVA: 0x004D650C File Offset: 0x004D470C
		public override string AuthorizerName
		{
			get
			{
				return "SteamFamily";
			}
		}

		// Token: 0x17001A69 RID: 6761
		// (get) Token: 0x0600D6E1 RID: 55009 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public override string StateLocalizationKey
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001A6A RID: 6762
		// (get) Token: 0x0600D6E2 RID: 55010 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override EPlatformIdentifier PlatformRestriction
		{
			get
			{
				return EPlatformIdentifier.Steam;
			}
		}

		// Token: 0x0600D6E3 RID: 55011 RVA: 0x004D6514 File Offset: 0x004D4714
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
