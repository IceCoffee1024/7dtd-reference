using System;
using UnityEngine.Scripting;

// Token: 0x02000784 RID: 1924
[Preserve]
public class HostMpAllowedAuthorizer : AuthorizerAbs
{
	// Token: 0x170005E8 RID: 1512
	// (get) Token: 0x06003992 RID: 14738 RVA: 0x001781A6 File Offset: 0x001763A6
	public override int Order
	{
		get
		{
			return 41;
		}
	}

	// Token: 0x170005E9 RID: 1513
	// (get) Token: 0x06003993 RID: 14739 RVA: 0x001781AA File Offset: 0x001763AA
	public override string AuthorizerName
	{
		get
		{
			return "MpHostAllowed";
		}
	}

	// Token: 0x170005EA RID: 1514
	// (get) Token: 0x06003994 RID: 14740 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06003995 RID: 14741 RVA: 0x001781B4 File Offset: 0x001763B4
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		if (!PermissionsManager.IsMultiplayerAllowed() || !PermissionsManager.CanHostMultiplayer())
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.MultiplayerBlockedForHostAccount, 0, default(DateTime), "")));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
