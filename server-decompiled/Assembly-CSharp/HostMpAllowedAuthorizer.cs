using System;
using UnityEngine.Scripting;

// Token: 0x0200078C RID: 1932
[Preserve]
public class HostMpAllowedAuthorizer : AuthorizerAbs
{
	// Token: 0x170005EC RID: 1516
	// (get) Token: 0x060039A9 RID: 14761 RVA: 0x0017913E File Offset: 0x0017733E
	public override int Order
	{
		get
		{
			return 41;
		}
	}

	// Token: 0x170005ED RID: 1517
	// (get) Token: 0x060039AA RID: 14762 RVA: 0x00179142 File Offset: 0x00177342
	public override string AuthorizerName
	{
		get
		{
			return "MpHostAllowed";
		}
	}

	// Token: 0x170005EE RID: 1518
	// (get) Token: 0x060039AB RID: 14763 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060039AC RID: 14764 RVA: 0x0017914C File Offset: 0x0017734C
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		if (!PermissionsManager.IsMultiplayerAllowed() || !PermissionsManager.CanHostMultiplayer())
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.MultiplayerBlockedForHostAccount, 0, default(DateTime), "")));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
