using System;
using UnityEngine.Scripting;

// Token: 0x02000783 RID: 1923
[Preserve]
public class PlayerIdAuthorizer : AuthorizerAbs
{
	// Token: 0x170005C6 RID: 1478
	// (get) Token: 0x06003955 RID: 14677 RVA: 0x00177A48 File Offset: 0x00175C48
	public override int Order
	{
		get
		{
			return 50;
		}
	}

	// Token: 0x170005C7 RID: 1479
	// (get) Token: 0x06003956 RID: 14678 RVA: 0x00177A4C File Offset: 0x00175C4C
	public override string AuthorizerName
	{
		get
		{
			return "PlayerId";
		}
	}

	// Token: 0x170005C8 RID: 1480
	// (get) Token: 0x06003957 RID: 14679 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06003958 RID: 14680 RVA: 0x00177A54 File Offset: 0x00175C54
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		if (_clientInfo.PlatformId == null)
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.EmptyNameOrPlayerID, 0, default(DateTime), "")));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
