using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000780 RID: 1920
[Preserve]
public class FriendsAuthorizer : AuthorizerAbs
{
	// Token: 0x170005D1 RID: 1489
	// (get) Token: 0x06003925 RID: 14629 RVA: 0x00175E01 File Offset: 0x00174001
	public override int Order
	{
		get
		{
			return 450;
		}
	}

	// Token: 0x170005D2 RID: 1490
	// (get) Token: 0x06003926 RID: 14630 RVA: 0x00175E08 File Offset: 0x00174008
	public override string AuthorizerName
	{
		get
		{
			return "Friends";
		}
	}

	// Token: 0x170005D3 RID: 1491
	// (get) Token: 0x06003927 RID: 14631 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170005D4 RID: 1492
	// (get) Token: 0x06003928 RID: 14632 RVA: 0x00175E0F File Offset: 0x0017400F
	public override bool AuthorizerActive
	{
		get
		{
			return !GameManager.IsDedicatedServer && GamePrefs.GetInt(EnumGamePrefs.ServerVisibility) == 1;
		}
	}

	// Token: 0x06003929 RID: 14633 RVA: 0x00175E28 File Offset: 0x00174028
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
