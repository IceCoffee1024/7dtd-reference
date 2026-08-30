using System;
using UnityEngine.Scripting;

// Token: 0x02000776 RID: 1910
[Preserve]
public class PlayerNameAuthorizer : AuthorizerAbs
{
	// Token: 0x170005B3 RID: 1459
	// (get) Token: 0x060038EE RID: 14574 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int Order
	{
		get
		{
			return 20;
		}
	}

	// Token: 0x170005B4 RID: 1460
	// (get) Token: 0x060038EF RID: 14575 RVA: 0x00175678 File Offset: 0x00173878
	public override string AuthorizerName
	{
		get
		{
			return "PlayerName";
		}
	}

	// Token: 0x170005B5 RID: 1461
	// (get) Token: 0x060038F0 RID: 14576 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060038F1 RID: 14577 RVA: 0x00175680 File Offset: 0x00173880
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		if (string.IsNullOrEmpty(_clientInfo.playerName))
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.EmptyNameOrPlayerID, 0, default(DateTime), "")));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
