using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000781 RID: 1921
[Preserve]
public class ServerStateAuthorizer : AuthorizerAbs
{
	// Token: 0x170005C0 RID: 1472
	// (get) Token: 0x0600394B RID: 14667 RVA: 0x00105CC1 File Offset: 0x00103EC1
	public override int Order
	{
		get
		{
			return 30;
		}
	}

	// Token: 0x170005C1 RID: 1473
	// (get) Token: 0x0600394C RID: 14668 RVA: 0x00177952 File Offset: 0x00175B52
	public override string AuthorizerName
	{
		get
		{
			return "ServerState";
		}
	}

	// Token: 0x170005C2 RID: 1474
	// (get) Token: 0x0600394D RID: 14669 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x0600394E RID: 14670 RVA: 0x0017795C File Offset: 0x00175B5C
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
