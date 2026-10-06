using System;
using UnityEngine.Scripting;

// Token: 0x02000785 RID: 1925
[Preserve]
public class PlayerIdAuthorizer : AuthorizerAbs
{
	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x06003997 RID: 14743 RVA: 0x00178200 File Offset: 0x00176400
	public override int Order
	{
		get
		{
			return 50;
		}
	}

	// Token: 0x170005EC RID: 1516
	// (get) Token: 0x06003998 RID: 14744 RVA: 0x00178204 File Offset: 0x00176404
	public override string AuthorizerName
	{
		get
		{
			return "PlayerId";
		}
	}

	// Token: 0x170005ED RID: 1517
	// (get) Token: 0x06003999 RID: 14745 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x0600399A RID: 14746 RVA: 0x0017820C File Offset: 0x0017640C
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		if (_clientInfo.PlatformId == null)
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.EmptyNameOrPlayerID, 0, default(DateTime), "")));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
