using System;
using UnityEngine.Scripting;

// Token: 0x02000779 RID: 1913
[Preserve]
public class PlayerIdAuthorizer : AuthorizerAbs
{
	// Token: 0x170005BC RID: 1468
	// (get) Token: 0x060038FD RID: 14589 RVA: 0x001757C8 File Offset: 0x001739C8
	public override int Order
	{
		get
		{
			return 50;
		}
	}

	// Token: 0x170005BD RID: 1469
	// (get) Token: 0x060038FE RID: 14590 RVA: 0x001757CC File Offset: 0x001739CC
	public override string AuthorizerName
	{
		get
		{
			return "PlayerId";
		}
	}

	// Token: 0x170005BE RID: 1470
	// (get) Token: 0x060038FF RID: 14591 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06003900 RID: 14592 RVA: 0x001757D4 File Offset: 0x001739D4
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		if (_clientInfo.PlatformId == null)
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.EmptyNameOrPlayerID, 0, default(DateTime), "")));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
