using System;
using UnityEngine.Scripting;

// Token: 0x02000780 RID: 1920
[Preserve]
public class PlayerNameAuthorizer : AuthorizerAbs
{
	// Token: 0x170005BD RID: 1469
	// (get) Token: 0x06003946 RID: 14662 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int Order
	{
		get
		{
			return 20;
		}
	}

	// Token: 0x170005BE RID: 1470
	// (get) Token: 0x06003947 RID: 14663 RVA: 0x001778F8 File Offset: 0x00175AF8
	public override string AuthorizerName
	{
		get
		{
			return "PlayerName";
		}
	}

	// Token: 0x170005BF RID: 1471
	// (get) Token: 0x06003948 RID: 14664 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06003949 RID: 14665 RVA: 0x00177900 File Offset: 0x00175B00
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		if (string.IsNullOrEmpty(_clientInfo.playerName))
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.EmptyNameOrPlayerID, 0, default(DateTime), "")));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
