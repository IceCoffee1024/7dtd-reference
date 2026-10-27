using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200078B RID: 1931
[Preserve]
public class ServerStateAuthorizer : AuthorizerAbs
{
	// Token: 0x170005E9 RID: 1513
	// (get) Token: 0x060039A4 RID: 14756 RVA: 0x00106B45 File Offset: 0x00104D45
	public override int Order
	{
		get
		{
			return 30;
		}
	}

	// Token: 0x170005EA RID: 1514
	// (get) Token: 0x060039A5 RID: 14757 RVA: 0x001790A2 File Offset: 0x001772A2
	public override string AuthorizerName
	{
		get
		{
			return "ServerState";
		}
	}

	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x060039A6 RID: 14758 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060039A7 RID: 14759 RVA: 0x001790AC File Offset: 0x001772AC
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
