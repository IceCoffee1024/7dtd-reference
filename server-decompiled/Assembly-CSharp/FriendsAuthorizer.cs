using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000794 RID: 1940
[Preserve]
public class FriendsAuthorizer : AuthorizerAbs
{
	// Token: 0x17000604 RID: 1540
	// (get) Token: 0x060039D6 RID: 14806 RVA: 0x001797D1 File Offset: 0x001779D1
	public override int Order
	{
		get
		{
			return 450;
		}
	}

	// Token: 0x17000605 RID: 1541
	// (get) Token: 0x060039D7 RID: 14807 RVA: 0x001797D8 File Offset: 0x001779D8
	public override string AuthorizerName
	{
		get
		{
			return "Friends";
		}
	}

	// Token: 0x17000606 RID: 1542
	// (get) Token: 0x060039D8 RID: 14808 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000607 RID: 1543
	// (get) Token: 0x060039D9 RID: 14809 RVA: 0x001797DF File Offset: 0x001779DF
	public override bool AuthorizerActive
	{
		get
		{
			return !GameManager.IsDedicatedServer && GamePrefs.GetInt(EnumGamePrefs.ServerVisibility) == 1;
		}
	}

	// Token: 0x060039DA RID: 14810 RVA: 0x001797F8 File Offset: 0x001779F8
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
