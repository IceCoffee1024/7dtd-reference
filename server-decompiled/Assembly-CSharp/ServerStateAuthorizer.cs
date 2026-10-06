using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000783 RID: 1923
[Preserve]
public class ServerStateAuthorizer : AuthorizerAbs
{
	// Token: 0x170005E5 RID: 1509
	// (get) Token: 0x0600398D RID: 14733 RVA: 0x001065F1 File Offset: 0x001047F1
	public override int Order
	{
		get
		{
			return 30;
		}
	}

	// Token: 0x170005E6 RID: 1510
	// (get) Token: 0x0600398E RID: 14734 RVA: 0x0017810A File Offset: 0x0017630A
	public override string AuthorizerName
	{
		get
		{
			return "ServerState";
		}
	}

	// Token: 0x170005E7 RID: 1511
	// (get) Token: 0x0600398F RID: 14735 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06003990 RID: 14736 RVA: 0x00178114 File Offset: 0x00176314
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		IMasterServerAnnouncer serverListAnnouncer = PlatformManager.MultiPlatform.ServerListAnnouncer;
		if ((serverListAnnouncer != null && !serverListAnnouncer.GameServerInitialized) || !GameManager.Instance.gameStateManager.IsGameStarted())
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.GameStillLoading, 0, default(DateTime), "")));
		}
		if (GameStats.GetInt(EnumGameStats.GameState) == 2)
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.GamePaused, 0, default(DateTime), "")));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
