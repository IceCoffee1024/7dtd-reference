using System;
using UnityEngine.Scripting;

// Token: 0x02000778 RID: 1912
[Preserve]
public class HostMpAllowedAuthorizer : AuthorizerAbs
{
	// Token: 0x170005B9 RID: 1465
	// (get) Token: 0x060038F8 RID: 14584 RVA: 0x0017576E File Offset: 0x0017396E
	public override int Order
	{
		get
		{
			return 41;
		}
	}

	// Token: 0x170005BA RID: 1466
	// (get) Token: 0x060038F9 RID: 14585 RVA: 0x00175772 File Offset: 0x00173972
	public override string AuthorizerName
	{
		get
		{
			return "MpHostAllowed";
		}
	}

	// Token: 0x170005BB RID: 1467
	// (get) Token: 0x060038FA RID: 14586 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060038FB RID: 14587 RVA: 0x0017577C File Offset: 0x0017397C
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		if (!PermissionsManager.IsMultiplayerAllowed() || !PermissionsManager.CanHostMultiplayer())
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.MultiplayerBlockedForHostAccount, 0, default(DateTime), "")));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
