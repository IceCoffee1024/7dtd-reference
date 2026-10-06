using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200078C RID: 1932
[Preserve]
public class FriendsAuthorizer : AuthorizerAbs
{
	// Token: 0x17000600 RID: 1536
	// (get) Token: 0x060039BF RID: 14783 RVA: 0x00178839 File Offset: 0x00176A39
	public override int Order
	{
		get
		{
			return 450;
		}
	}

	// Token: 0x17000601 RID: 1537
	// (get) Token: 0x060039C0 RID: 14784 RVA: 0x00178840 File Offset: 0x00176A40
	public override string AuthorizerName
	{
		get
		{
			return "Friends";
		}
	}

	// Token: 0x17000602 RID: 1538
	// (get) Token: 0x060039C1 RID: 14785 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000603 RID: 1539
	// (get) Token: 0x060039C2 RID: 14786 RVA: 0x00178847 File Offset: 0x00176A47
	public override bool AuthorizerActive
	{
		get
		{
			return !GameManager.IsDedicatedServer && GamePrefs.GetInt(EnumGamePrefs.ServerVisibility) == 1;
		}
	}

	// Token: 0x060039C3 RID: 14787 RVA: 0x00178860 File Offset: 0x00176A60
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
