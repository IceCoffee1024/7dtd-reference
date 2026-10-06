using System;
using UnityEngine.Scripting;

// Token: 0x02000782 RID: 1922
[Preserve]
public class PlayerNameAuthorizer : AuthorizerAbs
{
	// Token: 0x170005E2 RID: 1506
	// (get) Token: 0x06003988 RID: 14728 RVA: 0x000F659D File Offset: 0x000F479D
	public override int Order
	{
		get
		{
			return 20;
		}
	}

	// Token: 0x170005E3 RID: 1507
	// (get) Token: 0x06003989 RID: 14729 RVA: 0x001780B0 File Offset: 0x001762B0
	public override string AuthorizerName
	{
		get
		{
			return "PlayerName";
		}
	}

	// Token: 0x170005E4 RID: 1508
	// (get) Token: 0x0600398A RID: 14730 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x0600398B RID: 14731 RVA: 0x001780B8 File Offset: 0x001762B8
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		if (string.IsNullOrEmpty(_clientInfo.playerName))
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.EmptyNameOrPlayerID, 0, default(DateTime), "")));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
