using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000777 RID: 1911
[Preserve]
public class ServerStateAuthorizer : AuthorizerAbs
{
	// Token: 0x170005B6 RID: 1462
	// (get) Token: 0x060038F3 RID: 14579 RVA: 0x00104CB5 File Offset: 0x00102EB5
	public override int Order
	{
		get
		{
			return 30;
		}
	}

	// Token: 0x170005B7 RID: 1463
	// (get) Token: 0x060038F4 RID: 14580 RVA: 0x001756D2 File Offset: 0x001738D2
	public override string AuthorizerName
	{
		get
		{
			return "ServerState";
		}
	}

	// Token: 0x170005B8 RID: 1464
	// (get) Token: 0x060038F5 RID: 14581 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060038F6 RID: 14582 RVA: 0x001756DC File Offset: 0x001738DC
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
