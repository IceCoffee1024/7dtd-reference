using System;
using UnityEngine.Scripting;

// Token: 0x0200078D RID: 1933
[Preserve]
public class PlayerIdAuthorizer : AuthorizerAbs
{
	// Token: 0x170005EF RID: 1519
	// (get) Token: 0x060039AE RID: 14766 RVA: 0x00179198 File Offset: 0x00177398
	public override int Order
	{
		get
		{
			return 50;
		}
	}

	// Token: 0x170005F0 RID: 1520
	// (get) Token: 0x060039AF RID: 14767 RVA: 0x0017919C File Offset: 0x0017739C
	public override string AuthorizerName
	{
		get
		{
			return "PlayerId";
		}
	}

	// Token: 0x170005F1 RID: 1521
	// (get) Token: 0x060039B0 RID: 14768 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060039B1 RID: 14769 RVA: 0x001791A4 File Offset: 0x001773A4
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		if (_clientInfo.PlatformId == null)
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.EmptyNameOrPlayerID, 0, default(DateTime), "")));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
