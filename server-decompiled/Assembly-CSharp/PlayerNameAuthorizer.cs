using System;
using UnityEngine.Scripting;

// Token: 0x0200078A RID: 1930
[Preserve]
public class PlayerNameAuthorizer : AuthorizerAbs
{
	// Token: 0x170005E6 RID: 1510
	// (get) Token: 0x0600399F RID: 14751 RVA: 0x000F6AD5 File Offset: 0x000F4CD5
	public override int Order
	{
		get
		{
			return 20;
		}
	}

	// Token: 0x170005E7 RID: 1511
	// (get) Token: 0x060039A0 RID: 14752 RVA: 0x00179048 File Offset: 0x00177248
	public override string AuthorizerName
	{
		get
		{
			return "PlayerName";
		}
	}

	// Token: 0x170005E8 RID: 1512
	// (get) Token: 0x060039A1 RID: 14753 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060039A2 RID: 14754 RVA: 0x00179050 File Offset: 0x00177250
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		if (string.IsNullOrEmpty(_clientInfo.playerName))
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.EmptyNameOrPlayerID, 0, default(DateTime), "")));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
