using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200078A RID: 1930
[Preserve]
public class FriendsAuthorizer : AuthorizerAbs
{
	// Token: 0x170005DB RID: 1499
	// (get) Token: 0x0600397D RID: 14717 RVA: 0x00178081 File Offset: 0x00176281
	public override int Order
	{
		get
		{
			return 450;
		}
	}

	// Token: 0x170005DC RID: 1500
	// (get) Token: 0x0600397E RID: 14718 RVA: 0x00178088 File Offset: 0x00176288
	public override string AuthorizerName
	{
		get
		{
			return "Friends";
		}
	}

	// Token: 0x170005DD RID: 1501
	// (get) Token: 0x0600397F RID: 14719 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170005DE RID: 1502
	// (get) Token: 0x06003980 RID: 14720 RVA: 0x0017808F File Offset: 0x0017628F
	public override bool AuthorizerActive
	{
		get
		{
			return !GameManager.IsDedicatedServer && GamePrefs.GetInt(EnumGamePrefs.ServerVisibility) == 1;
		}
	}

	// Token: 0x06003981 RID: 14721 RVA: 0x001780A8 File Offset: 0x001762A8
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		if (PlatformManager.NativePlatform.User.IsFriend(_clientInfo.PlatformId))
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
		}
		DiscordManager.DiscordUser user;
		if (_clientInfo.DiscordUserId > 0UL && (user = DiscordManager.Instance.GetUser(_clientInfo.DiscordUserId)) != null && user.IsFriend)
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.FriendsOnly, 0, default(DateTime), "")));
	}
}
